using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface IEstableServicio
    {
        Task<Respuesta<List<EstableDTO>>> Lista(int page, int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<EstableDTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<Respuesta<List<EstableDTO>>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null);
        Task<bool> Eliminar(int id);
        Task<Respuesta<EstableDTO>> Crear(EstableDTO entidad);
        Task<bool> Editar(EstableDTO entidad);
        Task<Respuesta<EstableDTO>> Filtrar(string descripcion);
    }
}
