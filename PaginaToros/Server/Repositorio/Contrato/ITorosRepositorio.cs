using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface ITorosRepositorio
    {
        Task<List<Torosuni>> Lista(int page,int count);
        Task<Torosuni> Obtener(Expression<Func<Torosuni, bool>> filtro = null);
        Task<(List<Torosuni>, int)> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Torosuni entidad);
        Task<Torosuni> Crear(Torosuni entidad);
        Task<bool> Editar(Torosuni entidad);
        Task<IQueryable<Torosuni>> Consultar(Expression<Func<Torosuni, bool>> filtro = null);
        Task<int> CantidadTotal();

    }
}
