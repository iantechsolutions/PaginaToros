using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class InspectServicio : IInspectServicio
    {
        private readonly HttpClient _http;
        public InspectServicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<InspectDTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<InspectDTO>>>($"api/Inspect/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/Inspect/cantidad");
            return result;
        }
        public async Task<Respuesta<List<InspectDTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<InspectDTO>>>($"api/Inspect/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Inspect/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<InspectDTO>> Crear(InspectDTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/Inspect/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<InspectDTO>>();
            return response!;
        }

        public async Task<bool> Editar(InspectDTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/Inspect/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<InspectDTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<InspectDTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<InspectDTO>>($"api/Inspect/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
