using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Services;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Dynamic;
using System.Reflection;
namespace PaginaToros.Server.Controllers
{
    /// <summary>
    /// Transferencias de toros entre razones sociales (o entre establecimientos de la misma).
    /// Contrato de respuesta: <c>Exito = 1</c> SOLO cuando la operacion termino bien. Todo error
    /// (validacion, permisos, persistencia) devuelve <c>Exito = 0</c> con un status HTTP de error:
    /// 400 para rechazos de negocio, 403 sin permisos, 404 si no existe, 500 si fallo la base.
    /// El cliente (AddTransSb) muestra <c>Mensaje</c> y no toca el estado del toro en pantalla.
    /// Un 500 lleva SIEMPRE un mensaje funcional generico (<see cref="ErrorInterno{T}"/>): el detalle
    /// tecnico (SQL, columnas, excepcion EF/MariaDB, stack trace) va unicamente al log del servidor.
    /// Los rechazos de negocio se señalan con <see cref="TransferenciaRechazadaException"/>, asi el 400
    /// solo puede llevar mensajes escritos aca y nunca el texto de una excepcion de EF.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TranssbController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITranssbRepositorio _transsbRepositorio;
        private readonly hereford_prContext _db;
        private readonly IUserSocioContextService _userSocioContextService;
        private readonly ILogger<TranssbController> _logger;

        public TranssbController(
            ITranssbRepositorio transsbRepositorio,
            ITorosRepositorio torosRepositorio,
            IMapper mapper,
            hereford_prContext db,
            IUserSocioContextService userSocioContextService,
            ILogger<TranssbController> logger)
        {
            _mapper = mapper;
            _transsbRepositorio = transsbRepositorio;
            _db = db;
            _userSocioContextService = userSocioContextService;
            _logger = logger;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {
            Respuesta<List<TranssbDTO>> _ResponseDTO = new Respuesta<List<TranssbDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<TranssbDTO>>());
                }

