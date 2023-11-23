using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface IResin3Repositorio
    {
        Task<List<Resin3>> Lista(int page, int count);
        Task<Resin3> Obtener(Expression<Func<Resin3, bool>> filtro = null);
        Task<List<Resin3>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Resin3 entidad);
        Task<Resin3> Crear(Resin3 entidad);
        Task<bool> Editar(Resin3 entidad);
        Task<IQueryable<Resin3>> Consultar(Expression<Func<Resin3, bool>> filtro = null);
        Task<int> CantidadTotal();
    }
}
