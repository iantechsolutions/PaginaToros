using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface IResin4Servicio
    {
        Task<Respuesta<List<Resin4DTO>>> Lista(int page, int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<Resin4DTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(int id);
        Task<Respuesta<Resin4DTO>> Crear(Resin4DTO entidad);
        Task<bool> Editar(Resin4DTO entidad);
        Task<Respuesta<Resin4DTO>> Filtrar(string descripcion);
    }
}
