using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface ICertifsemanRepositorio
    {
        Task<List<Certifseman>> Lista(int page, int count);
        Task<Certifseman> Obtener(Expression<Func<Certifseman, bool>> filtro = null);
        Task<List<Certifseman>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<List<Certifseman>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Certifseman entidad);
        Task<Certifseman> Crear(Certifseman entidad);
        Task<bool> Editar(Certifseman entidad);
        Task<IQueryable<Certifseman>> Consultar(Expression<Func<Certifseman, bool>> filtro = null);
        Task<int> CantidadTotal();
    }
}
