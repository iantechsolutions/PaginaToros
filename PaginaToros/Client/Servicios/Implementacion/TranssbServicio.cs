using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class TranssbServicio : ITranssbServicio
    {
        private readonly HttpClient _http;
        public TranssbServicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<TranssbDTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<TranssbDTO>>>($"api/Transsb/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/Transsb/cantidad");
            return result;
        }
        public async Task<Respuesta<List<TranssbDTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<TranssbDTO>>>($"api/Transsb/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }
        public async Task<Respuesta<List<TranssbDTO>>> LimitadosFiltradosNoInclude(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<TranssbDTO>>>($"api/Transsb/LimitadosFiltradosNoInclude?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<Respuesta<List<TranssbDTO>>> GetBySocioCod(string socioCod)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<TranssbDTO>>>($"api/Transsb/LimitadosFiltrados?skip=0&take=0&expression=");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Transsb/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<TranssbDTO>> Crear(TranssbDTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/Transsb/Guardar", entidad);
            return await LeerRespuestaAsync(result, "No se pudo guardar la transferencia");
        }

        public async Task<bool> Editar(TranssbDTO entidad)
        {
            var response = await EditarConRespuesta(entidad);
            return response.Exito == 1;
        }

        public async Task<Respuesta<TranssbDTO>> EditarConRespuesta(TranssbDTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/Transsb/Editar", entidad);
            return await LeerRespuestaAsync(result, "No se pudo editar la transferencia");
        }

        /// <summary>
        /// Un status HTTP de error NUNCA se interpreta como exito, diga lo que diga el cuerpo. Antes se
        /// leia el JSON sin mirar el status y se comparaba Exito == 1: un 500 del servidor con
        /// Exito = 1 en el cuerpo (contrato viejo del controller) se mostraba como transferencia hecha.
        /// </summary>
        private static async Task<Respuesta<TranssbDTO>> LeerRespuestaAsync(HttpResponseMessage result, string accionFallida)
        {
            Respuesta<TranssbDTO>? response = null;
            try
            {
                response = await result.Content.ReadFromJsonAsync<Respuesta<TranssbDTO>>();
            }
            catch
            {
                // El cuerpo no era JSON (p. ej. un 500 con HTML o un 401 vacio): se informa el status.
            }

            if (response == null)
            {
                return new Respuesta<TranssbDTO>
                {
                    Exito = 0,
                    Mensaje = $"{accionFallida} (HTTP {(int)result.StatusCode})."
                };
            }

            if (!result.IsSuccessStatusCode && response.Exito == 1)
            {
                response.Exito = 0;
                response.Mensaje = string.IsNullOrWhiteSpace(response.Mensaje)
                    ? $"{accionFallida} (HTTP {(int)result.StatusCode})."
                    : response.Mensaje;
            }

            return response;
        }

        public async Task<Respuesta<TranssbDTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<TranssbDTO>>($"api/Transsb/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
