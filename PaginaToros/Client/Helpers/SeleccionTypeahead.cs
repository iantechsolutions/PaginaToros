namespace PaginaToros.Client.Helpers
{
    /// <summary>
    /// Corre el trabajo pesado que dispara una seleccion de BlazoredTypeahead FUERA del
    /// handler de ValueChanged.
    ///
    /// Por que hace falta: BlazoredTypeahead recien cierra el desplegable y pinta el item
    /// elegido DESPUES de que retorna el handler de ValueChanged, y Blazor no aplica
    /// ningun cambio de UI mientras la cadena del handler sigue corriendo. Si el handler
    /// espera un request HTTP, el usuario ve el desplegable abierto y sin cambios durante
    /// todo el viaje al servidor, cree que el click no funciono y vuelve a clickear (y
    /// cada click dispara otro request). Contra localhost no se nota; contra el servidor
    /// real si.
    ///
    /// Uso tipico:
    ///
    ///     private readonly SeleccionTypeahead _seleccionSocio = new();
    ///
    ///     private Task OnSocioChanged(SocioDTO? socio)
    ///     {
    ///         socioSeleccionado = socio;              // parte sincronica: se ve al instante
    ///         _seleccionSocio.Ejecutar(CargarDatosDelSocio,
    ///                                  () => InvokeAsync(StateHasChanged),
    ///                                  nameof(OnSocioChanged));
    ///         return Task.CompletedTask;             // el handler vuelve enseguida
    ///     }
    /// </summary>
    public sealed class SeleccionTypeahead
    {
        private Func<Task>? _pendiente;
        private bool _enCurso;

        /// <summary>
        /// True mientras hay trabajo corriendo o encolado. Sirve para deshabilitar el
        /// boton de guardar o mostrar un indicador de carga.
        /// </summary>
        public bool EnCurso => _enCurso;

        /// <summary>
        /// Encola <paramref name="trabajo"/> y vuelve inmediatamente.
        /// Si ya hay algo corriendo no se lanza en paralelo: queda pendiente y arranca al
        /// terminar el anterior, pisando cualquier pendiente previo. Asi una rafaga de
        /// clicks se colapsa en un solo request extra y siempre gana la ultima seleccion
        /// (nunca queda la pantalla con datos de un socio y el nombre de otro).
        /// </summary>
        /// <param name="trabajo">La parte lenta (requests, recargas de grilla).</param>
        /// <param name="refrescarUi">Normalmente () => InvokeAsync(StateHasChanged).</param>
        /// <param name="contexto">Nombre del handler, solo para el log de errores.</param>
        public void Ejecutar(Func<Task> trabajo, Func<Task>? refrescarUi = null, string contexto = "")
        {
            if (trabajo == null)
            {
                return;
            }

            _pendiente = trabajo;

            if (_enCurso)
            {
                return;
            }

            _enCurso = true;
            _ = ProcesarAsync(refrescarUi, contexto);
        }

        private async Task ProcesarAsync(Func<Task>? refrescarUi, string contexto)
        {
            try
            {
                while (_pendiente != null)
                {
                    var trabajo = _pendiente;
                    _pendiente = null;

                    try
                    {
                        await trabajo();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[SeleccionTypeahead {contexto}] {ex.Message}");
                    }

                    if (refrescarUi != null)
                    {
                        try
                        {
                            await refrescarUi();
                        }
                        catch
                        {
                            // el componente pudo haberse cerrado (modal) mientras corria el request
                        }
                    }
                }
            }
            finally
            {
                _enCurso = false;
            }
        }
    }
}
