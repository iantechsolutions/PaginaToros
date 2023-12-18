using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface ISolici1Repositorio
    {
        Task<List<Solici1>> Lista(int page,int count);
        Task<Solici1> Obtener(Expression<Func<Solici1, bool>> filtro = null);
        Task<List<Solici1>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<List<Solici1>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Solici1 entidad);
        Task<Solici1> Crear(Solici1 entidad);
        Task<bool> Editar(Solici1 entidad);
        Task<IQueryable<Solici1>> Consultar(Expression<Func<Solici1, bool>> filtro = null);
        Task<int> CantidadTotal();

    }
}
