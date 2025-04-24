using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class Resin2Repositorio : IResin2Repositorio
    {
        private readonly hereford_prContext _dbContext;

        public Resin2Repositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Resin2>> Lista(int skip, int take)
        {

            try
            {

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Resin2s
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


        public async Task<Resin2> Obtener(Expression<Func<Resin2, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Resin2s.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Resin2>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                List<Resin2> a;
                if(filtro is not null) { 
                    a = await _dbContext.Resin2s.Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    a = await _dbContext.Resin2s.Skip(skip).ToListAsync();
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

        public async Task<bool> Eliminar(Resin2 entidad)
        {
            try
            {
                _dbContext.Resin2s.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Resin2> Crear(Resin2 entidad)
        {
            try
            {
                _dbContext.Set<Resin2>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Resin2 entidad)
        {

            Console.WriteLine("Entro aca bien?");
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
        public async Task<IQueryable<Resin2>> Consultar(Expression<Func<Resin2, bool>> filtro = null)
        {
            IQueryable<Resin2> queryEntidad = filtro == null
                    ? _dbContext.Resin2s.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Resin2s.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Resin2s.Count();
            }
            catch
            {
                throw;
            }
        }
    }
}
