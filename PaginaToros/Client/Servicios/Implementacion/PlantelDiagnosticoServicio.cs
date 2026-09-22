using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Http.Json;

namespace PaginaToros.Client.Servicios.Implementacion
{
    public class PlantelDiagnosticoServicio : IPlantelDiagnosticoServicio
    {
        private readonly HttpClient _http;

        public PlantelDiagnosticoServicio(HttpClient http)
        {
            _http = http;
        }

        public async Task<Respuesta<List<PlantelDuplicadoGrupoDTO>>> Duplicados()
        {
            var result = await _http.GetFromJsonAsync<Respuesta<List<PlantelDuplicadoGrupoDTO>>>(
                "api/PlantelDiagnostico/Duplicados");
            return result!;
        }

        public async Task<Respuesta<ConsolidarPlantelesResultado>> Consolidar(ConsolidarPlantelesRequest request)
        {
            var respuesta = await _http.PostAsJsonAsync("api/PlantelDiagnostico/Consolidar", request);
            var result = await respuesta.Content.ReadFromJsonAsync<Respuesta<ConsolidarPlantelesResultado>>();
            return result!;
        }
    }
}
