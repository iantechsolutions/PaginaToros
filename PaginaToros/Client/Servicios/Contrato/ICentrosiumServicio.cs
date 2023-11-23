using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface ICentrosiumServicio
    {
        Task<Respuesta<List<CentrosiumDTO>>> Lista(int page, int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<CentrosiumDTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(int id);
        Task<Respuesta<CentrosiumDTO>> Crear(CentrosiumDTO entidad);
        Task<bool> Editar(CentrosiumDTO entidad);
        Task<Respuesta<CentrosiumDTO>> Filtrar(string descripcion);
    }
}
