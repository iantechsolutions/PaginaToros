using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Services;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Server.Controllers
{
    /// <summary>
    /// Lookups que usan los formularios de transferencias (animales, toros y futuras).
    ///
    /// Criterio de autorizacion, unico para todo el controller:
    /// <list type="bullet">
    /// <item>Un usuario socio (no privilegiado) consulta datos completos SOLO de su razon social
    /// activa (UserSocio / ActiveSocioId): planteles con existencias, establecimientos completos y
    /// toros transferibles. Pedir otra razon social por esos lookups devuelve 403 sin lista.</item>
    /// <item>La contraparte de una transferencia se consulta unicamente por
    /// <see cref="DestinoTransferencia"/>, que aplica la misma regla con la que
    /// TransanController/TranssbController/FutcontrolController aceptan guardar la transferencia
    /// (la razon social activa tiene que ser vendedor o comprador) y devuelve solo los datos
    /// minimos para elegir establecimiento y plantel de destino.</item>
    /// <item>ADMINISTRADOR y USUARIOMAESTRO consultan cualquier razon social con datos completos.</item>
    /// </list>
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransferenciasLookupController : ControllerBase
    {
        private readonly hereford_prContext _db;
        private readonly IMapper _mapper;
        private readonly IUserSocioContextService _userSocioContextService;
        private readonly ILogger<TransferenciasLookupController> _logger;

        public TransferenciasLookupController(
            hereford_prContext db,
            IMapper mapper,
            IUserSocioContextService userSocioContextService,
            ILogger<TransferenciasLookupController> logger)
        {
            _db = db;
            _mapper = mapper;
            _userSocioContextService = userSocioContextService;
            _logger = logger;
        }

        /// <summary>
        /// Planteles de una razon social para elegir el ORIGEN de una transferencia (con
        /// existencias). Incluye el establecimiento de cada plantel y, si se indica
        /// <paramref name="establecimientoId"/>, devuelve solo los de ese establecimiento mas los
        /// que todavia no tienen uno asignado. Un usuario socio solo puede consultar su razon
        /// social activa; para el destino de otra razon social usar <see cref="DestinoTransferencia"/>.
        /// </summary>
        [HttpGet("PlantelesBySocioCode")]
        public async Task<IActionResult> PlantelesBySocioCode(string socioCode, int? establecimientoId = null)
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

                if (!CanLookupSocio(accessContext, socioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<PlantelDTO>>(
                        "No tenes permisos para consultar planteles de otra razon social."));
                }

                var items = await QueryPlanteles(socioCode, establecimientoId)
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

        /// <summary>
        /// Establecimientos completos de una razon social. Misma regla que
        /// <see cref="PlantelesBySocioCode"/>: un usuario socio solo consulta su razon social
        /// activa; los del comprador de una transferencia salen de <see cref="DestinoTransferencia"/>.
        /// </summary>
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

                if (!CanLookupSocio(accessContext, socioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<EstableDTO>>(
                        "No tenes permisos para consultar establecimientos de otra razon social."));
                }

                var items = await QueryEstablecimientos(socioCode)
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

        /// <summary>
        /// Toros transferibles (activos, tipo "S") del VENDEDOR de una transferencia de toros.
        /// Un usuario socio solo consulta los de su razon social activa: el comprador nunca
        /// necesita la lista de toros de la contraparte.
        /// </summary>
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

                if (!CanLookupSocio(accessContext, socioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<TorosuniDTO>>(
                        "No tenes permisos para consultar toros de otra razon social."));
                }

                var items = await _db.Torosunis
                    .AsNoTracking()
                    .Include(x => x.Establecimiento)
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

        /// <summary>
        /// Establecimientos y planteles de la razon social COMPRADORA de una transferencia, para
        /// elegir el destino. Es la unica via por la que un usuario socio ve datos de otra razon
        /// social, y solo en el contexto de una transferencia en la que participa:
        /// <list type="bullet">
        /// <item>Se exige el par vendedor/comprador de la transferencia y se aplica la misma regla
        /// que usan Transan/Transsb/Futcontrol para aceptar guardarla (<see cref="CanAccessTransfer"/>):
        /// la razon social activa del usuario socio tiene que ser una de las dos partes. Pedir un
        /// destino para una transferencia ajena devuelve 403.</item>
        /// <item>Si el comprador es otra razon social, los datos son minimos: planteles sin
        /// existencias y establecimientos sin datos de contacto (ver <see cref="TransferenciaDestinoDTO"/>).
        /// Si el comprador es la propia razon social (movimiento interno) o el usuario es
        /// administrativo, van completos.</item>
        /// <item><paramref name="establecimientoId"/> tiene que pertenecer al comprador; un
        /// establecimiento de otra razon social se rechaza con 403 en vez de filtrar por el.</item>
        /// </list>
        /// Lo que un socio puede obtener aca es exactamente lo que necesita para una transferencia
        /// que el backend le aceptaria guardar; nada de esto habilita a leer existencias, toros ni
        /// datos completos de la contraparte.
        /// </summary>
        [HttpGet("DestinoTransferencia")]
        public async Task<IActionResult> DestinoTransferencia(string? vendedorSocioCode, string compradorSocioCode, int? establecimientoId = null)
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (!CanUseLookup(accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<TransferenciaDestinoDTO>());
                }

                if (string.IsNullOrWhiteSpace(compradorSocioCode))
                {
                    return BadRequest(new Respuesta<TransferenciaDestinoDTO>
                    {
                        Exito = 0,
                        Mensaje = "Debe indicar la razon social compradora."
                    });
                }

                var vendedor = vendedorSocioCode?.Trim();
                var comprador = compradorSocioCode.Trim();

                if (!CanAccessTransfer(accessContext, vendedor, comprador))
                {
                    _logger.LogWarning(
                        "TransferenciasLookup.DestinoTransferencia rechazado: usuario {UsuarioId} (razon social activa {Activa}) pidio destino {Comprador} de una transferencia con vendedor {Vendedor}.",
                        accessContext.CurrentUser?.Id, accessContext.ActiveSocioCode, comprador, vendedor);
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<TransferenciaDestinoDTO>(
                        "No tenes permisos para consultar el destino de una transferencia en la que tu razon social no participa."));
                }

                var compradorExiste = await _db.Socios
                    .AsNoTracking()
                    .AnyAsync(x => x.Scod == comprador);
                if (!compradorExiste)
                {
                    return NotFound(new Respuesta<TransferenciaDestinoDTO>
                    {
                        Exito = 0,
                        Mensaje = "La razon social compradora no existe."
                    });
                }

                if (establecimientoId.HasValue)
                {
                    var establecimientoDelComprador = await _db.Estables
                        .AsNoTracking()
                        .AnyAsync(x => x.Id == establecimientoId.Value && x.Codsoc == comprador);
                    if (!establecimientoDelComprador)
                    {
                        _logger.LogWarning(
                            "TransferenciasLookup.DestinoTransferencia rechazado: usuario {UsuarioId} pidio el establecimiento {EstablecimientoId}, que no pertenece al comprador {Comprador}.",
                            accessContext.CurrentUser?.Id, establecimientoId.Value, comprador);
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<TransferenciaDestinoDTO>(
                            "El establecimiento indicado no pertenece a la razon social compradora."));
                    }
                }

                // Datos completos solo cuando el usuario podria consultar esa razon social por los
                // lookups normales (propia o usuario administrativo). Para la contraparte, lo minimo.
                var datosCompletos = CanLookupSocio(accessContext, comprador);

                var establecimientos = await QueryEstablecimientos(comprador).ToListAsync();
                var planteles = await QueryPlanteles(comprador, establecimientoId).ToListAsync();

                var destino = new TransferenciaDestinoDTO
                {
                    CompradorSocioCode = comprador,
                    IncluyeExistencias = datosCompletos,
                    Establecimientos = datosCompletos
                        ? _mapper.Map<List<EstableDTO>>(establecimientos)
                        : establecimientos.Select(ToEstablecimientoDestino).ToList(),
                    Planteles = datosCompletos
                        ? _mapper.Map<List<PlantelDTO>>(planteles)
                        : planteles.Select(ToPlantelDestino).ToList()
                };

                if (!datosCompletos)
                {
                    _logger.LogInformation(
                        "TransferenciasLookup.DestinoTransferencia: usuario {UsuarioId} (razon social activa {Activa}) consulto el destino {Comprador} para una transferencia desde {Vendedor}. Establecimientos={Establecimientos} Planteles={Planteles}",
                        accessContext.CurrentUser?.Id, accessContext.ActiveSocioCode, comprador, vendedor,
                        destino.Establecimientos.Count, destino.Planteles.Count);
                }

                return Ok(new Respuesta<TransferenciaDestinoDTO>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = destino
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TransferenciasLookup.DestinoTransferencia fallo. Vendedor={Vendedor} Comprador={Comprador} EstablecimientoId={EstablecimientoId}",
                    vendedorSocioCode, compradorSocioCode, establecimientoId);
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<TransferenciaDestinoDTO>
                {
                    Exito = 0,
                    Mensaje = "No se pudo consultar el destino de la transferencia. Intenta nuevamente."
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

        private IQueryable<Plantel> QueryPlanteles(string socioCode, int? establecimientoId)
        {
            var query = _db.Planteles
                .AsNoTracking()
                .Include(x => x.Establecimiento)
                .Where(x => x.Nrocri == socioCode);

            if (establecimientoId.HasValue)
            {
                query = query.Where(x => x.EstablecimientoId == null || x.EstablecimientoId == establecimientoId.Value);
            }

            return query.OrderBy(x => x.Placod);
        }

        private IQueryable<Estable> QueryEstablecimientos(string socioCode)
            => _db.Estables
                .AsNoTracking()
                .Where(x => x.Codsoc == socioCode)
                .OrderBy(x => x.Nombre);

        /// <summary>Lo minimo para identificar un plantel destino: codigo, año y establecimiento. Sin existencias.</summary>
        private static PlantelDTO ToPlantelDestino(Plantel plantel) => new()
        {
            Id = plantel.Id,
            Placod = plantel.Placod,
            Anioex = plantel.Anioex,
            Nrocri = plantel.Nrocri,
            EstablecimientoId = plantel.EstablecimientoId,
            Establecimiento = plantel.Establecimiento == null ? null : ToEstablecimientoMinimo(plantel.Establecimiento)
        };

        /// <summary>Lo minimo para elegir un establecimiento destino: codigo y nombre. Sin direccion, telefono ni encargado.</summary>
        private static EstableDTO ToEstablecimientoDestino(Estable establecimiento) => new()
        {
            Id = establecimiento.Id,
            Ecod = establecimiento.Ecod,
            Codsoc = establecimiento.Codsoc,
            Nombre = establecimiento.Nombre
        };

        private static Estable ToEstablecimientoMinimo(Estable establecimiento) => new()
        {
            Id = establecimiento.Id,
            Ecod = establecimiento.Ecod,
            Codsoc = establecimiento.Codsoc,
            Nombre = establecimiento.Nombre
        };

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

        /// <summary>Mismo criterio que el resto de los controllers: solo el rol Socio no privilegiado queda acotado.</summary>
        private static bool RequiresActiveSocioScope(UserSocioAccessContext accessContext)
            => accessContext.IsSocioUser && !accessContext.IsPrivilegedUser;

        /// <summary>
        /// Un usuario socio solo puede consultar DATOS COMPLETOS de la razon social activa (la que
        /// eligio en UserSocio); los roles administrativos consultan cualquiera. Se compara por SCOD
        /// porque es la clave que recibe el lookup.
        /// </summary>
        private static bool CanLookupSocio(UserSocioAccessContext accessContext, string socioCode)
        {
            if (!RequiresActiveSocioScope(accessContext))
            {
                return true;
            }

            return !string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode)
                && string.Equals(socioCode.Trim(), accessContext.ActiveSocioCode.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Misma regla con la que TransanController, TranssbController y FutcontrolController
        /// aceptan guardar/editar una transferencia: un usuario socio tiene que ser vendedor o
        /// comprador. Asi el lookup de destino no habilita nada que el guardado no habilite.
        /// </summary>
        private static bool CanAccessTransfer(UserSocioAccessContext accessContext, string? sellerCode, string? buyerCode)
        {
            if (!accessContext.IsAuthenticated)
            {
                return false;
            }

            if (!RequiresActiveSocioScope(accessContext))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
            {
                return false;
            }

            var activo = accessContext.ActiveSocioCode.Trim();
            return string.Equals(sellerCode?.Trim(), activo, StringComparison.OrdinalIgnoreCase)
                || string.Equals(buyerCode?.Trim(), activo, StringComparison.OrdinalIgnoreCase);
        }

        private static Respuesta<T> BuildForbiddenResponse<T>(string? mensaje = null)
            => new Respuesta<T>
            {
                Exito = 0,
                Mensaje = mensaje ?? "No tenes permisos para consultar datos de transferencias."
            };
    }
}
