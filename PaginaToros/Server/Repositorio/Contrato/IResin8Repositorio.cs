using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface IResin8Repositorio
    {
        Task<List<Resin8>> Lista(int page, int count);
        Task<Resin8> Obtener(Expression<Func<Resin8, bool>> filtro = null);
        Task<List<Resin8>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Resin8 entidad);
        Task<Resin8> Crear(Resin8 entidad);
        Task<bool> Editar(Resin8 entidad);
        Task<IQueryable<Resin8>> Consultar(Expression<Func<Resin8, bool>> filtro = null);
        Task<int> CantidadTotal();
    }
}
