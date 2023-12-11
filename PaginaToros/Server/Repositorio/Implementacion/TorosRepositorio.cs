using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
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
        public async Task<List<Torosuni>> Lista(int skip, int take)
        {

            try
            {

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Torosunis.Include(t => t.Socio)
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
        public async Task<List<Torosuni>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                var a = await _dbContext.Torosunis.Include(t=>t.Socio).Where(filtro).Skip(skip).ToListAsync();
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

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Torosunis.Count();
            }
            catch
            {
                throw;
            }
        }
    }
}
