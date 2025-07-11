﻿using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class Desepla1Repositorio : IDesepla1Repositorio
    {
        private readonly hereford_prContext _dbContext;

        public Desepla1Repositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Desepla1>> Lista(int skip, int take)
        {
            try
            {
                var ids = await _dbContext.Desepla1s
                    .OrderByDescending(t => t.Nrodec)
                    .Select(x => x.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                var result = await _dbContext.Desepla1s
                    .Where(x => ids.Contains(x.Id))
                    .Include(x => x.Plantel)
                    .Include(x => x.Socio)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task<Desepla1> Obtener(Expression<Func<Desepla1, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Desepla1s.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Desepla1>> LimitadosFiltrados(int skip, int take, string? filtro = null)
        {
            try
                {
                DbSet<Desepla1> a;
                List<Desepla1> b;
                if(filtro is not null)
                {
                    b = await _dbContext.Desepla1s.Include(x => x.Socio).Include(a => a.Plantel).Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    b = await _dbContext.Desepla1s.Include(x => x.Socio).Include(a => a.Plantel).Skip(skip).ToListAsync();
                }
                if (take == 0)
                {
                    return b.OrderByDescending(t => t.Nrodec).ToList();
                }
                else
                {
                    return b.Take(take).OrderByDescending(t => t.Nrodec).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Desepla1>> LimitadosFiltradosNoInclude(int skip, int take, string? filtro = null)
        {
            try
            {
                DbSet<Desepla1> a;
                List<Desepla1> b;
                if (filtro is not null)
                {
                    b = await _dbContext.Desepla1s.Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    b = await _dbContext.Desepla1s.Skip(skip).ToListAsync();
                }
                if (take == 0)
                {
                    return b.OrderByDescending(t => t.Nrodec).ToList();
                }
                else
                {
                    return b.Take(take).OrderByDescending(t => t.Nrodec).ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        
        public async Task<bool> Eliminar(Desepla1 entidad)
        {
            try
            {
                _dbContext.Desepla1s.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Desepla1> Crear(Desepla1 entidad)
        {
            try
            {
                _dbContext.Set<Desepla1>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Desepla1 entidad)
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
        public async Task<IQueryable<Desepla1>> Consultar(Expression<Func<Desepla1, bool>> filtro = null)
        {
            IQueryable<Desepla1> queryEntidad = filtro == null
                    ? _dbContext.Desepla1s.Take(30) 
                    : _dbContext.Desepla1s.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Desepla1s.Count();
            }
            catch
            {
                throw;
            }
        }
    }
}
