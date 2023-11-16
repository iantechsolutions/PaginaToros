using Microsoft.AspNetCore.Http;
using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface ITorosRepositorio
    {
        Task<List<Torosuni>> Lista();
        Task<Torosuni> Obtener(Expression<Func<Torosuni, bool>> filtro = null);
        Task<bool> Eliminar(Torosuni entidad);
        Task<Torosuni> Crear(Torosuni entidad);
        Task<bool> Editar(Torosuni entidad);
        Task<IQueryable<Torosuni>> Consultar(Expression<Func<Torosuni, bool>> filtro = null);
    }
}
