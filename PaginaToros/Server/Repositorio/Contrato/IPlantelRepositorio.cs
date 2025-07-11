using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface IPlantelRepositorio
    {
        Task<List<Plantel>> Lista(int page, int count);
        Task<Plantel> Obtener(Expression<Func<Plantel, bool>> filtro = null);
        Task<List<Plantel>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<List<Plantel>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null);
        Task<List<Plantel>> ObtenerPorRangoFechas(DateTime desde, DateTime hasta);

        Task<bool> Eliminar(Plantel entidad);
        Task<Plantel> Crear(Plantel entidad);
        Task<bool> Editar(Plantel entidad);
        Task<IQueryable<Plantel>> Consultar(Expression<Func<Plantel, bool>> filtro = null);
        Task<int> CantidadTotal();
        Task<List<Plantel>> ObtenerPorAnios(int anio1, int anio2);

    }
}
