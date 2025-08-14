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
                List<Certifseman> a; 
                if(filtro is not null) {
                    a = await _dbContext.Certifsemen.OrderByDescending(x => x.Id).Include(t => t.Socio).Include(t => t.Centro).Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    a = await _dbContext.Certifsemen.OrderByDescending(x => x.Id).Include(t => t.Socio).Include(t => t.Centro).Skip(skip).ToListAsync();
                }
                if (take == 0)
                {
                    a = a.ToList();
                }
                else
                {
                    
                    a = a.Take(take).ToList();
                }
                Console.WriteLine("cantidad pre");
                Console.WriteLine(a.Count());
                a = RemoveDuplicates(a);
                Console.WriteLine("cantidad post");
                Console.WriteLine(a.Count());


                return a;
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
                List<Certifseman> a;
                if (filtro is not null)
                {
                    a = await _dbContext.Certifsemen.OrderByDescending(x => x.Id).Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    a = await _dbContext.Certifsemen.OrderByDescending(x => x.Id).Skip(skip).ToListAsync();
                }
                if (take == 0)
                {
                    a = a.ToList();
                }
                else
                {
                    a = a.Take(take).ToList();
                }
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
    }
}
