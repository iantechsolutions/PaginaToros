using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class TranssbRepositorio : ITranssbRepositorio
    {
        private readonly hereford_prContext _dbContext;

        public TranssbRepositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Transsb>> Lista(int skip, int take)
        {

            try
            {

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Transsbs.Include(x=>x.Establecimiento)
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


        public async Task<Transsb> Obtener(Expression<Func<Transsb, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Transsbs.Include(x => x.Establecimiento).Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Transsb>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                var a = await _dbContext.Transsbs.Include(t=>t.Establecimiento).Where(filtro).Skip(skip).ToListAsync();
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

        public async Task<bool> Eliminar(Transsb entidad)
        {
            try
            {
                _dbContext.Transsbs.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Transsb> Crear(Transsb entidad)
        {
            try
            {
                _dbContext.Set<Transsb>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Transsb entidad)
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
        public async Task<IQueryable<Transsb>> Consultar(Expression<Func<Transsb, bool>> filtro = null)
        {
            IQueryable<Transsb> queryEntidad = filtro == null
                    ? _dbContext.Transsbs.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Transsbs.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Transsbs.Count();
            }
            catch
            {
                throw;
            }
        }
    }
}
