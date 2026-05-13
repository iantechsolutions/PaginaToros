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
                IQueryable<Certifseman> query = ApplyCreationOrder(
                    _dbContext.Certifsemen
                        .AsNoTracking()
                        .Include(t => t.Socio)
                        .Include(e => e.Centro));

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

                return await query.ToListAsync();
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

                var a = await query.ToListAsync();
                a = RemoveDuplicates(a);
                return a;
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
                return _dbContext.Certifsemen.Count();
            }
            catch
            {
                throw;
            }
        }
        public static List<Certifseman> RemoveDuplicates(List<Certifseman> items)
        {
            var seenIds = new HashSet<int>();
            var uniqueItems = new List<Certifseman>();

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

        private static IOrderedQueryable<Certifseman> ApplyCreationOrder(IQueryable<Certifseman> query)
            => query
                .OrderByDescending(x => x.FchUsu.HasValue)
                .ThenByDescending(x => x.FchUsu)
                .ThenByDescending(x => x.Id);
    }
}
