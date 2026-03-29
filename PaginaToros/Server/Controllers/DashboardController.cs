using System.Globalization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Server.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMINISTRADOR,USUARIOMAESTRO")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly hereford_prContext _db;

        public DashboardController(hereford_prContext db)
        {
            _db = db;
        }

        [HttpGet("admin-home")]
        public async Task<IActionResult> GetAdminHome(CancellationToken cancellationToken)
        {
            var response = new Respuesta<AdminDashboardDTO>();

            try
            {
                var now = DateTime.Now;
                var currentMonthStart = new DateTime(now.Year, now.Month, 1);
                var firstVisibleMonth = currentMonthStart.AddMonths(-5);
                var nextMonthStart = currentMonthStart.AddMonths(1);

                var totalSolicitudes = await _db.Solici1s.AsNoTracking().CountAsync(cancellationToken);
                var solicitudesPendientes = await _db.Solici1s
                    .AsNoTracking()
                    .CountAsync(x => x.Fecins == null, cancellationToken);
                var solicitudesAutorizadas = await _db.Solici1s
                    .AsNoTracking()
                    .CountAsync(x => x.Fecins != null, cancellationToken);
                var solicitudesSinSocio = await _db.Solici1s
                    .AsNoTracking()
                    .CountAsync(x =>
                        string.IsNullOrWhiteSpace(x.Codest) ||
                        x.Establecimiento == null ||
                        string.IsNullOrWhiteSpace(x.Establecimiento.Codsoc) ||
                        x.Establecimiento.Socio == null,
                        cancellationToken);
                var inspeccionesRealizadas = await _db.Resin1s.AsNoTracking().CountAsync(cancellationToken);
                var sociosRegistrados = await _db.Socios.AsNoTracking().CountAsync(cancellationToken);
                var establecimientosRegistrados = await _db.Estables.AsNoTracking().CountAsync(cancellationToken);

                var solicitudesMensuales = await _db.Solici1s
                    .AsNoTracking()
                    .Where(x => x.Fecsol.HasValue && x.Fecsol.Value >= firstVisibleMonth && x.Fecsol.Value < nextMonthStart)
                    .GroupBy(x => new { x.Fecsol!.Value.Year, x.Fecsol.Value.Month })
                    .Select(g => new
                    {
                        g.Key.Year,
                        g.Key.Month,
                        Count = g.Count()
                    })
                    .ToListAsync(cancellationToken);

                var inspeccionesMensuales = await _db.Resin1s
                    .AsNoTracking()
                    .Where(x => x.Freali.HasValue && x.Freali.Value >= firstVisibleMonth && x.Freali.Value < nextMonthStart)
                    .GroupBy(x => new { x.Freali!.Value.Year, x.Freali.Value.Month })
                    .Select(g => new
                    {
                        g.Key.Year,
                        g.Key.Month,
                        Count = g.Count()
                    })
                    .ToListAsync(cancellationToken);

                var solicitudesRecientes = await _db.Solici1s
                    .AsNoTracking()
                    .Include(x => x.Establecimiento)
                    .ThenInclude(x => x.Socio)
                    .Where(x => x.Establecimiento != null && x.Establecimiento.Socio != null)
                    .OrderByDescending(x => x.Fecsol ?? DateTime.MinValue)
                    .ThenByDescending(x => x.Id)
                    .Take(2)
                    .Select(x => new DashboardRecentSolicitudDTO
                    {
                        Id = x.Id,
                        NroSolicitud = x.Nrosol ?? string.Empty,
                        SocioNombre = x.Establecimiento != null && x.Establecimiento.Socio != null
                            ? (x.Establecimiento.Socio.Nombre ?? string.Empty)
                            : string.Empty,
                        EstablecimientoNombre = x.Establecimiento != null
                            ? (x.Establecimiento.Nombre ?? string.Empty)
                            : string.Empty,
                        FechaSolicitud = x.Fecsol,
                        FechaAutorizacion = x.Fecins,
                        EstaAutorizada = x.Fecins != null
                    })
                    .ToListAsync(cancellationToken);
                
                var solicitudesMensualesMap = solicitudesMensuales
                    .ToDictionary(x => $"{x.Year:D4}-{x.Month:D2}", x => x.Count);
                var inspeccionesMensualesMap = inspeccionesMensuales
                    .ToDictionary(x => $"{x.Year:D4}-{x.Month:D2}", x => x.Count);

                var cultura = CultureInfo.GetCultureInfo("es-AR");
                var actividadMensual = new List<DashboardMonthlyPointDTO>();

                for (var month = firstVisibleMonth; month < nextMonthStart; month = month.AddMonths(1))
                {
                    var key = $"{month.Year:D4}-{month.Month:D2}";
                    actividadMensual.Add(new DashboardMonthlyPointDTO
                    {
                        Label = cultura.DateTimeFormat.GetAbbreviatedMonthName(month.Month),
                        Solicitudes = solicitudesMensualesMap.GetValueOrDefault(key),
                        Inspecciones = inspeccionesMensualesMap.GetValueOrDefault(key)
                    });
                }

                response = new Respuesta<AdminDashboardDTO>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = new AdminDashboardDTO
                    {
                        TotalSolicitudes = totalSolicitudes,
                        SolicitudesPendientes = solicitudesPendientes,
                        SolicitudesAutorizadas = solicitudesAutorizadas,
                        SolicitudesSinSocio = solicitudesSinSocio,
                        InspeccionesRealizadas = inspeccionesRealizadas,
                        SociosRegistrados = sociosRegistrados,
                        EstablecimientosRegistrados = establecimientosRegistrados,
                        TasaAutorizacion = totalSolicitudes == 0
                            ? 0
                            : Math.Round((double)solicitudesAutorizadas / totalSolicitudes * 100, 1),
                        GeneratedAt = now,
                        ActividadMensual = actividadMensual,
                        SolicitudesRecientes = solicitudesRecientes
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Exito = 0;
                response.Mensaje = ex.Message;
                response.List = null;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpDelete("orphan-solicitudes")]
        public async Task<IActionResult> DeleteOrphanSolicitudes(CancellationToken cancellationToken)
        {
            var response = new Respuesta<int>();

            try
            {
                var orphanIds = await _db.Solici1s
                    .Where(x =>
                        string.IsNullOrWhiteSpace(x.Codest) ||
                        x.Establecimiento == null ||
                        string.IsNullOrWhiteSpace(x.Establecimiento.Codsoc) ||
                        x.Establecimiento.Socio == null)
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                if (orphanIds.Count == 0)
                {
                    response.Exito = 1;
                    response.Mensaje = "No hay solicitudes sin socio para eliminar.";
                    response.List = 0;
                    return Ok(response);
                }

                var detalles = await _db.Solici1Auxs
                    .Where(x => orphanIds.Contains(x.IdSoli))
                    .ToListAsync(cancellationToken);

                if (detalles.Count > 0)
                {
                    _db.Solici1Auxs.RemoveRange(detalles);
                }

                var solicitudes = await _db.Solici1s
                    .Where(x => orphanIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);

                _db.Solici1s.RemoveRange(solicitudes);
                await _db.SaveChangesAsync(cancellationToken);

                response.Exito = 1;
                response.Mensaje = "Solicitudes sin socio eliminadas correctamente.";
                response.List = solicitudes.Count;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Exito = 0;
                response.Mensaje = ex.Message;
                response.List = 0;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
