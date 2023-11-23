using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface IDesepla3Repositorio
    {
        Task<List<Desepla3>> Lista(int page, int count);
        Task<Desepla3> Obtener(Expression<Func<Desepla3, bool>> filtro = null);
        Task<List<Desepla3>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Desepla3 entidad);
        Task<Desepla3> Crear(Desepla3 entidad);
        Task<bool> Editar(Desepla3 entidad);
        Task<IQueryable<Desepla3>> Consultar(Expression<Func<Desepla3, bool>> filtro = null);
        Task<int> CantidadTotal();
    }
}
