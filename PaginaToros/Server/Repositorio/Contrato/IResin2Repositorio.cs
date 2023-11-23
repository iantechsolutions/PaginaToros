using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface IResin2Repositorio
    {
        Task<List<Resin2>> Lista(int page, int count);
        Task<Resin2> Obtener(Expression<Func<Resin2, bool>> filtro = null);
        Task<List<Resin2>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Resin2 entidad);
        Task<Resin2> Crear(Resin2 entidad);
        Task<bool> Editar(Resin2 entidad);
        Task<IQueryable<Resin2>> Consultar(Expression<Func<Resin2, bool>> filtro = null);
        Task<int> CantidadTotal();
    }
}
