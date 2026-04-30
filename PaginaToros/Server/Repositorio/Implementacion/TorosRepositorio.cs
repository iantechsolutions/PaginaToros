using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class TorosRepositorio : ITorosRepositorio
    {
        private static readonly string[] ActiveEstadoCodes = new[] { "1", "2", "3", "4", "5" };
        private static readonly string[] TipoToroCodes = new[] { "P", "S", "GP", "A" };

        private readonly hereford_prContext _dbContext;

        public TorosRepositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }

        private static string NormalizeDynamicFilter(string? filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return null!;

            var f = filtro.Trim();
            f = Regex.Replace(f, @"(?<!=)=(?!=)", "==");
            f = f.Replace("Socio.Id==", "Socio.Id == ")
                 .Replace("CodEstado==", "CodEstado == ")
                 .Replace("TipToro==", "TipToro == ")
                 .Replace("Variedad==", "Variedad == ");

            return f;
        }

        private IQueryable<Torosuni> BaseQuery(
            bool includeSocio = true,
            bool includeEstablecimiento = false,
            bool prioritizeRecentFirst = false)
        {
            var query = _dbContext.Torosunis
                .AsNoTracking()
                .AsSplitQuery();

            if (includeSocio)
            {
                query = query.Include(t => t.Socio);
            }

            if (includeEstablecimiento)
            {
                query = query.Include(t => t.Establecimiento);
            }

            return ApplyDefaultOrdering(query, prioritizeRecentFirst);
        }

        private static IQueryable<Torosuni> ApplyDefaultOrdering(
            IQueryable<Torosuni> query,
            bool prioritizeRecentFirst)
        {
            if (prioritizeRecentFirst)
            {
                return query
                    .OrderByDescending(BuildFechaSSortKey())
                    .ThenByDescending(t => t.FchUsu ?? t.Fecha)
                    .ThenByDescending(t => t.Id);
            }

            return query
                .OrderByDescending(BuildFechaSSortKey())
                .ThenBy(t => t.CodEstado == "1" ? 0 : 1)
                .ThenBy(t => t.NomDad)
                .ThenByDescending(t => t.Id);
        }

        private static Expression<Func<Torosuni, string>> BuildFechaSSortKey()
        {
            return t => t.Fechasba != null && t.Fechasba.Length >= 10
                ? t.Fechasba.Substring(6, 4) + t.Fechasba.Substring(3, 2) + t.Fechasba.Substring(0, 2)
                : string.Empty;
        }

        public async Task<List<Torosuni>> Lista(int skip, int take)
        {
            if (take == 0)
            {
                take = int.MaxValue;
            }

            return await BaseQuery(includeSocio: true, includeEstablecimiento: true)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<Respuesta<Torosuni>> GetById(int id)
        {
            var toro = await BaseQuery(includeSocio: true, includeEstablecimiento: true)
                .FirstOrDefaultAsync(x => x.Id == id);

            return new Respuesta<Torosuni>
            {
                Exito = toro != null ? 1 : 0,
                Mensaje = toro != null ? "Éxito" : $"No existe toro con Id {id}",
                List = toro
            };
        }

        public async Task<Torosuni> Obtener(Expression<Func<Torosuni, bool>> filtro = null!)
        {
            return await BaseQuery(includeSocio: true, includeEstablecimiento: true)
                .FirstOrDefaultAsync(filtro);
        }

        public async Task<List<Torosuni>> LimitadosFiltrados(int skip, int take, string filtro = null!)
        {
            if (take == 0)
            {
                take = int.MaxValue;
            }

            var query = BaseQuery(includeSocio: true, includeEstablecimiento: true);
            if (!string.IsNullOrWhiteSpace(filtro))
            {
                query = query.Where(NormalizeDynamicFilter(filtro));
            }

            return await query.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<List<Torosuni>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null!)
        {
            if (take == 0)
            {
                take = int.MaxValue;
            }

            var query = BaseQuery(includeSocio: false, includeEstablecimiento: false);
            if (!string.IsNullOrWhiteSpace(filtro))
            {
                query = query.Where(NormalizeDynamicFilter(filtro));
            }

            return await query.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<(List<Torosuni> Items, int TotalCount)> SearchAsync(
            TorosFilterRequest request,
            IReadOnlyCollection<int>? allowedSocioIds = null,
            bool restrictToAllowedSocios = false)
        {
            request ??= new TorosFilterRequest();

            var normalizedTake = request.Take <= 0 ? 15 : request.Take;
            var normalizedSkip = request.Skip < 0 ? 0 : request.Skip;

            var query = ApplyServerFilters(
                BaseQuery(
                    includeSocio: true,
                    includeEstablecimiento: true,
                    prioritizeRecentFirst: !restrictToAllowedSocios),
                request,
                allowedSocioIds,
                restrictToAllowedSocios);

            var needsClientDateFilter = request.FechaSDesde.HasValue || request.FechaSHasta.HasValue;
            if (!needsClientDateFilter)
            {
                var totalCount = await query.CountAsync();
                var items = await query
                    .Skip(normalizedSkip)
                    .Take(normalizedTake)
                    .ToListAsync();

                return (items, totalCount);
            }

            var materialized = await query.ToListAsync();
            var filtered = ApplyFechaSFilter(materialized, request.FechaSDesde, request.FechaSHasta).ToList();
            var paged = filtered
                .Skip(normalizedSkip)
                .Take(normalizedTake)
                .ToList();

            return (paged, filtered.Count);
        }

        public async Task<TorosFilterMetadataResponse> GetFilterMetadataAsync(
            TorosFilterRequest request,
            IReadOnlyCollection<int>? allowedSocioIds = null,
            bool restrictToAllowedSocios = false)
        {
            request ??= new TorosFilterRequest();

            var response = new TorosFilterMetadataResponse
            {
                Estados = BuildEstadoOptions(),
                TiposToro = BuildTipoToroOptions()
            };

            var scopedQuery = ApplySecurityScope(
                BaseQuery(includeSocio: true, includeEstablecimiento: true),
                allowedSocioIds,
                restrictToAllowedSocios);

            response.Socios = await scopedQuery
                .Where(t => t.Socio != null)
                .Select(t => t.Socio!)
                .Distinct()
                .OrderBy(s => s.Nombre)
                .Select(s => new SocioLookupItemDTO
                {
                    Id = s.Id,
                    Scod = s.Scod ?? string.Empty,
                    NumeroSocio = s.Codpos2 ?? string.Empty,
                    Nombre = s.Nombre ?? string.Empty,
                    DisplayName = string.IsNullOrWhiteSpace(s.Codpos2)
                        ? (s.Nombre ?? string.Empty)
                        : $"{s.Codpos2} - {s.Nombre}"
                })
                .ToListAsync();

            response.ShowSocioSelector = response.Socios.Count > 1;

            var variedadQuery = ApplySecurityScope(
                _dbContext.Torosunis.AsNoTracking().Include(t => t.Socio),
                allowedSocioIds,
                restrictToAllowedSocios);

            if (request.SocioId.HasValue)
            {
                variedadQuery = variedadQuery.Where(t => t.Socio != null && t.Socio.Id == request.SocioId.Value);
            }
            else if (request.SocioIds?.Count > 0)
            {
                variedadQuery = variedadQuery.Where(t => t.Socio != null && request.SocioIds.Contains(t.Socio.Id));
            }

            response.Variedades = await variedadQuery
                .Where(t => !string.IsNullOrWhiteSpace(t.Variedad))
                .Select(t => t.Variedad!)
                .Distinct()
                .OrderBy(v => v)
                .Select(v => new TorosFilterOptionDTO { Value = v, Label = v })
                .ToListAsync();

            var establishmentQuery = ApplySecurityScope(
                _dbContext.Torosunis
                    .AsNoTracking()
                    .Include(t => t.Socio)
                    .Include(t => t.Establecimiento),
                allowedSocioIds,
                restrictToAllowedSocios);

            if (request.SocioId.HasValue)
            {
                establishmentQuery = establishmentQuery.Where(t => t.Socio != null && t.Socio.Id == request.SocioId.Value);
            }
            else if (request.SocioIds?.Count > 0)
            {
                establishmentQuery = establishmentQuery.Where(t => t.Socio != null && request.SocioIds.Contains(t.Socio.Id));
            }

            var establishmentItems = await establishmentQuery.ToListAsync();
            response.HasSinEstablecimientoOption = establishmentItems.Any(t => !t.EstablecimientoId.HasValue);
            response.Establecimientos = establishmentItems
                .Where(t => t.EstablecimientoId.HasValue && t.Establecimiento != null)
                .GroupBy(t => new { t.EstablecimientoId, t.Establecimiento!.Nombre, t.Establecimiento.Ecod })
                .OrderBy(g => g.Key.Nombre)
                .Select(g => new TorosFilterOptionDTO
                {
                    IntValue = g.Key.EstablecimientoId,
                    Value = g.Key.EstablecimientoId!.Value.ToString(),
                    Label = string.IsNullOrWhiteSpace(g.Key.Ecod)
                        ? (g.Key.Nombre ?? string.Empty)
                        : $"{g.Key.Ecod} - {g.Key.Nombre}"
                })
                .ToList();

            response.ShowEstablecimientoSelector = response.Establecimientos.Count > 1
                || response.HasSinEstablecimientoOption;

            return response;
        }

        public async Task<bool> Eliminar(Torosuni entidad)
        {
            _dbContext.Torosunis.Remove(entidad);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Torosuni> Crear(Torosuni entidad)
        {
            _dbContext.Set<Torosuni>().Add(entidad);
            await _dbContext.SaveChangesAsync();
            return entidad;
        }

        public async Task<bool> Editar(Torosuni entidad)
        {
            _dbContext.Update(entidad);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IQueryable<Torosuni>> Consultar(Expression<Func<Torosuni, bool>> filtro = null!)
        {
            var query = BaseQuery(includeSocio: true, includeEstablecimiento: true);
            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            return await Task.FromResult(query);
        }

        public async Task<int> CantidadTotal()
        {
            return await _dbContext.Torosunis.CountAsync();
        }

        public async Task<int> CantidadFiltrada(string? filtro = null)
        {
            var query = BaseQuery(includeSocio: true, includeEstablecimiento: false);
            if (!string.IsNullOrWhiteSpace(filtro))
            {
                query = query.Where(NormalizeDynamicFilter(filtro));
            }

            return await query.CountAsync();
        }

        private IQueryable<Torosuni> ApplyServerFilters(
            IQueryable<Torosuni> query,
            TorosFilterRequest request,
            IReadOnlyCollection<int>? allowedSocioIds,
            bool restrictToAllowedSocios)
        {
            query = ApplySecurityScope(query, allowedSocioIds, restrictToAllowedSocios);

            if (request.SocioId.HasValue)
            {
                query = query.Where(t => t.Socio != null && t.Socio.Id == request.SocioId.Value);
            }
            else if (request.SocioIds?.Count > 0)
            {
                query = query.Where(t => t.Socio != null && request.SocioIds.Contains(t.Socio.Id));
            }

            if (request.EstablecimientoId.HasValue)
            {
                query = query.Where(t => t.EstablecimientoId == request.EstablecimientoId.Value);
            }
            else if (request.IncluirSinEstablecimiento)
            {
                query = query.Where(t => !t.EstablecimientoId.HasValue);
            }

            if (!string.IsNullOrWhiteSpace(request.Estado))
            {
                query = query.Where(t => t.CodEstado == request.Estado);
            }

            if (!string.IsNullOrWhiteSpace(request.TipoToro))
            {
                query = query.Where(t => t.TipToro == request.TipoToro);
            }

            if (!string.IsNullOrWhiteSpace(request.Variedad))
            {
                query = query.Where(t => t.Variedad == request.Variedad);
            }

            if (!string.IsNullOrWhiteSpace(request.NombreToro))
            {
                var nombreToro = request.NombreToro.Trim();
                query = query.Where(t => t.NomDad != null && t.NomDad.Contains(nombreToro));
            }

            if (request.NroOrden.HasValue)
            {
                query = query.Where(t => t.Sbcod == request.NroOrden.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Hba))
            {
                var hba = request.Hba.Trim();
                query = query.Where(t => t.Hba != null && t.Hba.Contains(hba));
            }

            if (!string.IsNullOrWhiteSpace(request.Tatuaje))
            {
                var tatuaje = request.Tatuaje.Trim();
                query = query.Where(t => t.Tatpart != null && t.Tatpart.Contains(tatuaje));
            }

            if (!string.IsNullOrWhiteSpace(request.TipAdn))
            {
                var tipAdn = request.TipAdn.Trim();
                query = query.Where(t => t.NrTsan != null && t.NrTsan.Contains(tipAdn));
            }

            if (request.FechaNacimientoDesde.HasValue)
            {
                var desde = request.FechaNacimientoDesde.Value.Date;
                query = query.Where(t => t.Nacido.HasValue && t.Nacido.Value.Date >= desde);
            }

            if (request.FechaNacimientoHasta.HasValue)
            {
                var hasta = request.FechaNacimientoHasta.Value.Date;
                query = query.Where(t => t.Nacido.HasValue && t.Nacido.Value.Date <= hasta);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                query = ApplySearchFilter(query, request.SearchText.Trim());
            }

            return query;
        }

        private IQueryable<Torosuni> ApplySecurityScope(
            IQueryable<Torosuni> query,
            IReadOnlyCollection<int>? allowedSocioIds,
            bool restrictToAllowedSocios)
        {
            if (!restrictToAllowedSocios)
            {
                return query;
            }

            if (allowedSocioIds == null || allowedSocioIds.Count == 0)
            {
                return query.Where(_ => false);
            }

            return query.Where(t => t.Socio != null && allowedSocioIds.Contains(t.Socio.Id));
        }

        private IQueryable<Torosuni> ApplySearchFilter(IQueryable<Torosuni> query, string searchText)
        {
            var normalized = searchText.Trim();
            var isNumber = int.TryParse(normalized, out var numericValue);

            query = query.Where(t =>
                (t.NomDad != null && t.NomDad.Contains(normalized)) ||
                (t.Hba != null && t.Hba.Contains(normalized)) ||
                (t.Tatpart != null && t.Tatpart.Contains(normalized)) ||
                (t.NrTsan != null && t.NrTsan.Contains(normalized)) ||
                (t.Socio != null && t.Socio.Nombre != null && t.Socio.Nombre.Contains(normalized)) ||
                (t.Socio != null && t.Socio.Codpos2 != null && t.Socio.Codpos2.Contains(normalized)) ||
                (t.Socio != null && t.Socio.Scod != null && t.Socio.Scod.Contains(normalized)) ||
                (isNumber && t.Sbcod == numericValue));

            return query;
        }

        private static IEnumerable<Torosuni> ApplyFechaSFilter(
            IEnumerable<Torosuni> items,
            DateTime? desde,
            DateTime? hasta)
        {
            var normalizedDesde = desde?.Date;
            var normalizedHasta = hasta?.Date;

            foreach (var item in items)
            {
                if (!TryParseFechaS(item.Fechasba, out var fecha))
                {
                    if (normalizedDesde.HasValue || normalizedHasta.HasValue)
                    {
                        continue;
                    }
                }

                if (normalizedDesde.HasValue && fecha < normalizedDesde.Value)
                {
                    continue;
                }

                if (normalizedHasta.HasValue && fecha > normalizedHasta.Value)
                {
                    continue;
                }

                yield return item;
            }
        }

        private static bool TryParseFechaS(string? value, out DateTime fecha)
        {
            return DateTime.TryParseExact(
                value,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out fecha);
        }

        private static List<TorosFilterOptionDTO> BuildEstadoOptions()
        {
            return new List<TorosFilterOptionDTO>
            {
                new() { Value = "1", Label = "Activo" },
                new() { Value = "2", Label = "Inactivo" },
                new() { Value = "3", Label = "Inexistente" },
                new() { Value = "4", Label = "Muerto" },
                new() { Value = "5", Label = "Vendido" }
            };
        }

        private static List<TorosFilterOptionDTO> BuildTipoToroOptions()
        {
            return new List<TorosFilterOptionDTO>
            {
                new() { Value = "P", Label = "Puro Pedigree (SRA)" },
                new() { Value = "S", Label = "S/" },
                new() { Value = "GP", Label = "Ganador de prueba" },
                new() { Value = "A", Label = "Patagónico" }
            };
        }
    }
}
