using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface IResin3Servicio
    {
        Task<Respuesta<List<Resin3DTO>>> Lista(int page, int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<Resin3DTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(int id);
        Task<Respuesta<Resin3DTO>> Crear(Resin3DTO entidad);
        Task<bool> Editar(Resin3DTO entidad);
        Task<Respuesta<Resin3DTO>> Filtrar(string descripcion);
    }
}
