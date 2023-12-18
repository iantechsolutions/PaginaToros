using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class Desepla1Servicio : IDesepla1Servicio
    {
        private readonly HttpClient _http;
        public Desepla1Servicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<Desepla1DTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Desepla1DTO>>>($"api/Desepla1/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/Desepla1/cantidad");
            return result;
        }
        public async Task<Respuesta<List<Desepla1DTO>>> LimitadosFiltrados(int skip, int take, string? filter = null)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Desepla1DTO>>>($"api/Desepla1/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }
        public async Task<Respuesta<List<Desepla1DTO>>> LimitadosFiltradosNoInclude(int skip, int take, string? filter = null)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Desepla1DTO>>>($"api/Desepla1/LimitadosFiltradosNoInclude?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Desepla1/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<Desepla1DTO>> Crear(Desepla1DTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/Desepla1/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Desepla1DTO>>();
            return response!;
        }

        public async Task<bool> Editar(Desepla1DTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/Desepla1/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Desepla1DTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<Desepla1DTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<Desepla1DTO>>($"api/Desepla1/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
