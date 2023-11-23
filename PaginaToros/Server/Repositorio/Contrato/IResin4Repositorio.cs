using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface IResin4Repositorio
    {
        Task<List<Resin4>> Lista(int page, int count);
        Task<Resin4> Obtener(Expression<Func<Resin4, bool>> filtro = null);
        Task<List<Resin4>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Resin4 entidad);
        Task<Resin4> Crear(Resin4 entidad);
        Task<bool> Editar(Resin4 entidad);
        Task<IQueryable<Resin4>> Consultar(Expression<Func<Resin4, bool>> filtro = null);
        Task<int> CantidadTotal();
    }
}
