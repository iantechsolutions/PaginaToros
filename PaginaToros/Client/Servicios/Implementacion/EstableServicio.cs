using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class EstableServicio : IEstableServicio
    {
        private readonly HttpClient _http;
        public EstableServicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<EstableDTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<EstableDTO>>>($"api/establecimiento/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/establecimiento/cantidad");
            return result;
        }
        public async Task<Respuesta<List<EstableDTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<EstableDTO>>>($"api/establecimiento/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/establecimiento/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<EstableDTO>> Crear(EstableDTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/establecimiento/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<EstableDTO>>();
            return response!;
        }

        public async Task<bool> Editar(EstableDTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/establecimiento/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<EstableDTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<EstableDTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<EstableDTO>>($"api/establecimiento/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
