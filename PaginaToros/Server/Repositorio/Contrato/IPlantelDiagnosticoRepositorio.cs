using PaginaToros.Shared.Models;

namespace PaginaToros.Server.Repositorio.Contrato
{
    /// <summary>
    /// Diagnostico y consolidacion de planteles duplicados. Separado de IPlantelRepositorio
    /// a proposito: son operaciones de mantenimiento de datos, no el CRUD de la aplicacion.
    /// </summary>
    public interface IPlantelDiagnosticoRepositorio
    {
        Task<List<PlantelDuplicadoGrupoDTO>> ListarGruposDuplicadosAsync();

        /// <summary>
        /// Consolida un grupo. Con <see cref="ConsolidarPlantelesRequest.Aplicar"/> en false
        /// devuelve el plan sin tocar la base.
        /// </summary>
        Task<ConsolidarPlantelesResultado> ConsolidarAsync(ConsolidarPlantelesRequest request);
    }
}
