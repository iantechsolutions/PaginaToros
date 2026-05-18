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
                var pageKeys = await GetPagedDeclarationKeysAsync(skip, take, null);
                if (pageKeys.Count == 0)
                {
                    return new List<Desepla1>();
                }

                var items = await _dbContext.Desepla1s
                    .AsNoTracking()
                    .Include(x => x.Plantel)
                    .Include(x => x.Socio)
                    .Where(x => pageKeys.Contains(x.Nrodec))
                    .ToListAsync();

                return OrderAndDeduplicateByKeys(items, pageKeys);
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
                var pageKeys = await GetPagedDeclarationKeysAsync(skip, take, filtro);
                if (pageKeys.Count == 0)
                {
                    return new List<Desepla1>();
                }

                var query = _dbContext.Desepla1s
                    .AsNoTracking()
                    .Include(x => x.Socio)
                    .Include(x => x.Plantel)
                    .Where(x => pageKeys.Contains(x.Nrodec));

                var items = await query.ToListAsync();
                return OrderAndDeduplicateByKeys(items, pageKeys);
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
                var pageKeys = await GetPagedDeclarationKeysAsync(skip, take, filtro);
                if (pageKeys.Count == 0)
                {
                    return new List<Desepla1>();
                }

                var items = await _dbContext.Desepla1s
                    .AsNoTracking()
                    .Where(x => pageKeys.Contains(x.Nrodec))
                    .ToListAsync();

                return OrderAndDeduplicateByKeys(items, pageKeys);
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

        private async Task<List<string>> GetPagedDeclarationKeysAsync(int skip, int take, string? filtro)
        {
            IQueryable<Desepla1> query = _dbContext.Desepla1s.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(filtro))
            {
                query = query.Where(filtro);
            }

            var orderedQuery = query
                .Where(x => !string.IsNullOrWhiteSpace(x.Nrodec))
                .GroupBy(x => x.Nrodec)
                .Select(g => new
                {
                    Nrodec = g.Key,
                    HasFecdecl = g.Max(x => x.Fecdecl.HasValue),
                    Fecdecl = g.Max(x => x.Fecdecl),
                    HasFchUsu = g.Max(x => x.FchUsu.HasValue),
                    FchUsu = g.Max(x => x.FchUsu),
                    Id = g.Max(x => x.Id)
                })
                .OrderByDescending(x => x.HasFecdecl)
                .ThenByDescending(x => x.Fecdecl)
                .ThenByDescending(x => x.HasFchUsu)
                .ThenByDescending(x => x.FchUsu)
                .ThenByDescending(x => x.Id);

            IQueryable<string> keyQuery = orderedQuery.Select(x => x.Nrodec);

            if (skip > 0)
            {
                keyQuery = keyQuery.Skip(skip);
            }

            if (take > 0)
            {
                keyQuery = keyQuery.Take(take);
            }

            return await keyQuery.ToListAsync();
        }

        private static List<Desepla1> OrderAndDeduplicateByKeys(List<Desepla1> items, IReadOnlyList<string> pageKeys)
        {
            var deduped = DeduplicateByNrodec(items);
            var lookup = deduped
                .Where(x => !string.IsNullOrWhiteSpace(x.Nrodec))
                .GroupBy(x => x.Nrodec, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

            var ordered = new List<Desepla1>(pageKeys.Count);
            foreach (var key in pageKeys)
            {
                if (lookup.TryGetValue(key, out var item))
                {
                    ordered.Add(item);
                }
            }

            return ordered;
        }

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
