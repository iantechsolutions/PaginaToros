using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class Resin3Servicio : IResin3Servicio
    {
        private readonly HttpClient _http;
        public Resin3Servicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<Resin3DTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Resin3DTO>>>($"api/Resin3/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/Resin3/cantidad");
            return result;
        }
        public async Task<Respuesta<List<Resin3DTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Resin3DTO>>>($"api/Resin3/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Resin3/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<Resin3DTO>> Crear(Resin3DTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/Resin3/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Resin3DTO>>();
            return response!;
        }

        public async Task<bool> Editar(Resin3DTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/Resin3/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Resin3DTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<Resin3DTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<Resin3DTO>>($"api/Resin3/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
