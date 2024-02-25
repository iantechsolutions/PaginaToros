using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class FutcontrolServicio : IFutcontrolServicio
    {
        private readonly HttpClient _http;
        public FutcontrolServicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<FutcontrolDTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<FutcontrolDTO>>>($"api/Futcontrol/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/Futcontrol/cantidad");
            return result;
        }
        public async Task<Respuesta<List<FutcontrolDTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<FutcontrolDTO>>>($"api/Futcontrol/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<Respuesta<List<FutcontrolDTO>>> GetBySocioCod(string socioCod)
        { 
            string filter = $"Sven = \"{socioCod}\" || Scom = \"{socioCod}\"";
            var result = await _http.GetFromJsonAsync<Respuesta<List<FutcontrolDTO>>>($"api/Futcontrol/LimitadosFiltrados?skip=0&take=0&expression={filter}");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Futcontrol/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<FutcontrolDTO>> Crear(FutcontrolDTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/Futcontrol/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<FutcontrolDTO>>();
            return response!;
        }

        public async Task<bool> Editar(FutcontrolDTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/Futcontrol/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<FutcontrolDTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<FutcontrolDTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<FutcontrolDTO>>($"api/Futcontrol/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
