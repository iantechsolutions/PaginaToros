using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface IResin1Repositorio
    {
        Task<List<Resin1>> Lista(int page, int count);
        Task<Resin1> Obtener(Expression<Func<Resin1, bool>> filtro = null);
        Task<List<Resin1>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<List<Resin1>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Resin1 entidad);
        Task<Resin1> Crear(Resin1 entidad);
        Task<bool> Editar(Resin1 entidad);
        Task<IQueryable<Resin1>> Consultar(Expression<Func<Resin1, bool>> filtro = null);
        Task<int> CantidadTotal();
    }
}
