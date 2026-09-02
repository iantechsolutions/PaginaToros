using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class CentrosiumServicio : ICentrosiumServicio
    {
        private readonly HttpClient _http;
        public CentrosiumServicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<CentrosiumDTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<CentrosiumDTO>>>($"api/Centrosium/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/Centrosium/cantidad");
            return result;
        }
        public async Task<Respuesta<List<CentrosiumDTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<CentrosiumDTO>>>($"api/Centrosium/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Centrosium/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<CentrosiumDTO>> Crear(CentrosiumDTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/Centrosium/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<CentrosiumDTO>>();
            return response!;
        }

        public async Task<bool> Editar(CentrosiumDTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/Centrosium/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<CentrosiumDTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<CentroRepairResult>> RepararNrocen(bool aplicar)
        {
            var result = await _http.PostAsync($"api/Centrosium/RepararNrocen?aplicar={aplicar.ToString().ToLowerInvariant()}", null);

            try
            {
                var payload = await result.Content.ReadFromJsonAsync<Respuesta<CentroRepairResult>>();
                if (payload != null)
                {
                    return payload;
                }
            }
            catch
            {
                // Cae al error consistente de abajo.
            }

            return new Respuesta<CentroRepairResult>
            {
                Exito = 0,
                Mensaje = $"Error HTTP {(int)result.StatusCode} al reparar los centros.",
                List = new CentroRepairResult()
            };
        }

        public async Task<Respuesta<CentrosiumDTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<CentrosiumDTO>>($"api/Centrosium/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
