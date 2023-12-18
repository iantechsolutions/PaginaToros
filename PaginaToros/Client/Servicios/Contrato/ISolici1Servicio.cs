using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface ISolici1Servicio
    {
        Task<Respuesta<List<Solici1DTO>>> Lista(int page,int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<Solici1DTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<Respuesta<List<Solici1DTO>>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null);
        Task<bool> Eliminar(int id);
        Task<Respuesta<Solici1DTO>> Crear(Solici1DTO entidad);
        Task<bool> Editar(Solici1DTO entidad);
        Task<Respuesta<Solici1DTO>> Filtrar(string descripcion);
    }
}
