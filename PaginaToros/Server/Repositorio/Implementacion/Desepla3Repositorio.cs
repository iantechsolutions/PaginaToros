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
                List<Desepla3> a;
                if(!string.IsNullOrWhiteSpace(filtro)) {
                    a = await _dbContext.Desepla3s.Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    a = await _dbContext.Desepla3s.Skip(skip).ToListAsync();
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

        // New: get by Nrodec without dynamic LINQ
        public async Task<List<Desepla3>> GetByNrodec(string nrodec)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nrodec)) return new List<Desepla3>();

                // Normalize (trim and uppercase/lowercase depends on DB collation). Use exact match.
                var normalized = nrodec.Trim();

                return await _dbContext.Desepla3s
                    .Where(d => (d.Nrodec ?? "") == normalized)
                    .OrderByDescending(d => d.Id)
                    .ToListAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
