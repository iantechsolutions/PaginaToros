using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class TorosRepositorio : ITorosRepositorio
    {
        private readonly hereford_prContext _dbContext;

        public TorosRepositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Torosuni>> Lista(int page, int count)
        {

            try
            {
                int pageSize = count;
                int skipAmount = (page - 1) * pageSize; // Calculate the number of items to skip

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Torosunis.Include(t => t.Socio)
                                                 .Skip(skipAmount)
                                                 .Take(pageSize)
                                                 .ToListAsync();
            }
            catch
            {
                throw;
            }
        }


        public async Task<Torosuni> Obtener(Expression<Func<Torosuni, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Torosunis.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Eliminar(Torosuni entidad)
        {
            try
            {
                _dbContext.Torosunis.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Torosuni> Crear(Torosuni entidad)
        {
            try
            {
                _dbContext.Set<Torosuni>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Torosuni entidad)
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
        public async Task<IQueryable<Torosuni>> Consultar(Expression<Func<Torosuni, bool>> filtro = null)
        {
            IQueryable<Torosuni> queryEntidad = filtro == null
                    ? _dbContext.Torosunis.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Torosunis.Where(filtro);

            return queryEntidad; 
        }
    }
}
