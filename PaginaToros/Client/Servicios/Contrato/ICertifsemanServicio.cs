using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface ICertifsemanServicio
    {
        Task<Respuesta<List<CertifsemanDTO>>> Lista(int page, int count);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<CertifsemanDTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<Respuesta<List<CertifsemanDTO>>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null);
        Task<bool> Eliminar(int id);
        Task<Respuesta<CertifsemanDTO>> Crear(CertifsemanDTO entidad);
        Task<bool> Editar(CertifsemanDTO entidad);
        Task<Respuesta<CertifsemanDTO>> Filtrar(string descripcion);
    }
}
