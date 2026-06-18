using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.Json;

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
                return await ApplyCreationOrder(_dbContext.Transans)
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
                IQueryable<Transan> query = _dbContext.Transans;

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    query = query.Where(filtro);
                }

                query = ApplyCreationOrder(query)
                    .Skip(skip);

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
                Console.WriteLine("Entrar entro");
                Console.WriteLine(JsonSerializer.Serialize(entidad, new JsonSerializerOptions { WriteIndented = true }));

                _dbContext.Set<Transan>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar en base de datos:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException?.Message); // si hay error interno
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

        private static IOrderedQueryable<Transan> ApplyCreationOrder(IQueryable<Transan> query)
            // El orden estable de la grilla debe seguir la fecha real de proceso.
            // FchUsu puede ser nula en datos históricos, por eso quedan al final.
            => query
                .OrderByDescending(x => x.FchUsu.HasValue)
                .ThenByDescending(x => x.FchUsu)
                .ThenByDescending(x => x.Id);
    }
}
