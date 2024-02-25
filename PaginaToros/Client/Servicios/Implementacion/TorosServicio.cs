using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class TorosServicio : ITorosServicio
    {
        private readonly HttpClient _http;
        public TorosServicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<TorosuniDTO>>> Lista(int skip,int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<TorosuniDTO>>>($"api/toros/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/toros/cantidad");
            return result;
        }
        public async Task<Respuesta<List<TorosuniDTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var url = $"api/toros/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}";
            var result = await _http.GetFromJsonAsync<Respuesta<List<TorosuniDTO>>>(url);
            return result;
        }
        public async Task<Respuesta<List<TorosuniDTO>>> LimitadosFiltradosNoInclude(int skip, int take, string filter)
        {
            var url = $"api/toros/LimitadosFiltradosNoInclude?skip={skip}&take={take}&expression={filter}";
            var result = await _http.GetFromJsonAsync<Respuesta<List<TorosuniDTO>>>(url);
            return result;
        }
        public async Task<Respuesta<List<TorosuniDTO>>> GetBySocioId(int socioId)
        {
            string filter = $"Socio.Id=={socioId}";
            var url = $"api/toros/LimitadosFiltradosNoInclude?skip=0&take=0&expression={filter}";
            var result = await _http.GetFromJsonAsync<Respuesta<List<TorosuniDTO>>>(url);
            return result;
        }
        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/toros/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito==1;
        }

        public async Task<Respuesta<TorosuniDTO>> Crear(TorosuniDTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/toros/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<TorosuniDTO>>();
            return response!;
        }

        public async Task<bool> Editar(TorosuniDTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/toros/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<TorosuniDTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<TorosuniDTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<TorosuniDTO>>($"api/toros/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
