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
    public class Resin4Controller : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IResin4Repositorio _resin4Repositorio;
        private readonly IUserSocioContextService _userSocioContextService;
        private readonly hereford_prContext _db;

        public Resin4Controller(
            IResin4Repositorio resin4Repositorio,
            IMapper mapper,
            IUserSocioContextService userSocioContextService,
            hereford_prContext db)
        {
            _mapper = mapper;
            _resin4Repositorio = resin4Repositorio;
            _userSocioContextService = userSocioContextService;
            _db = db;
        }

        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {
            var response = new Respuesta<List<Resin4DTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Resin4DTO>>());
                }

                var query = ApplyActiveSocioScope(_db.Resin4s.AsQueryable(), accessContext)
                    .OrderByDescending(x => x.Id)
                    .Skip(skip);

                if (take > 0)
                {
                    query = query.Take(take);
                }

                var items = await query.ToListAsync();
                response = new Respuesta<List<Resin4DTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<Resin4DTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<Resin4DTO>> { Exito = 0, Mensaje = ex.Message, List = null };
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

                var count = await ApplyActiveSocioScope(_db.Resin4s.AsQueryable(), accessContext).CountAsync();
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
            var response = new Respuesta<List<Resin4DTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Resin4DTO>>());
                }

                var query = ApplyActiveSocioScope(_db.Resin4s.AsQueryable(), accessContext);
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
                response = new Respuesta<List<Resin4DTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<Resin4DTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<Resin4DTO>> { Exito = 0, Mensaje = ex.Message, List = null };
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

                var entity = await _resin4Repositorio.Obtener(u => u.Id == id);
                if (entity != null)
                {
                    if (!await CanAccessReportAsync(entity.Nrores, accessContext))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                    }

                    var ok = await _resin4Repositorio.Eliminar(entity);
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
        public async Task<IActionResult> Guardar([FromBody] Resin4DTO request)
        {
            if (request == null)
            {
                return BadRequest(new Respuesta<Resin4DTO> { Exito = 0, Mensaje = "La solicitud está vacía." });
            }

            if (string.IsNullOrWhiteSpace(request.Nrores))
            {
                return BadRequest(new Respuesta<Resin4DTO> { Exito = 0, Mensaje = "El resultado de inspección es obligatorio." });
            }

            var response = new Respuesta<Resin4DTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Resin4DTO>());
                }

                if (!await CanAccessReportAsync(request.Nrores, accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Resin4DTO>());
                }

                var entity = _mapper.Map<Resin4>(request);
                var created = await _resin4Repositorio.Crear(entity);

                response = created.Id != 0
                    ? new Respuesta<Resin4DTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin4DTO>(created) }
                    : new Respuesta<Resin4DTO> { Exito = 0, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<Resin4DTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Resin4DTO request)
        {
            var response = new Respuesta<Resin4DTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Resin4DTO>());
                }

                var entity = _mapper.Map<Resin4>(request);
                var entityToEdit = await _resin4Repositorio.Obtener(u => u.Id == entity.Id);

                if (entityToEdit != null)
                {
                    if (!await CanAccessReportAsync(entityToEdit.Nrores, accessContext) ||
                        !await CanAccessReportAsync(entity.Nrores, accessContext))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Resin4DTO>());
                    }

                    entityToEdit.Pedad = entity.Pedad;
                    entityToEdit.Ppeso = entity.Ppeso;
                    entityToEdit.Medad = entity.Medad;
                    entityToEdit.Mpeso = entity.Mpeso;
                    entityToEdit.Iedad = entity.Iedad;
                    entityToEdit.Ipeso = entity.Ipeso;
                    entityToEdit.Pdl = entity.Pdl;
                    entityToEdit.P2d = entity.P2d;
                    entityToEdit.P4d = entity.P4d;
                    entityToEdit.Sexo = entity.Sexo;
                    entityToEdit.Nrores = entity.Nrores;

                    var ok = await _resin4Repositorio.Editar(entityToEdit);
                    response = ok
                        ? new Respuesta<Resin4DTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin4DTO>(entityToEdit) }
                        : new Respuesta<Resin4DTO> { Exito = 0, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    response = new Respuesta<Resin4DTO> { Exito = 0, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<Resin4DTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        private IQueryable<Resin4> ApplyActiveSocioScope(IQueryable<Resin4> query, UserSocioAccessContext accessContext)
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
