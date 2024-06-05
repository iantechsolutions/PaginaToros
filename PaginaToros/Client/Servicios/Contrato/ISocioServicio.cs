using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface ISocioServicio
    {
        Task<Respuesta<List<SocioDTO>>> Lista(int page, int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<SocioDTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(int id);
        Task<Respuesta<SocioDTO>> Crear(SocioDTO entidad);
        Task<bool> Editar(SocioDTO entidad);
        Task<Respuesta<SocioDTO>> Filtrar(string descripcion);
        Task<Respuesta<List<SocioDTO>>> LimitadosFiltradoTodos(int skip, int take, string filtro = null);
    }
}
