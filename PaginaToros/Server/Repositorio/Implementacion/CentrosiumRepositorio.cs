using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class CentrosiumRepositorio : ICentrosiumRepositorio
    {
        private readonly hereford_prContext _dbContext;

        public CentrosiumRepositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Centrosium>> Lista(int skip, int take)
        {

            try
            {

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Centrosia
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


        public async Task<Centrosium> Obtener(Expression<Func<Centrosium, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Centrosia.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Centrosium>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                List<Centrosium> a;
                if (filtro is not null) { 
                    a = await _dbContext.Centrosia.Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    a = await _dbContext.Centrosia.Skip(skip).ToListAsync();
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

        public async Task<bool> Eliminar(Centrosium entidad)
        {
            try
            {
                _dbContext.Centrosia.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Centrosium> Crear(Centrosium entidad)
        {
            try
            {
                _dbContext.Set<Centrosium>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Centrosium entidad)
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
        public async Task<IQueryable<Centrosium>> Consultar(Expression<Func<Centrosium, bool>> filtro = null)
        {
            IQueryable<Centrosium> queryEntidad = filtro == null
                    ? _dbContext.Centrosia.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Centrosia.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Centrosia.Count();
            }
            catch
            {
                throw;
            }
        }
    }
}
