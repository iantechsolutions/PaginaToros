using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface IResin2Servicio
    {
        Task<Respuesta<List<Resin2DTO>>> Lista(int page, int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<Resin2DTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(int id);
        Task<Respuesta<Resin2DTO>> Crear(Resin2DTO entidad);
        Task<bool> Editar(Resin2DTO entidad);
        Task<Respuesta<Resin2DTO>> Filtrar(string descripcion);
    }
}
