using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface IResin6Repositorio
    {
        Task<List<Resin6>> Lista(int page, int count);
        Task<Resin6> Obtener(Expression<Func<Resin6, bool>> filtro = null);
        Task<List<Resin6>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Resin6 entidad);
        Task<Resin6> Crear(Resin6 entidad);
        Task<bool> Editar(Resin6 entidad);
        Task<IQueryable<Resin6>> Consultar(Expression<Func<Resin6, bool>> filtro = null);
        Task<int> CantidadTotal();
        Task<List<Resin6>> ObtenerFechas(long fecha1, long fecha2);
    }
}
