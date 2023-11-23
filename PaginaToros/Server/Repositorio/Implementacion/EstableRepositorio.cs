using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class EstableRepositorio : IEstableRepositorio
    {
        private readonly hereford_prContext _dbContext;

        public EstableRepositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Estable>> Lista(int skip, int take)
        {

            try
            {

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Estables.Include(t => t.Socio)
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


        public async Task<Estable> Obtener(Expression<Func<Estable, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Estables.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Estable>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                return await _dbContext.Estables
                                 .Include(t => t.Socio)
                                 .Where(filtro)
                                 .Skip(skip)
                                 .Take(take)
                                 .OrderByDescending(t => t.Id)
                                 .ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(Estable entidad)
        {
            try
            {
                _dbContext.Estables.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Estable> Crear(Estable entidad)
        {
            try
            {
                _dbContext.Set<Estable>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Estable entidad)
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
        public async Task<IQueryable<Estable>> Consultar(Expression<Func<Estable, bool>> filtro = null)
        {
            IQueryable<Estable> queryEntidad = filtro == null
                    ? _dbContext.Estables.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Estables.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Estables.Count();
            }
            catch
            {
                throw;
            }
        }
    }
}
