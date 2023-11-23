using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class Resin8Servicio : IResin8Servicio
    {
        private readonly HttpClient _http;
        public Resin8Servicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<Resin8DTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Resin8DTO>>>($"api/Resin8/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/Resin8/cantidad");
            return result;
        }
        public async Task<Respuesta<List<Resin8DTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Resin8DTO>>>($"api/Resin8/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Resin8/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<Resin8DTO>> Crear(Resin8DTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/Resin8/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Resin8DTO>>();
            return response!;
        }

        public async Task<bool> Editar(Resin8DTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/Resin8/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Resin8DTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<Resin8DTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<Resin8DTO>>($"api/Resin8/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
