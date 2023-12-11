using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class TransanRepositorio : ITransanRepositorio
    {
        private readonly hereford_prContext _dbContext;

        public TransanRepositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Transan>> Lista(int skip, int take)
        {

            try
            {

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Transans
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


        public async Task<Transan> Obtener(Expression<Func<Transan, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Transans.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Transan>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                var a = await _dbContext.Transans.Where(filtro).Skip(skip).ToListAsync();
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

        public async Task<bool> Eliminar(Transan entidad)
        {
            try
            {
                _dbContext.Transans.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Transan> Crear(Transan entidad)
        {
            try
            {
                _dbContext.Set<Transan>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Transan entidad)
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
        public async Task<IQueryable<Transan>> Consultar(Expression<Func<Transan, bool>> filtro = null)
        {
            IQueryable<Transan> queryEntidad = filtro == null
                    ? _dbContext.Transans.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Transans.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Transans.Count();
            }
            catch
            {
                throw;
            }
        }
    }
}
