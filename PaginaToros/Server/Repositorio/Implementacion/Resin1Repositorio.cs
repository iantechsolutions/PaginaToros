using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class Resin1Repositorio : IResin1Repositorio
    {
        private readonly hereford_prContext _dbContext;

        public Resin1Repositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Resin1>> Lista(int skip, int take)
        {

            try
            {
                IQueryable<Resin1> query = ApplyCreationOrder(
                    _dbContext.Resin1s
                        .AsNoTracking()
                        .Include(t => t.Socio)
                        .Include(x => x.Establecimiento));

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


        public async Task<Resin1> Obtener(Expression<Func<Resin1, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Resin1s.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Resin1>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                IQueryable<Resin1> query = _dbContext.Resin1s
                    .AsNoTracking()
                    .Include(t => t.Socio)
                    .Include(x => x.Establecimiento);

                if (!string.IsNullOrWhiteSpace(filtro))
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

                var a = await query.ToListAsync();
                a = RemoveDuplicates(a);
                return a;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Resin1>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null)
        {
            try
            {
                IQueryable<Resin1> query = _dbContext.Resin1s.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(filtro))
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

                var a = await query.ToListAsync();
                a = RemoveDuplicates(a);
                return a;
            }
            catch
            {
                throw;
            }
        }

        public async Task<(List<Resin1> Items, int TotalCount)> SearchPagedAsync(
            int skip,
            int take,
            string? searchText = null,
            string? activeSocioCode = null,
            string? sortBy = null,
            bool sortDescending = true)
        {
            try
            {
                if (take <= 0)
                {
                    take = 15;
                }

                IQueryable<Resin1> query = _dbContext.Resin1s
                    .AsNoTracking()
                    .Include(x => x.Socio)
                    .Include(x => x.Establecimiento);

                if (!string.IsNullOrWhiteSpace(activeSocioCode))
                {
                    query = query.Where(x => x.Scod == activeSocioCode);
                }

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    query = await ApplySearchFilterAsync(query, searchText);
                }

                var totalCount = await query.CountAsync();
                var items = await ApplySearchOrder(query, sortBy, sortDescending)
                    .Skip(Math.Max(0, skip))
                    .Take(take)
                    .ToListAsync();

                return (items, totalCount);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(Resin1 entidad)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    Console.WriteLine("Entro perfectop");

                    _dbContext.Resin2s.RemoveRange(_dbContext.Resin2s.Where(x => x.Nrores == entidad.Nrores));
                    _dbContext.Resin3s.RemoveRange(_dbContext.Resin3s.Where(x => x.Nrores == entidad.Nrores));
                    _dbContext.Resin4s.RemoveRange(_dbContext.Resin4s.Where(x => x.Nrores == entidad.Nrores));
                    _dbContext.Resin6s.RemoveRange(_dbContext.Resin6s.Where(x => x.Nrores == entidad.Nrores));
                    _dbContext.Resin8s.RemoveRange(_dbContext.Resin8s.Where(x => x.Nrores == entidad.Nrores));

                    _dbContext.Resin1s.Remove(entidad);

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.Error.WriteLine($"Error eliminando registros: {ex.Message}");
                    throw;
                }
            });
        }



        public async Task<Resin1> Crear(Resin1 entidad)
        {
            try
            {
                _dbContext.Set<Resin1>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }
        public async Task<int?> ObtenerUltimoNrores()
        {
            try
            {
                var values = await _dbContext.Resin1s
                    .AsNoTracking()
                    .Where(r => r.Nrores != null)
                    .Select(r => r.Nrores)
                    .ToListAsync();

                return values
                    .Select(v => int.TryParse(v, out var nro) ? (int?)nro : null)
                    .Where(v => v.HasValue)
                    .Max();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en ObtenerUltimoNrores: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> Editar(Resin1 entidad)
        {

            Console.WriteLine("Entro al editar");

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
        public async Task<IQueryable<Resin1>> Consultar(Expression<Func<Resin1, bool>> filtro = null)
        {
            IQueryable<Resin1> queryEntidad = filtro == null
                    ? _dbContext.Resin1s.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Resin1s.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Resin1s.Count();
            }
            catch
            {
                throw;
            }
        }
        public static List<Resin1> RemoveDuplicates(List<Resin1> items)
        {
            var seenIds = new HashSet<int>();
            var uniqueItems = new List<Resin1>();

            foreach (var item in items)
            {
                Console.WriteLine(item.Id);
                var boole = seenIds.Add(item.Id);
                Console.WriteLine(boole);
                if (boole)  // HashSet.Add returns false if the item was already in the set
                {
                    uniqueItems.Add(item);
                }
            }
            Console.WriteLine(uniqueItems.Count());
            return uniqueItems;
        }

        // Campos por los que se puede ordenar el listado de resultados (SearchPaged). Cualquier
        // otro valor de sortBy cae en el orden por defecto.
        public const string SortOrdenCarga = "Id";
        public const string SortNroResultado = "Nrores";
        public const string SortPlantel = "Nropla";
        public const string SortSocio = "Socio.Nombre";
        public const string SortEstablecimiento = "Establecimiento.Nombre";
        public const string SortFechaCreacion = "FchUsu";
        public const string SortFechaInspeccion = "Freali";

        /// <summary>
        /// Orden del listado paginado. Por defecto, el último resultado cargado primero: se usa el
        /// id (AUTO_INCREMENT) porque FCH_USU no es confiable como fecha de carga — los resultados
        /// cargados por la app antes de junio de 2026 lo tienen en NULL.
        /// Reglas comunes: los vacíos van siempre al final (en ambas direcciones) y los empates se
        /// resuelven por el último cargado primero.
        /// </summary>
        public static IOrderedQueryable<Resin1> ApplySearchOrder(IQueryable<Resin1> query, string? sortBy, bool descending)
        {
            IOrderedQueryable<Resin1> ordered;
            switch (sortBy?.Trim())
            {
                case SortNroResultado:
                    // NRORES son números de 6 dígitos con ceros a la izquierda (o un GUID en los
                    // cargados sin solicitud): largo y después texto da el orden numérico.
                    ordered = ThenBy(
                        ThenBy(query.OrderBy(x => x.Nrores == null || x.Nrores == ""), x => x.Nrores!.Length, descending),
                        x => x.Nrores,
                        descending);
                    break;
                case SortPlantel:
                    ordered = ThenBy(query.OrderBy(x => x.Nropla == null || x.Nropla == ""), x => x.Nropla, descending);
                    break;
                case SortSocio:
                    ordered = ThenBy(
                        query.OrderBy(x => x.Socio == null || x.Socio.Nombre == null || x.Socio.Nombre == ""),
                        x => x.Socio!.Nombre,
                        descending);
                    break;
                case SortEstablecimiento:
                    ordered = ThenBy(
                        query.OrderBy(x => x.Establecimiento == null || x.Establecimiento.Nombre == null || x.Establecimiento.Nombre == ""),
                        x => x.Establecimiento!.Nombre,
                        descending);
                    break;
                case SortFechaCreacion:
                    ordered = ThenBy(query.OrderBy(x => x.FchUsu == null), x => x.FchUsu, descending);
                    break;
                case SortFechaInspeccion:
                    ordered = ThenBy(query.OrderBy(x => x.Freali == null), x => x.Freali, descending);
                    break;
                case SortOrdenCarga:
                    return descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
                default:
                    return query.OrderByDescending(x => x.Id);
            }

            return ordered.ThenByDescending(x => x.Id);
        }

        private static IOrderedQueryable<Resin1> ThenBy<TKey>(
            IOrderedQueryable<Resin1> query,
            Expression<Func<Resin1, TKey>> keySelector,
            bool descending)
            => descending ? query.ThenByDescending(keySelector) : query.ThenBy(keySelector);

        private static IOrderedQueryable<Resin1> ApplyCreationOrder(IQueryable<Resin1> query)
            => query
                .OrderByDescending(x => x.FchUsu.HasValue)
                .ThenByDescending(x => x.FchUsu)
                .ThenByDescending(x => x.Freali.HasValue)
                .ThenByDescending(x => x.Freali.HasValue ? x.Freali.Value.Year : (int?)null)
                .ThenByDescending(x => x.Id);

        // RESIN1.SCOD / RESIN1.ESTCOD son latin1 mientras que SOCIOS.SCOD y ESTABLE.ECOD son utf8,
        // asi que MariaDB tiene que convertir en cada comparacion y el JOIN termina siendo un scan
        // completo (block nested loop) de las tres tablas. Filtrar por columnas de SOCIOS/ESTABLE
        // dentro de esa misma consulta multiplicaba las filas evaluadas y la busqueda tardaba ~15s
        // (en produccion supera el command timeout y devuelve 500). Por eso resolvemos primero que
        // codigos matchean, con dos consultas chicas contra cada tabla, y despues filtramos RESIN1
        // unicamente por sus propias columnas.
        private async Task<IQueryable<Resin1>> ApplySearchFilterAsync(IQueryable<Resin1> query, string searchText)
        {
            var terms = searchText
                .Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            foreach (var term in terms)
            {
                var like = $"%{term}%";

                var socioCodes = await _dbContext.Socios
                    .AsNoTracking()
                    .Where(s =>
                        (s.Prenom != null && EF.Functions.Like(s.Prenom, like)) ||
                        (s.Nombre != null && EF.Functions.Like(s.Nombre, like)) ||
                        (s.Posnom != null && EF.Functions.Like(s.Posnom, like)) ||
                        (s.Mail != null && EF.Functions.Like(s.Mail, like)) ||
                        (s.Scod != null && EF.Functions.Like(s.Scod, like)) ||
                        (s.Codpos2 != null && EF.Functions.Like(s.Codpos2, like)))
                    .Select(s => s.Scod!)
                    .Distinct()
                    .ToListAsync();

                var estableCodes = await _dbContext.Estables
                    .AsNoTracking()
                    .Where(e => e.Ecod != null)
                    .Where(e =>
                        EF.Functions.Like(e.Ecod!, like) ||
                        (e.Nombre != null && EF.Functions.Like(e.Nombre, like)) ||
                        (e.Codsoc != null && EF.Functions.Like(e.Codsoc, like)))
                    .Select(e => e.Ecod!)
                    .Distinct()
                    .ToListAsync();

                var hasFecha = TryParseFecha(term, out var desde);
                var hasta = hasFecha ? desde.AddDays(1) : desde;
                var hasAnio = TryParseAnio(term, out var anio);

                query = query.Where(x =>
                    (x.Nrores != null && EF.Functions.Like(x.Nrores, like)) ||
                    (x.Nropla != null && EF.Functions.Like(x.Nropla, like)) ||
                    EF.Functions.Like(x.Scod, like) ||
                    (x.Estcod != null && EF.Functions.Like(x.Estcod, like)) ||
                    socioCodes.Contains(x.Scod) ||
                    (x.Estcod != null && estableCodes.Contains(x.Estcod)) ||
                    (hasFecha && x.FchUsu >= desde && x.FchUsu < hasta) ||
                    (hasFecha && x.Freali >= desde && x.Freali < hasta) ||
                    (hasAnio && x.FchUsu.HasValue && x.FchUsu.Value.Year == anio) ||
                    (hasAnio && x.Freali.HasValue && x.Freali.Value.Year == anio));
            }

            return query;
        }

        // La grilla muestra las fechas como dd/MM/yyyy, asi que hay que aceptar ese formato
        // explicitamente: DateTime.TryParse depende de la cultura del proceso y en el servidor
        // interpretaba "27/06/2026" como mes 27 y descartaba el termino.
        private static readonly string[] FormatosFecha =
        {
            "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "d-M-yyyy",
            "yyyy-MM-dd", "yyyy/MM/dd", "dd/MM/yy", "d/M/yy"
        };

        private static bool TryParseFecha(string term, out DateTime fecha)
        {
            if (DateTime.TryParseExact(term, FormatosFecha, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out fecha))
            {
                fecha = fecha.Date;
                return true;
            }

            fecha = default;
            return false;
        }

        private static bool TryParseAnio(string term, out int anio)
        {
            anio = 0;

            return term.Length == 4
                && int.TryParse(term, NumberStyles.None, CultureInfo.InvariantCulture, out anio)
                && anio >= 1900
                && anio <= 2200;
        }
    }
}
