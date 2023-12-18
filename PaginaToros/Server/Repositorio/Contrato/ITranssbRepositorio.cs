using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface ITranssbRepositorio
    {
        Task<List<Transsb>> Lista(int page, int count);
        Task<Transsb> Obtener(Expression<Func<Transsb, bool>> filtro = null);
        Task<List<Transsb>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<List<Transsb>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Transsb entidad);
        Task<Transsb> Crear(Transsb entidad);
        Task<bool> Editar(Transsb entidad);
        Task<IQueryable<Transsb>> Consultar(Expression<Func<Transsb, bool>> filtro = null);
        Task<int> CantidadTotal();
    }
}
