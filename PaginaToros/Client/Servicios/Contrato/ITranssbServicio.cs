using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface ITranssbServicio
    {
        Task<Respuesta<List<TranssbDTO>>> Lista(int page, int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<TranssbDTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<Respuesta<List<TranssbDTO>>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null);
        Task<Respuesta<List<TranssbDTO>>> GetBySocioCod(string socioCod);
        Task<bool> Eliminar(int id);
        Task<Respuesta<TranssbDTO>> Crear(TranssbDTO entidad);
        Task<bool> Editar(TranssbDTO entidad);
        Task<Respuesta<TranssbDTO>> Filtrar(string descripcion);
    }
}
