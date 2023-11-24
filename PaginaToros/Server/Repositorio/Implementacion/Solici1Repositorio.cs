using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class Solici1Repositorio : ISolici1Repositorio
    {
        private readonly hereford_prContext _dbContext;

        public Solici1Repositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Solici1>> Lista(int skip, int take)
        {

            try
            {

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Solici1s.Include(t => t.Establecimiento).ThenInclude(e => e.Socio)
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


        public async Task<Solici1> Obtener(Expression<Func<Solici1, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Solici1s.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Solici1>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                return await _dbContext.Solici1s
                                 .Include(t => t.Establecimiento)
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

        public async Task<bool> Eliminar(Solici1 entidad)
        {
            try
            {
                _dbContext.Solici1s.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Solici1> Crear(Solici1 entidad)
        {
            try
            {
                _dbContext.Set<Solici1>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Solici1 entidad)
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
        public async Task<IQueryable<Solici1>> Consultar(Expression<Func<Solici1, bool>> filtro = null)
        {
            IQueryable<Solici1> queryEntidad = filtro == null
                    ? _dbContext.Solici1s.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Solici1s.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Solici1s.Count();
            }
            catch
            {
                throw;
            }
        }
    }
}
