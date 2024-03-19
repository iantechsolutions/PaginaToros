using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Net.Mail;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class Solici1Repositorio : ISolici1Repositorio
    {
        private readonly hereford_prContext _dbContext;

        public Solici1Repositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Solici1>> Lista(int skip, int take)
        {

            try
            {

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Solici1s.Include(t => t.Establecimiento).ThenInclude(e => e.Socio)
                                                .Include(t => t.Establecimiento).ThenInclude(e => e.Provincia)
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


        public async Task<Solici1> Obtener(Expression<Func<Solici1, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Solici1s.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Solici1>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                List<Solici1> a;
                if (filtro is not null) { 
                    a = await _dbContext.Solici1s.Include(t=>t.Establecimiento).ThenInclude(e=>e.Socio).Include(t => t.Establecimiento).ThenInclude(e => e.Provincia).Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    a = await _dbContext.Solici1s.Include(t => t.Establecimiento).ThenInclude(e => e.Socio).Include(t => t.Establecimiento).ThenInclude(e => e.Provincia).Skip(skip).ToListAsync();
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

        public async Task<List<Solici1>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null)
        {
            try
            {
                List<Solici1> a;
                if (filtro is not null)
                {
                    a = await _dbContext.Solici1s.Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    a = await _dbContext.Solici1s.Skip(skip).ToListAsync();
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

        public async Task<bool> Eliminar(Solici1 entidad)
        {
            try
            {
                _dbContext.Solici1s.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Solici1> Crear(Solici1 entidad)
        {
            try
            {
                Console.WriteLine(entidad.Anio);
                Console.WriteLine(entidad.Cantor);
                Console.WriteLine(entidad.Cantvq);
                Console.WriteLine(entidad.Canvac);
                Console.WriteLine(entidad.Canvaq);

                _dbContext.Set<Solici1>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Solici1 entidad)
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
        public async Task<IQueryable<Solici1>> Consultar(Expression<Func<Solici1, bool>> filtro = null)
        {
            IQueryable<Solici1> queryEntidad = filtro == null
                    ? _dbContext.Solici1s.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Solici1s.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Solici1s.Count();
            }
            catch
            {
                throw;
            }
        }

    }
}
