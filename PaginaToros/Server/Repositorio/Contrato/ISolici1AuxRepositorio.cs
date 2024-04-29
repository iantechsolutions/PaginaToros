using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface ISolici1AuxRepositorio
    {
        Task<List<Solici1Aux>> Lista(int page,int count);
        Task<Solici1Aux> Obtener(Expression<Func<Solici1Aux, bool>> filtro = null);
        Task<List<Solici1Aux>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<List<Solici1Aux>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Solici1Aux entidad);
        Task<Solici1Aux> Crear(Solici1Aux entidad);
        Task<bool> Editar(Solici1Aux entidad);
        Task<IQueryable<Solici1Aux>> Consultar(Expression<Func<Solici1Aux, bool>> filtro = null);
        Task<int> CantidadTotal();

    }
}
