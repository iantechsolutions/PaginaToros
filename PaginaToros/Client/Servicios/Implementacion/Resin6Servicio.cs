using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class Resin6Servicio : IResin6Servicio
    {
        private readonly HttpClient _http;
        public Resin6Servicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<Resin6DTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Resin6DTO>>>($"api/Resin6/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/Resin6/cantidad");
            return result;
        }
        public async Task<Respuesta<List<Resin6DTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Resin6DTO>>>($"api/Resin6/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Resin6/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<Resin6DTO>> Crear(Resin6DTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/Resin6/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Resin6DTO>>();
            return response!;
        }

        public async Task<bool> Editar(Resin6DTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/Resin6/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Resin6DTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<Resin6DTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<Resin6DTO>>($"api/Resin6/filtrar?categoriaItem={descripcion}");
            return result!;
        }

        public async Task<Respuesta<List<Resin6DTO>>> ObtenerFechas(long fecha1, long fecha2)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Resin6DTO>>>($"api/Resin6/ObtenerFechas?fecha1={fecha1}&fecha2={fecha2}");
            return result!;
        }
    }
}
