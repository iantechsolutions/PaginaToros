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

        private IQueryable<Solici1> QuerySolicitudesValidas(bool includeRelations = true)
        {
            IQueryable<Solici1> query = _dbContext.Solici1s
                .Where(s => !string.IsNullOrWhiteSpace(s.Codest))
                .Where(s => _dbContext.Estables.Any(e => e.Ecod == s.Codest));

            if (includeRelations)
            {
                query = query
                    .Include(t => t.Establecimiento).ThenInclude(e => e.Socio)
                    .Include(t => t.Establecimiento).ThenInclude(e => e.Provincia);
            }

            return query;
        }

        public async Task<List<Solici1>> Lista(int skip, int take)
        {

            try
            {
                IQueryable<Solici1> query = ApplyCreationOrder(QuerySolicitudesValidas(includeRelations: true));

                if (skip > 0)
                {
                    query = query.Skip(skip);
                }

                if (take > 0)
                {
                    query = query.Take(take);
                }

                return await query.ToListAsync();
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
                IQueryable<Solici1> query = QuerySolicitudesValidas(includeRelations: true);

                if (filtro is not null)
                {
                    query = query.Where(filtro);
                }

                query = ApplyCreationOrder(query);

                if (skip > 0)
                {
                    query = query.Skip(skip);
                }

                if (take > 0)
                {
                    query = query.Take(take);
                }

                return await query.ToListAsync();
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
                IQueryable<Solici1> query = _dbContext.Solici1s.AsNoTracking();
                if (filtro is not null)
                {
                    query = query.Where(filtro);
                }

                query = ApplyCreationOrder(query);

                if (skip > 0)
                {
                    query = query.Skip(skip);
                }

                if (take > 0)
                {
                    query = query.Take(take);
                }

                return await query.ToListAsync();
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
                return await QuerySolicitudesValidas(includeRelations: false).CountAsync();
            }
            catch
            {
                throw;
            }
        }

        private static IOrderedQueryable<Solici1> ApplyCreationOrder(IQueryable<Solici1> query)
            => query
                .OrderByDescending(x => x.FchUsu.HasValue)
                .ThenByDescending(x => x.FchUsu)
                .ThenByDescending(x => x.Id);

    }
}
