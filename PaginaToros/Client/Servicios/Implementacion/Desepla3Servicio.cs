using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class Desepla3Servicio : IDesepla3Servicio
    {
        private readonly HttpClient _http;
        public Desepla3Servicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<Desepla3DTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Desepla3DTO>>>($"api/Desepla3/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/Desepla3/cantidad");
            return result;
        }
        public async Task<Respuesta<List<Desepla3DTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Desepla3DTO>>>($"api/Desepla3/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Desepla3/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<Desepla3DTO>> Crear(Desepla3DTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/Desepla3/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Desepla3DTO>>();
            return response!;
        }

        public async Task<bool> Editar(Desepla3DTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/Desepla3/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Desepla3DTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<Desepla3DTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<Desepla3DTO>>($"api/Desepla3/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
