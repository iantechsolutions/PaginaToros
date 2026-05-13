using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class Desepla1Repositorio : IDesepla1Repositorio
    {
        private readonly hereford_prContext _dbContext;

        public Desepla1Repositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Desepla1>> Lista(int skip, int take)
        {
            try
            {
                var items = await ApplyCreationOrder(
                        _dbContext.Desepla1s
                            .AsNoTracking()
                            .Include(x => x.Plantel)
                            .Include(x => x.Socio))
                    .ToListAsync();

                var deduped = DeduplicateByNrodec(items);

                if (skip > 0)
                {
                    deduped = deduped.Skip(skip).ToList();
                }

                if (take > 0)
                {
                    deduped = deduped.Take(take).ToList();
                }

                return deduped;
            }
            catch
            {
                throw;
            }
        }



        public async Task<Desepla1> Obtener(Expression<Func<Desepla1, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Desepla1s.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Desepla1>> LimitadosFiltrados(int skip, int take, string? filtro = null)
        {
            try
            {
                IQueryable<Desepla1> query = _dbContext.Desepla1s
                    .AsNoTracking()
                    .Include(x => x.Socio)
                    .Include(x => x.Plantel);

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    query = query.Where(filtro);
                }

                var items = await ApplyCreationOrder(query).ToListAsync();
                var deduped = DeduplicateByNrodec(items);

                if (skip > 0)
                {
                    deduped = deduped.Skip(skip).ToList();
                }

                if (take > 0)
                {
                    deduped = deduped.Take(take).ToList();
                }

                return deduped;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Desepla1>> LimitadosFiltradosNoInclude(int skip, int take, string? filtro = null)
        {
            try
            {
                IQueryable<Desepla1> query = _dbContext.Desepla1s.AsNoTracking();
                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    query = query.Where(filtro);
                }

                var items = await ApplyCreationOrder(query).ToListAsync();
                var deduped = DeduplicateByNrodec(items);

                if (skip > 0)
                {
                    deduped = deduped.Skip(skip).ToList();
                }

                if (take > 0)
                {
                    deduped = deduped.Take(take).ToList();
                }

                return deduped;
            }
            catch
            {
                throw;
            }
        }
        
        public async Task<bool> Eliminar(Desepla1 entidad)
        {
            try
            {
                _dbContext.Desepla1s.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Desepla1> Crear(Desepla1 entidad)
        {
            try
            {
                _dbContext.Set<Desepla1>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Desepla1 entidad)
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
        public async Task<IQueryable<Desepla1>> Consultar(Expression<Func<Desepla1, bool>> filtro = null)
        {
            IQueryable<Desepla1> queryEntidad = filtro == null
                    ? _dbContext.Desepla1s.Take(30) 
                    : _dbContext.Desepla1s.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Desepla1s.Count();
            }
            catch
            {
                throw;
            }
        }

        private static IOrderedQueryable<Desepla1> ApplyCreationOrder(IQueryable<Desepla1> query)
            => query
                .OrderByDescending(x => x.Fecdecl.HasValue)
                .ThenByDescending(x => x.Fecdecl)
                .ThenByDescending(x => x.FchUsu.HasValue)
                .ThenByDescending(x => x.FchUsu)
                .ThenByDescending(x => x.Id);

        private static List<Desepla1> DeduplicateByNrodec(List<Desepla1> items)
        {
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var result = new List<Desepla1>();

            foreach (var item in items)
            {
                var key = item.Nrodec?.Trim() ?? string.Empty;
                if (seen.Add(key))
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }
}
