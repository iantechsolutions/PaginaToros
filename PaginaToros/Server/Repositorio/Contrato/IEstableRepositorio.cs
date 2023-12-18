using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface IEstableRepositorio
    {
        Task<List<Estable>> Lista(int page, int count);
        Task<Estable> Obtener(Expression<Func<Estable, bool>> filtro = null);
        Task<List<Estable>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<List<Estable>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Estable entidad);
        Task<Estable> Crear(Estable entidad);
        Task<bool> Editar(Estable entidad);
        Task<IQueryable<Estable>> Consultar(Expression<Func<Estable, bool>> filtro = null);
        Task<int> CantidadTotal();
    }
}
