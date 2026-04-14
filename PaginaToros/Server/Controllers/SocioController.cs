using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Client.Pages.Socios;
using PaginaToros.Server.Context;
using AutoMapper;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Repositorio.Implementacion;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using PaginaToros.Server.Services;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocioController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISocioRepositorio _SocioRepositorio;
        private readonly hereford_prContext _db;
        private readonly ApplicationDbContext _identityDb;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserSocioContextService _userSocioContextService;

        public SocioController(
            ISocioRepositorio SocioRepositorio,
            IMapper mapper,
            hereford_prContext db,
            ApplicationDbContext identityDb,
            UserManager<IdentityUser> userManager,
            IUserSocioContextService userSocioContextService)
        {
            _mapper = mapper;
            _SocioRepositorio = SocioRepositorio;
            _db = db;
            _identityDb = identityDb;
            _userManager = userManager;
            _userSocioContextService = userSocioContextService;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<SocioDTO>> _ResponseDTO = new Respuesta<List<SocioDTO>>();

            try
            {
                List<SocioDTO> listaPedido = new List<SocioDTO>();
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && !accessContext.ActiveSocioId.HasValue)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<SocioDTO>>());
                }

                var a = RequiresActiveSocioScope(accessContext)
                    ? await _SocioRepositorio.LimitadosFiltradosTodos(skip, take, BuildActiveSocioIdFilter(accessContext.ActiveSocioId!.Value))
                    : await _SocioRepositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<SocioDTO>>(a);

                _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                if (RequiresActiveSocioScope(accessContext))
                {
                    if (!accessContext.ActiveSocioId.HasValue)
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<int>());
                    }

                    var scopedCount = await _db.Socios
                        .AsNoTracking()
                        .CountAsync(x => x.Id == accessContext.ActiveSocioId.Value);

                    _ResponseDTO = new Respuesta<int>() { Exito = 1, Mensaje = "Exito", List = scopedCount };
                    return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
                }

                var a = await _SocioRepositorio.CantidadTotal();

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

            Respuesta<List<SocioDTO>> _ResponseDTO = new Respuesta<List<SocioDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext))
                {
                    if (!accessContext.ActiveSocioId.HasValue)
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<SocioDTO>>());
                    }

                    var scopedExpression = AppendFilter(expression, BuildActiveSocioIdFilter(accessContext.ActiveSocioId.Value));
                    var scoped = await _SocioRepositorio.LimitadosFiltradosTodos(skip, take, scopedExpression);
                    var scopedOrdered = scoped.OrderByDescending(s => s.Criador == "S").ToList();
                    var scopedMapped = _mapper.Map<List<SocioDTO>>(scopedOrdered);

                    _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = "Exito", List = scopedMapped };
                    return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
                }

                var a = await _SocioRepositorio.LimitadosFiltrados(skip, take, expression);

                var listaOrdenada = a.OrderByDescending(s => s.Criador == "S").ToList();

                var listaFiltrada = _mapper.Map<List<SocioDTO>>(listaOrdenada);
                //var listaFiltrada = _mapper.Map<List<SocioDTO>>(a);

                _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }


        [Route("LimitadosFiltradoTodos")]
        public async Task<IActionResult> LimitadosFiltradosTodos(int skip, int take, string? expression = null)
        {
            Respuesta<List<SocioDTO>> _ResponseDTO = new Respuesta<List<SocioDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext))
                {
                    if (!accessContext.ActiveSocioId.HasValue)
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<SocioDTO>>());
                    }

                    expression = AppendFilter(expression, BuildActiveSocioIdFilter(accessContext.ActiveSocioId.Value));
                }

                var a = await _SocioRepositorio.LimitadosFiltradosTodos(skip, take, expression);

                //var listaOrdenada = a.OrderByDescending(s => s.Criador == "S").ToList();

                var listaFiltrada = _mapper.Map<List<SocioDTO>>(a);

                _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = "Éxito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                if (RequiresActiveSocioScope(accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                }

                Socio _SocioEliminar = await _SocioRepositorio.Obtener(u => u.Id == id);
                if (_SocioEliminar != null)
                {

                    bool respuesta = await _SocioRepositorio.Eliminar(_SocioEliminar);

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
        public async Task<IActionResult> Guardar([FromBody] SocioDTO request)
        {
            Respuesta<SocioDTO> _Respuesta = new Respuesta<SocioDTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<SocioDTO>());
                }

                request.Codpos2 = request.Codpos2?.Trim();
                request.Placod = request.Placod?.Trim();
                request.Mail = request.Mail?.Trim();
                request.Nombre = request.Nombre?.Trim();
                request.Direcc1 = request.Direcc1?.Trim();
                request.Locali1 = request.Locali1?.Trim();
                request.Telefo1 = request.Telefo1?.Trim();

                if (!string.IsNullOrWhiteSpace(request.Codpos2) && request.Codpos2.Length > 4)
                {
                    _Respuesta = new Respuesta<SocioDTO>()
                    {
                        Exito = 0,
                        Mensaje = "El código de socio no puede tener más de 4 caracteres."
                    };
                    return StatusCode(StatusCodes.Status400BadRequest, _Respuesta);
                }

                if (!string.IsNullOrWhiteSpace(request.Placod) && request.Placod.Length > 4)
                {
                    _Respuesta = new Respuesta<SocioDTO>()
                    {
                        Exito = 0,
                        Mensaje = "El plantel no puede tener más de 4 caracteres."
                    };
                    return StatusCode(StatusCodes.Status400BadRequest, _Respuesta);
                }

                // Codpos2 duplicates are allowed

                // Generate next Scod server-side (numeric sequence using numeric Scod values)
                var allScods = _db.Socios.Select(s => s.Scod).ToList();
                int max = 0;
                foreach (var sc in allScods)
                {
                    if (int.TryParse(sc, out var v)) if (v > max) max = v;
                }
                var nextScod = (max + 1).ToString("D4");

                var nuevoMap = _mapper.Map<Socio>(request);
                nuevoMap.Scod = nextScod;
                // ensure Id is 0 so repository will create
                nuevoMap.Id = 0;

                Socio _SocioCreado = await _SocioRepositorio.Crear(nuevoMap);

                if (_SocioCreado.Id != 0)
                    _Respuesta = new Respuesta<SocioDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<SocioDTO>(_SocioCreado) };
                else
                    _Respuesta = new Respuesta<SocioDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                    errorMessage += " | Inner: " + ex.InnerException.Message;

                _Respuesta = new Respuesta<SocioDTO>()
                {
                    Exito = 1,
                    Mensaje = errorMessage
                };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }

        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] SocioDTO request)
        {
            var resp = new Respuesta<SocioDTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) &&
                    (!accessContext.ActiveSocioId.HasValue || request.Id != accessContext.ActiveSocioId.Value))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<SocioDTO>());
                }

                var socioReq = _mapper.Map<Socio>(request);
                var socioDb = await _SocioRepositorio.Obtener(u => u.Id == socioReq.Id);

                if (socioDb == null)
                    return NotFound(new Respuesta<SocioDTO> { Exito = 0, Mensaje = "No se encontró el identificador" });

                // Do not allow changing Scod from client; keep existing

                // Validate Codpos2 uniqueness if provided and changed
                if (!string.IsNullOrWhiteSpace(socioReq.Codpos2) && !string.Equals(socioDb.Codpos2, socioReq.Codpos2, StringComparison.Ordinal))
                {
                    var conflictCod = _db.Socios.FirstOrDefault(s => s.Codpos2 == socioReq.Codpos2 && s.Id != socioReq.Id);
                    if (conflictCod != null)
                    {
                        return Conflict(new Respuesta<SocioDTO> { Exito = 0, Mensaje = "Codpos2 duplicado", List = _mapper.Map<SocioDTO>(conflictCod) });
                    }
                }

                socioDb.Nombre = socioReq.Nombre;
                socioDb.Direcc1 = socioReq.Direcc1;
                socioDb.Telefo1 = socioReq.Telefo1;
                socioDb.Telefo2 = socioReq.Telefo2;
                socioDb.Locali1 = socioReq.Locali1;
                socioDb.Codpos1 = socioReq.Codpos1;
                socioDb.Codpro1 = socioReq.Codpro1;
                socioDb.Criador = socioReq.Criador;
                socioDb.Mail = socioReq.Mail;
                socioDb.Fecing = socioReq.Fecing;
                socioDb.Placod = socioReq.Placod;
                socioDb.Codpos2 = socioReq.Codpos2;

                var ok = await _SocioRepositorio.Editar(socioDb);

                if (!ok)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new Respuesta<SocioDTO> { Exito = 0, Mensaje = "No se pudo editar el socio" });

                resp = new Respuesta<SocioDTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<SocioDTO>(socioDb) };
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Respuesta<SocioDTO> { Exito = 0, Mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("GuardarYEnviarInscripcion")]
        public async Task<IActionResult> GuardarYEnviarInscripcion([FromBody] SocioRegistrationRequest request)
        {
            var response = new Respuesta<SocioRegistrationResult>
            {
                Exito = 0,
                List = new SocioRegistrationResult()
            };

            var accessContext = await _userSocioContextService.ResolveAsync(User);
            if (RequiresActiveSocioScope(accessContext))
            {
                response.Mensaje = "No tenes permisos para enviar inscripciones.";
                response.List.CodigoError = "FORBIDDEN";
                response.List.MensajeUsuario = "No tenes permisos para enviar mails de inscripción desde otra razón social.";
                return StatusCode(StatusCodes.Status403Forbidden, response);
            }

            if (request?.Socio == null)
            {
                response.Mensaje = "Solicitud inválida.";
                response.List.MensajeUsuario = "No se recibieron los datos del socio.";
                return BadRequest(response);
            }

            var socioDto = request.Socio;
            NormalizeSocio(socioDto);

            var validationErrors = ValidateSocioForRegistration(socioDto);
            if (validationErrors.Count > 0)
            {
                response.Mensaje = "Hay datos obligatorios o inválidos.";
                response.List.CodigoError = "VALIDATION_ERROR";
                response.List.MensajeUsuario = "Revisá los datos obligatorios antes de enviar el mail de inscripción.";
                response.List.Errores = validationErrors;
                return BadRequest(response);
            }

            var socioDb = await _db.Socios.FirstOrDefaultAsync(s => s.Id == socioDto.Id);
            if (socioDb == null)
            {
                response.Mensaje = "Socio no encontrado.";
                response.List.CodigoError = "SOCIO_NOT_FOUND";
                response.List.MensajeUsuario = "No se encontró el socio que se intenta actualizar.";
                return NotFound(response);
            }

            if (!string.IsNullOrWhiteSpace(socioDto.Codpos2))
            {
                var conflictCodpos2 = await _db.Socios
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Id != socioDto.Id && s.Codpos2 == socioDto.Codpos2);

                if (conflictCodpos2 != null)
                {
                    response.Mensaje = "Código de socio duplicado.";
                    response.List.CodigoError = "DUPLICATE_CODPOS2";
                    response.List.MensajeUsuario = $"El código de socio '{socioDto.Codpos2}' ya está en uso por {BuildSocioDisplayName(conflictCodpos2)}.";
                    response.List.Errores.Add("El código de socio ya existe en la base de datos.");
                    return Conflict(response);
                }
            }

            var targetDomainUser = await FindDomainUserBySocioIdAsync(socioDto.Id);
            if (targetDomainUser == null)
            {
                response.Mensaje = "El socio no tiene usuario asociado.";
                response.List.CodigoError = "SOCIO_WITHOUT_USER";
                response.List.MensajeUsuario = $"El socio {BuildSocioDisplayName(socioDb)} no tiene un usuario asociado. No se puede enviar el mail de inscripción.";
                AddUniqueError(response.List, response.List.MensajeUsuario);
                return Conflict(response);
            }

            if (!IsSocioRole(targetDomainUser.Rol))
            {
                response.Mensaje = "El usuario asociado no es un socio.";
                response.List.CodigoError = "ASSOCIATED_USER_INVALID_ROLE";
                response.List.MensajeUsuario = $"El usuario asociado al socio tiene rol {targetDomainUser.Rol}. No se puede enviar el mail de inscripción desde este flujo.";
                AddUniqueError(response.List, response.List.MensajeUsuario);
                return Conflict(response);
            }

            var targetIdentityUser = await FindIdentityUserForDomainUserAsync(targetDomainUser);
            if (targetIdentityUser == null)
            {
                response.Mensaje = "Usuario de acceso inconsistente.";
                response.List.CodigoError = "IDENTITY_USER_NOT_FOUND";
                response.List.MensajeUsuario = "El usuario asociado existe en la base, pero no se encontró el acceso de Identity. Revisá el usuario antes de reenviar la inscripción.";
                return BadRequest(response);
            }

            var normalizedEmail = socioDto.Mail!.Trim().ToUpperInvariant();
            var emailDomainUser = await _identityDb.User.FirstOrDefaultAsync(
                u => u.Email != null && u.Email.ToUpper() == normalizedEmail);
            if (emailDomainUser != null && emailDomainUser.Id != targetDomainUser.Id)
            {
                response.Mensaje = "El mail ya pertenece a otro usuario.";
                response.List.CodigoError = "EMAIL_IN_USE_BY_OTHER_USER";
                response.List.MensajeUsuario = $"El mail '{socioDto.Mail}' ya está asociado a otro usuario. No se puede actualizar el usuario asociado a este socio con ese mail.";
                response.List.MailAsociado = emailDomainUser.Email;
                AddUniqueError(response.List, response.List.MensajeUsuario);
                return Conflict(response);
            }

            var duplicateIdentity = await _userManager.FindByEmailAsync(socioDto.Mail!);
            if (duplicateIdentity != null && !string.Equals(duplicateIdentity.Id, targetIdentityUser.Id, StringComparison.Ordinal))
            {
                response.Mensaje = "El mail ya pertenece a otro usuario.";
                response.List.CodigoError = "EMAIL_IN_USE_BY_OTHER_USER";
                response.List.MensajeUsuario = $"El mail '{socioDto.Mail}' ya está registrado en otro acceso. No se puede actualizar el usuario asociado a este socio con ese mail.";
                AddUniqueError(response.List, response.List.MensajeUsuario);
                return Conflict(response);
            }

            var previousSocioState = CaptureSocioState(socioDb);
            var previousDomainUserState = CloneDomainUser(targetDomainUser);
            var previousIdentityState = CloneIdentityUser(targetIdentityUser);
            var password = GenerateServerPassword(12);

            try
            {
                ApplySocioChanges(socioDb, socioDto);
                await _db.SaveChangesAsync();

                ApplyDomainUserChanges(targetDomainUser, socioDto);
                targetDomainUser.Status = string.IsNullOrWhiteSpace(targetDomainUser.Status)
                    ? "ACTIVO"
                    : targetDomainUser.Status.ToUpperInvariant();

                targetIdentityUser.Email = socioDto.Mail!;
                targetIdentityUser.UserName = socioDto.Mail!;
                targetIdentityUser.NormalizedEmail = socioDto.Mail!.ToUpperInvariant();
                targetIdentityUser.NormalizedUserName = socioDto.Mail.ToUpperInvariant();
                targetIdentityUser.EmailConfirmed = true;
                targetIdentityUser.PhoneNumber = socioDto.Telefo1;

                var passwordValidation = await ValidatePasswordAsync(targetIdentityUser, password);
                if (!passwordValidation.Succeeded)
                {
                    response.Mensaje = "No se pudo generar una contraseña válida.";
                    response.List.CodigoError = "PASSWORD_POLICY_FAILED";
                    response.List.MensajeUsuario = MapIdentityErrors(passwordValidation.Errors);
                    AddUniqueError(response.List, response.List.MensajeUsuario);
                    throw new FriendlySocioRegistrationException(response);
                }

                targetIdentityUser.PasswordHash = _userManager.PasswordHasher.HashPassword(targetIdentityUser, password);
                targetIdentityUser.SecurityStamp = Guid.NewGuid().ToString("D");
                targetIdentityUser.ConcurrencyStamp = Guid.NewGuid().ToString("D");

                var identityUpdate = await _userManager.UpdateAsync(targetIdentityUser);
                if (!identityUpdate.Succeeded)
                {
                    response.Mensaje = "No se pudo actualizar el usuario.";
                    response.List.CodigoError = "UPDATE_USER_FAILED";
                    response.List.MensajeUsuario = MapIdentityErrors(identityUpdate.Errors);
                    AddUniqueError(response.List, response.List.MensajeUsuario);
                    throw new FriendlySocioRegistrationException(response);
                }

                await _identityDb.SaveChangesAsync();

                var mailSent = await TrySendRegistrationMailAsync(targetDomainUser, password);
                if (!mailSent.Success)
                {
                    response.Mensaje = "No se pudo enviar el mail de inscripción.";
                    response.List.CodigoError = "SEND_MAIL_FAILED";
                    response.List.MensajeUsuario = "No se pudo enviar el mail de inscripción. No se guardaron los cambios.";
                    AddUniqueError(response.List, mailSent.ErrorMessage ?? "Error desconocido al enviar el correo.");
                    throw new FriendlySocioRegistrationException(response);
                }

                response.Exito = 1;
                response.Mensaje = "OK";
                response.List.UsuarioCreado = false;
                response.List.UsuarioReiniciado = true;
                response.List.CodigoError = null;
                response.List.MensajeUsuario = "Se actualizaron los datos del socio, se restableció la contraseña y se reenvió el mail de inscripción.";
                response.List.Socio = _mapper.Map<SocioDTO>(socioDb);
                return Ok(response);
            }
            catch (FriendlySocioRegistrationException)
            {
                await RollbackSocioRegistrationAsync(
                    socioDb,
                    previousSocioState,
                    targetDomainUser,
                    previousDomainUserState,
                    null,
                    targetIdentityUser,
                    previousIdentityState,
                    null,
                    null);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await RollbackSocioRegistrationAsync(
                    socioDb,
                    previousSocioState,
                    targetDomainUser,
                    previousDomainUserState,
                    null,
                    targetIdentityUser,
                    previousIdentityState,
                    null,
                    null);

                response.Mensaje = ex.Message;
                response.List.CodigoError = "UNEXPECTED_ERROR";
                response.List.MensajeUsuario = "Ocurrió un error al enviar el mail de inscripción. No se guardaron los cambios.";
                if (!string.IsNullOrWhiteSpace(ex.Message))
                {
                    AddUniqueError(response.List, ex.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("SyncCodpos2")]
        public async Task<IActionResult> SyncCodpos2()
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                }

                var all = _db.Socios.ToList();
                foreach (var s in all)
                {
                    if (string.IsNullOrEmpty(s.Scod)) { s.Codpos2 = s.Scod; continue; }
                    // Codpos2 column has max length 4 in DB. Use rightmost 4 chars to avoid overflow.
                    s.Codpos2 = s.Scod.Length <= 4 ? s.Scod : s.Scod.Substring(s.Scod.Length - 4);
                }
                await _db.SaveChangesAsync();
                return Ok(new Respuesta<string> { Exito = 1, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<string> { Exito = 0, Mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("Reserve")]
        public async Task<IActionResult> Reserve()
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<SocioDTO>());
                }

                // Compute next numeric Scod (consider only fully numeric Scod values)
                var allScods = _db.Socios.Select(s => s.Scod).ToList();
                int max = 0;
                foreach (var sc in allScods)
                {
                    if (int.TryParse(sc, out var v))
                    {
                        if (v > max) max = v;
                    }
                }
                var next = (max + 1).ToString("D4");

                var nuevo = new Socio
                {
                    Scod = next,
                    Criador = "N",
                    Nombre = string.Empty
                };

                _db.Socios.Add(nuevo);
                await _db.SaveChangesAsync();

                var dto = _mapper.Map<SocioDTO>(nuevo);
                return Ok(new Respuesta<SocioDTO> { Exito = 1, Mensaje = "Reservado", List = dto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<string> { Exito = 0, Mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("ExistsCodpos2")]
        public async Task<IActionResult> ExistsCodpos2(string codpos2, int? excludeId = null)
        {
            try
            {
                // Codpos2 duplicates are allowed, so always report "not exists"
                return Ok(new Respuesta<bool> { Exito = 1, Mensaje = "OK", List = false });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<string> { Exito = 0, Mensaje = ex.Message });
            }
        }

        private static void NormalizeSocio(SocioDTO socio)
        {
            socio.Nombre = socio.Nombre?.Trim();
            socio.Direcc1 = socio.Direcc1?.Trim();
            socio.Locali1 = socio.Locali1?.Trim();
            socio.Codpos1 = socio.Codpos1?.Trim();
            socio.Telefo1 = socio.Telefo1?.Trim();
            socio.Telefo2 = socio.Telefo2?.Trim();
            socio.Mail = socio.Mail?.Trim();
            socio.Codpos2 = socio.Codpos2?.Trim();
            socio.Placod = socio.Placod?.Trim();
            socio.Codpro1 = socio.Codpro1?.Trim();
        }

        private static List<string> ValidateSocioForRegistration(SocioDTO socio)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(socio.Nombre)) errors.Add("Completá el nombre completo.");
            if (string.IsNullOrWhiteSpace(socio.Direcc1)) errors.Add("Completá el domicilio.");
            if (string.IsNullOrWhiteSpace(socio.Locali1)) errors.Add("Completá la localidad.");
            if (string.IsNullOrWhiteSpace(socio.Codpos1)) errors.Add("Completá el código postal.");
            if (string.IsNullOrWhiteSpace(socio.Codpro1)) errors.Add("Seleccioná una provincia válida.");
            if (string.IsNullOrWhiteSpace(socio.Telefo1)) errors.Add("Completá el teléfono.");
            if (string.IsNullOrWhiteSpace(socio.Mail)) errors.Add("Completá el mail.");
            if (string.IsNullOrWhiteSpace(socio.Codpos2)) errors.Add("Completá el código de socio.");
            if (string.IsNullOrWhiteSpace(socio.Placod)) errors.Add("Completá el plantel.");
            if (!socio.Fecing.HasValue) errors.Add("Completá la fecha de ingreso.");

            if (!string.IsNullOrWhiteSpace(socio.Mail))
            {
                try
                {
                    var addr = new MailAddress(socio.Mail);
                    if (!string.Equals(addr.Address, socio.Mail, StringComparison.OrdinalIgnoreCase))
                    {
                        errors.Add("El formato del mail no es válido.");
                    }
                }
                catch
                {
                    errors.Add("El formato del mail no es válido.");
                }
            }

            return errors.Distinct().ToList();
        }

        private static string BuildSocioDisplayName(Socio socio)
        {
            var nombre = string.IsNullOrWhiteSpace(socio.Nombre) ? "Socio sin nombre" : socio.Nombre.Trim();
            var codigo = string.IsNullOrWhiteSpace(socio.Codpos2) ? socio.Scod?.Trim() : socio.Codpos2.Trim();
            return string.IsNullOrWhiteSpace(codigo) ? nombre : $"{nombre} (código {codigo})";
        }

        private static string BuildEmailConflictMessage(User user, Socio? socio)
        {
            var mail = user.Email ?? "(sin mail)";

            if (socio == null)
            {
                return $"El mail '{mail}' ya está asociado a otro usuario. Eliminá o corregí ese usuario antes de continuar.";
            }

            var codigo = string.IsNullOrWhiteSpace(socio.Codpos2) ? socio.Scod : socio.Codpos2;
            var nombre = string.IsNullOrWhiteSpace(socio.Nombre) ? "Socio sin nombre" : socio.Nombre.Trim();
            return $"El mail '{mail}' ya está asociado al socio {nombre} (código {codigo}). Eliminá o corregí ese usuario antes de continuar.";
        }

        private static bool RequiresActiveSocioScope(UserSocioAccessContext accessContext)
            => accessContext.IsSocioUser && !accessContext.IsPrivilegedUser;

        private static string BuildActiveSocioIdFilter(int socioId)
            => $"Id == {socioId}";

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

        private static bool IsSocioRole(string? role)
            => string.Equals(role, "SOCIO", StringComparison.OrdinalIgnoreCase)
               || string.Equals(role, "Socio", StringComparison.OrdinalIgnoreCase);

        private async Task<User?> FindDomainUserBySocioIdAsync(int socioId)
        {
            var userId = await _identityDb.UserSocios
                .AsNoTracking()
                .Where(x => x.SocioId == socioId)
                .OrderBy(x => x.Id)
                .Select(x => x.UserId)
                .FirstOrDefaultAsync();

            if (userId > 0)
            {
                return await _identityDb.User.FirstOrDefaultAsync(x => x.Id == userId);
            }

            return await _identityDb.User.FirstOrDefaultAsync(x => x.SocioId == socioId);
        }

        private async Task<IdentityUser?> FindIdentityUserForDomainUserAsync(User domainUser)
        {
            if (!string.IsNullOrWhiteSpace(domainUser.IdentityUserId))
            {
                var byId = await _userManager.FindByIdAsync(domainUser.IdentityUserId);
                if (byId != null)
                {
                    return byId;
                }
            }

            return await FindIdentityUserAsync(domainUser.Email);
        }

        private async Task<List<int>> GetUserSocioIdsAsync(User user)
        {
            var ids = await _identityDb.UserSocios
                .AsNoTracking()
                .Where(x => x.UserId == user.Id)
                .Select(x => x.SocioId)
                .ToListAsync();

            if (ids.Count == 0 && user.SocioId.HasValue && user.SocioId.Value > 0)
            {
                ids.Add(user.SocioId.Value);
            }

            return ids
                .Where(x => x > 0)
                .Distinct()
                .ToList();
        }

        private async Task EnsureSocioLinkedAsync(User user, int socioId)
        {
            if (socioId <= 0)
            {
                return;
            }

            var existing = await _identityDb.UserSocios
                .FirstOrDefaultAsync(x => x.UserId == user.Id && x.SocioId == socioId);

            if (existing == null)
            {
                await _identityDb.UserSocios.AddAsync(new UserSocio
                {
                    UserId = user.Id,
                    SocioId = socioId,
                    CreatedAt = DateTime.UtcNow
                });
            }

            var linkedSocioIds = await GetUserSocioIdsAsync(user);
            if (!user.SocioId.HasValue || user.SocioId.Value <= 0 || !linkedSocioIds.Contains(user.SocioId.Value))
            {
                user.SocioId = socioId;
            }
        }

        private async Task RestoreUserSociosAsync(User user, IReadOnlyCollection<int>? previousSocioIds)
        {
            var existing = await _identityDb.UserSocios
                .Where(x => x.UserId == user.Id)
                .ToListAsync();

            if (previousSocioIds == null)
            {
                if (existing.Count > 0)
                {
                    _identityDb.UserSocios.RemoveRange(existing);
                }
                return;
            }

            var previous = previousSocioIds
                .Where(x => x > 0)
                .Distinct()
                .ToHashSet();

            var toRemove = existing.Where(x => !previous.Contains(x.SocioId)).ToList();
            if (toRemove.Count > 0)
            {
                _identityDb.UserSocios.RemoveRange(toRemove);
            }

            var currentIds = existing.Select(x => x.SocioId).ToHashSet();
            foreach (var socioId in previous.Where(x => !currentIds.Contains(x)))
            {
                await _identityDb.UserSocios.AddAsync(new UserSocio
                {
                    UserId = user.Id,
                    SocioId = socioId,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        private async Task<Socio?> FindAssociatedSocioAsync(User user, int? preferredSocioId = null)
        {
            var socioIds = await GetUserSocioIdsAsync(user);
            if (preferredSocioId.HasValue && socioIds.Contains(preferredSocioId.Value))
            {
                return await _db.Socios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == preferredSocioId.Value);
            }

            var socioId = socioIds.FirstOrDefault();
            if (socioId <= 0)
            {
                return null;
            }

            return await _db.Socios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == socioId);
        }

        private static void ApplyDomainUserChanges(User user, SocioDTO socioDto)
        {
            if (string.IsNullOrWhiteSpace(user.Names))
            {
                user.Names = socioDto.Nombre ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(user.LastNames))
            {
                user.LastNames = socioDto.Nombre ?? string.Empty;
            }

            user.Phone = socioDto.Telefo1 ?? user.Phone;
            user.Email = socioDto.Mail ?? user.Email;
        }

        private async Task<(bool Success, string? ErrorMessage)> TrySendRegistrationMailAsync(User model, string password)
        {
            try
            {
                using var mail = new MailMessage();
                mail.From = new MailAddress("planteles@hereford.org.ar");
                mail.To.Add(model.Email);
                mail.Subject = "Hereford - Puro Registrado.";

                string projectRoot = Directory.GetCurrentDirectory();
                string imagePath = Path.Combine(projectRoot, "wwwroot", "images", "backgroundEnvio.jpg");
                string logoPath = Path.Combine(projectRoot, "wwwroot", "images", "LOGO.jpg");

                if (model.Email.Contains("outlook", StringComparison.OrdinalIgnoreCase))
                {
                    logoPath = string.Empty;
                    imagePath = string.Empty;
                }

                if (!string.IsNullOrEmpty(imagePath) && !System.IO.File.Exists(imagePath))
                {
                    return (false, "No se encontró la imagen de fondo del correo.");
                }

                if (!string.IsNullOrEmpty(logoPath) && !System.IO.File.Exists(logoPath))
                {
                    return (false, "No se encontró el logo del correo.");
                }

                var logoHtml = string.IsNullOrEmpty(logoPath)
                    ? string.Empty
                    : "<img src='cid:logoImage' alt='Hereford Logo' style='width:150px; height:auto;' />";

                string body = $@"
            <html>
            <body style='margin:0;padding:0;'>
                <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                    <tr>
                        <td>
                            <table width='600' border='0' cellspacing='0' cellpadding='0' align='center' style='background-repeat:no-repeat;background-image:url(cid:backgroundImage);background-size:cover;'>
                                <tr>
                                    <td style='padding: 20px; text-align: left;'>
                                        {logoHtml}
                                    </td>
                                </tr>
                                <tr>
                                    <td style='padding: 20px; padding-top: 10px; color: #000;'>
                                        <h2>Buenos Aires, {DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"))}</h2>
                                        <p>Señor {WebUtility.HtmlEncode(model.Names ?? "criador")} {WebUtility.HtmlEncode(model.LastNames ?? string.Empty)}:</p>
                                        <p>Les informamos que, a partir de este momento, el sistema de autogestión anterior ya no estará en funcionamiento. Hemos implementado una nueva plataforma para mejorar la gestión y facilitarles el acceso a los servicios. Puede acceder a su perfil <a href='https://herefordapp.com.ar:1050/'>aquí</a>.</p>
                                        <p><strong>Detalles de inicio de sesión:</strong></p>
                                        <p>Correo electrónico registrado: {WebUtility.HtmlEncode(model.Email)}<br>Contraseña: {WebUtility.HtmlEncode(password)}</p>
                                        <p>Recuerde mantener segura esta información y no compartirla. Gracias por su tiempo y ante cualquier consulta no dude en comunicarse por mail a planteles@hereford.org.ar</p>
                                        <p>Gracias por su comprensión y colaboración.</p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='padding: 20px; padding-top: 25px; color: #777;'>
                                        <p>Ing. Emilio Ortiz (Responsable Puro Registrado) - <a href='mailto:eortiz@hereford.org.ar'>eortiz@hereford.org.ar</a></p>
                                        <p>Paz Hernández (Encargada Registros) - <a href='mailto:planteles@hereford.org.ar'>planteles@hereford.org.ar</a></p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='height: 200px;'></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>";

                var htmlView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

                if (!string.IsNullOrEmpty(imagePath))
                {
                    var background = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg)
                    {
                        ContentId = "backgroundImage",
                        TransferEncoding = TransferEncoding.Base64
                    };
                    htmlView.LinkedResources.Add(background);
                }

                if (!string.IsNullOrEmpty(logoPath))
                {
                    var logo = new LinkedResource(logoPath, MediaTypeNames.Image.Jpeg)
                    {
                        ContentId = "logoImage",
                        TransferEncoding = TransferEncoding.Base64
                    };
                    htmlView.LinkedResources.Add(logo);
                }

                mail.AlternateViews.Add(htmlView);
                mail.IsBodyHtml = true;

                using var smtp = new SmtpClient("mail.hereford.org.ar", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("planteles@hereford.org.ar", "Hereford.2033"),
                    EnableSsl = true
                };

                smtp.Send(mail);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        private async Task RollbackSocioRegistrationAsync(
            Socio socioDb,
            SocioRollbackState previousSocioState,
            User? existingDomainUser,
            User? previousDomainUserState,
            IReadOnlyCollection<int>? previousUserSocioIds,
            IdentityUser? existingIdentityUser,
            IdentityUser? previousIdentityState,
            User? createdDomainUser,
            IdentityUser? createdIdentityUser)
        {
            try
            {
                if (createdDomainUser != null)
                {
                    var createdRelations = await _identityDb.UserSocios
                        .Where(x => x.UserId == createdDomainUser.Id)
                        .ToListAsync();

                    if (createdRelations.Count > 0)
                    {
                        _identityDb.UserSocios.RemoveRange(createdRelations);
                    }

                    _identityDb.User.Remove(createdDomainUser);
                }

                if (existingDomainUser != null && previousDomainUserState != null)
                {
                    RestoreDomainUser(existingDomainUser, previousDomainUserState);
                    await RestoreUserSociosAsync(existingDomainUser, previousUserSocioIds);
                }

                if (createdIdentityUser != null)
                {
                    var persistedIdentity = await _userManager.FindByIdAsync(createdIdentityUser.Id);
                    if (persistedIdentity != null)
                    {
                        await _userManager.DeleteAsync(persistedIdentity);
                    }
                }

                if (existingIdentityUser != null && previousIdentityState != null)
                {
                    RestoreIdentityUser(existingIdentityUser, previousIdentityState);
                }

                await _identityDb.SaveChangesAsync();
            }
            catch
            {
            }

            try
            {
                RestoreSocio(socioDb, previousSocioState);
                await _db.SaveChangesAsync();
            }
            catch
            {
            }
        }

        private async Task<IdentityResult> ValidatePasswordAsync(IdentityUser user, string password)
        {
            foreach (var validator in _userManager.PasswordValidators)
            {
                var result = await validator.ValidateAsync(_userManager, user, password);
                if (!result.Succeeded)
                {
                    return result;
                }
            }

            return IdentityResult.Success;
        }

        private async Task<IdentityUser?> FindIdentityUserAsync(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            return await _userManager.FindByEmailAsync(email)
                ?? await _userManager.FindByNameAsync(email);
        }

        private static string MapIdentityErrors(IEnumerable<IdentityError> errors)
        {
            var list = errors?.ToList() ?? new List<IdentityError>();
            if (list.Count == 0)
            {
                return "Ocurrió un error al procesar el usuario.";
            }

            if (list.Any(e =>
                string.Equals(e.Code, "DuplicateUserName", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(e.Code, "DuplicateEmail", StringComparison.OrdinalIgnoreCase)))
            {
                return "Este mail ya está registrado en el sistema. Eliminá o corregí ese usuario antes de continuar.";
            }

            return string.Join(" ", list.Where(e => !string.IsNullOrWhiteSpace(e.Description)).Select(e => e.Description));
        }

        private static void AddUniqueError(SocioRegistrationResult? result, string? message)
        {
            if (result == null || string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            if (!result.Errores.Any(x => string.Equals(x, message, StringComparison.OrdinalIgnoreCase)))
            {
                result.Errores.Add(message);
            }
        }

        private static User CloneDomainUser(User source)
        {
            return new User
            {
                Id = source.Id,
                Names = source.Names,
                LastNames = source.LastNames,
                Dni = source.Dni,
                Phone = source.Phone,
                Email = source.Email,
                Rol = source.Rol,
                Status = source.Status,
                Created = source.Created,
                IdentityUserId = source.IdentityUserId,
                SocioId = source.SocioId
            };
        }

        private static void RestoreDomainUser(User target, User source)
        {
            target.Names = source.Names;
            target.LastNames = source.LastNames;
            target.Dni = source.Dni;
            target.Phone = source.Phone;
            target.Email = source.Email;
            target.Rol = source.Rol;
            target.Status = source.Status;
            target.Created = source.Created;
            target.IdentityUserId = source.IdentityUserId;
            target.SocioId = source.SocioId;
        }

        private static IdentityUser CloneIdentityUser(IdentityUser source)
        {
            return new IdentityUser
            {
                Id = source.Id,
                UserName = source.UserName,
                NormalizedUserName = source.NormalizedUserName,
                Email = source.Email,
                NormalizedEmail = source.NormalizedEmail,
                EmailConfirmed = source.EmailConfirmed,
                PasswordHash = source.PasswordHash,
                SecurityStamp = source.SecurityStamp,
                ConcurrencyStamp = source.ConcurrencyStamp,
                PhoneNumber = source.PhoneNumber,
                PhoneNumberConfirmed = source.PhoneNumberConfirmed,
                TwoFactorEnabled = source.TwoFactorEnabled,
                LockoutEnd = source.LockoutEnd,
                LockoutEnabled = source.LockoutEnabled,
                AccessFailedCount = source.AccessFailedCount
            };
        }

        private static void RestoreIdentityUser(IdentityUser target, IdentityUser source)
        {
            target.UserName = source.UserName;
            target.NormalizedUserName = source.NormalizedUserName;
            target.Email = source.Email;
            target.NormalizedEmail = source.NormalizedEmail;
            target.EmailConfirmed = source.EmailConfirmed;
            target.PasswordHash = source.PasswordHash;
            target.SecurityStamp = source.SecurityStamp;
            target.ConcurrencyStamp = source.ConcurrencyStamp;
            target.PhoneNumber = source.PhoneNumber;
            target.PhoneNumberConfirmed = source.PhoneNumberConfirmed;
            target.TwoFactorEnabled = source.TwoFactorEnabled;
            target.LockoutEnd = source.LockoutEnd;
            target.LockoutEnabled = source.LockoutEnabled;
            target.AccessFailedCount = source.AccessFailedCount;
        }

        private static SocioRollbackState CaptureSocioState(Socio socio)
        {
            return new SocioRollbackState
            {
                Nombre = socio.Nombre,
                Direcc1 = socio.Direcc1,
                Locali1 = socio.Locali1,
                Codpos1 = socio.Codpos1,
                Codpro1 = socio.Codpro1,
                Telefo1 = socio.Telefo1,
                Telefo2 = socio.Telefo2,
                Mail = socio.Mail,
                Criador = socio.Criador,
                Fecing = socio.Fecing,
                Placod = socio.Placod,
                Codpos2 = socio.Codpos2
            };
        }

        private static void RestoreSocio(Socio socio, SocioRollbackState state)
        {
            socio.Nombre = state.Nombre;
            socio.Direcc1 = state.Direcc1;
            socio.Locali1 = state.Locali1;
            socio.Codpos1 = state.Codpos1;
            socio.Codpro1 = state.Codpro1;
            socio.Telefo1 = state.Telefo1;
            socio.Telefo2 = state.Telefo2;
            socio.Mail = state.Mail;
            socio.Criador = state.Criador;
            socio.Fecing = state.Fecing;
            socio.Placod = state.Placod;
            socio.Codpos2 = state.Codpos2;
        }

        private static void ApplySocioChanges(Socio socioDb, SocioDTO socioDto)
        {
            socioDb.Nombre = socioDto.Nombre;
            socioDb.Direcc1 = socioDto.Direcc1;
            socioDb.Locali1 = socioDto.Locali1;
            socioDb.Codpos1 = socioDto.Codpos1;
            socioDb.Codpro1 = socioDto.Codpro1;
            socioDb.Telefo1 = socioDto.Telefo1;
            socioDb.Telefo2 = socioDto.Telefo2;
            socioDb.Mail = socioDto.Mail;
            socioDb.Criador = socioDto.Criador;
            socioDb.Fecing = socioDto.Fecing;
            socioDb.Placod = socioDto.Placod;
            socioDb.Codpos2 = socioDto.Codpos2;
        }

        private static string GenerateServerPassword(int length)
        {
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            string all = upper + digits;

            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            int Next(int max)
            {
                Span<byte> bytes = stackalloc byte[4];
                rng.GetBytes(bytes);
                return (int)(BitConverter.ToUInt32(bytes) % (uint)max);
            }

            var sb = new StringBuilder();
            sb.Append(upper[Next(upper.Length)]);
            sb.Append(digits[Next(digits.Length)]);

            while (sb.Length < Math.Max(length, 10))
            {
                sb.Append(all[Next(all.Length)]);
            }

            var chars = sb.ToString().ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                int j = Next(chars.Length);
                (chars[i], chars[j]) = (chars[j], chars[i]);
            }

            return new string(chars);
        }

        private sealed class FriendlySocioRegistrationException : Exception
        {
            public FriendlySocioRegistrationException(Respuesta<SocioRegistrationResult> response)
                : base(response.Mensaje)
            {
            }
        }

        private sealed class SocioRollbackState
        {
            public string? Nombre { get; set; }
            public string? Direcc1 { get; set; }
            public string? Locali1 { get; set; }
            public string? Codpos1 { get; set; }
            public string? Codpro1 { get; set; }
            public string? Telefo1 { get; set; }
            public string? Telefo2 { get; set; }
            public string? Mail { get; set; }
            public string? Criador { get; set; }
            public DateTime? Fecing { get; set; }
            public string? Placod { get; set; }
            public string? Codpos2 { get; set; }
        }
    }
}
