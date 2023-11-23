using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface ICentrosiumRepositorio
    {
        Task<List<Centrosium>> Lista(int page, int count);
        Task<Centrosium> Obtener(Expression<Func<Centrosium, bool>> filtro = null);
        Task<List<Centrosium>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Centrosium entidad);
        Task<Centrosium> Crear(Centrosium entidad);
        Task<bool> Editar(Centrosium entidad);
        Task<IQueryable<Centrosium>> Consultar(Expression<Func<Centrosium, bool>> filtro = null);
        Task<int> CantidadTotal();
    }
}
