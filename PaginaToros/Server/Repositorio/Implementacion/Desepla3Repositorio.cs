using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class Desepla3Repositorio : IDesepla3Repositorio
    {
        private readonly hereford_prContext _dbContext;

        public Desepla3Repositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Desepla3>> Lista(int skip, int take)
        {

            try
            {

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Desepla3s
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


        public async Task<Desepla3> Obtener(Expression<Func<Desepla3, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Desepla3s.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Desepla3>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                return await _dbContext.Desepla3s
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

        public async Task<bool> Eliminar(Desepla3 entidad)
        {
            try
            {
                _dbContext.Desepla3s.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Desepla3> Crear(Desepla3 entidad)
        {
            try
            {
                _dbContext.Set<Desepla3>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Desepla3 entidad)
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
        public async Task<IQueryable<Desepla3>> Consultar(Expression<Func<Desepla3, bool>> filtro = null)
        {
            IQueryable<Desepla3> queryEntidad = filtro == null
                    ? _dbContext.Desepla3s.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Desepla3s.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Desepla3s.Count();
            }
            catch
            {
                throw;
            }
        }
    }
}
