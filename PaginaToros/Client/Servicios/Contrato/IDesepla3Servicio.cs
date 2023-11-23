using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface IDesepla3Servicio
    {
        Task<Respuesta<List<Desepla3DTO>>> Lista(int page, int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<Desepla3DTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(int id);
        Task<Respuesta<Desepla3DTO>> Crear(Desepla3DTO entidad);
        Task<bool> Editar(Desepla3DTO entidad);
        Task<Respuesta<Desepla3DTO>> Filtrar(string descripcion);
    }
}
