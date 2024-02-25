using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class TranssbServicio : ITranssbServicio
    {
        private readonly HttpClient _http;
        public TranssbServicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<TranssbDTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<TranssbDTO>>>($"api/Transsb/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/Transsb/cantidad");
            return result;
        }
        public async Task<Respuesta<List<TranssbDTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<TranssbDTO>>>($"api/Transsb/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }
        public async Task<Respuesta<List<TranssbDTO>>> LimitadosFiltradosNoInclude(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<TranssbDTO>>>($"api/Transsb/LimitadosFiltradosNoInclude?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<Respuesta<List<TranssbDTO>>> GetBySocioCod(string socioCod)
        {
            string filter = $"Sven = \"{socioCod}\" || Scom = \"{socioCod}\"";
            var result = await _http.GetFromJsonAsync<Respuesta<List<TranssbDTO>>>($"api/Transsb/LimitadosFiltrados?skip=0&take=0&expression={filter}");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Transsb/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<TranssbDTO>> Crear(TranssbDTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/Transsb/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<TranssbDTO>>();
            return response!;
        }

        public async Task<bool> Editar(TranssbDTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/Transsb/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<TranssbDTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<TranssbDTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<TranssbDTO>>($"api/Transsb/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
