using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Services;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Dynamic;
using System.Reflection;
namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranssbController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITranssbRepositorio _transsbRepositorio;
        private readonly hereford_prContext _db;
        private readonly IUserSocioContextService _userSocioContextService;

        public TranssbController(
            ITranssbRepositorio transsbRepositorio,
            ITorosRepositorio torosRepositorio,
            IMapper mapper,
            hereford_prContext db,
            IUserSocioContextService userSocioContextService)
        {
            _mapper = mapper;
            _transsbRepositorio = transsbRepositorio;
            _db = db;
            _userSocioContextService = userSocioContextService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {
            Respuesta<List<TranssbDTO>> _ResponseDTO = new Respuesta<List<TranssbDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<TranssbDTO>>());
                }

                List<TranssbDTO> listaPedido = new List<TranssbDTO>();
                var a = RequiresActiveSocioScope(accessContext)
                    ? await _transsbRepositorio.LimitadosFiltrados(skip, take, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : await _transsbRepositorio.Lista(skip, take);

                listaPedido = _mapper.Map<List<TranssbDTO>>(a);

                _ResponseDTO = new Respuesta<List<TranssbDTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<TranssbDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                    var query = await _transsbRepositorio.Consultar(x =>
                        x.Sven == accessContext.ActiveSocioCode || x.Scom == accessContext.ActiveSocioCode);
                    a = query.Count();
                }
                else
                {
                    a = await _transsbRepositorio.CantidadTotal();
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
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<TranssbDTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? AppendFilter(expression, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : expression;
                var transfers = await _transsbRepositorio.LimitadosFiltrados(skip, take, effectiveExpression);

                var toroIds = transfers
                    .Where(t => t.Torovendido.HasValue)
                    .Select(t => t.Torovendido!.Value)
                    .Distinct()
                    .ToList();

                var toroExtras = await _db.Torosunis
                    .AsNoTracking()
                    .Where(t => toroIds.Contains(t.Id))
                    .ToDictionaryAsync(
                        t => t.Id,
                        t => new
                        {
                            Hba = t.Hba,
                            Tatpart = t.Tatpart,
                            ToroNombre = t.NomDad
                        });


                var resultList = new List<object>(transfers.Count);
                var dtoProps = typeof(TranssbDTO).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var t in transfers)
                {
                    var dto = _mapper.Map<TranssbDTO>(t);

                    var expando = new ExpandoObject() as IDictionary<string, object?>;
                    foreach (var p in dtoProps)
                        expando[p.Name] = p.GetValue(dto);

                    if (t.Torovendido.HasValue && toroExtras.TryGetValue(t.Torovendido.Value, out var te))
                        expando["Toro"] = te;
                    else
                        expando["Toro"] = null;

                    resultList.Add(expando!);
                }


                return Ok(new
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = resultList
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TranssbController] Error: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = (List<TranssbDTO>?)null
                });
            }
        }


        [HttpGet]
        [Route("LimitadosFiltradosNoInclude")]
        public async Task<IActionResult> LimitadosFiltradosNoInclude(int skip, int take, string? expression = null)
        {
            Respuesta<List<TranssbDTO>> _ResponseDTO = new Respuesta<List<TranssbDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<TranssbDTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? AppendFilter(expression, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : expression;
                var a = await _transsbRepositorio.LimitadosFiltradosNoInclude(skip, take, effectiveExpression);

                var listaFiltrada = _mapper.Map<List<TranssbDTO>>(a);

                _ResponseDTO = new Respuesta<List<TranssbDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<TranssbDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Transsb _TranssbEliminar = await _transsbRepositorio.Obtener(u => u.Id == id);
                if (_TranssbEliminar != null)
                {
                    if (!CanAccessTransfer(accessContext, _TranssbEliminar.Sven, _TranssbEliminar.Scom))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                    }

                    bool respuesta = await _transsbRepositorio.Eliminar(_TranssbEliminar);

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
        public async Task<IActionResult> Guardar([FromBody] TranssbDTO request)
        {
            Respuesta<TranssbDTO> _Respuesta = new Respuesta<TranssbDTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (!CanAccessTransfer(accessContext, request?.Sven, request?.Scom))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<TranssbDTO>());
                }

                Transsb _Transsb = _mapper.Map<Transsb>(request);

                Transsb _TranssbCreado = await _transsbRepositorio.Crear(_Transsb);

                if (_TranssbCreado.Id != 0)
                    _Respuesta = new Respuesta<TranssbDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<TranssbDTO>(_TranssbCreado) };
                else
                    _Respuesta = new Respuesta<TranssbDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<TranssbDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] TranssbDTO request)
        {
            Respuesta<TranssbDTO> _Respuesta = new Respuesta<TranssbDTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                Transsb _Transsb = _mapper.Map<Transsb>(request);
                Transsb _TranssbParaEditar = await _transsbRepositorio.Obtener(u => u.Id == _Transsb.Id);

                if (_TranssbParaEditar != null)
                {
                    if (!CanAccessTransfer(accessContext, _TranssbParaEditar.Sven, _TranssbParaEditar.Scom) ||
                        !CanAccessTransfer(accessContext, _Transsb.Sven, _Transsb.Scom))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<TranssbDTO>());
                    }

                    _TranssbParaEditar.NroTrans = _Transsb.NroTrans;
                    _TranssbParaEditar.Fectrans = _Transsb.Fectrans;
                    _TranssbParaEditar.NroOrden = _Transsb.NroOrden;
                    _TranssbParaEditar.Sven = _Transsb.Sven;
                    _TranssbParaEditar.CategSv = _Transsb.CategSv;
                    _TranssbParaEditar.Vnom = _Transsb.Vnom;
                    _TranssbParaEditar.Scom = _Transsb.Scom;
                    _TranssbParaEditar.CategSc = _Transsb.CategSc;
                    _TranssbParaEditar.Cnom = _Transsb.Cnom;
                    _TranssbParaEditar.Ecod = _Transsb.Ecod;
                    _TranssbParaEditar.FchUsu = _Transsb.FchUsu;
                    _TranssbParaEditar.CodUsu = _Transsb.CodUsu;
                    _TranssbParaEditar.Torovendido = _Transsb.Torovendido;
                    bool respuesta = await _transsbRepositorio.Editar(_TranssbParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<TranssbDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<TranssbDTO>(_TranssbParaEditar) };
                    else
                        _Respuesta = new Respuesta<TranssbDTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<TranssbDTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<TranssbDTO>() { Exito = 1, Mensaje = ex.Message };
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
