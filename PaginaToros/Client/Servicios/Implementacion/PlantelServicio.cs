using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class PlantelServicio : IPlantelServicio
    {
        private readonly HttpClient _http;
        public PlantelServicio(HttpClient http)
        {
            _http = http;
        }
        public async Task<Respuesta<List<PlantelDTO>>> Lista(int skip, int take)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<PlantelDTO>>>($"api/plantel/lista?skip={skip}&take={take}");
            return result!;
        }

        public async Task<Respuesta<int>> CantidadTotal()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<int>>($"api/plantel/cantidad");
            return result;
        }
        public async Task<Respuesta<List<PlantelDTO>>> LimitadosFiltrados(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<PlantelDTO>>>($"api/plantel/LimitadosFiltrados?skip={skip}&take={take}&expression={filter}");
            return result;
        }
        public async Task<Respuesta<List<PlantelDTO>>> LimitadosFiltradosNoInclude(int skip, int take, string filter)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<PlantelDTO>>>($"api/plantel/LimitadosFiltradosNoInclude?skip={skip}&take={take}&expression={filter}");
            return result;
        }

        public async Task<Respuesta<List<PlantelDTO>>> GetBySocioId(int socioId)
        {
            string filter = $"Socio.Id == {socioId}";
            var result = await _http.GetFromJsonAsync<Respuesta<List<PlantelDTO>>>($"api/plantel/LimitadosFiltradosNoInclude?skip=0&take=0&expression={filter}");
            return result;
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/plantel/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<Respuesta<string>>();
            return response!.Exito == 1;
        }

        public async Task<Respuesta<PlantelDTO>> Crear(PlantelDTO entidad)
        {
            var result = await _http.PostAsJsonAsync("api/plantel/Guardar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<PlantelDTO>>();
            return response!;
        }

        public async Task<bool> Editar(PlantelDTO entidad)
        {
            var result = await _http.PutAsJsonAsync("api/plantel/Editar", entidad);
            var response = await result.Content.ReadFromJsonAsync<Respuesta<PlantelDTO>>();

            return response!.Exito == 1;
        }
        public async Task<Respuesta<List<PlantelDTO>>> ObtenerPorRangoFechas(DateTime desde, DateTime hasta)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<PlantelDTO>>>(
                $"api/plantel/ObtenerPorRangoFechas?desde={desde:yyyy-MM-dd}&hasta={hasta:yyyy-MM-dd}");
            return result!;
        }

        public async Task<Respuesta<PlantelDTO>> Filtrar(string descripcion)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<PlantelDTO>>($"api/plantel/filtrar?categoriaItem={descripcion}");
            return result!;
        }
        public async Task<Respuesta<List<PlantelDTO>>> ObtenerPorAnios(int anio1, int anio2)
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<PlantelDTO>>>($"api/plantel/ObtenerPorAnios?anio1={anio1}&anio2={anio2}");
            return result!;
        }
    }
}
