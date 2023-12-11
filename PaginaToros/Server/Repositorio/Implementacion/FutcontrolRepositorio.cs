using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class FutcontrolRepositorio : IFutcontrolRepositorio
    {
        private readonly hereford_prContext _dbContext;

        public FutcontrolRepositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Futcontrol>> Lista(int skip, int take)
        {

            try
            {

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Futcontrols
                                                 .OrderByDescending(t => t.Id)
                                                 .Skip(skip)
                                                 .Take(take)
                                                 .ToListAsync();
            }
            catch
            {
                throw;
            }
        }


        public async Task<Futcontrol> Obtener(Expression<Func<Futcontrol, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Futcontrols.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Futcontrol>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                var a = await _dbContext.Futcontrols.Where(filtro).Skip(skip).ToListAsync();
                if (take == 0)
                {
                    return a.OrderByDescending(t => t.Id).ToList();
                }
                else
                {
                    return a.Take(take).OrderByDescending(t => t.Id).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(Futcontrol entidad)
        {
            try
            {
                _dbContext.Futcontrols.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Futcontrol> Crear(Futcontrol entidad)
        {
            try
            {
                _dbContext.Set<Futcontrol>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Futcontrol entidad)
        {
            try
            {
                _dbContext.Update(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<IQueryable<Futcontrol>> Consultar(Expression<Func<Futcontrol, bool>> filtro = null)
        {
            IQueryable<Futcontrol> queryEntidad = filtro == null
                    ? _dbContext.Futcontrols.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Futcontrols.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Futcontrols.Count();
            }
            catch
            {
                throw;
            }
        }
    }
}
