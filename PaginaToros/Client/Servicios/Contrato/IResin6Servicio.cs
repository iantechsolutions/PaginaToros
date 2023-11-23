using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface IResin6Servicio
    {
        Task<Respuesta<List<Resin6DTO>>> Lista(int page, int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<Resin6DTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(int id);
        Task<Respuesta<Resin6DTO>> Crear(Resin6DTO entidad);
        Task<bool> Editar(Resin6DTO entidad);
        Task<Respuesta<Resin6DTO>> Filtrar(string descripcion);
    }
}
