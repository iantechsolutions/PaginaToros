using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Helpers;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using static System.Net.WebRequestMethods;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class PlantelRepositorio : IPlantelRepositorio
    {
        private readonly hereford_prContext _dbContext;

        public PlantelRepositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Plantel>> Lista(int skip, int take)
        {

            try
            {
                var items = await _dbContext.Planteles
                    .AsNoTracking()
                    .Include(t => t.Socio)
                    .ToListAsync();

                return ApplyOrdering(items, skip, take, preferYearOrdering: false);
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Plantel>> ObtenerPorRangoFechas(DateTime desde, DateTime hasta)
        {
            try
            {
                var yDesde = desde.Year.ToString("0000");
                var yHasta = hasta.Year.ToString("0000");

                return await _dbContext.Planteles
                    .Include(p => p.Socio)
                    .Where(p => p.Anioex != null
                             && p.Anioex.Length == 4
                             && string.Compare(p.Anioex, yDesde) >= 0
                             && string.Compare(p.Anioex, yHasta) <= 0)
                    .OrderByDescending(p => p.Urein)
                    .ToListAsync();
            }
            catch
            {
                throw;
            }
        }


        public async Task<Plantel> Obtener(Expression<Func<Plantel, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Planteles.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Plantel>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                // Build query and apply filters on the database side
                IQueryable<Plantel> query = _dbContext.Planteles.AsNoTracking().Include(x => x.Socio);
                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    query = query.Where(filtro);
                }

                var items = await query.ToListAsync();
                return ApplyOrdering(items, skip, take, preferYearOrdering: ShouldPreferYearOrdering(filtro));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Página del listado de planteles con el total del conjunto filtrado. El orden se aplica
        /// sobre todo el conjunto filtrado (en memoria, igual que Lista/LimitadosFiltrados, porque
        /// la fecha de alta sale de FECING, que es texto) y los duplicados se descartan antes de
        /// contar, así el total coincide con lo que se puede paginar.
        /// </summary>
        public async Task<(List<Plantel> Items, int TotalCount)> SearchPagedAsync(
            int skip,
            int take,
            string? filtro = null,
            string? sortBy = null,
            bool sortDescending = true)
        {
            if (take <= 0)
            {
                take = 15;
            }

            IQueryable<Plantel> query = _dbContext.Planteles.AsNoTracking().Include(x => x.Socio);
            if (!string.IsNullOrWhiteSpace(filtro))
            {
                query = query.Where(filtro);
            }

            var items = await query.ToListAsync();
            var ordered = Deduplicate(ApplySearchOrder(items, sortBy, sortDescending, ShouldPreferYearOrdering(filtro)));

            return (ordered.Skip(Math.Max(0, skip)).Take(take).ToList(), ordered.Count);
        }

        // Campos por los que se puede ordenar el listado (SearchPaged). Coinciden con la Property
        // de las columnas de Client/Pages/Planteles/Planteles.razor.
        public const string SortFechaAlta = "FechaAlta";
        public const string SortPlantel = "Placod";
        public const string SortActivo = "Estado";
        public const string SortAnioExistencia = "Anioex";
        public const string SortSocio = "Socio.Nombre";

        private static readonly IReadOnlyDictionary<string, Func<Plantel, object?>> SortKeys =
            new Dictionary<string, Func<Plantel, object?>>(StringComparer.Ordinal)
            {
                [SortPlantel] = x => x.Placod,
                [SortActivo] = x => x.Estado,
                [SortAnioExistencia] = x => InMemorySort.ParseYear(x.Anioex),
                [SortSocio] = x => x.Socio?.Nombre,
                [nameof(Plantel.Varede)] = x => x.Varede,
                [nameof(Plantel.Vqcsrd)] = x => x.Vqcsrd,
                [nameof(Plantel.Vqssrd)] = x => x.Vqssrd,
                [nameof(Plantel.Varepr)] = x => x.Varepr,
                [nameof(Plantel.Vqcsrp)] = x => x.Vqcsrp,
                [nameof(Plantel.Vqssrp)] = x => x.Vqssrp,
            };

        /// <summary>
        /// Por defecto (o con "FechaAlta" descendente) se conserva el orden histórico del listado
        /// (<see cref="ApplyOrdering"/>: fecha de alta, año, id). El resto de los campos sigue las
        /// reglas de <see cref="InMemorySort"/>: vacíos al final y empates por id descendente.
        /// </summary>
        public static List<Plantel> ApplySearchOrder(IEnumerable<Plantel> items, string? sortBy, bool descending, bool preferYearOrdering)
        {
            var field = sortBy?.Trim();
            if (string.IsNullOrEmpty(field) || !(field == SortFechaAlta || SortKeys.ContainsKey(field)))
            {
                return ApplyOrdering(items, 0, 0, preferYearOrdering);
            }

            if (field == SortFechaAlta)
            {
                return descending
                    ? ApplyOrdering(items, 0, 0, preferYearOrdering)
                    : InMemorySort.OrderNullsLast(items, x => GetPlantelCreationDate(x), descending: false, x => x.Id);
            }

            return InMemorySort.OrderNullsLast(items, SortKeys[field], descending, x => x.Id);
        }

        // Misma clave que PlantelController.Deduplicate: se queda con el primero según el orden.
        private static List<Plantel> Deduplicate(IEnumerable<Plantel> items)
            => items
                .GroupBy(p =>
                {
                    var key = ((p.Placod ?? string.Empty) + "|" + (p.Anioex ?? string.Empty) + "|" + (p.Nrocri ?? string.Empty)).Trim();
                    return string.IsNullOrEmpty(key) ? "ID|" + p.Id : "P|" + key;
                })
                .Select(g => g.First())
                .ToList();

        public async Task<List<Plantel>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null)
        {
            try
            {
                IQueryable<Plantel> query = _dbContext.Planteles.AsNoTracking();
                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    query = query.Where(filtro);
                }

                var items = await query.ToListAsync();
                return ApplyOrdering(items, skip, take, preferYearOrdering: ShouldPreferYearOrdering(filtro));
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(Plantel entidad)
        {
            try
            {
                _dbContext.Planteles.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Plantel> Crear(Plantel entidad)
        {
            try
            {
                _dbContext.Set<Plantel>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Plantel entidad)
        {
            try
            {
                _dbContext.Update(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Editar: {ex.Message}");
                throw;
            }
        }

        public async Task<IQueryable<Plantel>> Consultar(Expression<Func<Plantel, bool>> filtro = null)
        {
            IQueryable<Plantel> queryEntidad = filtro == null
                    ? _dbContext.Planteles.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Planteles.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Planteles.Count();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Plantel>> ObtenerPorAnios(int anio1, int anio2)
        {
            try
            {
                var todos = await _dbContext.Planteles
                    .Include(p => p.Socio)
                    .ToListAsync();

                var filtrados = todos
                    .Where(x =>
                        !string.IsNullOrEmpty(x.Anioex) &&
                        int.TryParse(x.Anioex, out var anio) &&
                        anio >= anio1 && anio <= anio2)
                    .OrderByDescending(x => x.Anioex)
                    .ThenBy(x => x.Placod)
                    .ToList();

                return filtrados;
            }
            catch
            {
                throw;
            }
        }

        private static List<Plantel> ApplyOrdering(IEnumerable<Plantel> items, int skip, int take, bool preferYearOrdering)
        {
            IOrderedEnumerable<Plantel> ordered = preferYearOrdering
                ? items
                    .OrderByDescending(GetPlantelCreationYear)
                    .ThenByDescending(GetPlantelCreationDate)
                    .ThenByDescending(t => t.Id)
                : items
                    .OrderByDescending(GetPlantelCreationDate)
                    .ThenByDescending(GetPlantelCreationYear)
                    .ThenByDescending(t => t.Id);

            if (take == 0)
            {
                return ordered.ToList();
            }

            return ordered.Skip(skip).Take(take).ToList();
        }

        private static bool ShouldPreferYearOrdering(string? filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
            {
                return false;
            }

            return filtro.Contains("Socio.Id", StringComparison.OrdinalIgnoreCase)
                || filtro.Contains("Nrocri", StringComparison.OrdinalIgnoreCase);
        }

        private static DateTime? GetPlantelCreationDate(Plantel plantel)
            => plantel == null ? null : PlantelFechaAlta.Obtener(plantel.Fecing, plantel.FchUsu);

        private static int? GetPlantelCreationYear(Plantel plantel)
        {
            if (!string.IsNullOrWhiteSpace(plantel?.Anioex) && int.TryParse(plantel.Anioex, out var year))
            {
                return year;
            }

            return null;
        }


    }
    

}
