using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using AutoMapper;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Services;
using System.Linq.Dynamic.Core;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Desepla3Controller : ControllerBase
    {
        private const int MaxNrodecLength = 15;
        private readonly IMapper _mapper;
        private readonly IDesepla3Repositorio _Desepla3Repositorio;
        private readonly IUserSocioContextService _userSocioContextService;
        private readonly hereford_prContext _dbContext;

        public Desepla3Controller(
            IDesepla3Repositorio Desepla3Repositorio,
            IMapper mapper,
            IUserSocioContextService userSocioContextService,
            hereford_prContext dbContext)
        {
            _mapper = mapper;
            _Desepla3Repositorio = Desepla3Repositorio;
            _userSocioContextService = userSocioContextService;
            _dbContext = dbContext;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<Desepla3DTO>> _ResponseDTO = new Respuesta<List<Desepla3DTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Desepla3DTO>>());
                }

                IQueryable<Desepla3> query = BuildScopedQuery(accessContext).OrderByDescending(t => t.Id);
                if (skip > 0)
                {
                    query = query.Skip(skip);
                }

                if (take > 0)
                {
                    query = query.Take(take);
                }

                var listaPedido = _mapper.Map<List<Desepla3DTO>>(await query.ToListAsync());

                _ResponseDTO = new Respuesta<List<Desepla3DTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Desepla3DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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

                var a = await BuildScopedQuery(accessContext).CountAsync();

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

            Respuesta<List<Desepla3DTO>> _ResponseDTO = new Respuesta<List<Desepla3DTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Desepla3DTO>>());
                }

                var query = BuildScopedQuery(accessContext);
                if (!string.IsNullOrWhiteSpace(expression))
                {
                    query = query.Where(expression);
                }

                query = query.OrderByDescending(t => t.Id);
                if (skip > 0)
                {
                    query = query.Skip(skip);
                }

                if (take > 0)
                {
                    query = query.Take(take);
                }

                var listaFiltrada = _mapper.Map<List<Desepla3DTO>>(await query.ToListAsync());

                _ResponseDTO = new Respuesta<List<Desepla3DTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Desepla3DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }

        // New: Get lines by exact Nrodec parameter (avoids dynamic LINQ in querystring)
        [HttpGet]
        [Route("GetByNrodec")]
        public async Task<IActionResult> GetByNrodec(string nrodec)
        {
            var resp = new Respuesta<List<Desepla3DTO>>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Desepla3DTO>>());
                }

                if (string.IsNullOrWhiteSpace(nrodec))
                {
                    resp.Exito = 1;
                    resp.Mensaje = "Nrodec vacío";
                    resp.List = new List<Desepla3DTO>();
                    return Ok(resp);
                }

                if (nrodec.Length > MaxNrodecLength)
                {
                    return BadRequest(new Respuesta<List<Desepla3DTO>> { Exito = 0, Mensaje = "Nrodec demasiado largo" });
                }

                if (RequiresActiveSocioScope(accessContext) &&
                    !await CanAccessDeclarationAsync(nrodec.Trim(), accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Desepla3DTO>>());
                }

                var lista = await _Desepla3Repositorio.GetByNrodec(nrodec.Trim());
                Console.WriteLine($"[Desepla3Controller] GetByNrodec nrodec={nrodec.Trim()} total={lista?.Count ?? 0}");
                if (lista != null)
                {
                    foreach (var item in lista.Take(12))
                    {
                        Console.WriteLine($"[Desepla3Controller] GetByNrodec item id={item.Id} tipo={item.Tipo} hba={item.Hardb} tat={item.Tatpart} cantv={item.Cantv} desde={item.Desde} hasta={item.Hasta}");
                    }
                }
                resp.Exito = 1;
                resp.Mensaje = "Éxito";
                resp.List = _mapper.Map<List<Desepla3DTO>>(lista);
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

                Desepla3 _Desepla3Eliminar = await _Desepla3Repositorio.Obtener(u => u.Id == id);
                if (_Desepla3Eliminar != null)
                {
                    if (RequiresActiveSocioScope(accessContext) &&
                        !await CanAccessDeclarationAsync(_Desepla3Eliminar.Nrodec, accessContext))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                    }

                    bool respuesta = await _Desepla3Repositorio.Eliminar(_Desepla3Eliminar);

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
        public async Task<IActionResult> Guardar([FromBody] Desepla3DTO request)
        {
            Respuesta<Desepla3DTO> _Respuesta = new Respuesta<Desepla3DTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Desepla3DTO>());
                }

                Desepla3 _Desepla3 = _mapper.Map<Desepla3>(request);
                if (!await DeclarationExistsAsync(_Desepla3.Nrodec))
                {
                    return BadRequest(new Respuesta<Desepla3DTO>
                    {
                        Exito = 0,
                        Mensaje = "La declaracion de servicio indicada no existe."
                    });
                }

                if (RequiresActiveSocioScope(accessContext) &&
                    !await CanAccessDeclarationAsync(_Desepla3.Nrodec, accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Desepla3DTO>());
                }

                Console.WriteLine($"[Desepla3Controller] Guardar detalle nrodec={_Desepla3.Nrodec} tipo={_Desepla3.Tipo} hba={_Desepla3.Hardb} tat={_Desepla3.Tatpart} cantv={_Desepla3.Cantv} desde={_Desepla3.Desde} hasta={_Desepla3.Hasta}");

                Desepla3 _Desepla3Creado = await _Desepla3Repositorio.Crear(_Desepla3);
                Console.WriteLine($"[Desepla3Controller] Guardar detalle creado id={_Desepla3Creado.Id} nrodec={_Desepla3Creado.Nrodec} tipo={_Desepla3Creado.Tipo} hba={_Desepla3Creado.Hardb}");

                if (_Desepla3Creado.Id != 0)
                    _Respuesta = new Respuesta<Desepla3DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Desepla3DTO>(_Desepla3Creado) };
                else
                    _Respuesta = new Respuesta<Desepla3DTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Desepla3DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Desepla3DTO request)
        {
            Respuesta<Desepla3DTO> _Respuesta = new Respuesta<Desepla3DTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Desepla3DTO>());
                }

                Desepla3 _Desepla3 = _mapper.Map<Desepla3>(request);
                Desepla3 _Desepla3ParaEditar = await _Desepla3Repositorio.Obtener(u => u.Id == _Desepla3.Id);

                if (_Desepla3ParaEditar != null)
                {
                    if (RequiresActiveSocioScope(accessContext) &&
                        !await CanAccessDeclarationAsync(_Desepla3ParaEditar.Nrodec, accessContext))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Desepla3DTO>());
                    }

                    var targetNrodec = string.IsNullOrWhiteSpace(_Desepla3.Nrodec)
                        ? _Desepla3ParaEditar.Nrodec
                        : _Desepla3.Nrodec;

                    if (!await DeclarationExistsAsync(targetNrodec))
                    {
                        return BadRequest(new Respuesta<Desepla3DTO>
                        {
                            Exito = 0,
                            Mensaje = "La declaracion de servicio indicada no existe."
                        });
                    }

                    if (RequiresActiveSocioScope(accessContext) &&
                        !await CanAccessDeclarationAsync(targetNrodec, accessContext))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Desepla3DTO>());
                    }

                    _Desepla3ParaEditar.Nrodec = targetNrodec;
                    _Desepla3ParaEditar.Cantv = _Desepla3.Cantv;
                    _Desepla3ParaEditar.Desde = _Desepla3.Desde;
                    _Desepla3ParaEditar.Hasta = _Desepla3.Hasta;
                    _Desepla3ParaEditar.Tatpart = _Desepla3.Tatpart;
                    _Desepla3ParaEditar.Hardb = _Desepla3.Hardb;

                    bool respuesta = await _Desepla3Repositorio.Editar(_Desepla3ParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<Desepla3DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Desepla3DTO>(_Desepla3ParaEditar) };
                    else
                        _Respuesta = new Respuesta<Desepla3DTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<Desepla3DTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Desepla3DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        private IQueryable<Desepla3> BuildScopedQuery(UserSocioAccessContext accessContext)
        {
            var query = _dbContext.Desepla3s.AsNoTracking().AsQueryable();

            if (RequiresActiveSocioScope(accessContext) && !string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
            {
                var activeSocioCode = accessContext.ActiveSocioCode;
                query = query.Where(
                    detail => _dbContext.Desepla1s
                        .AsNoTracking()
                        .Any(header => header.Nrodec == detail.Nrodec && header.Nrocri == activeSocioCode));
            }

            return query;
        }

        private async Task<bool> DeclarationExistsAsync(string? nrodec, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(nrodec))
            {
                return false;
            }

            return await _dbContext.Desepla1s
                .AsNoTracking()
                .AnyAsync(x => x.Nrodec == nrodec, cancellationToken);
        }

        private async Task<bool> CanAccessDeclarationAsync(
            string? nrodec,
            UserSocioAccessContext accessContext,
            CancellationToken cancellationToken = default)
        {
            if (!RequiresActiveSocioScope(accessContext))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(nrodec) || string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
            {
                return false;
            }

            return await _dbContext.Desepla1s
                .AsNoTracking()
                .AnyAsync(
                    x => x.Nrodec == nrodec && x.Nrocri == accessContext.ActiveSocioCode,
                    cancellationToken);
        }

        private static bool RequiresActiveSocioScope(UserSocioAccessContext accessContext)
            => accessContext.IsSocioUser && !accessContext.IsPrivilegedUser;

        private static Respuesta<T> BuildForbiddenResponse<T>()
            => new Respuesta<T>
            {
                Exito = 0,
                Mensaje = "No tenes permisos para operar sobre otra razon social."
            };
    }
}
