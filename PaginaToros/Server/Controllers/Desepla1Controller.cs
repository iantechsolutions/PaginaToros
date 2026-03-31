using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Context;
using AutoMapper;
using PaginaToros.Server.Repositorio.Contrato;
using Newtonsoft.Json;
using PaginaToros.Server.Services;
using System.Globalization;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Desepla1Controller : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDesepla1Repositorio _Desepla1Repositorio;
        private readonly IUserSocioContextService _userSocioContextService;
        private readonly hereford_prContext _dbContext;

        public Desepla1Controller(
            IDesepla1Repositorio Desepla1Repositorio,
            IMapper mapper,
            IUserSocioContextService userSocioContextService,
            hereford_prContext dbContext)
        {
            _mapper = mapper;
            _Desepla1Repositorio = Desepla1Repositorio;
            _userSocioContextService = userSocioContextService;
            _dbContext = dbContext;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {
            Respuesta<List<Desepla1DTO>> _ResponseDTO = new Respuesta<List<Desepla1DTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Desepla1DTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? BuildActiveSocioFilter(accessContext.ActiveSocioCode!)
                    : null;
                var lista = await _Desepla1Repositorio.LimitadosFiltrados(skip, take, effectiveExpression);
                var listaPedido = DeduplicateByDeclaration(_mapper.Map<List<Desepla1DTO>>(lista));

                _ResponseDTO = new Respuesta<List<Desepla1DTO>>()
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = listaPedido
                };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Desepla1DTO>>()
                {
                    Exito = 1,
                    Mensaje = ex.Message,
                    List = null
                };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }

        [HttpGet]
        [Route("Cantidad")]
        public async Task<IActionResult> CantidadTotal()
        {

            Respuesta<int> _ResponseDTO = new Respuesta<int>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<int>());
                }

                var query = _dbContext.Desepla1s.AsNoTracking().AsQueryable();
                if (RequiresActiveSocioScope(accessContext))
                {
                    query = query.Where(x => x.Nrocri == accessContext.ActiveSocioCode);
                }

                var a = await query
                    .Select(x => x.Nrodec)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .CountAsync();

                _ResponseDTO = new Respuesta<int>() { Exito = 1, Mensaje = "Exito", List = a };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<int>() { Exito = 1, Mensaje = ex.Message, List = 0 };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }

        [HttpGet("LimitadosFiltrados")]
        public async Task<IActionResult> LimitadosFiltrados(int skip, int take, string? expression = null)
        {
            var resp = new Respuesta<List<Desepla1DTO>>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Desepla1DTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? AppendFilter(expression, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : expression;
                var lista = await _Desepla1Repositorio.LimitadosFiltrados(skip, take, effectiveExpression);

                resp.Exito = 1;
                resp.Mensaje = "Éxito";
                resp.List = DeduplicateByDeclaration(_mapper.Map<List<Desepla1DTO>>(lista));
                return Ok(resp);
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = ex.Message;
                resp.List = null;
                return StatusCode(StatusCodes.Status500InternalServerError, resp);
            }
        }




        [HttpGet]
        [Route("LimitadosFiltradosNoInclude")]
        public async Task<IActionResult> LimitadosFiltradosNoInclude(int skip, int take, string? expression = null)
        {

            Respuesta<List<Desepla1DTO>> _ResponseDTO = new Respuesta<List<Desepla1DTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Desepla1DTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? AppendFilter(expression, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : expression;
                var a = await _Desepla1Repositorio.LimitadosFiltradosNoInclude(skip, take, effectiveExpression);

                var listaFiltrada = DeduplicateByDeclaration(_mapper.Map<List<Desepla1DTO>>(a));

                _ResponseDTO = new Respuesta<List<Desepla1DTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Desepla1DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            Respuesta<string> _Respuesta = new Respuesta<string>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                }

                Desepla1 _Desepla1Eliminar = await _Desepla1Repositorio.Obtener(u => u.Id == id);
                if (_Desepla1Eliminar != null)
                {
                    if (RequiresActiveSocioScope(accessContext) &&
                        !string.Equals(_Desepla1Eliminar.Nrocri, accessContext.ActiveSocioCode, StringComparison.OrdinalIgnoreCase))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                    }

                    bool respuesta = await _Desepla1Repositorio.Eliminar(_Desepla1Eliminar);

                    if (respuesta)
                        _Respuesta = new Respuesta<string>() { Exito = 1, Mensaje = "ok", List = "" };
                    else
                        _Respuesta = new Respuesta<string>() { Exito = 1, Mensaje = "No se pudo eliminar el identificador", List = "" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<string>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] Desepla1DTO request)
        {
            Respuesta<Desepla1DTO> _Respuesta = new Respuesta<Desepla1DTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Desepla1DTO>());
                }

                Console.WriteLine("Request recibido:");
                Console.WriteLine(JsonConvert.SerializeObject(request));
                Desepla1 _Desepla1 = _mapper.Map<Desepla1>(request);

                if (RequiresActiveSocioScope(accessContext))
                {
                    _Desepla1.Nrocri = accessContext.ActiveSocioCode!;
                }

                if (!string.IsNullOrWhiteSpace(_Desepla1.Nroplan) &&
                    RequiresActiveSocioScope(accessContext) &&
                    !await ActiveSocioOwnsPlantelAsync(_Desepla1.Nroplan, accessContext.ActiveSocioCode!))
                {
                    return BadRequest(new Respuesta<Desepla1DTO>
                    {
                        Exito = 0,
                        Mensaje = "El plantel seleccionado no pertenece a la razon social activa."
                    });
                }

                _Desepla1.Nrodec = await GenerateNextNrodecAsync();

                Console.WriteLine("Entro aca?");

                Desepla1 _Desepla1Creado = await _Desepla1Repositorio.Crear(_Desepla1);

                if (_Desepla1Creado.Id != 0)
                    _Respuesta = new Respuesta<Desepla1DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Desepla1DTO>(_Desepla1Creado) };
                else
                    _Respuesta = new Respuesta<Desepla1DTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.ToString());
                _Respuesta = new Respuesta<Desepla1DTO>() { Exito = 0, Mensaje = ex.ToString() };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Desepla1DTO request)
        {
            Respuesta<Desepla1DTO> _Respuesta = new Respuesta<Desepla1DTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Desepla1DTO>());
                }

                Desepla1 _Desepla1 = _mapper.Map<Desepla1>(request);
                Desepla1 _Desepla1ParaEditar = await _Desepla1Repositorio.Obtener(u => u.Id == _Desepla1.Id);

                if (_Desepla1ParaEditar != null)
                {
                    if (RequiresActiveSocioScope(accessContext) &&
                        !string.Equals(_Desepla1ParaEditar.Nrocri, accessContext.ActiveSocioCode, StringComparison.OrdinalIgnoreCase))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Desepla1DTO>());
                    }

                    if (!string.IsNullOrWhiteSpace(_Desepla1.Nroplan) &&
                        RequiresActiveSocioScope(accessContext) &&
                        !await ActiveSocioOwnsPlantelAsync(_Desepla1.Nroplan, accessContext.ActiveSocioCode!))
                    {
                        return BadRequest(new Respuesta<Desepla1DTO>
                        {
                            Exito = 0,
                            Mensaje = "El plantel seleccionado no pertenece a la razon social activa."
                        });
                    }

                    _Desepla1ParaEditar.Nrodec = _Desepla1.Nrodec;
                    _Desepla1ParaEditar.Nroplan = _Desepla1.Nroplan;
                    _Desepla1ParaEditar.Tipse = _Desepla1.Tipse;
                    _Desepla1ParaEditar.Semen = _Desepla1.Semen;
                    _Desepla1ParaEditar.Cantv = _Desepla1.Cantv;
                    _Desepla1ParaEditar.Cantb = _Desepla1.Cantb;
                    _Desepla1ParaEditar.Remba = _Desepla1.Remba;
                    _Desepla1ParaEditar.Remba = _Desepla1.Remba;
                    _Desepla1ParaEditar.Rempr = _Desepla1.Rempr;
                    _Desepla1ParaEditar.Ctrlu = _Desepla1.Ctrlu;
                    _Desepla1ParaEditar.Ctrlm = _Desepla1.Ctrlm;
                    _Desepla1ParaEditar.CoefAutoSn = _Desepla1.CoefAutoSn;
                    _Desepla1ParaEditar.CoefAutoIa = _Desepla1.CoefAutoIa;
                    _Desepla1ParaEditar.CoefAutoIar = _Desepla1.CoefAutoIar;
                    _Desepla1ParaEditar.IaSincro = _Desepla1.IaSincro;
                    _Desepla1ParaEditar.PastillasSincro = _Desepla1.PastillasSincro;
                    _Desepla1ParaEditar.Fecret = _Desepla1.Fecret;
                    _Desepla1ParaEditar.NrFolio = _Desepla1.NrFolio;
                    _Desepla1ParaEditar.FchUsu = _Desepla1.FchUsu;
                    _Desepla1ParaEditar.CodUsu = _Desepla1.CodUsu;
                    _Desepla1ParaEditar.Reten = _Desepla1.Reten;
                    _Desepla1ParaEditar.Edicion = _Desepla1.Edicion;
                    _Desepla1ParaEditar.Nrocri = RequiresActiveSocioScope(accessContext)
                        ? accessContext.ActiveSocioCode!
                        : _Desepla1.Nrocri;
                    _Desepla1ParaEditar.Desde = _Desepla1.Desde;
                    _Desepla1ParaEditar.Hasta = _Desepla1.Hasta;
                    _Desepla1ParaEditar.Fecdecl = _Desepla1.Fecdecl;
                    _Desepla1ParaEditar.Fchrecepcion = _Desepla1.Fchrecepcion;

                    bool respuesta = await _Desepla1Repositorio.Editar(_Desepla1ParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<Desepla1DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Desepla1DTO>(_Desepla1ParaEditar) };
                    else
                        _Respuesta = new Respuesta<Desepla1DTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<Desepla1DTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Desepla1DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPost("RecalcularPeriodosCabecera")]
        public async Task<IActionResult> RecalcularPeriodosCabecera()
        {
            var response = new Respuesta<int>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (!accessContext.IsPrivilegedUser)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<int>());
                }

                var headers = await _dbContext.Desepla1s.ToListAsync();
                var details = await _dbContext.Desepla3s
                    .AsNoTracking()
                    .Where(x => !string.IsNullOrWhiteSpace(x.Nrodec))
                    .Select(x => new { x.Nrodec, x.Desde, x.Hasta })
                    .ToListAsync();

                var periodosPorDeclaracion = details
                    .Select(x => new
                    {
                        Nrodec = x.Nrodec!,
                        Desde = TryParseDetalleDate(x.Desde),
                        Hasta = TryParseDetalleDate(x.Hasta)
                    })
                    .Where(x => x.Desde.HasValue && x.Hasta.HasValue)
                    .GroupBy(x => x.Nrodec)
                    .ToDictionary(
                        g => g.Key,
                        g => new
                        {
                            Desde = g.Min(x => x.Desde)!.Value,
                            Hasta = g.Max(x => x.Hasta)!.Value
                        });

                var updated = 0;
                foreach (var header in headers)
                {
                    if (string.IsNullOrWhiteSpace(header.Nrodec))
                    {
                        continue;
                    }

                    if (!periodosPorDeclaracion.TryGetValue(header.Nrodec, out var periodo))
                    {
                        continue;
                    }

                    if (header.Desde != periodo.Desde || header.Hasta != periodo.Hasta)
                    {
                        header.Desde = periodo.Desde;
                        header.Hasta = periodo.Hasta;
                        updated++;
                    }
                }

                if (updated > 0)
                {
                    await _dbContext.SaveChangesAsync();
                }

                response.Exito = 1;
                response.Mensaje = $"Se recalcularon {updated} declaraciones.";
                response.List = updated;
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response.Exito = 0;
                response.Mensaje = ex.Message;
                response.List = 0;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        private async Task<string> GenerateNextNrodecAsync(CancellationToken cancellationToken = default)
        {
            var nrodecs = await _dbContext.Desepla1s
                .AsNoTracking()
                .Select(x => x.Nrodec)
                .ToListAsync(cancellationToken);

            var maxNrodec = 0;
            foreach (var nrodec in nrodecs)
            {
                if (int.TryParse(nrodec, out var parsed) && parsed > maxNrodec)
                {
                    maxNrodec = parsed;
                }
            }

            return (maxNrodec + 1).ToString();
        }

        private async Task<bool> ActiveSocioOwnsPlantelAsync(string nroplan, string activeSocioCode, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Planteles
                .AsNoTracking()
                .AnyAsync(
                    x => x.Placod == nroplan && x.Nrocri == activeSocioCode,
                    cancellationToken);
        }

        private static List<Desepla1DTO> DeduplicateByDeclaration(List<Desepla1DTO>? list)
        {
            if (list is null)
            {
                return new List<Desepla1DTO>();
            }

            return list
                .GroupBy(x => x.Nrodec ?? string.Empty)
                .Select(g => g.OrderByDescending(x => x.Id).First())
                .OrderByDescending(x => x.Nrodec)
                .ToList();
        }

        private static bool RequiresActiveSocioScope(UserSocioAccessContext accessContext)
            => accessContext.IsSocioUser && !accessContext.IsPrivilegedUser;

        private static string BuildActiveSocioFilter(string activeSocioCode)
            => $"Nrocri == \"{activeSocioCode}\"";

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

        private static DateTime? TryParseDetalleDate(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return DateTime.TryParseExact(
                value,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var parsed)
                ? parsed
                : null;
        }
    }
}