                List<TranssbDTO> listaPedido = new List<TranssbDTO>();
                var a = RequiresActiveSocioScope(accessContext)
                    ? await _transsbRepositorio.LimitadosFiltrados(skip, take, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : await _transsbRepositorio.Lista(skip, take);

                listaPedido = _mapper.Map<List<TranssbDTO>>(a);

                _ResponseDTO = new Respuesta<List<TranssbDTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transsb.Lista falló. Skip={Skip} Take={Take}", skip, take);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorInterno<List<TranssbDTO>>("obtener las transferencias"));
            }
        }

        [HttpGet]
        [Route("Cantidad")]
        public async Task<IActionResult> CantidadTotal()
        {
            Respuesta<int> _ResponseDTO = new Respuesta<int>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<int>());
                }

                int a;
                if (RequiresActiveSocioScope(accessContext))
                {
                    var query = await _transsbRepositorio.Consultar(x =>
                        x.Sven == accessContext.ActiveSocioCode || x.Scom == accessContext.ActiveSocioCode);
                    a = query.Count();
                }
                else
                {
                    a = await _transsbRepositorio.CantidadTotal();
                }

                _ResponseDTO = new Respuesta<int>() { Exito = 1, Mensaje = "Exito", List = a };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transsb.CantidadTotal falló.");
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorInterno<int>("obtener la cantidad de transferencias"));
            }
        }
        [HttpGet]
        [Route("LimitadosFiltrados")]
        public async Task<IActionResult> LimitadosFiltrados(int skip, int take, string? expression = null)
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<TranssbDTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? AppendFilter(expression, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : expression;
                var transfers = await _transsbRepositorio.LimitadosFiltrados(skip, take, effectiveExpression);

                var toroIds = transfers
                    .Where(t => t.Torovendido.HasValue)
                    .Select(t => t.Torovendido!.Value)
                    .Distinct()
                    .ToList();

                var toroExtras = await _db.Torosunis
                    .AsNoTracking()
                    .Where(t => toroIds.Contains(t.Id))
                    .ToDictionaryAsync(
                        t => t.Id,
                        t => new
                        {
                            Hba = t.Hba,
                            Tatpart = t.Tatpart,
                            ToroNombre = t.NomDad
                        });


                var resultList = new List<object>(transfers.Count);
                var dtoProps = typeof(TranssbDTO).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var t in transfers)
                {
                    var dto = _mapper.Map<TranssbDTO>(t);

                    var expando = new ExpandoObject() as IDictionary<string, object?>;
                    foreach (var p in dtoProps)
                        expando[p.Name] = p.GetValue(dto);

                    if (t.Torovendido.HasValue && toroExtras.TryGetValue(t.Torovendido.Value, out var te))
                        expando["Toro"] = te;
                    else
                        expando["Toro"] = null;

                    resultList.Add(expando!);
                }


                return Ok(new
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = resultList
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transsb.LimitadosFiltrados falló. Skip={Skip} Take={Take}", skip, take);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorInterno<List<TranssbDTO>>("obtener las transferencias"));
            }
        }


        [HttpGet]
        [Route("LimitadosFiltradosNoInclude")]
        public async Task<IActionResult> LimitadosFiltradosNoInclude(int skip, int take, string? expression = null)
        {
            Respuesta<List<TranssbDTO>> _ResponseDTO = new Respuesta<List<TranssbDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<TranssbDTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? AppendFilter(expression, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : expression;
                var a = await _transsbRepositorio.LimitadosFiltradosNoInclude(skip, take, effectiveExpression);

                var listaFiltrada = _mapper.Map<List<TranssbDTO>>(a);

                _ResponseDTO = new Respuesta<List<TranssbDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transsb.LimitadosFiltradosNoInclude falló. Skip={Skip} Take={Take}", skip, take);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorInterno<List<TranssbDTO>>("obtener las transferencias"));
            }
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            Respuesta<string> _Respuesta = new Respuesta<string>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                Transsb _TranssbEliminar = await _transsbRepositorio.Obtener(u => u.Id == id);
                if (_TranssbEliminar != null)
                {
                    if (!CanAccessTransfer(accessContext, _TranssbEliminar.Sven, _TranssbEliminar.Scom))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                    }

                    bool respuesta = await _transsbRepositorio.Eliminar(_TranssbEliminar);

                    if (!respuesta)
                    {
                        return BadRequest(new Respuesta<string>() { Exito = 0, Mensaje = "No se pudo eliminar el identificador", List = "" });
                    }

                    _Respuesta = new Respuesta<string>() { Exito = 1, Mensaje = "ok", List = "" };
                }
                else
                {
                    return NotFound(new Respuesta<string>() { Exito = 0, Mensaje = "No se encontró el identificador", List = "" });
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transsb.Eliminar falló. TranssbId={TranssbId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorInterno<string>("eliminar la transferencia"));
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] TranssbDTO request)
        {
            Respuesta<TranssbDTO> _Respuesta = new Respuesta<TranssbDTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (!CanAccessTransfer(accessContext, request?.Sven, request?.Scom))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<TranssbDTO>());
                }

                NormalizeTransfer(request);
                await ValidateTransferRequestAsync(request);

                Transsb _Transsb = _mapper.Map<Transsb>(request);
                var strategy = _db.Database.CreateExecutionStrategy();
                Transsb _TranssbCreado = null!;

                await strategy.ExecuteAsync(async () =>
                {
                    await using var transaction = await _db.Database.BeginTransactionAsync();
                    _TranssbCreado = await _transsbRepositorio.Crear(_Transsb);
                    await TransferToroAsync(request);
                    await transaction.CommitAsync();
                });

                if (_TranssbCreado.Id == 0)
                {
                    return BadRequest(new Respuesta<TranssbDTO>() { Exito = 0, Mensaje = "No se pudo crear el identificador" });
                }

                _Respuesta = new Respuesta<TranssbDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<TranssbDTO>(_TranssbCreado) };
                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (TransferenciaRechazadaException ex)
            {
                // Rechazo de negocio (socio inexistente, establecimiento ajeno, toro de otro dueño,
                // mismo establecimiento de origen y destino...). La transaccion no se confirmo:
                // ni la transferencia ni el toro cambiaron. El mensaje es nuestro, no de EF/MariaDB.
                return BadRequest(new Respuesta<TranssbDTO>() { Exito = 0, Mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                // Base, EF o infraestructura: el detalle (p. ej. "Data too long for column 'X'") queda
                // en el log; el cliente recibe un mensaje funcional. La transaccion no se confirmo.
                _logger.LogError(ex, "Transsb.Guardar falló. Sven={Sven} Scom={Scom} Ecod={Ecod} Toro={Toro}",
                    request?.Sven, request?.Scom, request?.Ecod, request?.Torovendido);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorInterno<TranssbDTO>("guardar la transferencia"));
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] TranssbDTO request)
        {
            Respuesta<TranssbDTO> _Respuesta = new Respuesta<TranssbDTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                NormalizeTransfer(request);
                Transsb _Transsb = _mapper.Map<Transsb>(request);
                Transsb _TranssbParaEditar = await _transsbRepositorio.Obtener(u => u.Id == _Transsb.Id);

                if (_TranssbParaEditar != null)
                {
                    var previousTransferState = new Transsb
                    {
                        Id = _TranssbParaEditar.Id,
                        Sven = _TranssbParaEditar.Sven,
                        Scom = _TranssbParaEditar.Scom,
                        Torovendido = _TranssbParaEditar.Torovendido
                    };

                    if (!CanAccessTransfer(accessContext, _TranssbParaEditar.Sven, _TranssbParaEditar.Scom) ||
                        !CanAccessTransfer(accessContext, _Transsb.Sven, _Transsb.Scom))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<TranssbDTO>());
                    }

                    await ValidateTransferRequestAsync(request, previousTransferState);

                    _TranssbParaEditar.NroTrans = _Transsb.NroTrans;
                    _TranssbParaEditar.Fectrans = _Transsb.Fectrans;
                    _TranssbParaEditar.NroOrden = _Transsb.NroOrden;
                    _TranssbParaEditar.Sven = _Transsb.Sven;
                    _TranssbParaEditar.CategSv = _Transsb.CategSv;
                    _TranssbParaEditar.Vnom = _Transsb.Vnom;
                    _TranssbParaEditar.Scom = _Transsb.Scom;
                    _TranssbParaEditar.CategSc = _Transsb.CategSc;
                    _TranssbParaEditar.Cnom = _Transsb.Cnom;
                    _TranssbParaEditar.Ecod = _Transsb.Ecod;
                    _TranssbParaEditar.FchUsu = _Transsb.FchUsu;
                    _TranssbParaEditar.CodUsu = _Transsb.CodUsu;
                    _TranssbParaEditar.Torovendido = _Transsb.Torovendido;
                    var strategy = _db.Database.CreateExecutionStrategy();
                    bool respuesta = false;

                    await strategy.ExecuteAsync(async () =>
                    {
                        await using var transaction = await _db.Database.BeginTransactionAsync();
                        respuesta = await _transsbRepositorio.Editar(_TranssbParaEditar);

                        if (!respuesta)
                        {
                            return;
                        }

                        await TransferToroAsync(request, previousTransferState);
                        await transaction.CommitAsync();
                    });

                    if (!respuesta)
                    {
                        return BadRequest(new Respuesta<TranssbDTO>() { Exito = 0, Mensaje = "No se pudo editar el identificador" });
                    }

                    _Respuesta = new Respuesta<TranssbDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<TranssbDTO>(_TranssbParaEditar) };
                }
                else
                {
                    return NotFound(new Respuesta<TranssbDTO>() { Exito = 0, Mensaje = "No se encontró el identificador" });
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (TransferenciaRechazadaException ex)
            {
                // Rechazo de negocio (socio inexistente, establecimiento ajeno, toro de otro dueño,
                // mismo establecimiento de origen y destino...). La transaccion no se confirmo:
                // ni la transferencia ni el toro cambiaron. El mensaje es nuestro, no de EF/MariaDB.
                return BadRequest(new Respuesta<TranssbDTO>() { Exito = 0, Mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                // Base, EF o infraestructura: el detalle (p. ej. "Data too long for column 'X'") queda
                // en el log; el cliente recibe un mensaje funcional. La transaccion no se confirmo.
                _logger.LogError(ex, "Transsb.Editar falló. TranssbId={TranssbId} Sven={Sven} Scom={Scom} Ecod={Ecod} Toro={Toro}",
                    request?.Id, request?.Sven, request?.Scom, request?.Ecod, request?.Torovendido);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorInterno<TranssbDTO>("editar la transferencia"));
            }
        }

        /// <summary>Ancho de TRANSSB.NRO_TRANS (varchar(6), ver hereford_prContext).</summary>
        private const int NroTransMaxLength = 6;

        private static bool RequiresActiveSocioScope(UserSocioAccessContext accessContext)
            => accessContext.IsSocioUser && !accessContext.IsPrivilegedUser;

        /// <summary>
        /// Respuesta de un 500. El texto es fijo y funcional: no se usa <c>ex.Message</c> ni la excepcion
        /// raiz porque traen nombres de columnas, SQL o mensajes del motor ("Data too long for column
        /// 'ECOD'", FK violada, timeouts...). El detalle real se loguea en el catch que llama a esto.
        /// </summary>
        private static Respuesta<T> ErrorInterno<T>(string accionFallida)
            => new Respuesta<T>
            {
                Exito = 0,
                Mensaje = $"No se pudo {accionFallida} por un error interno del servidor. " +
                          "Intentá nuevamente y, si el problema persiste, contactá al administrador."
            };

        /// <summary>
        /// Rechazo de negocio de una transferencia (dato faltante, socio/establecimiento/toro que no
        /// corresponde). Es el UNICO tipo que termina en un 400 con su mensaje: una
        /// InvalidOperationException cualquiera (EF las usa para errores internos de tracking,
        /// conexion, etc.) cae en el catch generico y se responde como 500 sin exponer el texto.
        /// </summary>
        private sealed class TransferenciaRechazadaException : InvalidOperationException
        {
            public TransferenciaRechazadaException(string message) : base(message)
            {
            }
        }

        private static void NormalizeTransfer(TranssbDTO? request)
        {
            if (request is null)
            {
                return;
            }

            request.NroTrans = request.NroTrans?.Trim() ?? string.Empty;
            request.NroOrden = request.NroOrden?.Trim();
            request.Sven = request.Sven?.Trim();
            request.Scom = request.Scom?.Trim();
            request.Vnom = request.Vnom?.Trim();
            request.Cnom = request.Cnom?.Trim();
            request.Ecod = request.Ecod?.Trim();
        }

        private async Task ValidateTransferRequestAsync(TranssbDTO? request, Transsb? existingTransfer = null)
        {
            if (request is null)
            {
                throw new TransferenciaRechazadaException("No se recibieron datos de la transferencia.");
            }

            if (string.IsNullOrWhiteSpace(request.Sven))
            {
                throw new TransferenciaRechazadaException("Debe seleccionar un socio vendedor.");
            }

            if (string.IsNullOrWhiteSpace(request.Scom))
            {
                throw new TransferenciaRechazadaException("Debe seleccionar un socio comprador.");
            }

            if (!request.Torovendido.HasValue || request.Torovendido.Value <= 0)
            {
                throw new TransferenciaRechazadaException("Debe seleccionar un toro válido.");
            }

            // TRANSSB.NRO_TRANS es varchar(6): se rechaza con un mensaje claro en vez de dejar que la
            // base conteste "Data too long for column".
            if (request.NroTrans.Length > NroTransMaxLength)
            {
                throw new TransferenciaRechazadaException($"El número de certificado no puede superar los {NroTransMaxLength} caracteres.");
            }

            // Misma razon social: solo se admite como movimiento entre establecimientos propios
            // (el toro cambia de campo sin cambiar de dueño). Se valida mas abajo, cuando ya se
            // conocen el establecimiento destino y el toro.
            var mismoSocio = string.Equals(request.Sven, request.Scom, StringComparison.OrdinalIgnoreCase);

            var vendedorExiste = await _db.Socios.AnyAsync(x => x.Scod == request.Sven);
            if (!vendedorExiste)
            {
                throw new TransferenciaRechazadaException("El socio vendedor no existe.");
            }

            var compradorExiste = await _db.Socios.AnyAsync(x => x.Scod == request.Scom);
            if (!compradorExiste)
            {
                throw new TransferenciaRechazadaException("El socio comprador no existe.");
            }

            if (string.IsNullOrWhiteSpace(request.Ecod))
            {
                throw new TransferenciaRechazadaException("Debe seleccionar un establecimiento válido.");
            }

            var establecimientoDestino = await _db.Estables
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Ecod == request.Ecod && x.Codsoc == request.Scom);

            if (establecimientoDestino is null)
            {
                throw new TransferenciaRechazadaException("El establecimiento seleccionado no pertenece al socio comprador.");
            }

            var toro = await _db.Torosunis
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Torovendido.Value);

            if (toro is null)
            {
                throw new TransferenciaRechazadaException("El toro seleccionado no existe.");
            }

            if (mismoSocio)
            {
                var mismaTransferencia = existingTransfer is not null
                    && existingTransfer.Torovendido == request.Torovendido
                    && string.Equals(existingTransfer.Ecod?.Trim(), request.Ecod, StringComparison.OrdinalIgnoreCase);

                if (!mismaTransferencia && toro.EstablecimientoId.HasValue && toro.EstablecimientoId.Value == establecimientoDestino.Id)
                {
                    throw new TransferenciaRechazadaException(
                        "El socio comprador es el mismo que el vendedor y el toro ya esta en ese establecimiento. " +
                        "Para un movimiento interno elegi un establecimiento distinto del actual.");
                }

                if (!mismaTransferencia && !toro.EstablecimientoId.HasValue && string.Equals(toro.Estcod?.Trim(), request.Ecod, StringComparison.OrdinalIgnoreCase))
                {
                    throw new TransferenciaRechazadaException(
                        "El socio comprador es el mismo que el vendedor y el toro ya esta en ese establecimiento. " +
                        "Para un movimiento interno elegi un establecimiento distinto del actual.");
                }
            }

            var ownerCode = toro.Criador?.Trim();
            var ownerMatchesSeller = string.Equals(ownerCode, request.Sven, StringComparison.OrdinalIgnoreCase);
            var ownerMatchesPreviousBuyer = existingTransfer is not null &&
                string.Equals(ownerCode, existingTransfer.Scom, StringComparison.OrdinalIgnoreCase) &&
                existingTransfer.Torovendido == request.Torovendido;

            if (!ownerMatchesSeller && !ownerMatchesPreviousBuyer)
            {
                throw new TransferenciaRechazadaException("El toro seleccionado ya no pertenece al socio vendedor.");
            }
        }

        private async Task TransferToroAsync(TranssbDTO request, Transsb? existingTransfer = null)
        {
            if (!request.Torovendido.HasValue)
            {
                throw new TransferenciaRechazadaException("La transferencia no tiene un toro asociado.");
            }

            var toro = await _db.Torosunis.FirstOrDefaultAsync(x => x.Id == request.Torovendido.Value);
            if (toro is null)
            {
                throw new TransferenciaRechazadaException("No se encontró el toro a transferir.");
            }

            var ownerCode = toro.Criador?.Trim();
            var ownerMatchesSeller = string.Equals(ownerCode, request.Sven, StringComparison.OrdinalIgnoreCase);
            var ownerMatchesPreviousBuyer = existingTransfer is not null &&
                string.Equals(ownerCode, existingTransfer.Scom, StringComparison.OrdinalIgnoreCase) &&
                existingTransfer.Torovendido == request.Torovendido;

            if (!ownerMatchesSeller && !ownerMatchesPreviousBuyer)
            {
                throw new TransferenciaRechazadaException("El toro ya no pertenece al socio vendedor.");
            }

            Estable? establecimiento = null;
            if (!string.IsNullOrWhiteSpace(request.Ecod))
            {
                establecimiento = await _db.Estables.FirstOrDefaultAsync(x => x.Ecod == request.Ecod && x.Codsoc == request.Scom);

                if (establecimiento is null)
                {
                    throw new TransferenciaRechazadaException("No se encontró el establecimiento seleccionado para el socio comprador.");
                }
            }

            toro.Criador = request.Scom;
            toro.Estcod = establecimiento?.Ecod;
            toro.EstablecimientoId = establecimiento?.Id;
            await _db.SaveChangesAsync();
        }

        private static bool CanAccessTransfer(UserSocioAccessContext accessContext, string? sellerCode, string? buyerCode)
        {
            if (!RequiresActiveSocioScope(accessContext))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
            {
                return false;
            }

            return string.Equals(sellerCode, accessContext.ActiveSocioCode, StringComparison.OrdinalIgnoreCase)
                || string.Equals(buyerCode, accessContext.ActiveSocioCode, StringComparison.OrdinalIgnoreCase);
        }

        private static string BuildActiveSocioFilter(string socioCode)
            => $"(Sven == \"{EscapeValue(socioCode)}\" || Scom == \"{EscapeValue(socioCode)}\")";

        private static string AppendFilter(string? expression, string enforcedFilter)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return enforcedFilter;
            }

            return $"({expression}) && ({enforcedFilter})";
        }

        private static string EscapeValue(string value)
            => value.Replace("\\", "\\\\").Replace("\"", "\\\"");

        private static Respuesta<T> BuildForbiddenResponse<T>()
            => new Respuesta<T>
            {
                Exito = 0,
                Mensaje = "No tenes permisos para operar sobre otra razon social."
            };
    }
}
