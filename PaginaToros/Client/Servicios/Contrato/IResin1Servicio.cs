using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface IResin1Servicio
    {
        Task<Respuesta<List<Resin1DTO>>> Lista(int page, int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<Resin1DTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<Respuesta<List<Resin1DTO>>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null);
        Task<bool> Eliminar(int id);
        Task<Respuesta<Resin1DTO>> Crear(Resin1DTO entidad);
        Task<bool> Editar(Resin1DTO entidad);
        Task<Respuesta<Resin1DTO>> Filtrar(string descripcion);
    }
}
