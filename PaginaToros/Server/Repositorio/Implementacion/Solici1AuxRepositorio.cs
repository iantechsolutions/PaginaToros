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
    public class Solici1AuxRepositorio : ISolici1AuxRepositorio
    {
        private readonly hereford_prContext _dbContext;

        public Solici1AuxRepositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Solici1Aux>> Lista(int skip, int take)
        {

            try
            {

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Solici1Auxs.Include(t => t.Solicitud)
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


        public async Task<Solici1Aux> Obtener(Expression<Func<Solici1Aux, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Solici1Auxs.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Solici1Aux>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                List<Solici1Aux> a;
                if (filtro is not null) { 
                    a = await _dbContext.Solici1Auxs.Include(t=>t.Solicitud).Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    a = await _dbContext.Solici1Auxs.Include(t => t.Solicitud).Skip(skip).ToListAsync();
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

        public async Task<List<Solici1Aux>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null)
        {
            try
            {
                List<Solici1Aux> a;
                if (filtro is not null)
                {
                    a = await _dbContext.Solici1Auxs.Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    a = await _dbContext.Solici1Auxs.Skip(skip).ToListAsync();
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

        public async Task<bool> Eliminar(Solici1Aux entidad)
        {
            try
            {
                _dbContext.Solici1Auxs.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Solici1Aux> Crear(Solici1Aux entidad)
        {
            try
            {
                Console.WriteLine(entidad.Anio);
                Console.WriteLine(entidad.Cantor);
                Console.WriteLine(entidad.Cantvq);
                Console.WriteLine(entidad.Canvac);
                Console.WriteLine(entidad.Canvaq);

                _dbContext.Set<Solici1Aux>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Solici1Aux entidad)
        {
            Console.WriteLine(entidad.Anio);
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
        public async Task<IQueryable<Solici1Aux>> Consultar(Expression<Func<Solici1Aux, bool>> filtro = null)
        {
            IQueryable<Solici1Aux> queryEntidad = filtro == null
                    ? _dbContext.Solici1Auxs.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Solici1Auxs.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Solici1Auxs.Count();
            }
            catch
            {
                throw;
            }
        }

    }
}
