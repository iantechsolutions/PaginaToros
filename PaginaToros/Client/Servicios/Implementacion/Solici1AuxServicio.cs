using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class Solici1AuxServicio : ISolici1AuxServicio
    {
        private readonly HttpClient _http;
        public Solici1AuxServicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<Solici1AuxDTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Solici1AuxDTO>>>($"api/Solici1Aux/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/Solici1Aux/cantidad");
            return result;
        }
        public async Task<Respuesta<List<Solici1AuxDTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Solici1AuxDTO>>>($"api/Solici1Aux/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }
        public async Task<Respuesta<List<Solici1AuxDTO>>> LimitadosFiltradosNoInclude(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<Solici1AuxDTO>>>($"api/Solici1Aux/LimitadosFiltradosNoInclude?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<Respuesta<List<Solici1AuxDTO>>> GetBySocioId(int socioId)
        {
            string filter = $"Establecimiento.Socio.Id={socioId}";
            var result = await _http.GetFromJsonAsync<Respuesta<List<Solici1AuxDTO>>>($"api/Solici1Aux/LimitadosFiltrados?skip=0&take=0&expression={filter}");
            return result;
        }

        public async Task<Respuesta<List<Solici1AuxDTO>>> GetBySolId(int SoliId)
        {
            string filter = $"IdSoli={SoliId}";
            var result = await _http.GetFromJsonAsync<Respuesta<List<Solici1AuxDTO>>>($"api/Solici1Aux/LimitadosFiltradosNoInclude?skip=0&take=0&expression={filter}");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Solici1Aux/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<Solici1AuxDTO>> Crear(Solici1AuxDTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/Solici1Aux/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Solici1AuxDTO>>();
            return response!;
        }

        public async Task<bool> Editar(Solici1AuxDTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/Solici1Aux/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<Solici1AuxDTO>>();

            return response!.Exito == 1;
        }

        public async Task<Respuesta<Solici1AuxDTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<Solici1AuxDTO>>($"api/Solici1Aux/filtrar?categoriaItem={descripcion}");
            return result!;
        }
    }
}
