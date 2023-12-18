using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface IDesepla1Servicio
    {
        Task<Respuesta<List<Desepla1DTO>>> Lista(int page, int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<Desepla1DTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<Respuesta<List<Desepla1DTO>>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null);
        Task<bool> Eliminar(int id);
        Task<Respuesta<Desepla1DTO>> Crear(Desepla1DTO entidad);
        Task<bool> Editar(Desepla1DTO entidad);
        Task<Respuesta<Desepla1DTO>> Filtrar(string descripcion);
    }
}
