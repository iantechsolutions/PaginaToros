using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class Resin6Repositorio : IResin6Repositorio
    {
        private readonly hereford_prContext _dbContext;

        public Resin6Repositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Resin6>> Lista(int skip, int take)
        {

            try
            {

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Resin6s.Include(x => x.Resin1).ThenInclude(x=>x.Establecimiento)
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


        public async Task<Resin6> Obtener(Expression<Func<Resin6, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Resin6s.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Resin6>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                List<Resin6> a;
                if(filtro is not null)
                {
                    a = await _dbContext.Resin6s.Include(x => x.Resin1).ThenInclude(x => x.Establecimiento).Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    a = await _dbContext.Resin6s.Include(x => x.Resin1).ThenInclude(x => x.Establecimiento).Skip(skip).ToListAsync();
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

        public async Task<bool> Eliminar(Resin6 entidad)
        {
            try
            {
                _dbContext.Resin6s.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Resin6> Crear(Resin6 entidad)
        {
            try
            {
                _dbContext.Set<Resin6>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Resin6 entidad)
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
        public async Task<IQueryable<Resin6>> Consultar(Expression<Func<Resin6, bool>> filtro = null)
        {
            IQueryable<Resin6> queryEntidad = filtro == null
                    ? _dbContext.Resin6s.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Resin6s.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Resin6s.Count();
            }
            catch
            {
                throw;
            }
        }
    }
}
