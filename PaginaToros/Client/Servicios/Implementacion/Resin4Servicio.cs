using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class Resin4Servicio : IResin4Servicio
    {
        private readonly HttpClient _http;
        public Resin4Servicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<Resin4DTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Resin4DTO>>>($"api/Resin4/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/Resin4/cantidad");
            return result;
        }
        public async Task<Respuesta<List<Resin4DTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Resin4DTO>>>($"api/Resin4/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Resin4/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<Resin4DTO>> Crear(Resin4DTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/Resin4/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Resin4DTO>>();
            return response!;
        }

        public async Task<bool> Editar(Resin4DTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/Resin4/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Resin4DTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<Resin4DTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<Resin4DTO>>($"api/Resin4/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
