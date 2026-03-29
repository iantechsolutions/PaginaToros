using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Services;
using AutoMapper;
using System;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TorosController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITorosRepositorio _torosRepositorio;
        private readonly IUserSocioContextService _userSocioContextService;

        public TorosController(
            ITorosRepositorio torosRepositorio,
            IMapper mapper,
            IUserSocioContextService userSocioContextService)
        {
            _mapper = mapper;
            _torosRepositorio = torosRepositorio;
            _userSocioContextService = userSocioContextService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip,int take)
        {

            Respuesta<List<TorosuniDTO>> _ResponseDTO = new Respuesta<List<TorosuniDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<TorosuniDTO>>());
                }

                List<TorosuniDTO> listaPedido = new List<TorosuniDTO>();
                var a = RequiresActiveSocioScope(accessContext)
                    ? await _torosRepositorio.LimitadosFiltrados(skip, take, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : await _torosRepositorio.Lista(skip, take);

                
                listaPedido = _mapper.Map<List<TorosuniDTO>>(a);

                _ResponseDTO = new Respuesta<List<TorosuniDTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
             

            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<TorosuniDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                    var query = await _torosRepositorio.Consultar(x => x.Criador == accessContext.ActiveSocioCode);
                    a = query.Count();
                }
                else
                {
                    a = await _torosRepositorio.CantidadTotal();
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
        [Route("CantidadFiltrada")]
        public async Task<IActionResult> CantidadFiltrada(string? expression = null)
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<int>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? AppendFilter(expression, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : expression;
                var count = await _torosRepositorio.CantidadFiltrada(effectiveExpression);
                return Ok(new Respuesta<int> { Exito = 1, Mensaje = "Éxito", List = count });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Respuesta<int> { Exito = 0, Mensaje = ex.Message, List = 0 });
            }
        }
        [HttpGet]
        [Route("LimitadosFiltrados")]
        public async Task<IActionResult> LimitadosFiltrados(int skip, int take, string? expression = null)
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<TorosuniDTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? AppendFilter(expression, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : expression;
                var entidades = await _torosRepositorio.LimitadosFiltrados(skip, take, effectiveExpression);

                var listaFiltrada = _mapper.Map<List<TorosuniDTO>>(entidades)
                    .GroupBy(x => x.Id)
                    .Select(g => g.First())
                    .ToList();

                var resp = new Respuesta<List<TorosuniDTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = listaFiltrada
                };

                return Ok(resp);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TorosController] Error: {ex}");
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Respuesta<List<TorosuniDTO>>
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

            Respuesta<List<TorosuniDTO>> _ResponseDTO = new Respuesta<List<TorosuniDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<TorosuniDTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? AppendFilter(expression, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : expression;
                var a = await _torosRepositorio.LimitadosFiltradosNoInclude(skip, take, effectiveExpression);

                var listaFiltrada = _mapper.Map<List<TorosuniDTO>>(a);

                _ResponseDTO = new Respuesta<List<TorosuniDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<TorosuniDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }

        [HttpGet("ById/{id:int}")]
        public async Task<IActionResult> ById(int id)
        {
            Console.WriteLine($"=== ENTRO Toros/ById/{id} ===");
            try
            {
                var repo = await _torosRepositorio.GetById(id);
                Console.WriteLine($"[Ctrl] Repo.Exito={repo.Exito}, HasObj={repo.List != null}");

                if (repo.Exito == 0 || repo.List == null)
                {
                    Console.WriteLine($"[Ctrl] NOT FOUND Id={id}. Msg={repo.Mensaje}");
                    return NotFound(new Respuesta<TorosuniDTO>
                    {
                        Exito = 0,
                        Mensaje = repo.Mensaje
                    });
                }

                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext))
                {
                    if (string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode) ||
                        !string.Equals(repo.List.Criador, accessContext.ActiveSocioCode, StringComparison.OrdinalIgnoreCase))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden,
                            BuildForbiddenResponse<TorosuniDTO>());
                    }
                }

                var dto = _mapper.Map<TorosuniDTO>(repo.List);
                Console.WriteLine($"[Ctrl] Map OK -> DTO Id={dto?.Id}");

                var resp = new Respuesta<TorosuniDTO>
                {
                    Exito = 1,
                    Mensaje = "Éxito",
                    List = dto
                };

                Console.WriteLine("=== SALIO OK Toros/ById ===");
                return Ok(resp);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Ctrl] EX ById {id}: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Respuesta<TorosuniDTO> { Exito = 0, Mensaje = ex.Message });
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

                Torosuni _ToroEliminar = await _torosRepositorio.Obtener(u => u.Id == id);
                if (_ToroEliminar != null)
                {
                    if (RequiresActiveSocioScope(accessContext) &&
                        !string.Equals(_ToroEliminar.Criador, accessContext.ActiveSocioCode, StringComparison.OrdinalIgnoreCase))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                    }

                    bool respuesta = await _torosRepositorio.Eliminar(_ToroEliminar);

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
        public async Task<IActionResult> Guardar([FromBody] TorosuniDTO request)
        {
            Respuesta<TorosuniDTO> _Respuesta = new Respuesta<TorosuniDTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<TorosuniDTO>());
                }

                Torosuni _Toro = _mapper.Map<Torosuni>(request);
                if (RequiresActiveSocioScope(accessContext))
                {
                    _Toro.Criador = accessContext.ActiveSocioCode;
                }

                Torosuni _ToroCreado = await _torosRepositorio.Crear(_Toro);

                if (_ToroCreado.Id != 0)
                    _Respuesta = new Respuesta<TorosuniDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<TorosuniDTO>(_ToroCreado) };
                else
                    _Respuesta = new Respuesta<TorosuniDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<TorosuniDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] TorosuniDTO request)
        {
            var resp = new Respuesta<TorosuniDTO>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<TorosuniDTO>());
                }

                Torosuni entidad;

                if (request.Id != 0)
                {
                    entidad = await _torosRepositorio.Obtener(t => t.Id == request.Id);

                    if (entidad == null)
                    {
                        resp = new Respuesta<TorosuniDTO> { Exito = 0, Mensaje = "No se encontró el identificador" };
                        return StatusCode(StatusCodes.Status404NotFound, resp);
                    }

                    if (RequiresActiveSocioScope(accessContext) &&
                        !string.Equals(entidad.Criador, accessContext.ActiveSocioCode, StringComparison.OrdinalIgnoreCase))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<TorosuniDTO>());
                    }

                    if (!string.IsNullOrWhiteSpace(request.Criador))
                        request.Criador = request.Criador.Trim();

                    _mapper.Map(request, entidad);

                    if (RequiresActiveSocioScope(accessContext))
                    {
                        entidad.Criador = accessContext.ActiveSocioCode;
                    }


                    var ok = await _torosRepositorio.Editar(entidad);
                    if (!ok)
                    {
                        resp = new Respuesta<TorosuniDTO> { Exito = 0, Mensaje = "No se pudo editar el identificador" };
                        return StatusCode(StatusCodes.Status200OK, resp);
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(request.Criador))
                        request.Criador = request.Criador.Trim();

                    entidad = _mapper.Map<Torosuni>(request);
                    if (RequiresActiveSocioScope(accessContext))
                    {
                        entidad.Criador = accessContext.ActiveSocioCode;
                    }
                    entidad = await _torosRepositorio.Crear(entidad);

                    if (entidad.Id == 0)
                    {
                        resp = new Respuesta<TorosuniDTO> { Exito = 0, Mensaje = "No se pudo crear el identificador" };
                        return StatusCode(StatusCodes.Status200OK, resp);
                    }
                }

                resp = new Respuesta<TorosuniDTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<TorosuniDTO>(entidad) };
                return StatusCode(StatusCodes.Status200OK, resp);
            }
            catch (Exception ex)
            {
                resp = new Respuesta<TorosuniDTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, resp);
            }
        }

        private static bool RequiresActiveSocioScope(UserSocioAccessContext accessContext)
            => accessContext.IsSocioUser && !accessContext.IsPrivilegedUser;

        private static string BuildActiveSocioFilter(string activeSocioCode)
            => $"Criador == \"{activeSocioCode}\"";

        private static string AppendFilter(string? expression, string requiredFilter)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return requiredFilter;
            }

            return $"({expression}) and ({requiredFilter})";
        }

        private static Respuesta<T> BuildForbiddenResponse<T>()
            => new Respuesta<T>
            {
                Exito = 0,
                Mensaje = "No tenes permisos para operar sobre otra razon social."
            };
    }
}
