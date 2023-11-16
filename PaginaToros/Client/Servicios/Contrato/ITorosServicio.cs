using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface ITorosServicio
    {
        Task<Respuesta<List<TorosuniDTO>>> Lista();
        Task<bool> Eliminar(int id);
        Task<Respuesta<TorosuniDTO>> Crear(TorosuniDTO entidad);
        Task<bool> Editar(TorosuniDTO entidad);
        Task<Respuesta<TorosuniDTO>> Filtrar(string descripcion);
    }
}
