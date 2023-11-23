using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface IInspectRepositorio
    {
        Task<List<Inspect>> Lista(int page, int count);
        Task<Inspect> Obtener(Expression<Func<Inspect, bool>> filtro = null);
        Task<List<Inspect>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Inspect entidad);
        Task<Inspect> Crear(Inspect entidad);
        Task<bool> Editar(Inspect entidad);
        Task<IQueryable<Inspect>> Consultar(Expression<Func<Inspect, bool>> filtro = null);
        Task<int> CantidadTotal();
    }
}
