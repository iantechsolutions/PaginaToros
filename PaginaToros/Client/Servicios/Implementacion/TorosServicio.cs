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
        public async Task<Respuesta<List<TorosuniDTO>>> Lista()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<TorosuniDTO>>>("api/categoria/lista");
            return result!;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/categoria/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito==1;
        }

        public async Task<Respuesta<TorosuniDTO>> Crear(TorosuniDTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/categoria/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<TorosuniDTO>>();
            return response!;
        }

        public async Task<bool> Editar(TorosuniDTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/categoria/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<TorosuniDTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<TorosuniDTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<TorosuniDTO>>($"api/categoria/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
