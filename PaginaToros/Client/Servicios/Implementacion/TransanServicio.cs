using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class TransanServicio : ITransanServicio
    {
        private readonly HttpClient _http;
        public TransanServicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<TransanDTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<TransanDTO>>>($"api/Transan/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/Transan/cantidad");
            return result;
        }
        public async Task<Respuesta<List<TransanDTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<TransanDTO>>>($"api/Transan/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Transan/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<TransanDTO>> Crear(TransanDTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/Transan/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<TransanDTO>>();
            return response!;
        }

        public async Task<bool> Editar(TransanDTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/Transan/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<TransanDTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<TransanDTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<TransanDTO>>($"api/Transan/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
