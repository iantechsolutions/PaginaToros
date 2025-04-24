using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class PlantelRepositorio : IPlantelRepositorio
    {
        private readonly hereford_prContext _dbContext;

        public PlantelRepositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Plantel>> Lista(int skip, int take)
        {

            try
            {

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Planteles.Include(t => t.Socio)
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


        public async Task<Plantel> Obtener(Expression<Func<Plantel, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Planteles.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Plantel>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                List<Plantel> a;
                if(filtro is not null)
                {
                    a = await _dbContext.Planteles.OrderByDescending(t => t.Id).Include(x => x.Socio).Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    a= await _dbContext.Planteles.OrderByDescending(t => t.Id).Include(x => x.Socio).Skip(skip).ToListAsync();
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

        public async Task<List<Plantel>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null)
        {
            try
            {
                List<Plantel> a;
                if (filtro is not null)
                {
                    a = await _dbContext.Planteles.OrderByDescending(t => t.Id).Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    a = await _dbContext.Planteles.OrderByDescending(t => t.Id).Skip(skip).ToListAsync();
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

        public async Task<bool> Eliminar(Plantel entidad)
        {
            try
            {
                _dbContext.Planteles.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Plantel> Crear(Plantel entidad)
        {
            try
            {
                _dbContext.Set<Plantel>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Plantel entidad)
        {
            try
            {
                _dbContext.Update(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Editar: {ex.Message}");
                throw;
            }
        }

        public async Task<IQueryable<Plantel>> Consultar(Expression<Func<Plantel, bool>> filtro = null)
        {
            IQueryable<Plantel> queryEntidad = filtro == null
                    ? _dbContext.Planteles.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Planteles.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Planteles.Count();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Plantel>> ObtenerPorAnios(int anio1, int anio2)
        {
            try { 
            return await _dbContext.Planteles.OrderByDescending(t => t.Id).Include(p=>p.Socio).Where(x => !string.IsNullOrEmpty(x.Anioex) && Convert.ToInt32(x.Anioex) >= anio1 && Convert.ToInt32(x.Anioex) <= anio2).ToListAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
