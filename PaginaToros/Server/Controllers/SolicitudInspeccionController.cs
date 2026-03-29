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
    public class Solici1Controller : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISolici1Repositorio _solicitudRepositorio;
        private readonly ISocioRepositorio _socioRepositorio;
        private readonly IUserSocioContextService _userSocioContextService;
        private readonly hereford_prContext _db;

        public Solici1Controller(
            ISolici1Repositorio solicitudRepositorio,
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
            var response = new Respuesta<List<Solici1DTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Solici1DTO>>());
                }

                var query = QuerySolicitudes(includeRelations: true);
                query = ApplyActiveSocioScope(query, accessContext);
                query = query.OrderByDescending(x => x.Id).Skip(skip);

                if (take > 0)
                {
                    query = query.Take(take);
                }

                var items = await query.ToListAsync();

                response = new Respuesta<List<Solici1DTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<Solici1DTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<Solici1DTO>>
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

                var count = await ApplyActiveSocioScope(QuerySolicitudes(includeRelations: false), accessContext).CountAsync();

                response = new Respuesta<int>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = count
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<int>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = 0
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route("LimitadosFiltrados")]
        public async Task<IActionResult> LimitadosFiltrados(int skip, int take, string? expression = null)
        {
            var response = new Respuesta<List<Solici1DTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Solici1DTO>>());
                }

                var query = ApplyActiveSocioScope(QuerySolicitudes(includeRelations: true), accessContext);
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
                response = new Respuesta<List<Solici1DTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<Solici1DTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<Solici1DTO>>
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
            var response = new Respuesta<List<Solici1DTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Solici1DTO>>());
                }

                var query = ApplyActiveSocioScope(QuerySolicitudes(includeRelations: false), accessContext);
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
                response = new Respuesta<List<Solici1DTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<Solici1DTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<Solici1DTO>>
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
                    if (!await CanAccessSolicitudAsync(entity, accessContext))
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
        public async Task<IActionResult> Guardar([FromBody] Solici1DTO request)
        {
            var response = new Respuesta<Solici1DTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Solici1DTO>());
                }

                if (!await IsAllowedEstablecimientoAsync(request.Codest, accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Solici1DTO>());
                }

                var entity = _mapper.Map<Solici1>(request);
                entity.Nrosol = string.IsNullOrWhiteSpace(request.Nrosol)
                    ? await GetNextNrosolAsync()
                    : request.Nrosol;

                var created = await _solicitudRepositorio.Crear(entity);

                response = created.Id != 0
                    ? new Respuesta<Solici1DTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<Solici1DTO>(created) }
                    : new Respuesta<Solici1DTO> { Exito = 0, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<Solici1DTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Solici1DTO request)
        {
            var response = new Respuesta<Solici1DTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Solici1DTO>());
                }

                var entity = _mapper.Map<Solici1>(request);
                var entityToEdit = await _solicitudRepositorio.Obtener(u => u.Id == entity.Id);

                if (entityToEdit != null)
                {
                    if (!await CanAccessSolicitudAsync(entityToEdit, accessContext) ||
                        !await IsAllowedEstablecimientoAsync(entity.Codest, accessContext))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Solici1DTO>());
                    }

                    entityToEdit.Nrosol = entity.Nrosol;
                    entityToEdit.Codest = entity.Codest;
                    entityToEdit.Fecsol = entity.Fecsol;
                    entityToEdit.Lugar = entity.Lugar;
                    entityToEdit.Cantor = entity.Cantor;
                    entityToEdit.Cantvq = entity.Cantvq;
                    entityToEdit.Produc = entity.Produc;
                    entityToEdit.Reinsp = entity.Reinsp;
                    entityToEdit.Canvac = entity.Canvac;
                    entityToEdit.Canvaq = entity.Canvaq;
                    entityToEdit.EdadMinMac = entity.EdadMinMac;
                    entityToEdit.EdadMaxHem = entity.EdadMaxHem;
                    entityToEdit.EdadMinHem = entity.EdadMinHem;
                    entityToEdit.EdadMaxMac = entity.EdadMaxMac;
                    entityToEdit.Tyncte = entity.Tyncte;
                    entityToEdit.Banco = entity.Banco;
                    entityToEdit.Import = entity.Import;
                    entityToEdit.Fecins = entity.Fecins;
                    entityToEdit.Ctrl1 = entity.Ctrl1;
                    entityToEdit.Ctrl2 = entity.Ctrl2;
                    entityToEdit.Fecret = entity.Fecret;
                    entityToEdit.FchUsu = entity.FchUsu;
                    entityToEdit.CodUsu = entity.CodUsu;
                    entityToEdit.Anio = entity.Anio;

                    var ok = await _solicitudRepositorio.Editar(entityToEdit);

                    response = ok
                        ? new Respuesta<Solici1DTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<Solici1DTO>(entityToEdit) }
                        : new Respuesta<Solici1DTO> { Exito = 0, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    response = new Respuesta<Solici1DTO> { Exito = 0, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<Solici1DTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = new Respuesta<Solici1>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Solici1>());
                }

                var item = await ApplyActiveSocioScope(QuerySolicitudes(includeRelations: true), accessContext)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (item == null)
                {
                    response = new Respuesta<Solici1> { Exito = 0, Mensaje = "No se encontró la solicitud." };
                }
                else
                {
                    response = new Respuesta<Solici1> { Exito = 1, List = item };
                }
            }
            catch (Exception ex)
            {
                response = new Respuesta<Solici1> { Exito = 0, Mensaje = ex.Message };
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = new Respuesta<List<Solici1>>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Solici1>>());
                }

                var items = await ApplyActiveSocioScope(QuerySolicitudes(includeRelations: false), accessContext)
                    .OrderByDescending(s => s.Nrosol)
                    .ToListAsync();

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

        [HttpGet("Nrores/{nro}")]
        public async Task<IActionResult> GetByRes(string nro)
        {
            var response = new Respuesta<Solici1>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<Solici1>());
                }

                var item = await ApplyActiveSocioScope(QuerySolicitudes(includeRelations: false), accessContext)
                    .FirstOrDefaultAsync(x => x.Nrosol == nro);

                if (item == null)
                {
                    response = new Respuesta<Solici1> { Exito = 0, Mensaje = "No se encontró la solicitud." };
                }
                else
                {
                    response = new Respuesta<Solici1> { Exito = 1, List = item };
                }
            }
            catch (Exception ex)
            {
                response.Exito = 0;
                response.Mensaje = ex.Message;
            }

            return Ok(response);
        }

        [HttpGet("pendientes")]
        public async Task<IActionResult> GetPendientes()
        {
            var response = new Respuesta<List<Solici1>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Solici1>>());
                }

                var items = await ApplyActiveSocioScope(QuerySolicitudes(includeRelations: false), accessContext)
                    .Where(t1 => !_db.Resin1s.Any(t2 => t2.Nrores == t1.Nrosol))
                    .ToListAsync();

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
        public IActionResult Add(Solici1 solicitud)
        {
            return Ok(new Respuesta<List<Solici1>>
            {
                Exito = 0,
                Mensaje = "Endpoint no utilizado."
            });
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Solici1 solicitud)
        {
            var response = new Respuesta<List<Solici1>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Solici1>>());
                }

                var entityToEdit = await _db.Solici1s.FindAsync(solicitud.Id);
                if (entityToEdit == null ||
                    !await CanAccessSolicitudAsync(entityToEdit, accessContext) ||
                    !await IsAllowedEstablecimientoAsync(solicitud.Codest, accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Solici1>>());
                }

                entityToEdit.Codest = solicitud.Codest;
                entityToEdit.Nrosol = solicitud.Nrosol;
                entityToEdit.Fecsol = solicitud.Fecsol;
                entityToEdit.Lugar = solicitud.Lugar;
                entityToEdit.Cantor = solicitud.Cantor;
                entityToEdit.Cantvq = solicitud.Cantvq;
                entityToEdit.Produc = solicitud.Produc;
                entityToEdit.Reinsp = solicitud.Reinsp;
                entityToEdit.Canvac = solicitud.Canvac;
                entityToEdit.Canvaq = solicitud.Canvaq;
                entityToEdit.EdadMinMac = solicitud.EdadMinMac;
                entityToEdit.EdadMaxHem = solicitud.EdadMaxHem;
                entityToEdit.EdadMinHem = solicitud.EdadMinHem;
                entityToEdit.EdadMaxMac = solicitud.EdadMaxMac;
                entityToEdit.Tyncte = solicitud.Tyncte;
                entityToEdit.Banco = solicitud.Banco;
                entityToEdit.Import = solicitud.Import;
                entityToEdit.Fecins = solicitud.Fecins;
                entityToEdit.Ctrl1 = solicitud.Ctrl1;
                entityToEdit.Ctrl2 = solicitud.Ctrl2;
                entityToEdit.Fecret = solicitud.Fecret;
                entityToEdit.FchUsu = solicitud.FchUsu;
                entityToEdit.CodUsu = solicitud.CodUsu;
                entityToEdit.Anio = solicitud.Anio;

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
            var response = new Respuesta<List<Solici1>>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Solici1>>());
                }

                var entityToDelete = await _db.Solici1s.FindAsync(id);
                if (entityToDelete == null || !await CanAccessSolicitudAsync(entityToDelete, accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<Solici1>>());
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

                if (file == null || file.Length == 0)
                {
                    return BadRequest("No se recibió el archivo o está vacío.");
                }

                var tempFilePath = Path.Combine(Path.GetTempPath(), $"Excel_Solicitud_{DateTime.Now:dd_MM_yyyy}.xls");
                using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                string filtro = $"Id = {socioId}";
                var socios = await _socioRepositorio.LimitadosFiltrados(0, 1, filtro);
                Socio? socio = socios.FirstOrDefault();
                if (socio == null)
                {
                    return BadRequest("No se encontró el socio indicado.");
                }

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("planteles@hereford.org.ar");
                    mail.To.Add("puroregistradohereford@gmail.com");
                    mail.To.Add("planteles@hereford.org.ar");
                    mail.Subject = $"Solicitud de Inspeccion de: {socio.Nombre}";
                    mail.Body = $"Nueva solicitud de inspeccion\nSocio: {socio.Nombre}";
                    mail.Attachments.Add(new Attachment(tempFilePath, MediaTypeNames.Application.Octet));

                    using (SmtpClient smtp = new SmtpClient("mail.hereford.org.ar", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("planteles@hereford.org.ar", "Hereford.2033");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al enviar el correo: {ex.Message}");
            }
        }

        private IQueryable<Solici1> QuerySolicitudes(bool includeRelations)
        {
            IQueryable<Solici1> query = _db.Solici1s
                .Where(s => !string.IsNullOrWhiteSpace(s.Codest))
                .Where(s => _db.Estables.Any(e => e.Ecod == s.Codest));

            if (includeRelations)
            {
                query = query
                    .Include(t => t.Establecimiento).ThenInclude(e => e.Socio)
                    .Include(t => t.Establecimiento).ThenInclude(e => e.Provincia);
            }

            return query;
        }

        private IQueryable<Solici1> ApplyActiveSocioScope(IQueryable<Solici1> query, UserSocioAccessContext accessContext)
        {
            if (!RequiresActiveSocioScope(accessContext))
            {
                return query;
            }

            return query.Where(x => _db.Estables.Any(e => e.Ecod == x.Codest && e.Codsoc == accessContext.ActiveSocioCode));
        }

        private async Task<bool> CanAccessSolicitudAsync(Solici1 solicitud, UserSocioAccessContext accessContext)
        {
            if (!RequiresActiveSocioScope(accessContext))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(solicitud.Codest))
            {
                return false;
            }

            return await _db.Estables.AsNoTracking().AnyAsync(
                e => e.Ecod == solicitud.Codest && e.Codsoc == accessContext.ActiveSocioCode);
        }

        private async Task<bool> IsAllowedEstablecimientoAsync(string? codest, UserSocioAccessContext accessContext)
        {
            if (string.IsNullOrWhiteSpace(codest) || !RequiresActiveSocioScope(accessContext))
            {
                return true;
            }

            return await _db.Estables.AsNoTracking().AnyAsync(
                e => e.Ecod == codest && e.Codsoc == accessContext.ActiveSocioCode);
        }

        private async Task<string> GetNextNrosolAsync()
        {
            var valores = await _db.Solici1s
                .AsNoTracking()
                .Where(x => !string.IsNullOrWhiteSpace(x.Nrosol))
                .Select(x => x.Nrosol!)
                .ToListAsync();

            var max = valores
                .Select(x => int.TryParse(x, out var nro) ? nro : 0)
                .DefaultIfEmpty(0)
                .Max();

            return (max + 1).ToString("D6");
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
