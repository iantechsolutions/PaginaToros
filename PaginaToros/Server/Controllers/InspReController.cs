using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Services;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Resin1Controller : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IResin1Repositorio _resin1Repositorio;
        private readonly IUserSocioContextService _userSocioContextService;
        private readonly hereford_prContext _db;

        public Resin1Controller(
            IResin1Repositorio resin1Repositorio,
            IMapper mapper,
            IUserSocioContextService userSocioContextService,
            hereford_prContext db)
        {
            _mapper = mapper;
            _resin1Repositorio = resin1Repositorio;
            _userSocioContextService = userSocioContextService;
            _db = db;
        }

        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {
            var response = new Respuesta<List<Resin1DTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Resin1DTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? BuildActiveSocioFilter(accessContext.ActiveSocioCode!)
                    : null;
                var items = await _resin1Repositorio.LimitadosFiltrados(skip, take, effectiveExpression);

                response = new Respuesta<List<Resin1DTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<Resin1DTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<Resin1DTO>>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = null
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route("Cantidad")]
        public async Task<IActionResult> CantidadTotal()
        {
            var response = new Respuesta<int>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<int>());
                }

                int cantidad;
                if (RequiresActiveSocioScope(accessContext))
                {
                    var query = await _resin1Repositorio.Consultar(x => x.Scod == accessContext.ActiveSocioCode);
                    cantidad = query.Count();
                }
                else
                {
                    cantidad = await _resin1Repositorio.CantidadTotal();
                }

                response = new Respuesta<int>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = cantidad
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<int>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = 0
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route("LimitadosFiltrados")]
        public async Task<IActionResult> LimitadosFiltrados(int skip, int take, string? expression = null)
        {
            var response = new Respuesta<List<Resin1DTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Resin1DTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? AppendFilter(expression, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : expression;
                var items = await _resin1Repositorio.LimitadosFiltrados(skip, take, effectiveExpression);

                response = new Respuesta<List<Resin1DTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<Resin1DTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<Resin1DTO>>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = null
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route("LimitadosFiltradosNoInclude")]
        public async Task<IActionResult> LimitadosFiltradosNoInclude(int skip, int take, string? expression = null)
        {
            var response = new Respuesta<List<Resin1DTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Resin1DTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? AppendFilter(expression, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : expression;
                var items = await _resin1Repositorio.LimitadosFiltradosNoInclude(skip, take, effectiveExpression);

                response = new Respuesta<List<Resin1DTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<Resin1DTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<Resin1DTO>>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = null
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = new Respuesta<string>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                }

                var entity = await _resin1Repositorio.Obtener(u => u.Id == id);
                if (entity != null)
                {
                    if (RequiresActiveSocioScope(accessContext) &&
                        !string.Equals(entity.Scod, accessContext.ActiveSocioCode, StringComparison.OrdinalIgnoreCase))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                    }

                    var ok = await _resin1Repositorio.Eliminar(entity);

                    response = ok
                        ? new Respuesta<string> { Exito = 1, Mensaje = "ok", List = string.Empty }
                        : new Respuesta<string> { Exito = 0, Mensaje = "No se pudo eliminar el identificador", List = string.Empty };
                }

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<string> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] Resin1DTO request)
        {
            if (request == null)
            {
                return BadRequest(new Respuesta<Resin1DTO> { Exito = 0, Mensaje = "La solicitud está vacía." });
            }

            var response = new Respuesta<Resin1DTO>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Resin1DTO>());
                }

                if (RequiresActiveSocioScope(accessContext))
                {
                    request.Scod = accessContext.ActiveSocioCode;
                }

                if (string.IsNullOrWhiteSpace(request.Scod))
                {
                    return BadRequest(new Respuesta<Resin1DTO> { Exito = 0, Mensaje = "El socio es obligatorio." });
                }

                if (string.IsNullOrWhiteSpace(request.Icod) || request.Icod == "0")
                {
                    return BadRequest(new Respuesta<Resin1DTO> { Exito = 0, Mensaje = "El inspector es obligatorio." });
                }

                if (!await IsAllowedEstablecimientoAsync(request.Estcod, accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Resin1DTO>());
                }

                if (string.IsNullOrWhiteSpace(request.Nrores))
                {
                    var ultimo = await _resin1Repositorio.ObtenerUltimoNrores();
                    var nuevoNro = (ultimo ?? 0) + 1;
                    request.Nrores = nuevoNro.ToString("D6");
                }

                var entity = _mapper.Map<Resin1>(request);
                if (RequiresActiveSocioScope(accessContext))
                {
                    entity.Scod = accessContext.ActiveSocioCode!;
                }

                var created = await _resin1Repositorio.Crear(entity);

                response = created?.Id > 0
                    ? new Respuesta<Resin1DTO>
                    {
                        Exito = 1,
                        Mensaje = "ok",
                        List = _mapper.Map<Resin1DTO>(created)
                    }
                    : new Respuesta<Resin1DTO>
                    {
                        Exito = 0,
                        Mensaje = "No se pudo crear el identificador del resultado de inspección."
                    };

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<Resin1DTO>
                {
                    Exito = 0,
                    Mensaje = ex.InnerException?.Message ?? ex.Message
                };

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Resin1DTO request)
        {
            var response = new Respuesta<Resin1DTO>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Resin1DTO>());
                }

                var entity = _mapper.Map<Resin1>(request);
                var entityToEdit = await _resin1Repositorio.Obtener(u => u.Id == entity.Id);

                if (entityToEdit != null)
                {
                    if (RequiresActiveSocioScope(accessContext) &&
                        !string.Equals(entityToEdit.Scod, accessContext.ActiveSocioCode, StringComparison.OrdinalIgnoreCase))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Resin1DTO>());
                    }

                    if (!await IsAllowedEstablecimientoAsync(entity.Estcod, accessContext))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Resin1DTO>());
                    }

                    entityToEdit.Nrores = entity.Nrores;
                    entityToEdit.Nropla = entity.Nropla;
                    entityToEdit.Observ = entity.Observ;
                    entityToEdit.Ppajust = entity.Ppajust;
                    entityToEdit.Epromedio = entity.Epromedio;
                    entityToEdit.Emax = entity.Emax;
                    entityToEdit.Emin = entity.Emin;
                    entityToEdit.Tortot = entity.Tortot;
                    entityToEdit.Torsb = entity.Torsb;
                    entityToEdit.CodUsu = entity.CodUsu;
                    entityToEdit.Editar = entity.Editar;
                    entityToEdit.Icod = entity.Icod!;
                    entityToEdit.Scod = RequiresActiveSocioScope(accessContext)
                        ? accessContext.ActiveSocioCode!
                        : entity.Scod!;
                    entityToEdit.Estcod = entity.Estcod;
                    entityToEdit.Freali = entity.Freali;

                    var ok = await _resin1Repositorio.Editar(entityToEdit);

                    response = ok
                        ? new Respuesta<Resin1DTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin1DTO>(entityToEdit) }
                        : new Respuesta<Resin1DTO> { Exito = 0, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    response = new Respuesta<Resin1DTO> { Exito = 0, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<Resin1DTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        private async Task<bool> IsAllowedEstablecimientoAsync(string? estcod, UserSocioAccessContext accessContext)
        {
            if (string.IsNullOrWhiteSpace(estcod) || !RequiresActiveSocioScope(accessContext))
            {
                return true;
            }

            return await _db.Estables.AsNoTracking().AnyAsync(
                x => x.Ecod == estcod && x.Codsoc == accessContext.ActiveSocioCode);
        }

        private static bool RequiresActiveSocioScope(UserSocioAccessContext accessContext)
            => accessContext.IsSocioUser && !accessContext.IsPrivilegedUser;

        private static string BuildActiveSocioFilter(string activeSocioCode)
            => $"Scod == \"{activeSocioCode}\"";

        private static string AppendFilter(string? expression, string requiredFilter)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return requiredFilter;
            }

            return $"({expression}) && ({requiredFilter})";
        }

        private static Respuesta<T> BuildForbiddenResponse<T>()
            => new Respuesta<T>
            {
                Exito = 0,
                Mensaje = "No tenes permisos para operar sobre otra razon social."
            };
    }
}
