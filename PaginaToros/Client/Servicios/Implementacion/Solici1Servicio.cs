using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class Solici1Servicio : ISolici1Servicio
    {
        private readonly HttpClient _http;
        public Solici1Servicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<Solici1DTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Solici1DTO>>>($"api/Solici1/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/Solici1/cantidad");
            return result;
        }
        public async Task<Respuesta<List<Solici1DTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Solici1DTO>>>($"api/Solici1/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }
        public async Task<Respuesta<List<Solici1DTO>>> LimitadosFiltradosNoInclude(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Solici1DTO>>>($"api/Solici1/LimitadosFiltradosNoInclude?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<Respuesta<List<Solici1DTO>>> GetBySocioId(int socioId)
        {
            string filter = $"Establecimiento.Socio.Id={socioId}";
            var result = await _http.GetFromJsonAsync<Respuesta<List<Solici1DTO>>>($"api/Solici1/LimitadosFiltrados?skip=0&take=0&expression={filter}");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Solici1/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<Solici1DTO>> Crear(Solici1DTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/Solici1/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Solici1DTO>>();
            return response!;
        }

        public async Task<bool> Editar(Solici1DTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/Solici1/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Solici1DTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<Solici1DTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<Solici1DTO>>($"api/Solici1/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
