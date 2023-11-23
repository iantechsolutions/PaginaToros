using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class Resin2Servicio : IResin2Servicio
    {
        private readonly HttpClient _http;
        public Resin2Servicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<Resin2DTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Resin2DTO>>>($"api/Resin2/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/Resin2/cantidad");
            return result;
        }
        public async Task<Respuesta<List<Resin2DTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Resin2DTO>>>($"api/Resin2/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Resin2/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<Resin2DTO>> Crear(Resin2DTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/Resin2/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Resin2DTO>>();
            return response!;
        }

        public async Task<bool> Editar(Resin2DTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/Resin2/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Resin2DTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<Resin2DTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<Resin2DTO>>($"api/Resin2/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
