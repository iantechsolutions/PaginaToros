using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class InspectRepositorio : IInspectRepositorio
    {
        private readonly hereford_prContext _dbContext;

        public InspectRepositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Inspect>> Lista(int skip, int take)
        {

            try
            {

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Inspects.Include(x => x.Provincia)
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


        public async Task<Inspect> Obtener(Expression<Func<Inspect, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Inspects.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Inspect>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                List<Inspect> a;
                if(filtro is not null)
                {
                    a = await _dbContext.Inspects.Include(x => x.Provincia).Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    a = await _dbContext.Inspects.Include(x => x.Provincia).Skip(skip).ToListAsync();
                }
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

        public async Task<bool> Eliminar(Inspect entidad)
        {
            try
            {
                _dbContext.Inspects.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Inspect> Crear(Inspect entidad)
        {
            try
            {
                _dbContext.Set<Inspect>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Inspect entidad)
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
        public async Task<IQueryable<Inspect>> Consultar(Expression<Func<Inspect, bool>> filtro = null)
        {
            IQueryable<Inspect> queryEntidad = filtro == null
                    ? _dbContext.Inspects.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Inspects.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Inspects.Count();
            }
            catch
            {
                throw;
            }
        }
    }
}
