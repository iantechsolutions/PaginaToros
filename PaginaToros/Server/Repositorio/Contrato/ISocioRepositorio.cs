using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface ISocioRepositorio
    {
        Task<List<Socio>> Lista(int page, int count);
        Task<Socio> Obtener(Expression<Func<Socio, bool>> filtro = null);
        Task<Socio> ObtenerPorId(int id);
        Task<List<Socio>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Socio entidad);
        Task<Socio> Crear(Socio entidad);
        Task<bool> Editar(Socio entidad);
        Task<IQueryable<Socio>> Consultar(Expression<Func<Socio, bool>> filtro = null);
        Task<int> CantidadTotal();
        Task<List<Socio>> LimitadosFiltradosTodos(int skip, int take, string expression = null);
    }
}
