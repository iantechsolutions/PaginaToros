using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class CertifsemanServicio : ICertifsemanServicio
    {
        private readonly HttpClient _http;
        public CertifsemanServicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<CertifsemanDTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<CertifsemanDTO>>>($"api/Certifseman/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>("api/Certifseman/cantidad");
            return result;
        }
        public async Task<Respuesta<List<CertifsemanDTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var encoded = Uri.EscapeDataString(filter ?? string.Empty);
            var result = await _http.GetFromJsonAsync<Respuesta<List<CertifsemanDTO>>>($"api/Certifseman/LimitadosFiltrados?skip={skip}&take={take}&expression={encoded}");
            return result!;
        }
        public async Task<Respuesta<List<CertifsemanDTO>>> LimitadosFiltradosNoInclude(int skip, int take, string filter)
        {
            var encoded = Uri.EscapeDataString(filter ?? string.Empty);
            var result = await _http.GetFromJsonAsync<Respuesta<List<CertifsemanDTO>>>($"api/Certifseman/LimitadosFiltradosNoInclude?skip={skip}&take={take}&expression={encoded}");
            return result!;
        }

        public async Task<Respuesta<List<CertifsemanDTO>>> GetBySocioId(int socioId)
        {
            string filter = $"Socio.Id={socioId}";
            var encoded = Uri.EscapeDataString(filter);
            var result = await _http.GetFromJsonAsync<Respuesta<List<CertifsemanDTO>>>($"api/Certifseman/LimitadosFiltrados?skip=0&take=0&expression={encoded}");
            return result;
        }
        public async Task<Respuesta<CertifsemanDTO>> UpdateNrDosi(int id, int nrDosi)
        {
            var url = $"api/Certifseman/{id}/nr-dosi";
            var res = await _http.PutAsJsonAsync(url, nrDosi);

            var payload = await res.Content.ReadFromJsonAsync<Respuesta<CertifsemanDTO>>();

            return payload ?? new Respuesta<CertifsemanDTO>
            {
                Exito = 0,
                Mensaje = $"Error HTTP {(int)res.StatusCode} al actualizar NrDosi"
            };
        }



        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Certifseman/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<CertifsemanDTO>> Crear(CertifsemanDTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/Certifseman/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<CertifsemanDTO>>();
            return response!;
        }

        public async Task<bool> Editar(CertifsemanDTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/Certifseman/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<CertifsemanDTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<CertifsemanDTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<CertifsemanDTO>>($"api/Certifseman/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
