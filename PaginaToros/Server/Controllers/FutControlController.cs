using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Services;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FutcontrolController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFutcontrolRepositorio _futcontrolRepositorio;
        private readonly IUserSocioContextService _userSocioContextService;

        public FutcontrolController(
            IFutcontrolRepositorio futcontrolRepositorio,
            IMapper mapper,
            IUserSocioContextService userSocioContextService)
        {
            _mapper = mapper;
            _futcontrolRepositorio = futcontrolRepositorio;
            _userSocioContextService = userSocioContextService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {
            Respuesta<List<FutcontrolDTO>> _ResponseDTO = new Respuesta<List<FutcontrolDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<FutcontrolDTO>>());
                }

                List<FutcontrolDTO> listaPedido = new List<FutcontrolDTO>();
                var a = RequiresActiveSocioScope(accessContext)
                    ? await _futcontrolRepositorio.LimitadosFiltrados(skip, take, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : await _futcontrolRepositorio.Lista(skip, take);

                listaPedido = _mapper.Map<List<FutcontrolDTO>>(a);

                _ResponseDTO = new Respuesta<List<FutcontrolDTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<FutcontrolDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                    var query = await _futcontrolRepositorio.Consultar(x =>
                        x.Sven == accessContext.ActiveSocioCode || x.Scom == accessContext.ActiveSocioCode);
                    a = query.Count();
                }
                else
                {
                    a = await _futcontrolRepositorio.CantidadTotal();
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
            Respuesta<List<FutcontrolDTO>> _ResponseDTO = new Respuesta<List<FutcontrolDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<FutcontrolDTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? AppendFilter(expression, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : expression;
                var a = await _futcontrolRepositorio.LimitadosFiltrados(skip, take, effectiveExpression);

                var listaFiltrada = _mapper.Map<List<FutcontrolDTO>>(a);

                _ResponseDTO = new Respuesta<List<FutcontrolDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<FutcontrolDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Futcontrol _FutcontrolEliminar = await _futcontrolRepositorio.Obtener(u => u.Id == id);
                if (_FutcontrolEliminar != null)
                {
                    if (!CanAccessTransfer(accessContext, _FutcontrolEliminar.Sven, _FutcontrolEliminar.Scom))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                    }

                    bool respuesta = await _futcontrolRepositorio.Eliminar(_FutcontrolEliminar);

                    if (respuesta)
                        _Respuesta = new Respuesta<string>() { Exito = 1, Mensaje = "ok", List = "" };
                    else
                        _Respuesta = new Respuesta<string>() { Exito = 1, Mensaje = "No se pudo eliminar el identificador", List = "" };
                }
                else
                {
                    _Respuesta = new Respuesta<string>() { Exito = 0, Mensaje = "No se encontró el identificador", List = "" };
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
        public async Task<IActionResult> Guardar([FromBody] FutcontrolDTO request)
        {
            Respuesta<FutcontrolDTO> _Respuesta = new Respuesta<FutcontrolDTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (!CanAccessTransfer(accessContext, request?.Sven, request?.Scom))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<FutcontrolDTO>());
                }

                Futcontrol _Futcontrol = _mapper.Map<Futcontrol>(request);

                Futcontrol _FutcontrolCreado = await _futcontrolRepositorio.Crear(_Futcontrol);

                if (_FutcontrolCreado.Id != 0)
                    _Respuesta = new Respuesta<FutcontrolDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<FutcontrolDTO>(_FutcontrolCreado) };
                else
                    _Respuesta = new Respuesta<FutcontrolDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<FutcontrolDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] FutcontrolDTO request)
        {
            Respuesta<FutcontrolDTO> _Respuesta = new Respuesta<FutcontrolDTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                Futcontrol _Futcontrol = _mapper.Map<Futcontrol>(request);
                Futcontrol _FutcontrolParaEditar = await _futcontrolRepositorio.Obtener(u => u.Id == _Futcontrol.Id);

                if (_FutcontrolParaEditar != null)
                {
                    if (!CanAccessTransfer(accessContext, _FutcontrolParaEditar.Sven, _FutcontrolParaEditar.Scom) ||
                        !CanAccessTransfer(accessContext, _Futcontrol.Sven, _Futcontrol.Scom))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<FutcontrolDTO>());
                    }

                    _FutcontrolParaEditar.NroTrans = _Futcontrol.NroTrans;
                    _FutcontrolParaEditar.Fectrans = _Futcontrol.Fectrans;
                    _FutcontrolParaEditar.Sven = _Futcontrol.Sven;
                    _FutcontrolParaEditar.CategSv = _Futcontrol.CategSv;
                    _FutcontrolParaEditar.Vnom = _Futcontrol.Vnom;
                    _FutcontrolParaEditar.Scom = _Futcontrol.Scom;
                    _FutcontrolParaEditar.CategSc = _Futcontrol.CategSc;
                    _FutcontrolParaEditar.Cnom = _Futcontrol.Cnom;
                    _FutcontrolParaEditar.Plantel = _Futcontrol.Plantel;
                    _FutcontrolParaEditar.EdadCrias = _Futcontrol.EdadCrias;
                    _FutcontrolParaEditar.CantHem = _Futcontrol.CantHem;
                    _FutcontrolParaEditar.CantMach = _Futcontrol.CantMach;
                    _FutcontrolParaEditar.PlantDest = _Futcontrol.PlantDest;
                    _FutcontrolParaEditar.Incorp = _Futcontrol.Incorp;
                    _FutcontrolParaEditar.Hemsta = _Futcontrol.Hemsta;
                    _FutcontrolParaEditar.FchUsu = _Futcontrol.FchUsu;
                    _FutcontrolParaEditar.CodUsu = _Futcontrol.CodUsu;
                    bool respuesta = await _futcontrolRepositorio.Editar(_FutcontrolParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<FutcontrolDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<FutcontrolDTO>(_FutcontrolParaEditar) };
                    else
                        _Respuesta = new Respuesta<FutcontrolDTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<FutcontrolDTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<FutcontrolDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        private static bool RequiresActiveSocioScope(UserSocioAccessContext accessContext)
            => accessContext.IsSocioUser && !accessContext.IsPrivilegedUser;

        private static bool CanAccessTransfer(UserSocioAccessContext accessContext, string? sellerCode, string? buyerCode)
        {
            if (!RequiresActiveSocioScope(accessContext))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
            {
                return false;
            }

            return string.Equals(sellerCode, accessContext.ActiveSocioCode, StringComparison.OrdinalIgnoreCase)
                || string.Equals(buyerCode, accessContext.ActiveSocioCode, StringComparison.OrdinalIgnoreCase);
        }

        private static string BuildActiveSocioFilter(string socioCode)
            => $"(Sven == \"{EscapeValue(socioCode)}\" || Scom == \"{EscapeValue(socioCode)}\")";

        private static string AppendFilter(string? expression, string enforcedFilter)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return enforcedFilter;
            }

            return $"({expression}) && ({enforcedFilter})";
        }

        private static string EscapeValue(string value)
            => value.Replace("\\", "\\\\").Replace("\"", "\\\"");

        private static Respuesta<T> BuildForbiddenResponse<T>()
            => new Respuesta<T>
            {
                Exito = 0,
                Mensaje = "No tenes permisos para operar sobre otra razon social."
            };
    }
}
