using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface IResin8Servicio
    {
        Task<Respuesta<List<Resin8DTO>>> Lista(int page, int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<Resin8DTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(int id);
        Task<Respuesta<Resin8DTO>> Crear(Resin8DTO entidad);
        Task<bool> Editar(Resin8DTO entidad);
        Task<Respuesta<Resin8DTO>> Filtrar(string descripcion);
    }
}
