using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;
using System.Text;

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
            var response = await _http.GetAsync("api/Socio/cantidad");
            if (!response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                return new Respuesta<int> { Exito = 0, Mensaje = $"Server error: {(int)response.StatusCode} - {text}", List = 0 };
            }
            var result = await response.Content.ReadFromJsonAsync<Respuesta<int>>();
            return result!;
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

        public async Task<Respuesta<List<SocioDTO>>> LimitadosFiltradoTodos(int skip, int take, string expression)
        {
            var url = new StringBuilder($"api/Socio/LimitadosFiltradoTodos?skip={skip}&take={take}");
            if (!string.IsNullOrEmpty(expression))
            {
                url.Append($"&expression={Uri.EscapeDataString(expression)}");
            }


            Console.WriteLine(url);


            var response = await _http.GetAsync(url.ToString());

            if (!response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException(
                    $"Llamada a {url} devolvió {(int)response.StatusCode}:\n{text}");
            }

            var dto = await response.Content
                                    .ReadFromJsonAsync<Respuesta<List<SocioDTO>>>();
            return dto!;
        }

        public async Task<Respuesta<SocioDTO>> Reserve()
        {
            var response = await _http.PostAsync("api/Socio/Reserve", null);
            if (!response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Reserve failed: {(int)response.StatusCode}: {text}");
            }

            var dto = await response.Content.ReadFromJsonAsync<Respuesta<SocioDTO>>();
            return dto!;
        }

        public async Task<Respuesta<bool>> ExistsCodpos2(string codpos2, int? excludeId = null)
        {
            var url = new StringBuilder($"api/Socio/ExistsCodpos2?codpos2={Uri.EscapeDataString(codpos2)}");
            if (excludeId.HasValue) url.Append($"&excludeId={excludeId.Value}");

            var response = await _http.GetAsync(url.ToString());
            if (!response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"ExistsCodpos2 failed: {(int)response.StatusCode}: {text}");
            }

            var dto = await response.Content.ReadFromJsonAsync<Respuesta<bool>>();
            return dto!;
        }
    }
}
