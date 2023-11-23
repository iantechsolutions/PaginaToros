using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Contrato
{
    public interface IFutcontrolRepositorio
    {
        Task<List<Futcontrol>> Lista(int page, int count);
        Task<Futcontrol> Obtener(Expression<Func<Futcontrol, bool>> filtro = null);
        Task<List<Futcontrol>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<bool> Eliminar(Futcontrol entidad);
        Task<Futcontrol> Crear(Futcontrol entidad);
        Task<bool> Editar(Futcontrol entidad);
        Task<IQueryable<Futcontrol>> Consultar(Expression<Func<Futcontrol, bool>> filtro = null);
        Task<int> CantidadTotal();
    }
}
