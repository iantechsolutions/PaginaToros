using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Services;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferenciasLookupController : ControllerBase
    {
        private readonly hereford_prContext _db;
        private readonly IMapper _mapper;
        private readonly IUserSocioContextService _userSocioContextService;

        public TransferenciasLookupController(
            hereford_prContext db,
            IMapper mapper,
            IUserSocioContextService userSocioContextService)
        {
            _db = db;
            _mapper = mapper;
            _userSocioContextService = userSocioContextService;
        }

        [HttpGet("PlantelesBySocioCode")]
        public async Task<IActionResult> PlantelesBySocioCode(string socioCode)
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (!CanUseLookup(accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<PlantelDTO>>());
                }

                if (string.IsNullOrWhiteSpace(socioCode))
                {
                    return BadRequest(new Respuesta<List<PlantelDTO>>
                    {
                        Exito = 0,
                        Mensaje = "Debe indicar una razon social valida."
                    });
                }

                var items = await _db.Planteles
                    .AsNoTracking()
                    .Where(x => x.Nrocri == socioCode)
                    .OrderBy(x => x.Placod)
                    .ToListAsync();

                return Ok(new Respuesta<List<PlantelDTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<PlantelDTO>>(items)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<List<PlantelDTO>>
                {
                    Exito = 0,
                    Mensaje = ex.Message
                });
            }
        }

        [HttpGet("EstablecimientosBySocioCode")]
        public async Task<IActionResult> EstablecimientosBySocioCode(string socioCode)
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (!CanUseLookup(accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<EstableDTO>>());
                }

                if (string.IsNullOrWhiteSpace(socioCode))
                {
                    return BadRequest(new Respuesta<List<EstableDTO>>
                    {
                        Exito = 0,
                        Mensaje = "Debe indicar una razon social valida."
                    });
                }

                var items = await _db.Estables
                    .AsNoTracking()
                    .Where(x => x.Codsoc == socioCode)
                    .OrderBy(x => x.Nombre)
                    .ToListAsync();

                return Ok(new Respuesta<List<EstableDTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<EstableDTO>>(items)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<List<EstableDTO>>
                {
                    Exito = 0,
                    Mensaje = ex.Message
                });
            }
        }

        [HttpGet("TorosTransferiblesBySocioCode")]
        public async Task<IActionResult> TorosTransferiblesBySocioCode(string socioCode)
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (!CanUseLookup(accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<TorosuniDTO>>());
                }

                if (string.IsNullOrWhiteSpace(socioCode))
                {
                    return BadRequest(new Respuesta<List<TorosuniDTO>>
                    {
                        Exito = 0,
                        Mensaje = "Debe indicar una razon social valida."
                    });
                }

                var items = await _db.Torosunis
                    .AsNoTracking()
                    .Where(x => x.Criador == socioCode && x.TipToro == "S" && x.CodEstado == "1")
                    .OrderBy(x => x.NomDad)
                    .ToListAsync();

                return Ok(new Respuesta<List<TorosuniDTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<TorosuniDTO>>(items)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<List<TorosuniDTO>>
                {
                    Exito = 0,
                    Mensaje = ex.Message
                });
            }
        }

        [HttpGet("Socios")]
        public async Task<IActionResult> Socios()
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (!CanUseLookup(accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<SocioDTO>>());
                }

                var items = await _db.Socios
                    .AsNoTracking()
                    .OrderBy(x => x.Nombre)
                    .ToListAsync();

                return Ok(new Respuesta<List<SocioDTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<SocioDTO>>(items)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<List<SocioDTO>>
                {
                    Exito = 0,
                    Mensaje = ex.Message
                });
            }
        }

        private static bool CanUseLookup(UserSocioAccessContext accessContext)
        {
            if (!accessContext.IsAuthenticated || accessContext.CurrentUser is null)
            {
                return false;
            }

            if (accessContext.IsPrivilegedUser)
            {
                return true;
            }

            return accessContext.IsSocioUser && !string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode);
        }

        private static Respuesta<T> BuildForbiddenResponse<T>()
            => new Respuesta<T>
            {
                Exito = 0,
                Mensaje = "No tenes permisos para consultar datos de transferencias."
            };
    }
}
