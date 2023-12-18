using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class Resin1Servicio : IResin1Servicio
    {
        private readonly HttpClient _http;
        public Resin1Servicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<Resin1DTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Resin1DTO>>>($"api/Resin1/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/Resin1/cantidad");
            return result;
        }
        public async Task<Respuesta<List<Resin1DTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Resin1DTO>>>($"api/Resin1/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }
        public async Task<Respuesta<List<Resin1DTO>>> LimitadosFiltradosNoInclude(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Resin1DTO>>>($"api/Resin1/LimitadosFiltradosNoInclude?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Resin1/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<Resin1DTO>> Crear(Resin1DTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/Resin1/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Resin1DTO>>();
            return response!;
        }

        public async Task<bool> Editar(Resin1DTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/Resin1/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Resin1DTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<Resin1DTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<Resin1DTO>>($"api/Resin1/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
