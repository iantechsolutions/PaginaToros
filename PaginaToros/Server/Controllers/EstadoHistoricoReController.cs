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
    public class Resin2Controller : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IResin2Repositorio _resin2Repositorio;
        private readonly IUserSocioContextService _userSocioContextService;
        private readonly hereford_prContext _db;

        public Resin2Controller(
            IResin2Repositorio resin2Repositorio,
            IMapper mapper,
            IUserSocioContextService userSocioContextService,
            hereford_prContext db)
        {
            _mapper = mapper;
            _resin2Repositorio = resin2Repositorio;
            _userSocioContextService = userSocioContextService;
            _db = db;
        }

        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {
            var response = new Respuesta<List<Resin2DTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Resin2DTO>>());
                }

                var query = ApplyActiveSocioScope(_db.Resin2s.AsQueryable(), accessContext)
                    .OrderByDescending(x => x.Id)
                    .Skip(skip);

                if (take > 0)
                {
                    query = query.Take(take);
                }

                var items = await query.ToListAsync();
                response = new Respuesta<List<Resin2DTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<Resin2DTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<Resin2DTO>> { Exito = 0, Mensaje = ex.Message, List = null };
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

                var count = await ApplyActiveSocioScope(_db.Resin2s.AsQueryable(), accessContext).CountAsync();
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
            var response = new Respuesta<List<Resin2DTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Resin2DTO>>());
                }

                var query = ApplyActiveSocioScope(_db.Resin2s.AsQueryable(), accessContext);
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
                response = new Respuesta<List<Resin2DTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<Resin2DTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<Resin2DTO>> { Exito = 0, Mensaje = ex.Message, List = null };
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

                var entity = await _resin2Repositorio.Obtener(u => u.Id == id);
                if (entity != null)
                {
                    if (!await CanAccessReportAsync(entity.Nrores, accessContext))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                    }

                    var ok = await _resin2Repositorio.Eliminar(entity);
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
        public async Task<IActionResult> Guardar([FromBody] Resin2DTO request)
        {
            if (request == null)
            {
                return BadRequest(new Respuesta<Resin2DTO> { Exito = 0, Mensaje = "La solicitud está vacía." });
            }

            if (string.IsNullOrWhiteSpace(request.Nrores))
            {
                return BadRequest(new Respuesta<Resin2DTO> { Exito = 0, Mensaje = "El resultado de inspección es obligatorio." });
            }

            var response = new Respuesta<Resin2DTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Resin2DTO>());
                }

                if (!await CanAccessReportAsync(request.Nrores, accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Resin2DTO>());
                }

                var entity = _mapper.Map<Resin2>(request);
                var created = await _resin2Repositorio.Crear(entity);

                response = created.Id != 0
                    ? new Respuesta<Resin2DTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin2DTO>(created) }
                    : new Respuesta<Resin2DTO> { Exito = 0, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<Resin2DTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Resin2DTO request)
        {
            var response = new Respuesta<Resin2DTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Resin2DTO>());
                }

                var entity = _mapper.Map<Resin2>(request);
                var entityToEdit = await _resin2Repositorio.Obtener(u => u.Id == entity.Id);

                if (entityToEdit != null)
                {
                    if (!await CanAccessReportAsync(entityToEdit.Nrores, accessContext) ||
                        !await CanAccessReportAsync(entity.Nrores, accessContext))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Resin2DTO>());
                    }

                    entityToEdit.Ea1 = entity.Ea1;
                    entityToEdit.Ea2 = entity.Ea2;
                    entityToEdit.Ea3 = entity.Ea3;
                    entityToEdit.Ea4 = entity.Ea4;
                    entityToEdit.Ea5 = entity.Ea5;
                    entityToEdit.Ea6 = entity.Ea6;
                    entityToEdit.Nrores = entity.Nrores;

                    var ok = await _resin2Repositorio.Editar(entityToEdit);
                    response = ok
                        ? new Respuesta<Resin2DTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin2DTO>(entityToEdit) }
                        : new Respuesta<Resin2DTO> { Exito = 0, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    response = new Respuesta<Resin2DTO> { Exito = 0, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<Resin2DTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        private IQueryable<Resin2> ApplyActiveSocioScope(IQueryable<Resin2> query, UserSocioAccessContext accessContext)
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
