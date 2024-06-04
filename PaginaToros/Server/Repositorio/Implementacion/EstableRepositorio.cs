using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class EstableRepositorio : IEstableRepositorio
    {
        private readonly hereford_prContext _dbContext;

        public EstableRepositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Estable>> Lista(int skip, int take)
        {

            try
            {

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Estables.Include(t => t.Socio)
                                                 .Include(x=>x.Provincia)
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


        public async Task<Estable> Obtener(Expression<Func<Estable, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Estables.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Estable>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                List<Estable> a;
                if (filtro is not null)
                {
                    a = await _dbContext.Estables.Include(t => t.Socio).Include(x => x.Provincia).Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    a = await _dbContext.Estables.Include(t=>t.Socio).Include(x => x.Provincia).Skip(skip).ToListAsync();
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

        public async Task<List<Estable>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null)
        {
            try
            {
                List<Estable> a;
                if (filtro is not null)
                {
                    a = await _dbContext.Estables.Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    a = await _dbContext.Estables.Skip(skip).ToListAsync();
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

        public async Task<bool> Eliminar(Estable entidad)
        {
            try
            {
                _dbContext.Estables.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Estable> Crear(Estable entidad)
        {
            try
            {
                _dbContext.Set<Estable>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Estable entidad)
        {
            try
            {
                var estableviejo = _dbContext.Estables.Where(x=>x.Id == entidad.Id).First();
                try
                {
                    if(estableviejo.Ecod!= entidad.Ecod)
                    {
                        var solicis = _dbContext.Solici1s.Where(x => x.Codest == estableviejo.Ecod).ToList();
                        foreach(var soli in solicis)
                        {
                            soli.Codest = entidad.Ecod;
                            _dbContext.Update(soli);
                        }
                        var transsbs = _dbContext.Transsbs.Where(x => x.Ecod == estableviejo.Ecod).ToList();
                        foreach(var tran in transsbs)
                        {
                            tran.Ecod = entidad.Ecod;
                            _dbContext.Update(tran);
                        }
                        var resins = _dbContext.Resin1s.Where(x => x.Estcod == estableviejo.Ecod).ToList();
                        foreach(var resi in resins)
                        {
                            resi.Estcod = entidad.Ecod;
                            _dbContext.Update(resi);
                        }
                    }
                    _dbContext.Update(entidad);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                catch {
                    throw;
                }
                
            }
            catch
            {
                throw;
            }
        }
        public async Task<IQueryable<Estable>> Consultar(Expression<Func<Estable, bool>> filtro = null)
        {
            IQueryable<Estable> queryEntidad = filtro == null
                    ? _dbContext.Estables.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Estables.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Estables.Count();
            }
            catch
            {
                throw;
            }
        }
    }
}
