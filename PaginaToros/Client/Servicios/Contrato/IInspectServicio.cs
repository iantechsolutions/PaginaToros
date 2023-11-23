using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface IInspectServicio
    {
        Task<Respuesta<List<InspectDTO>>> Lista(int page, int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<InspectDTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(int id);
        Task<Respuesta<InspectDTO>> Crear(InspectDTO entidad);
        Task<bool> Editar(InspectDTO entidad);
        Task<Respuesta<InspectDTO>> Filtrar(string descripcion);
    }
}
