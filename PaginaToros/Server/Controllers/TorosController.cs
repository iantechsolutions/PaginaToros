using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Services;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TorosController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITorosRepositorio _torosRepositorio;
        private readonly IUserSocioContextService _userSocioContextService;
        private readonly hereford_prContext _dbContext;

        public TorosController(
            ITorosRepositorio torosRepositorio,
            IMapper mapper,
            IUserSocioContextService userSocioContextService,
            hereford_prContext dbContext)
        {
            _mapper = mapper;
            _torosRepositorio = torosRepositorio;
            _userSocioContextService = userSocioContextService;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                var scopedResponse = await _torosRepositorio.SearchAsync(
                    new TorosFilterRequest { Skip = skip, Take = take },
                    accessContext.AllowedSocioIds,
                    RequiresAllowedSocioScope(accessContext));

                return Ok(new Respuesta<List<TorosuniDTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<TorosuniDTO>>(scopedResponse.Items)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<List<TorosuniDTO>>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = null
                });
            }
        }

        [HttpGet]
        [Route("Cantidad")]
        public async Task<IActionResult> CantidadTotal()
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (!RequiresAllowedSocioScope(accessContext))
                {
                    return Ok(new Respuesta<int> { Exito = 1, Mensaje = "Exito", List = await _torosRepositorio.CantidadTotal() });
                }

                var query = _dbContext.Torosunis
                    .AsNoTracking()
                    .Include(t => t.Socio)
                    .Where(t => t.Socio != null && accessContext.AllowedSocioIds.Contains(t.Socio.Id));

                return Ok(new Respuesta<int> { Exito = 1, Mensaje = "Exito", List = await query.CountAsync() });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<int>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = 0
                });
            }
        }

        [HttpGet]
        [Route("CantidadFiltrada")]
        public async Task<IActionResult> CantidadFiltrada(string? expression = null)
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                var query = BuildLegacyScopedQuery(accessContext);

                if (!string.IsNullOrWhiteSpace(expression))
                {
                    query = query.Where(NormalizeDynamicFilter(expression));
                }

                return Ok(new Respuesta<int> { Exito = 1, Mensaje = "Éxito", List = await query.CountAsync() });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<int>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = 0
                });
            }
        }

        [HttpGet]
        [Route("LimitadosFiltrados")]
        public async Task<IActionResult> LimitadosFiltrados(int skip, int take, string? expression = null)
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                var query = BuildLegacyScopedQuery(accessContext);

                if (!string.IsNullOrWhiteSpace(expression))
                {
                    query = query.Where(NormalizeDynamicFilter(expression));
                }

                if (take == 0)
                {
                    take = int.MaxValue;
                }

                var entidades = await query
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return Ok(new Respuesta<List<TorosuniDTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<TorosuniDTO>>(entidades)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Respuesta<List<TorosuniDTO>>
                {
                    Exito = 0,
                    Mensaje = $"Error en filtro: {ex.Message}",
                    List = null
                });
            }
        }

        [HttpGet]
        [Route("LimitadosFiltradosNoInclude")]
        public async Task<IActionResult> LimitadosFiltradosNoInclude(int skip, int take, string? expression = null)
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                var query = BuildLegacyScopedQuery(accessContext, includeEstablecimiento: false);

                if (!string.IsNullOrWhiteSpace(expression))
                {
                    query = query.Where(NormalizeDynamicFilter(expression));
                }

                if (take == 0)
                {
                    take = int.MaxValue;
                }

                var entidades = await query
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return Ok(new Respuesta<List<TorosuniDTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<TorosuniDTO>>(entidades)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<List<TorosuniDTO>>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = null
                });
            }
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] TorosFilterRequest request)
        {
            try
            {
                request ??= new TorosFilterRequest();
                var accessContext = await _userSocioContextService.ResolveAsync(User);

                if (RequiresAllowedSocioScope(accessContext))
                {
                    request.SocioIds = request.SocioIds
                        .Where(id => accessContext.AllowedSocioIds.Contains(id))
                        .Distinct()
                        .ToList();

                    if (request.SocioId.HasValue && !accessContext.AllowedSocioIds.Contains(request.SocioId.Value))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<TorosPagedResponse>());
                    }
                }

                var result = await _torosRepositorio.SearchAsync(
                    request,
                    accessContext.AllowedSocioIds,
                    RequiresAllowedSocioScope(accessContext));

                return Ok(new Respuesta<TorosPagedResponse>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = new TorosPagedResponse
                    {
                        Items = _mapper.Map<List<TorosuniDTO>>(result.Items),
                        TotalCount = result.TotalCount
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<TorosPagedResponse>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = null
                });
            }
        }

        [HttpPost("filter-metadata")]
        public async Task<IActionResult> FilterMetadata([FromBody] TorosFilterRequest request)
        {
            try
            {
                request ??= new TorosFilterRequest();
                var accessContext = await _userSocioContextService.ResolveAsync(User);

                if (RequiresAllowedSocioScope(accessContext))
                {
                    request.SocioIds = request.SocioIds
                        .Where(id => accessContext.AllowedSocioIds.Contains(id))
                        .Distinct()
                        .ToList();

                    if (request.SocioId.HasValue && !accessContext.AllowedSocioIds.Contains(request.SocioId.Value))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<TorosFilterMetadataResponse>());
                    }
                }

                var metadata = await _torosRepositorio.GetFilterMetadataAsync(
                    request,
                    accessContext.AllowedSocioIds,
                    RequiresAllowedSocioScope(accessContext));

                if (RequiresAllowedSocioScope(accessContext) && metadata.Socios.Count > 0)
                {
                    metadata.ShowSocioSelector = metadata.Socios.Count > 1;
                }

                return Ok(new Respuesta<TorosFilterMetadataResponse>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = metadata
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<TorosFilterMetadataResponse>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = null
                });
            }
        }

        [HttpGet("ById/{id:int}")]
        public async Task<IActionResult> ById(int id)
        {
            try
            {
                var repo = await _torosRepositorio.GetById(id);
                if (repo.Exito == 0 || repo.List == null)
                {
                    return NotFound(new Respuesta<TorosuniDTO> { Exito = 0, Mensaje = repo.Mensaje });
                }

                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresAllowedSocioScope(accessContext) && !CanAccessToro(accessContext, repo.List))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<TorosuniDTO>());
                }

                return Ok(new Respuesta<TorosuniDTO>
                {
                    Exito = 1,
                    Mensaje = "Éxito",
                    List = _mapper.Map<TorosuniDTO>(repo.List)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<TorosuniDTO>
                {
                    Exito = 0,
                    Mensaje = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                var toro = await _torosRepositorio.Obtener(u => u.Id == id);
                if (toro == null)
                {
                    return NotFound(new Respuesta<string> { Exito = 0, Mensaje = "Toro inexistente", List = string.Empty });
                }

                if (RequiresAllowedSocioScope(accessContext) && !CanAccessToro(accessContext, toro))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                }

                var deleted = await _torosRepositorio.Eliminar(toro);
                return Ok(new Respuesta<string>
                {
                    Exito = deleted ? 1 : 0,
                    Mensaje = deleted ? "ok" : "No se pudo eliminar el identificador",
                    List = string.Empty
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<string>
                {
                    Exito = 0,
                    Mensaje = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] TorosuniDTO request)
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresAllowedSocioScope(accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new Respuesta<TorosuniDTO>
                    {
                        Exito = 0,
                        Mensaje = "Los usuarios socio no pueden agregar nuevos toros."
                    });
                }

                if (string.IsNullOrWhiteSpace(request.Criador))
                {
                    return BadRequest(new Respuesta<TorosuniDTO>
                    {
                        Exito = 0,
                        Mensaje = "Debe seleccionar un socio propietario para crear el toro."
                    });
                }

                var toro = _mapper.Map<Torosuni>(request);
                await SyncEstablecimientoAsync(toro, request.EstablecimientoId, request.Criador);

                var created = await _torosRepositorio.Crear(toro);
                return Ok(new Respuesta<TorosuniDTO>
                {
                    Exito = created.Id != 0 ? 1 : 0,
                    Mensaje = created.Id != 0 ? "ok" : "No se pudo crear el identificador",
                    List = _mapper.Map<TorosuniDTO>(created)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<TorosuniDTO>
                {
                    Exito = 0,
                    Mensaje = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] TorosuniDTO request)
        {
            try
            {
                Torosuni entidad;
                var accessContext = await _userSocioContextService.ResolveAsync(User);

                if (request.Id == 0)
                {
                    if (RequiresAllowedSocioScope(accessContext))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, new Respuesta<TorosuniDTO>
                        {
                            Exito = 0,
                            Mensaje = "Los usuarios socio no pueden agregar nuevos toros."
                        });
                    }

                    entidad = _mapper.Map<Torosuni>(request);
                    await SyncEstablecimientoAsync(entidad, request.EstablecimientoId, request.Criador);
                    entidad = await _torosRepositorio.Crear(entidad);

                    return Ok(new Respuesta<TorosuniDTO>
                    {
                        Exito = entidad.Id != 0 ? 1 : 0,
                        Mensaje = entidad.Id != 0 ? "ok" : "No se pudo crear el identificador",
                        List = _mapper.Map<TorosuniDTO>(entidad)
                    });
                }

                entidad = await _torosRepositorio.Obtener(t => t.Id == request.Id);
                if (entidad == null)
                {
                    return NotFound(new Respuesta<TorosuniDTO> { Exito = 0, Mensaje = "No se encontró el identificador" });
                }

                if (RequiresAllowedSocioScope(accessContext) && !CanAccessToro(accessContext, entidad))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<TorosuniDTO>());
                }

                var originalCriador = entidad.Criador;
                _mapper.Map(request, entidad);
                if (RequiresAllowedSocioScope(accessContext))
                {
                    entidad.Criador = originalCriador;
                }

                await SyncEstablecimientoAsync(entidad, request.EstablecimientoId, entidad.Criador);
                var ok = await _torosRepositorio.Editar(entidad);

                return Ok(new Respuesta<TorosuniDTO>
                {
                    Exito = ok ? 1 : 0,
                    Mensaje = ok ? "ok" : "No se pudo editar el identificador",
                    List = _mapper.Map<TorosuniDTO>(entidad)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<TorosuniDTO>
                {
                    Exito = 0,
                    Mensaje = ex.Message
                });
            }
        }

        private IQueryable<Torosuni> BuildLegacyScopedQuery(UserSocioAccessContext accessContext, bool includeEstablecimiento = true)
        {
            var prioritizeRecentFirst = !RequiresAllowedSocioScope(accessContext);

            var query = _dbContext.Torosunis
                .AsNoTracking()
                .AsSplitQuery()
                .Include(t => t.Socio)
                .AsQueryable();

            query = prioritizeRecentFirst
                ? query.OrderByDescending(t => t.Fechasba != null && t.Fechasba.Length >= 10
                        ? t.Fechasba.Substring(6, 4) + t.Fechasba.Substring(3, 2) + t.Fechasba.Substring(0, 2)
                        : string.Empty)
                    .ThenByDescending(t => t.FchUsu ?? t.Fecha)
                    .ThenByDescending(t => t.Id)
                : query.OrderByDescending(t => t.Fechasba != null && t.Fechasba.Length >= 10
                        ? t.Fechasba.Substring(6, 4) + t.Fechasba.Substring(3, 2) + t.Fechasba.Substring(0, 2)
                        : string.Empty)
                    .ThenBy(t => t.CodEstado == "1" ? 0 : 1)
                    .ThenBy(t => t.NomDad)
                    .ThenByDescending(t => t.Id);

            if (includeEstablecimiento)
            {
                query = query.Include(t => t.Establecimiento);
            }

            if (RequiresAllowedSocioScope(accessContext))
            {
                query = query.Where(t => t.Socio != null && accessContext.AllowedSocioIds.Contains(t.Socio.Id));
            }

            return query;
        }

        private static bool RequiresAllowedSocioScope(UserSocioAccessContext accessContext)
            => accessContext.IsSocioUser && !accessContext.IsPrivilegedUser;

        private static string NormalizeDynamicFilter(string expression)
        {
            var normalized = expression.Trim();
            normalized = Regex.Replace(normalized, @"(?<!=)=(?!=)", "==");
            normalized = normalized.Replace("Socio.Id==", "Socio.Id == ")
                .Replace("CodEstado==", "CodEstado == ")
                .Replace("TipToro==", "TipToro == ")
                .Replace("Variedad==", "Variedad == ");

            return normalized;
        }

        private static bool CanAccessToro(UserSocioAccessContext accessContext, Torosuni toro)
        {
            if (!RequiresAllowedSocioScope(accessContext))
            {
                return true;
            }

            return toro.Socio != null && accessContext.AllowedSocioIds.Contains(toro.Socio.Id);
        }

        private async Task SyncEstablecimientoAsync(Torosuni toro, int? establecimientoId, string? criador)
        {
            if (!establecimientoId.HasValue)
            {
                toro.EstablecimientoId = null;
                toro.Estcod = null;
                toro.Establecimiento = null;
                return;
            }

            var establecimiento = await _dbContext.Estables
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == establecimientoId.Value);

            if (establecimiento == null)
            {
                throw new InvalidOperationException("El establecimiento seleccionado no existe.");
            }

            if (!string.IsNullOrWhiteSpace(criador) &&
                !string.Equals(establecimiento.Codsoc, criador, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("El establecimiento seleccionado no pertenece al socio del toro.");
            }

            toro.EstablecimientoId = establecimiento.Id;
            toro.Estcod = establecimiento.Ecod;
        }

        private static Respuesta<T> BuildForbiddenResponse<T>()
            => new()
            {
                Exito = 0,
                Mensaje = "No tenes permisos para operar sobre otra razon social."
            };
    }
}
