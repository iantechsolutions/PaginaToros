using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using static System.Net.WebRequestMethods;

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
                var items = await GetOrderedItems(includeNavigation: true);
                var uniqueItems = RemoveDuplicatesByBusinessKey(items);

                return ApplyPaging(uniqueItems, skip, take);
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

        public async Task<Certifseman?> ObtenerPorClave(string nroCert, string hba, int? excludeId = null)
        {
            try
            {
                var normalizedNroCert = NormalizeKeyPart(nroCert);
                var normalizedHba = NormalizeKeyPart(hba);

                var query = _dbContext.Certifsemen
                    .AsNoTracking()
                    .Include(x => x.Socio)
                    .Include(x => x.Centro)
                    .Where(x => (x.NroCert ?? string.Empty).Trim() == normalizedNroCert
                             && (x.Hba ?? string.Empty).Trim() == normalizedHba);

                if (excludeId.HasValue)
                {
                    query = query.Where(x => x.Id != excludeId.Value);
                }

                return await ApplyCreationOrder(query).FirstOrDefaultAsync();
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
                IQueryable<Certifseman> query = _dbContext.Certifsemen
                    .AsNoTracking()
                    .Include(x => x.Socio)
                    .Include(x => x.Centro);

                if (!string.IsNullOrWhiteSpace(filtro))
                    query = query.Where(filtro); 

                query = ApplyCreationOrder(query);

                if (skip > 0)
                {
                    query = query.Skip(skip);
                }

                if (take > 0)
                {
                    query = query.Take(take);
                }

                var items = await query.ToListAsync();
                var uniqueItems = RemoveDuplicatesByBusinessKey(items);
                return uniqueItems;
            }
            catch
            {
                throw;
            }
        }


        public async Task<List<Certifseman>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null)
        {
            try
            {
                IQueryable<Certifseman> query = _dbContext.Certifsemen.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    query = query.Where(filtro);
                }

                query = ApplyCreationOrder(query);

                if (skip > 0)
                {
                    query = query.Skip(skip);
                }

                if (take > 0)
                {
                    query = query.Take(take);
                }

                var items = await query.ToListAsync();
                var uniqueItems = RemoveDuplicatesByBusinessKey(items);
                return uniqueItems;
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
                Console.WriteLine("SE CREA CERTIFICADO");
                Console.WriteLine(entidad);
                entidad.FchUsu ??= DateTime.Now;
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

        public async Task<bool> UpdateNrDosiAsync(int id, int nrDosi)
        {
            try
            {
                var entity = await _dbContext.Certifsemen.FirstOrDefaultAsync(e => e.Id == id);
                if (entity == null) return false;

                entity.NrDosi = nrDosi;
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
                var items = await _dbContext.Certifsemen
                    .AsNoTracking()
                    .ToListAsync();

                return RemoveDuplicatesByBusinessKey(items).Count;
            }
            catch
            {
                throw;
            }
        }

        public static List<Certifseman> RemoveDuplicatesByBusinessKey(IEnumerable<Certifseman> items)
        {
            var uniqueItems = new List<Certifseman>();
            var seenKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in items)
            {
                var key = BuildBusinessKey(item);
                if (seenKeys.Add(key))
                {
                    uniqueItems.Add(item);
                }
            }

            return uniqueItems;
        }

        private async Task<List<Certifseman>> GetOrderedItems(bool includeNavigation)
        {
            IQueryable<Certifseman> query = _dbContext.Certifsemen.AsNoTracking();

            if (includeNavigation)
            {
                query = query.Include(x => x.Socio)
                             .Include(x => x.Centro);
            }

            var ordered = ApplyCreationOrder(query);
            return await ordered.ToListAsync();
        }

        private static List<Certifseman> ApplyPaging(List<Certifseman> items, int skip, int take)
        {
            if (skip < 0)
            {
                skip = 0;
            }

            if (take <= 0)
            {
                return items.Skip(skip).ToList();
            }

            return items.Skip(skip).Take(take).ToList();
        }

        private static string BuildBusinessKey(Certifseman item)
            => $"{NormalizeKeyPart(item.NroCert)}|{NormalizeKeyPart(item.Hba)}";

        private static string NormalizeKeyPart(string? value)
            => (value ?? string.Empty).Trim();

        private static IOrderedQueryable<Certifseman> ApplyCreationOrder(IQueryable<Certifseman> query)
            => query
                .OrderByDescending(x => x.FchUsu.HasValue)
                .ThenByDescending(x => x.FchUsu)
                .ThenByDescending(x => x.Id);
    }
}
