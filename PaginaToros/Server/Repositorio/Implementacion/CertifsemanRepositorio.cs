using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class CertifsemanRepositorio : ICertifsemanRepositorio
    {
        private readonly hereford_prContext _dbContext;

        public CertifsemanRepositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Certifseman>> Lista(int skip, int take)
        {

            try
            {

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Certifsemen.Include(t => t.Socio)
                                                 .Include(e=>e.Centro)
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


        public async Task<Certifseman> Obtener(Expression<Func<Certifseman, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Certifsemen.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Certifseman>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                return await _dbContext.Certifsemen
                                 .Include(t => t.Socio)
                                 .Include(t => t.Centro)
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

        public async Task<bool> Eliminar(Certifseman entidad)
        {
            try
            {
                _dbContext.Certifsemen.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Certifseman> Crear(Certifseman entidad)
        {
            try
            {
                _dbContext.Set<Certifseman>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Certifseman entidad)
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
        public async Task<IQueryable<Certifseman>> Consultar(Expression<Func<Certifseman, bool>> filtro = null)
        {
            IQueryable<Certifseman> queryEntidad = filtro == null
                    ? _dbContext.Certifsemen.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Certifsemen.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Certifsemen.Count();
            }
            catch
            {
                throw;
            }
        }
    }
}
