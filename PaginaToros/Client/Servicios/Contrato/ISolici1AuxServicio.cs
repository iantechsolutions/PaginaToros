using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface ISolici1AuxServicio
    {
        Task<Respuesta<List<Solici1AuxDTO>>> Lista(int page,int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<Solici1AuxDTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<Respuesta<List<Solici1AuxDTO>>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null);
        Task<Respuesta<List<Solici1AuxDTO>>> GetBySocioId(int socioId);
        Task<Respuesta<List<Solici1AuxDTO>>> GetBySolId(int SoliId);
        Task<bool> Eliminar(int id);
        Task<Respuesta<Solici1AuxDTO>> Crear(Solici1AuxDTO entidad);
        Task<bool> Editar(Solici1AuxDTO entidad);
        Task<Respuesta<Solici1AuxDTO>> Filtrar(string descripcion);
    }
}
