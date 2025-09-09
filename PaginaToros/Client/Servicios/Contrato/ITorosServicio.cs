using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface ITorosServicio
    {
        Task<Respuesta<List<TorosuniDTO>>> Lista(int page,int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<TorosuniDTO>> GetById(int id);

        Task<Respuesta<List<TorosuniDTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<Respuesta<List<TorosuniDTO>>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null);
        Task<Respuesta<List<TorosuniDTO>>> GetBySocioId(int socioId);
        Task<bool> Eliminar(int id);
        Task<Respuesta<TorosuniDTO>> Crear(TorosuniDTO entidad);
        Task<bool> Editar(TorosuniDTO entidad);
        Task<Respuesta<TorosuniDTO>> Filtrar(string descripcion);
    }
}
