using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Services;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Linq.Dynamic.Core;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Resin6Controller : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IResin6Repositorio _resin6Repositorio;
        private readonly IUserSocioContextService _userSocioContextService;
        private readonly hereford_prContext _db;

        public Resin6Controller(
            IResin6Repositorio resin6Repositorio,
            IMapper mapper,
            IUserSocioContextService userSocioContextService,
            hereford_prContext db)
        {
            _mapper = mapper;
            _resin6Repositorio = resin6Repositorio;
            _userSocioContextService = userSocioContextService;
            _db = db;
        }

        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {
            var response = new Respuesta<List<Resin6DTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Resin6DTO>>());
                }

                var query = ApplyActiveSocioScope(
                        _db.Resin6s.Include(x => x.Resin1).ThenInclude(x => x.Establecimiento),
                        accessContext)
                    .OrderByDescending(x => x.Id)
                    .Skip(skip);

                if (take > 0)
                {
                    query = query.Take(take);
                }

                var items = await query.ToListAsync();
                response = new Respuesta<List<Resin6DTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<Resin6DTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<Resin6DTO>> { Exito = 0, Mensaje = ex.Message, List = null };
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

                var count = await ApplyActiveSocioScope(_db.Resin6s.AsQueryable(), accessContext).CountAsync();
                response = new Respuesta<int> { Exito = 1, Mensaje = "Exito", List = count };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<int> { Exito = 0, Mensaje = ex.Message, List = 0 };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route("LimitadosFiltrados")]
        public async Task<IActionResult> LimitadosFiltrados(int skip, int take, string? expression = null)
        {
            var response = new Respuesta<List<Resin6DTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Resin6DTO>>());
                }

                var query = ApplyActiveSocioScope(
                    _db.Resin6s.Include(x => x.Resin1).ThenInclude(x => x.Establecimiento),
                    accessContext);

                if (!string.IsNullOrWhiteSpace(expression))
                {
                    query = query.Where(expression);
                }

                query = query.OrderByDescending(x => x.Id).Skip(skip);
                if (take > 0)
                {
                    query = query.Take(take);
                }

                var items = await query.ToListAsync();
                response = new Respuesta<List<Resin6DTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<Resin6DTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<Resin6DTO>> { Exito = 0, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route("ObtenerFechas")]
        public async Task<IActionResult> ObtenerFechas(long fecha1, long fecha2)
        {
            var response = new Respuesta<List<Resin6DTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Resin6DTO>>());
                }

                var fechaini = new DateTime(fecha1);
                var fechafin = new DateTime(fecha2);

                var query = ApplyActiveSocioScope(
                    _db.Resin6s.Include(x => x.Resin1).ThenInclude(x => x.Establecimiento),
                    accessContext)
                    .Where(x => x.Resin1 != null
                        && x.Resin1.Freali.HasValue
                        && x.Resin1.Freali.Value >= fechaini
                        && x.Resin1.Freali.Value <= fechafin);

                var items = await query.ToListAsync();
                response = new Respuesta<List<Resin6DTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<Resin6DTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<Resin6DTO>> { Exito = 0, Mensaje = ex.Message, List = null };
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

                var entity = await _resin6Repositorio.Obtener(u => u.Id == id);
                if (entity != null)
                {
                    if (!await CanAccessReportAsync(entity.Nrores, accessContext))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                    }

                    var ok = await _resin6Repositorio.Eliminar(entity);
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
        public async Task<IActionResult> Guardar([FromBody] Resin6DTO request)
        {
            if (request == null)
            {
                return BadRequest(new Respuesta<Resin6DTO> { Exito = 0, Mensaje = "La solicitud está vacía." });
            }

            if (string.IsNullOrWhiteSpace(request.Nrores))
            {
                return BadRequest(new Respuesta<Resin6DTO> { Exito = 0, Mensaje = "El resultado de inspección es obligatorio." });
            }

            var response = new Respuesta<Resin6DTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Resin6DTO>());
                }

                if (!await CanAccessReportAsync(request.Nrores, accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Resin6DTO>());
                }

                var entity = _mapper.Map<Resin6>(request);
                var created = await _resin6Repositorio.Crear(entity);

                response = created.Id != 0
                    ? new Respuesta<Resin6DTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin6DTO>(created) }
                    : new Respuesta<Resin6DTO> { Exito = 0, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<Resin6DTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Resin6DTO request)
        {
            var response = new Respuesta<Resin6DTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Resin6DTO>());
                }

                var entity = _mapper.Map<Resin6>(request);
                var entityToEdit = await _resin6Repositorio.Obtener(u => u.Id == entity.Id);

                if (entityToEdit != null)
                {
                    if (!await CanAccessReportAsync(entityToEdit.Nrores, accessContext) ||
                        !await CanAccessReportAsync(entity.Nrores, accessContext))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Resin6DTO>());
                    }

                    entityToEdit.Hdp = entity.Hdp;
                    entityToEdit.HdpM = entity.HdpM;
                    entityToEdit.HdpAs = entity.HdpAs;
                    entityToEdit.Hdt = entity.Hdt;
                    entityToEdit.Hdb = entity.Hdb;
                    entityToEdit.Hpp = entity.Hpp;
                    entityToEdit.HppM = entity.HppM;
                    entityToEdit.HppAs = entity.HppAs;
                    entityToEdit.Hpt = entity.Hpt;
                    entityToEdit.Hpb = entity.Hpb;
                    entityToEdit.Hgvp = entity.Hgvp;
                    entityToEdit.Hgvb = entity.Hgvb;
                    entityToEdit.Hgqp = entity.Hgqp;
                    entityToEdit.Hgqb = entity.Hgqb;
                    entityToEdit.Mcp = entity.Mcp;
                    entityToEdit.McpM = entity.McpM;
                    entityToEdit.McpAs = entity.McpAs;
                    entityToEdit.Mct = entity.Mct;
                    entityToEdit.Msp = entity.Msp;
                    entityToEdit.MspM = entity.MspM;
                    entityToEdit.MspAs = entity.MspAs;
                    entityToEdit.Mst = entity.Mst;
                    entityToEdit.Mspsb = entity.Mspsb;
                    entityToEdit.Nrores = entity.Nrores;

                    var ok = await _resin6Repositorio.Editar(entityToEdit);
                    response = ok
                        ? new Respuesta<Resin6DTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin6DTO>(entityToEdit) }
                        : new Respuesta<Resin6DTO> { Exito = 0, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    response = new Respuesta<Resin6DTO> { Exito = 0, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<Resin6DTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        private IQueryable<Resin6> ApplyActiveSocioScope(IQueryable<Resin6> query, UserSocioAccessContext accessContext)
        {
            if (!RequiresActiveSocioScope(accessContext))
            {
                return query;
            }

            return query.Where(x => _db.Resin1s.Any(r => r.Nrores == x.Nrores && r.Scod == accessContext.ActiveSocioCode));
        }

        private async Task<bool> CanAccessReportAsync(string? nrores, UserSocioAccessContext accessContext)
        {
            if (string.IsNullOrWhiteSpace(nrores))
            {
                return false;
            }

            if (!RequiresActiveSocioScope(accessContext))
            {
                return true;
            }

            return await _db.Resin1s.AsNoTracking().AnyAsync(r => r.Nrores == nrores && r.Scod == accessContext.ActiveSocioCode);
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
