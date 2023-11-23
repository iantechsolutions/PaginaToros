using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface IFutcontrolServicio
    {
        Task<Respuesta<List<FutcontrolDTO>>> Lista(int page, int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<FutcontrolDTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(int id);
        Task<Respuesta<FutcontrolDTO>> Crear(FutcontrolDTO entidad);
        Task<bool> Editar(FutcontrolDTO entidad);
        Task<Respuesta<FutcontrolDTO>> Filtrar(string descripcion);
    }
}
