using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class Resin8Repositorio : IResin8Repositorio
    {
        private readonly hereford_prContext _dbContext;

        public Resin8Repositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Resin8>> Lista(int skip, int take)
        {

            try
            {

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Resin8s
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


        public async Task<Resin8> Obtener(Expression<Func<Resin8, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Resin8s.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Resin8>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                var a = await _dbContext.Resin8s.Where(filtro).Skip(skip).ToListAsync();
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
        

        public async Task<bool> Eliminar(Resin8 entidad)
        {
            try
            {
                _dbContext.Resin8s.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Resin8> Crear(Resin8 entidad)
        {
            try
            {
                _dbContext.Set<Resin8>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Resin8 entidad)
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
        public async Task<IQueryable<Resin8>> Consultar(Expression<Func<Resin8, bool>> filtro = null)
        {
            IQueryable<Resin8> queryEntidad = filtro == null
                    ? _dbContext.Resin8s.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Resin8s.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Resin8s.Count();
            }
            catch
            {
                throw;
            }
        }
    }
}
