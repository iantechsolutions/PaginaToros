using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface IDesepla1Repositorio
    {
        Task<List<Desepla1>> Lista(int page, int count);
        Task<Desepla1> Obtener(Expression<Func<Desepla1, bool>> filtro = null);
        Task<List<Desepla1>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<List<Desepla1>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Desepla1 entidad);
        Task<Desepla1> Crear(Desepla1 entidad);
        Task<bool> Editar(Desepla1 entidad);
        Task<IQueryable<Desepla1>> Consultar(Expression<Func<Desepla1, bool>> filtro = null);
        Task<int> CantidadTotal();
    }
}
