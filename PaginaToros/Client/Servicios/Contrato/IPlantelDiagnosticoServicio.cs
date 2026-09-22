using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface IPlantelDiagnosticoServicio
    {
        Task<Respuesta<List<PlantelDuplicadoGrupoDTO>>> Duplicados();
        Task<Respuesta<ConsolidarPlantelesResultado>> Consolidar(ConsolidarPlantelesRequest request);
    }
}
