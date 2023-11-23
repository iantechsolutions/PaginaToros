using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface ITransanRepositorio
    {
        Task<List<Transan>> Lista(int page, int count);
        Task<Transan> Obtener(Expression<Func<Transan, bool>> filtro = null);
        Task<List<Transan>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Transan entidad);
        Task<Transan> Crear(Transan entidad);
        Task<bool> Editar(Transan entidad);
        Task<IQueryable<Transan>> Consultar(Expression<Func<Transan, bool>> filtro = null);
        Task<int> CantidadTotal();
    }
}
