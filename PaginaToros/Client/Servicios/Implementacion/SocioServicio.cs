using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class SocioServicio : ISocioServicio
    {
        private readonly HttpClient _http;
        public SocioServicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<SocioDTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<SocioDTO>>>($"api/Socio/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/Socio/cantidad");
            return result;
        }
        public async Task<Respuesta<List<SocioDTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<SocioDTO>>>($"api/Socio/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Socio/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<SocioDTO>> Crear(SocioDTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/Socio/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<SocioDTO>>();
            return response!;
        }

        public async Task<bool> Editar(SocioDTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/Socio/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<SocioDTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<SocioDTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<SocioDTO>>($"api/Socio/filtrar?categoriaItem={descripcion}");
            return result!;
        }

        public async Task<Respuesta<List<SocioDTO>>> LimitadosFiltradoTodos(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<SocioDTO>>>($"api/Socio/LimitadosFiltradoTodos?skip={skip}&take={take}&expression={filter}");
            return result;
        }
    }
}
