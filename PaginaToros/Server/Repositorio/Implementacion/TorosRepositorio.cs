using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class TorosRepositorio : ITorosRepositorio
    {
        private readonly hereford_prContext _dbContext;

        public TorosRepositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ========= Helpers =========
        private static string NormalizeDynamicFilter(string? filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro)) return null!;

            var f = filtro
                .Replace("!=", "!=")
                .Replace(">=", ">=")
                .Replace("<=", "<=");

            f = f.Replace(" = ", " == ");
            f = f.Replace("= ", "== ");
            f = f.Replace(" =", " ==");

            f = f.Replace("Socio.Id=", "Socio.Id == ");
            f = f.Replace("CodEstado=", "CodEstado == ");
            f = f.Replace("TipToro=", "TipToro == ");
            f = f.Replace("Variedad=", "Variedad == ");
            return f;
        }

        private IQueryable<Torosuni> BaseQuery(bool includeSocio = true)
        {
            var q = _dbContext.Torosunis
                .AsNoTracking()
                .AsSplitQuery();

            if (includeSocio)
                q = q.Include(t => t.Socio);

            // Orden estable para TODAS las consultas
            q = q.OrderBy(x => x.CodEstado == "1" ? 0 : 1)
                 .ThenByDescending(x => x.Id);

            return q;
        }

        // ========= CRUD/Queries =========

        public async Task<List<Torosuni>> Lista(int skip, int take)
        {
            try
            {
                if (take == 0) take = int.MaxValue;

                return await BaseQuery(includeSocio: true)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Torosuni> Obtener(Expression<Func<Torosuni, bool>> filtro = null!)
        {
            try
            {
                return await BaseQuery(includeSocio: true).FirstOrDefaultAsync(filtro);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Torosuni>> LimitadosFiltrados(int skip, int take, string filtro = null!)
        {
            try
            {
                if (take == 0) take = int.MaxValue;

                var q = BaseQuery(includeSocio: true);

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    var f = NormalizeDynamicFilter(filtro);
                    q = q.Where(f);
                }

                return await q
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Torosuni>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null!)
        {
            try
            {
                if (take == 0) take = int.MaxValue;

                var q = BaseQuery(includeSocio: false);

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    var f = NormalizeDynamicFilter(filtro);
                    q = q.Where(f);
                }

                return await q
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(Torosuni entidad)
        {
            try
            {
                _dbContext.Torosunis.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Torosuni> Crear(Torosuni entidad)
        {
            try
            {
                _dbContext.Set<Torosuni>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Torosuni entidad)
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

        public async Task<IQueryable<Torosuni>> Consultar(Expression<Func<Torosuni, bool>> filtro = null!)
        {
            // Alineado al resto de repos: devolvemos la query base con orden estable.
            var q = BaseQuery(includeSocio: true);
            if (filtro != null)
                q = q.Where(filtro);

            return await Task.FromResult(q);
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return await _dbContext.Torosunis.CountAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> CantidadFiltrada(string filtro = null!)
        {
            try
            {
                var q = _dbContext.Torosunis.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    var f = NormalizeDynamicFilter(filtro);
                    q = q.Where(f);
                }

                return await q.CountAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
