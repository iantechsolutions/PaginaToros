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
using System.Net.Mail;
using System.Net.Mime;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Solici1AuxController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISolici1AuxRepositorio _solicitudRepositorio;
        private readonly ISocioRepositorio _socioRepositorio;
        private readonly IUserSocioContextService _userSocioContextService;
        private readonly hereford_prContext _db;

        public Solici1AuxController(
            ISolici1AuxRepositorio solicitudRepositorio,
            ISocioRepositorio socioRepositorio,
            IMapper mapper,
            hereford_prContext db,
            IUserSocioContextService userSocioContextService)
        {
            _mapper = mapper;
            _solicitudRepositorio = solicitudRepositorio;
            _socioRepositorio = socioRepositorio;
            _db = db;
            _userSocioContextService = userSocioContextService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {
            var response = new Respuesta<List<Solici1AuxDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Solici1AuxDTO>>());
                }

                var query = ApplyActiveSocioScope(QuerySolicitudesAux(includeRelations: true), accessContext)
                    .OrderByDescending(x => x.Id)
                    .Skip(skip);

                if (take > 0)
                {
                    query = query.Take(take);
                }

                var items = await query.ToListAsync();

                response = new Respuesta<List<Solici1AuxDTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<Solici1AuxDTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<Solici1AuxDTO>>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = null
                };
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

                var count = await ApplyActiveSocioScope(QuerySolicitudesAux(includeRelations: false), accessContext).CountAsync();
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
            var response = new Respuesta<List<Solici1AuxDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Solici1AuxDTO>>());
                }

                var query = ApplyActiveSocioScope(QuerySolicitudesAux(includeRelations: true), accessContext);
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
                response = new Respuesta<List<Solici1AuxDTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<Solici1AuxDTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<Solici1AuxDTO>>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = null
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route("LimitadosFiltradosNoInclude")]
        public async Task<IActionResult> LimitadosFiltradosNoInclude(int skip, int take, string? expression = null)
        {
            var response = new Respuesta<List<Solici1AuxDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Solici1AuxDTO>>());
                }

                var query = ApplyActiveSocioScope(QuerySolicitudesAux(includeRelations: false), accessContext);
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
                response = new Respuesta<List<Solici1AuxDTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<Solici1AuxDTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<Solici1AuxDTO>>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = null
                };
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

                var entity = await _solicitudRepositorio.Obtener(u => u.Id == id);
                if (entity != null)
                {
                    if (!await CanAccessSolicitudAuxAsync(entity, accessContext))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                    }

                    var ok = await _solicitudRepositorio.Eliminar(entity);

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
        public async Task<IActionResult> Guardar([FromBody] Solici1AuxDTO request)
        {
            var response = new Respuesta<Solici1AuxDTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Solici1AuxDTO>());
                }

                if (!await IsAllowedSolicitudParentAsync(request.IdSoli, accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Solici1AuxDTO>());
                }

                var entity = _mapper.Map<Solici1Aux>(request);
                var created = await _solicitudRepositorio.Crear(entity);

                response = created.Id != 0
                    ? new Respuesta<Solici1AuxDTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<Solici1AuxDTO>(created) }
                    : new Respuesta<Solici1AuxDTO> { Exito = 0, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<Solici1AuxDTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Solici1AuxDTO request)
        {
            var response = new Respuesta<Solici1AuxDTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Solici1AuxDTO>());
                }

                var entity = _mapper.Map<Solici1Aux>(request);
                var entityToEdit = await _solicitudRepositorio.Obtener(u => u.Id == entity.Id);

                if (entityToEdit != null)
                {
                    if (!await CanAccessSolicitudAuxAsync(entityToEdit, accessContext) ||
                        !await IsAllowedSolicitudParentAsync(entity.IdSoli, accessContext))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Solici1AuxDTO>());
                    }

                    entityToEdit.Cantor = entity.Cantor;
                    entityToEdit.Cantvq = entity.Cantvq;
                    entityToEdit.Canvac = entity.Canvac;
                    entityToEdit.Canvaq = entity.Canvaq;
                    entityToEdit.Anio = entity.Anio;
                    entityToEdit.IdSoli = entity.IdSoli;

                    var ok = await _solicitudRepositorio.Editar(entityToEdit);

                    response = ok
                        ? new Respuesta<Solici1AuxDTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<Solici1AuxDTO>(entityToEdit) }
                        : new Respuesta<Solici1AuxDTO> { Exito = 0, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    response = new Respuesta<Solici1AuxDTO> { Exito = 0, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<Solici1AuxDTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = new Respuesta<Solici1Aux>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Solici1Aux>());
                }

                var item = await ApplyActiveSocioScope(QuerySolicitudesAux(includeRelations: true), accessContext)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (item == null)
                {
                    response = new Respuesta<Solici1Aux> { Exito = 0, Mensaje = "No se encontró el detalle de solicitud." };
                }
                else
                {
                    response = new Respuesta<Solici1Aux> { Exito = 1, List = item };
                }
            }
            catch (Exception ex)
            {
                response = new Respuesta<Solici1Aux> { Exito = 0, Mensaje = ex.Message };
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = new Respuesta<List<Solici1Aux>>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Solici1Aux>>());
                }

                var items = await ApplyActiveSocioScope(QuerySolicitudesAux(includeRelations: false), accessContext).ToListAsync();
                response.Exito = 1;
                response.List = items;
            }
            catch (Exception ex)
            {
                response.Exito = 0;
                response.Mensaje = ex.Message;
            }

            return Ok(response);
        }

        [HttpPost]
        public IActionResult Add(Solici1Aux solicitud)
        {
            return Ok(new Respuesta<List<Solici1Aux>>
            {
                Exito = 0,
                Mensaje = "Endpoint no utilizado."
            });
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Solici1Aux solicitud)
        {
            var response = new Respuesta<List<Solici1Aux>>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Solici1Aux>>());
                }

                var entityToEdit = await _db.Solici1Auxs.FindAsync(solicitud.Id);
                if (entityToEdit == null ||
                    !await CanAccessSolicitudAuxAsync(entityToEdit, accessContext) ||
                    !await IsAllowedSolicitudParentAsync(solicitud.IdSoli, accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Solici1Aux>>());
                }

                entityToEdit.Cantor = solicitud.Cantor;
                entityToEdit.Cantvq = solicitud.Cantvq;
                entityToEdit.Canvac = solicitud.Canvac;
                entityToEdit.Canvaq = solicitud.Canvaq;
                entityToEdit.Anio = solicitud.Anio;
                entityToEdit.IdSoli = solicitud.IdSoli;
                _db.Entry(entityToEdit).State = EntityState.Modified;
                _db.SaveChanges();
                response.Exito = 1;
            }
            catch (Exception ex)
            {
                response.Exito = 0;
                response.Mensaje = ex.Message;
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new Respuesta<List<Solici1Aux>>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Solici1Aux>>());
                }

                var entityToDelete = await _db.Solici1Auxs.FindAsync(id);
                if (entityToDelete == null || !await CanAccessSolicitudAuxAsync(entityToDelete, accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Solici1Aux>>());
                }

                _db.Remove(entityToDelete);
                _db.SaveChanges();
                response.Exito = 1;
            }
            catch (Exception ex)
            {
                response.Exito = 0;
                response.Mensaje = ex.Message;
            }

            return Ok(response);
        }

        [HttpPost("SendExcel/{socioId}")]
        public async Task<IActionResult> SendExcel(int socioId, [FromForm] IFormFile file)
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext))
                {
                    if (!accessContext.ActiveSocioId.HasValue || accessContext.ActiveSocioId.Value != socioId)
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                    }
                }

                if (socioId <= 0)
                {
                    return BadRequest("Falta socioId.");
                }

                if (file == null || file.Length == 0)
                {
                    return BadRequest("No se recibió el archivo o está vacío.");
                }

                string filtro = $"Id = {socioId}";
                var socios = await _socioRepositorio.LimitadosFiltrados(0, 1, filtro);
                var socio = socios.FirstOrDefault();
                if (socio == null)
                {
                    return BadRequest($"No se encontró el socio para el filtro: {filtro}");
                }

                var safeExt = Path.GetExtension(file.FileName);
                if (string.IsNullOrWhiteSpace(safeExt))
                {
                    safeExt = ".xls";
                }

                var tempFilePath = Path.Combine(Path.GetTempPath(), $"Excel_Solicitud_{DateTime.Now:yyyyMMdd_HHmmss}{safeExt}");
                using (var fs = new FileStream(tempFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }

                using (var mail = new MailMessage())
                {
                    var from = "planteles@hereford.org.ar";
                    mail.From = new MailAddress(from);
                    mail.To.Add("puroregistradohereford@gmail.com");
                    mail.To.Add("planteles@hereford.org.ar");
                    mail.Subject = $"Solicitud de Inspección de: {socio.Nombre}";
                    mail.Body = $"Nueva solicitud de inspección\nSocio: {socio.Nombre}";
                    mail.IsBodyHtml = false;
                    mail.Attachments.Add(new Attachment(tempFilePath, MediaTypeNames.Application.Octet));

                    using (var smtp = new SmtpClient("mail.hereford.org.ar", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential(from, "Hereford.2033");
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(mail);
                    }
                }

                System.IO.File.Delete(tempFilePath);
                return Ok("Correo enviado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al enviar el correo: {ex.Message}");
            }
        }

        private IQueryable<Solici1Aux> QuerySolicitudesAux(bool includeRelations)
        {
            IQueryable<Solici1Aux> query = _db.Solici1Auxs
                .Where(x => _db.Solici1s.Any(s => s.Id == x.IdSoli && !string.IsNullOrWhiteSpace(s.Codest)))
                .Where(x => _db.Solici1s.Any(s => s.Id == x.IdSoli && _db.Estables.Any(e => e.Ecod == s.Codest)));

            if (includeRelations)
            {
                query = query
                    .Include(x => x.Solicitud)
                    .ThenInclude(s => s.Establecimiento)
                    .ThenInclude(e => e.Socio);
            }

            return query;
        }

        private IQueryable<Solici1Aux> ApplyActiveSocioScope(IQueryable<Solici1Aux> query, UserSocioAccessContext accessContext)
        {
            if (!RequiresActiveSocioScope(accessContext))
            {
                return query;
            }

            return query.Where(x =>
                _db.Solici1s.Any(s => s.Id == x.IdSoli && _db.Estables.Any(e => e.Ecod == s.Codest && e.Codsoc == accessContext.ActiveSocioCode)));
        }

        private async Task<bool> CanAccessSolicitudAuxAsync(Solici1Aux solicitudAux, UserSocioAccessContext accessContext)
        {
            if (!RequiresActiveSocioScope(accessContext))
            {
                return true;
            }

            return await IsAllowedSolicitudParentAsync(solicitudAux.IdSoli, accessContext);
        }

        private async Task<bool> IsAllowedSolicitudParentAsync(int solicitudId, UserSocioAccessContext accessContext)
        {
            if (solicitudId <= 0)
            {
                return false;
            }

            if (!RequiresActiveSocioScope(accessContext))
            {
                return await _db.Solici1s.AsNoTracking().AnyAsync(x => x.Id == solicitudId);
            }

            return await _db.Solici1s.AsNoTracking().AnyAsync(
                s => s.Id == solicitudId && _db.Estables.Any(e => e.Ecod == s.Codest && e.Codsoc == accessContext.ActiveSocioCode));
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
