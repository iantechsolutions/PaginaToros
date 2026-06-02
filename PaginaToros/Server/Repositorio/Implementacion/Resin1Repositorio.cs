using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class Resin1Repositorio : IResin1Repositorio
    {
        private readonly hereford_prContext _dbContext;

        public Resin1Repositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Resin1>> Lista(int skip, int take)
        {

            try
            {
                IQueryable<Resin1> query = ApplyCreationOrder(
                    _dbContext.Resin1s
                        .AsNoTracking()
                        .Include(t => t.Socio)
                        .Include(x => x.Establecimiento));

                if (skip > 0)
                {
                    query = query.Skip(skip);
                }

                if (take > 0)
                {
                    query = query.Take(take);
                }

                return await query.ToListAsync();
            }
            catch
            {
                throw;
            }
        }


        public async Task<Resin1> Obtener(Expression<Func<Resin1, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Resin1s.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Resin1>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                IQueryable<Resin1> query = _dbContext.Resin1s
                    .AsNoTracking()
                    .Include(t => t.Socio)
                    .Include(x => x.Establecimiento);

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

                var a = await query.ToListAsync();
                a = RemoveDuplicates(a);
                return a;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Resin1>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null)
        {
            try
            {
                IQueryable<Resin1> query = _dbContext.Resin1s.AsNoTracking();

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

                var a = await query.ToListAsync();
                a = RemoveDuplicates(a);
                return a;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(Resin1 entidad)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    Console.WriteLine("Entro perfectop");

                    _dbContext.Resin2s.RemoveRange(_dbContext.Resin2s.Where(x => x.Nrores == entidad.Nrores));
                    _dbContext.Resin3s.RemoveRange(_dbContext.Resin3s.Where(x => x.Nrores == entidad.Nrores));
                    _dbContext.Resin4s.RemoveRange(_dbContext.Resin4s.Where(x => x.Nrores == entidad.Nrores));
                    _dbContext.Resin6s.RemoveRange(_dbContext.Resin6s.Where(x => x.Nrores == entidad.Nrores));
                    _dbContext.Resin8s.RemoveRange(_dbContext.Resin8s.Where(x => x.Nrores == entidad.Nrores));

                    _dbContext.Resin1s.Remove(entidad);

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.Error.WriteLine($"Error eliminando registros: {ex.Message}");
                    throw;
                }
            });
        }



        public async Task<Resin1> Crear(Resin1 entidad)
        {
            try
            {
                _dbContext.Set<Resin1>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }
        public async Task<int?> ObtenerUltimoNrores()
        {
            try
            {
                return await _dbContext.Resin1s
                    .Where(r => r.Nrores != null)
                    .OrderByDescending(r => Convert.ToInt32(r.Nrores))
                    .Select(r => (int?)Convert.ToInt32(r.Nrores))
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en ObtenerUltimoNrores: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> Editar(Resin1 entidad)
        {

            Console.WriteLine("Entro al editar");

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
        public async Task<IQueryable<Resin1>> Consultar(Expression<Func<Resin1, bool>> filtro = null)
        {
            IQueryable<Resin1> queryEntidad = filtro == null
                    ? _dbContext.Resin1s.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Resin1s.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Resin1s.Count();
            }
            catch
            {
                throw;
            }
        }
        public static List<Resin1> RemoveDuplicates(List<Resin1> items)
        {
            var seenIds = new HashSet<int>();
            var uniqueItems = new List<Resin1>();

            foreach (var item in items)
            {
                Console.WriteLine(item.Id);
                var boole = seenIds.Add(item.Id);
                Console.WriteLine(boole);
                if (boole)  // HashSet.Add returns false if the item was already in the set
                {
                    uniqueItems.Add(item);
                }
            }
            Console.WriteLine(uniqueItems.Count());
            return uniqueItems;
        }

        private static IOrderedQueryable<Resin1> ApplyCreationOrder(IQueryable<Resin1> query)
            => query
                .OrderByDescending(x => x.FchUsu.HasValue)
                .ThenByDescending(x => x.FchUsu)
                .ThenByDescending(x => x.Freali.HasValue)
                .ThenByDescending(x => x.Freali.HasValue ? x.Freali.Value.Year : (int?)null)
                .ThenByDescending(x => x.Id);
    }
}
