using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface ITransanServicio
    {
        Task<Respuesta<List<TransanDTO>>> Lista(int page, int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<TransanDTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(int id);
        Task<Respuesta<TransanDTO>> Crear(TransanDTO entidad);
        Task<bool> Editar(TransanDTO entidad);
        Task<Respuesta<TransanDTO>> Filtrar(string descripcion);
    }
}
