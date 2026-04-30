using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Client.Pages.Establecimientos;
using PaginaToros.Client.Pages.Socios;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Services;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstablecimientoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEstableRepositorio _EstableRepositorio;
        private readonly IUserSocioContextService _userSocioContextService;
        private readonly hereford_prContext _dbContext;

        public EstablecimientoController(
            IEstableRepositorio EstableRepositorio,
            IMapper mapper,
            IUserSocioContextService userSocioContextService,
            hereford_prContext dbContext)
        {
            _mapper = mapper;
            _EstableRepositorio = EstableRepositorio;
            _userSocioContextService = userSocioContextService;
            _dbContext = dbContext;
        }

        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {
            var _ResponseDTO = new Respuesta<List<EstableDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<EstableDTO>>());
                }

                var entities = RequiresActiveSocioScope(accessContext)
                    ? await _EstableRepositorio.LimitadosFiltrados(skip, take, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : await _EstableRepositorio.Lista(skip, take);

                var listaPedido = _mapper.Map<List<EstableDTO>>(entities);

                _ResponseDTO = new Respuesta<List<EstableDTO>> { Exito = 1, Mensaje = "Exito", List = listaPedido };
                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<EstableDTO>> { Exito = 0, Mensaje = ex.Message, List = null };
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

                int a;
                if (RequiresActiveSocioScope(accessContext))
                {
                    var query = await _EstableRepositorio.Consultar(x => x.Codsoc == accessContext.ActiveSocioCode);
                    a = query.Count();
                }
                else
                {
                    a = await _EstableRepositorio.CantidadTotal();
                }

                _ResponseDTO = new Respuesta<int>() { Exito = 1, Mensaje = "Exito", List = a };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<int>() { Exito = 1, Mensaje = ex.Message, List = 0 };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }
        [HttpGet]
        [Route("LimitadosFiltrados")]
        public async Task<IActionResult> LimitadosFiltrados(int skip, int take, string? expression = null)
        {

            Respuesta<List<EstableDTO>> _ResponseDTO = new Respuesta<List<EstableDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<EstableDTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? AppendFilter(expression, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : expression;
                var a = await _EstableRepositorio.LimitadosFiltrados(skip, take, effectiveExpression);

                var listaFiltrada = _mapper.Map<List<EstableDTO>>(a);

                _ResponseDTO = new Respuesta<List<EstableDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<EstableDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }
        [HttpGet]
        [Route("LimitadosFiltradosNoInclude")]
        public async Task<IActionResult> LimitadosFiltradosNoInclude(int skip, int take, string? expression = null)
        {

            Respuesta<List<EstableDTO>> _ResponseDTO = new Respuesta<List<EstableDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<EstableDTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? AppendFilter(expression, BuildActiveSocioCodeFilter(accessContext.ActiveSocioCode!))
                    : expression;
                var a = await _EstableRepositorio.LimitadosFiltradosNoInclude(skip, take, effectiveExpression);

                var listaFiltrada = _mapper.Map<List<EstableDTO>>(a);

                _ResponseDTO = new Respuesta<List<EstableDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<EstableDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string? term = null, int? socioId = null, string? socioIds = null, int take = 20)
        {
            try
            {
                if (take <= 0)
                {
                    take = 20;
                }

                var accessContext = await _userSocioContextService.ResolveAsync(User);
                var scopedSocioIds = new List<int>();
                if (!string.IsNullOrWhiteSpace(socioIds))
                {
                    scopedSocioIds = socioIds
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => int.TryParse(s, out var parsed) ? parsed : 0)
                        .Where(id => id > 0)
                        .Distinct()
                        .ToList();
                }

                var query = _dbContext.Estables
                    .AsNoTracking()
                    .Include(e => e.Socio)
                    .AsQueryable();

                if (RequiresActiveSocioScope(accessContext))
                {
                    query = query.Where(e => e.Socio != null && accessContext.AllowedSocioIds.Contains(e.Socio.Id));
                }

                if (socioId.HasValue)
                {
                    query = query.Where(e => e.Socio != null && e.Socio.Id == socioId.Value);
                }
                else if (scopedSocioIds.Count > 0)
                {
                    query = query.Where(e => e.Socio != null && scopedSocioIds.Contains(e.Socio.Id));
                }

                if (!string.IsNullOrWhiteSpace(term))
                {
                    var normalized = term.Trim();
                    query = query.Where(e =>
                        (e.Nombre != null && e.Nombre.Contains(normalized)) ||
                        (e.Ecod != null && e.Ecod.Contains(normalized)) ||
                        (e.Codsoc != null && e.Codsoc.Contains(normalized)));
                }

                var result = await query
                    .OrderBy(e => e.Nombre)
                    .Take(take)
                    .Select(e => new TorosFilterOptionDTO
                    {
                        IntValue = e.Id,
                        Value = e.Id.ToString(),
                        Label = string.IsNullOrWhiteSpace(e.Ecod)
                            ? (e.Nombre ?? string.Empty)
                            : $"{e.Ecod} - {e.Nombre}"
                    })
                    .ToListAsync();

                return Ok(new Respuesta<List<TorosFilterOptionDTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<List<TorosFilterOptionDTO>>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = null
                });
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

                Estable _EstableEliminar = await _EstableRepositorio.Obtener(u => u.Id == id);
                if (_EstableEliminar != null)
                {
                    if (RequiresActiveSocioScope(accessContext) &&
                        !string.Equals(_EstableEliminar.Codsoc, accessContext.ActiveSocioCode, StringComparison.OrdinalIgnoreCase))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                    }

                    bool respuesta = await _EstableRepositorio.Eliminar(_EstableEliminar);

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
        public async Task<IActionResult> Guardar([FromBody] EstableDTO request)
        {
            Respuesta<EstableDTO> _Respuesta = new Respuesta<EstableDTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<EstableDTO>());
                }

                Estable _Estable = _mapper.Map<Estable>(request);
                if (RequiresActiveSocioScope(accessContext))
                {
                    _Estable.Codsoc = accessContext.ActiveSocioCode;
                }

                var EstL = await _EstableRepositorio.Lista(0, 1);
                Estable _EstableViejo = EstL.FirstOrDefault();
                _Estable.Ecod = (Int32.Parse(_EstableViejo.Ecod) + 1).ToString("D6");

                Estable _EstableCreado = await _EstableRepositorio.Crear(_Estable);

                if (_EstableCreado.Id != 0)
                    _Respuesta = new Respuesta<EstableDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<EstableDTO>(_EstableCreado) };
                else
                    _Respuesta = new Respuesta<EstableDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<EstableDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] EstableDTO request)
        {
            Respuesta<EstableDTO> _Respuesta = new Respuesta<EstableDTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<EstableDTO>());
                }

                Estable _Estable = _mapper.Map<Estable>(request);
                Estable _EstableParaEditar = await _EstableRepositorio.Obtener(u => u.Id == _Estable.Id);

                if (_EstableParaEditar != null)
                {
                    if (RequiresActiveSocioScope(accessContext) &&
                        !string.Equals(_EstableParaEditar.Codsoc, accessContext.ActiveSocioCode, StringComparison.OrdinalIgnoreCase))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<EstableDTO>());
                    }

                    _EstableParaEditar.Ecod = _Estable.Ecod;
                    _EstableParaEditar.Codsoc = RequiresActiveSocioScope(accessContext)
                        ? accessContext.ActiveSocioCode
                        : _Estable.Codsoc;
                    _EstableParaEditar.Activo = _Estable.Activo;
                    _EstableParaEditar.Nombre = _Estable.Nombre;
                    _EstableParaEditar.Encargado = _Estable.Encargado;
                    _EstableParaEditar.Direcc = _Estable.Direcc;
                    _EstableParaEditar.Locali = _Estable.Locali;
                    _EstableParaEditar.Tel = _Estable.Tel;
                    _EstableParaEditar.Codpos = _Estable.Codpos;
                    _EstableParaEditar.Codpro = _Estable.Codpro;
                    _EstableParaEditar.Plano = _Estable.Plano;
                    _EstableParaEditar.Catego = _Estable.Catego;
                    _EstableParaEditar.Codzon = _Estable.Codzon;
                    _EstableParaEditar.Fechacreacion = _Estable.Fechacreacion;
                    _EstableParaEditar.Fecing = _Estable.Fecing;

                    bool respuesta = await _EstableRepositorio.Editar(_EstableParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<EstableDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<EstableDTO>(_EstableParaEditar) };
                    else
                        _Respuesta = new Respuesta<EstableDTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<EstableDTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<EstableDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        private static bool RequiresActiveSocioScope(UserSocioAccessContext accessContext)
            => accessContext.IsSocioUser && !accessContext.IsPrivilegedUser;

        private static string BuildActiveSocioFilter(string activeSocioCode)
            => $"Socio != null && Socio.Scod == \"{activeSocioCode}\"";

        private static string BuildActiveSocioCodeFilter(string activeSocioCode)
            => $"Codsoc == \"{activeSocioCode}\"";

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
