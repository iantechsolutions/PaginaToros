using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class Resin4Repositorio : IResin4Repositorio
    {
        private readonly hereford_prContext _dbContext;

        public Resin4Repositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Resin4>> Lista(int skip, int take)
        {

            try
            {

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Resin4s
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


        public async Task<Resin4> Obtener(Expression<Func<Resin4, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Resin4s.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Resin4>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                return await _dbContext.Resin4s
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

        public async Task<bool> Eliminar(Resin4 entidad)
        {
            try
            {
                _dbContext.Resin4s.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Resin4> Crear(Resin4 entidad)
        {
            try
            {
                _dbContext.Set<Resin4>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Resin4 entidad)
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
        public async Task<IQueryable<Resin4>> Consultar(Expression<Func<Resin4, bool>> filtro = null)
        {
            IQueryable<Resin4> queryEntidad = filtro == null
                    ? _dbContext.Resin4s.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Resin4s.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Resin4s.Count();
            }
            catch
            {
                throw;
            }
        }
    }
}
