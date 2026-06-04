using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Client.Pages.Socios;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Services;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransanController : ControllerBase
    {
        private readonly hereford_prContext db;
        private readonly IMapper _mapper;
        private readonly ITransanRepositorio _TransanRepositorio;
        private readonly IWebHostEnvironment _env;
        private readonly IUserSocioContextService _userSocioContextService;
        private readonly ILogger<TransanController> _logger;
        public TransanController(
            hereford_prContext db,
            ITransanRepositorio TransanRepositorio,
            IMapper mapper,
            IWebHostEnvironment env,
            IUserSocioContextService userSocioContextService,
            ILogger<TransanController> logger)
        {
            this.db = db;
            _mapper = mapper;
            _TransanRepositorio = TransanRepositorio;
            _env = env;
            _userSocioContextService = userSocioContextService;
            _logger = logger;
        }
        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<TransanDTO>> _ResponseDTO = new Respuesta<List<TransanDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<TransanDTO>>());
                }

                List<TransanDTO> listaPedido = new List<TransanDTO>();
                var a = RequiresActiveSocioScope(accessContext)
                    ? await _TransanRepositorio.LimitadosFiltrados(skip, take, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : await _TransanRepositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<TransanDTO>>(a);

                _ResponseDTO = new Respuesta<List<TransanDTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<TransanDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                    var query = await _TransanRepositorio.Consultar(x =>
                        x.Sven == accessContext.ActiveSocioCode || x.Scom == accessContext.ActiveSocioCode);
                    a = query.Count();
                }
                else
                {
                    a = await _TransanRepositorio.CantidadTotal();
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

            Respuesta<List<TransanDTO>> _ResponseDTO = new Respuesta<List<TransanDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<TransanDTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? AppendFilter(expression, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : expression;
                var a = await _TransanRepositorio.LimitadosFiltrados(skip, take, effectiveExpression);

                var listaFiltrada = _mapper.Map<List<TransanDTO>>(a);

                _ResponseDTO = new Respuesta<List<TransanDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<TransanDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }


        [HttpPost]
        [Route("SendMail")]
        public async Task<IActionResult> ExampleMethod([FromBody] ContenidoMails request)
        {
            using (MailMessage mail = new MailMessage())
            {
                try
                {
                    var accessContext = await _userSocioContextService.ResolveAsync(User);
                    if (!CanAccessTransfer(accessContext, request?.Vendedor, request?.Comprador))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                    }

                    // Obtener correo del socio vendedor
                    var socioVendedor = await db.Socios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Vendedor);
                    if (socioVendedor != null && IsValidEmail(socioVendedor.Mail))
                    {
                        mail.To.Add(socioVendedor.Mail); // Se agrega el correo del vendedor si es válido
                    }
                    else
                    {
                        Console.WriteLine("Socio vendedor sin cuenta o correo inválido");
                    }


                    // Obtener correo del socio comprador o verificar si hay una dirección
                    if (string.IsNullOrEmpty(request.Direccion))
                    {
                        var socioComprador = await db.Socios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Comprador);
                        if (socioComprador != null && IsValidEmail(socioComprador.Mail))
                        {
                            mail.To.Add(socioComprador.Mail); // Se agrega el correo del comprador si es válido
                        }
                        else
                        {
                            Console.WriteLine("Socio comprador sin cuenta o correo inválido");
                        }
                    }
                    else
                    {
                        if (IsValidEmail(request.Direccion))
                        {
                            mail.To.Add(request.Direccion); // Se agrega la dirección de correo proporcionada si es válida
                        }
                        else
                        {
                            Console.WriteLine("Dirección de correo inválida");
                        }
                    }
                    Console.WriteLine(mail.Body);

                    // Agregar correo de "puroregistrado"
                    string correoPuroRegistrado = "planteles@hereford.org.ar";
                    if (IsValidEmail(correoPuroRegistrado))
                    {
                        mail.To.Add(correoPuroRegistrado); // Se agrega el correo de "puroregistrado" si es válido
                    }
                    else
                    {
                        Console.WriteLine("Correo de puroregistrado no válido");
                    }

                    // Configurar el mensaje
                    var rawBody = request.Mail ?? "(sin cuerpo)";
                    var isHtml = rawBody.Contains("<html", StringComparison.OrdinalIgnoreCase)
                        || rawBody.Contains("<table", StringComparison.OrdinalIgnoreCase)
                        || rawBody.Contains("<br", StringComparison.OrdinalIgnoreCase);
                    var body = isHtml
                        ? rawBody
                        : WebUtility.HtmlEncode(rawBody).Replace("\r\n", "\n").Replace("\n", "<br/>");

                    mail.From = new MailAddress(correoPuroRegistrado);
                    mail.Subject = "Nueva transferencia animal";
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    // Configurar SMTP y enviar
                    using (SmtpClient smtp = new SmtpClient("mail.hereford.org.ar", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential(correoPuroRegistrado, "Hereford.2033"); // Agrega tu contraseña aquí
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(mail);
                    }

                    Console.WriteLine("Transferencia Enviada");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                    return BadRequest("Error al enviar el correo.");
                }

                return Ok("Correo enviado correctamente.");
            }
        }

        //Hereford.2033


        [HttpPost]
        [Route("SendMailChange")]
        public async Task<IActionResult> ExampleMethodChange([FromBody] ContenidoMails2 request)
        {
            using (MailMessage mail = new MailMessage())
            {
                try
                {
                    var accessContext = await _userSocioContextService.ResolveAsync(User);
                    if (!CanAccessTransfer(accessContext, request?.Tipo, request?.Clase))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                    }

                    string correoPuroRegistrado = "planteles@hereford.org.ar";
                    string smtpHost = "mail.hereford.org.ar";
                    int smtpPort = 587;
                    string smtpUsername = "planteles@hereford.org.ar";
                    string smtpPassword = "Hereford.2033"; // ideal: usar variable de entorno

                    // Siempre agregar planteles
                    TryAddRecipient(mail, correoPuroRegistrado);

                    // Vendedor: si vino MailVendedor y es válido, usarlo. Si no, resolver por socio (Tipo)
                    if (!string.IsNullOrWhiteSpace(request.MailVendedor) && IsValidEmail(request.MailVendedor))
                    {
                        TryAddRecipient(mail, request.MailVendedor);
                    }
                    else if (request.Tipo > 0)
                    {
                        var socioVendedor = await db.Socios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Tipo);
                        if (socioVendedor != null && IsValidEmail(socioVendedor.Mail))
                            TryAddRecipient(mail, socioVendedor.Mail);
                        else
                            Console.WriteLine("No se pudo resolver mail del vendedor.");
                    }

                    // Comprador: idem con MailCompra o socio (Clase)
                    if (!string.IsNullOrWhiteSpace(request.MailCompra) && IsValidEmail(request.MailCompra))
                    {
                        TryAddRecipient(mail, request.MailCompra);
                    }
                    else if (request.Clase > 0)
                    {
                        var socioComprador = await db.Socios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Clase);
                        if (socioComprador != null && IsValidEmail(socioComprador.Mail))
                            TryAddRecipient(mail, socioComprador.Mail);
                        else
                            Console.WriteLine("No se pudo resolver mail del comprador.");
                    }

                    // Mensaje
                    var rawBody = request.Mail ?? "(sin cuerpo)";
                    var isHtml = rawBody.Contains("<html", StringComparison.OrdinalIgnoreCase)
                        || rawBody.Contains("<table", StringComparison.OrdinalIgnoreCase)
                        || rawBody.Contains("<br", StringComparison.OrdinalIgnoreCase);

                    var body = isHtml
                        ? rawBody
                        : WebUtility.HtmlEncode(rawBody).Replace("\r\n", "\n").Replace("\n", "<br/>");

                    mail.From = new MailAddress(correoPuroRegistrado);
                    mail.Subject = "El socio " + (request.Nombre ?? "N/D") + " ha modificado una transferencia";
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(mail);
                    }

                    Console.WriteLine("Transferencia modificada - mail enviado");
                    return Ok("Correo enviado correctamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                    return BadRequest("Error al enviar el correo.");
                }
            }
        }

        private void TryAddRecipient(MailMessage mail, string email)
        {
            if (IsValidEmail(email) && !mail.To.Any(x => string.Equals(x.Address, email, StringComparison.OrdinalIgnoreCase)))
                mail.To.Add(email);
        }



        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }


        //dmuu kdke jobp bhgo dmuu kdke jobp bhgo
        // puroregistradohereford@gmail.com

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            Respuesta<string> _Respuesta = new Respuesta<string>();
            try
            {
                _logger.LogInformation("Transan.Eliminar iniciado. TransanId={TransanId}", id);
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                // Load existing transfer using DbContext
                var existing = await db.Transans.FirstOrDefaultAsync(u => u.Id == id);
                if (existing == null)
                {
                    _logger.LogWarning("Transan.Eliminar no encontró la transferencia. TransanId={TransanId}", id);
                    _Respuesta = new Respuesta<string>() { Exito = 0, Mensaje = "No se encontró el identificador" };
                    return StatusCode(StatusCodes.Status404NotFound, _Respuesta);
                }

                if (!CanAccessTransfer(accessContext, existing.Sven, existing.Scom))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                }

                var strategy = db.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await db.Database.BeginTransactionAsync();
                    _logger.LogInformation("Transan.Eliminar transacción abierta. TransanId={TransanId}", existing.Id);

                    var prevCantHem = existing.CantHem ?? 0;
                    var seller = await ResolveTransferPlantelAsync(existing.PlantOrigenId, existing.Plant, "de origen");
                    var buyer = !string.IsNullOrWhiteSpace(existing.NvoPla) || existing.PlantDestinoId.HasValue
                        ? await ResolveTransferPlantelAsync(existing.PlantDestinoId, existing.NvoPla, "de destino")
                        : null;

                    var field = TransferenciaAnimalImpactoHelper.ResolveFieldName(existing.Tiphac, existing.Hemsta, existing.Tipohem, out var fieldError);
                    if (string.IsNullOrWhiteSpace(field))
                        throw new Exception(fieldError ?? "No se pudo determinar la categoría de hembras a revertir.");

                    _logger.LogInformation("Transan.Eliminar revirtiendo stock. TransanId={TransanId} Bucket={Bucket} Cantidad={Cantidad} OrigenPlantelId={OrigenPlantelId} DestinoPlantelId={DestinoPlantelId}",
                        existing.Id, field, prevCantHem, seller.Id, buyer?.Id);

                    if (prevCantHem > 0)
                    {
                        var sellerBefore = TransferenciaAnimalImpactoHelper.GetPlantelFieldValue(_mapper.Map<PlantelDTO>(seller), field);
                        SetPlantelFieldValue(seller, field, sellerBefore + prevCantHem);
                        seller.FchUsu = DateTime.Now;
                        db.Planteles.Update(seller);
                        _logger.LogInformation("Transan.Eliminar origen actualizado. PlantelId={PlantelId} Campo={Campo} Antes={Antes} Despues={Despues}",
                            seller.Id, field, sellerBefore, sellerBefore + prevCantHem);

                        if (buyer != null)
                        {
                            var buyerBefore = TransferenciaAnimalImpactoHelper.GetPlantelFieldValue(_mapper.Map<PlantelDTO>(buyer), field);
                            var buyerAfter = Math.Max(0, buyerBefore - prevCantHem);
                            SetPlantelFieldValue(buyer, field, buyerAfter);
                            buyer.FchUsu = DateTime.Now;
                            db.Planteles.Update(buyer);
                            _logger.LogInformation("Transan.Eliminar destino actualizado. PlantelId={PlantelId} Campo={Campo} Antes={Antes} Despues={Despues}",
                                buyer.Id, field, buyerBefore, buyerAfter);
                        }
                    }

                    var sellerSnapshot = _mapper.Map<PlantelDTO>(seller);
                    var buyerSnapshot = buyer != null ? _mapper.Map<PlantelDTO>(buyer) : null;
                    var impacto = new TransferenciaAnimalImpacto
                    {
                        FieldName = field,
                        BucketLabel = TransferenciaAnimalImpactoHelper.GetBucketLabel(field),
                        OrigenAntes = TransferenciaAnimalImpactoHelper.GetPlantelFieldValue(sellerSnapshot, field) - prevCantHem,
                        OrigenDespues = TransferenciaAnimalImpactoHelper.GetPlantelFieldValue(sellerSnapshot, field),
                        DestinoAntes = buyerSnapshot != null ? TransferenciaAnimalImpactoHelper.GetPlantelFieldValue(buyerSnapshot, field) + prevCantHem : 0,
                        DestinoDespues = buyerSnapshot != null ? TransferenciaAnimalImpactoHelper.GetPlantelFieldValue(buyerSnapshot, field) : 0,
                        Cantidad = prevCantHem
                    };

                    var audit = BuildTransferAudit(_mapper.Map<TransanDTO>(existing), accessContext, "Deleted", impacto, null, null, "N/A", "La transferencia fue eliminada y el stock se revirtió.");
                    db.TransanTransferAudits.Add(audit);

                    var writes1 = await db.SaveChangesAsync();
                    _logger.LogInformation("Transan.Eliminar audit guardada. Writes={Writes} AuditId={AuditId} TransanId={TransanId}", writes1, audit.Id, existing.Id);

                    db.Transans.Remove(existing);
                    var writes2 = await db.SaveChangesAsync();
                    _logger.LogInformation("Transan.Eliminar transferencia borrada. Writes={Writes} TransanId={TransanId}", writes2, existing.Id);

                    await transaction.CommitAsync();
                    _logger.LogInformation("Transan.Eliminar commit completado. TransanId={TransanId}", existing.Id);
                });

                // Log history
                try
                {
                    LogTransferHistory(existing, "Deleted");
                }
                catch { }

                _Respuesta = new Respuesta<string>() { Exito = 1, Mensaje = "ok", List = "" };
                _logger.LogInformation("Transan.Eliminar finalizado correctamente. TransanId={TransanId}", id);

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transan.Eliminar falló. TransanId={TransanId}", id);
                _Respuesta = new Respuesta<string>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] TransanSaveRequestDTO request)
        {
            var respuesta = new Respuesta<TransanDTO>();
            try
            {
                _logger.LogInformation("Transan.Guardar iniciado. VendedorId={VendedorId} CompradorId={CompradorId} PlantOrigenId={PlantOrigenId} PlantDestinoId={PlantDestinoId} CantHem={CantHem} CantMach={CantMach} CantChem={CantChem} CantCmach={CantCmach} Tiphac={Tiphac} Hemsta={Hemsta} Tipohem={Tipohem}",
                    request?.VendedorId,
                    request?.CompradorId,
                    request?.PlantOrigenId,
                    request?.PlantDestinoId,
                    request?.Transan?.CantHem,
                    request?.Transan?.CantMach,
                    request?.Transan?.CantChem,
                    request?.Transan?.CantCmach,
                    request?.Transan?.Tiphac,
                    request?.Transan?.Hemsta,
                    request?.Transan?.Tipohem);

                if (request?.Transan == null)
                {
                    respuesta = new Respuesta<TransanDTO> { Exito = 0, Mensaje = "No se recibió la transferencia a guardar." };
                    return BadRequest(respuesta);
                }

                var accessContext = await _userSocioContextService.ResolveAsync(User);

                NormalizeTransfer(request.Transan);

                if (!CanAccessTransfer(accessContext, request.Transan.Sven, request.Transan.Scom))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<TransanDTO>());
                }

                var errores = await ValidateTransferRequestAsync(request, false);
                if (errores.Count > 0)
                {
                    _logger.LogWarning("Transan.Guardar rechazado por validación: {Errores}", string.Join(" | ", errores));
                    respuesta = new Respuesta<TransanDTO> { Exito = 0, Mensaje = string.Join("<br/>", errores) };
                    return BadRequest(respuesta);
                }

                var transan = _mapper.Map<Transan>(request.Transan);
                var strategy = db.Database.CreateExecutionStrategy();
                TransanTransferAudit? audit = null;

                await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await db.Database.BeginTransactionAsync();
                    _logger.LogInformation("Transan.Guardar transacción abierta. DraftId={DraftId} PlantOrigenId={PlantOrigenId} PlantDestinoId={PlantDestinoId}",
                        transan.Id,
                        transan.PlantOrigenId,
                        transan.PlantDestinoId);

                    var impacto = await ApplyTransferChangesAsync(request.Transan);
                    var sellerPlant = await ResolveTransferPlantelAsync(request.Transan.PlantOrigenId, request.Transan.Plant, "de origen");
                    var buyerPlant = await ResolveTransferPlantelAsync(request.Transan.PlantDestinoId, request.Transan.NvoPla, "de destino");
                    ApplyTransferPlantelMetadata(transan, sellerPlant, buyerPlant);
                    _logger.LogInformation("Transan.Guardar impacto calculado. Bucket={Bucket} OrigenAntes={OrigenAntes} OrigenDespues={OrigenDespues} DestinoAntes={DestinoAntes} DestinoDespues={DestinoDespues}",
                        impacto.FieldName,
                        impacto.OrigenAntes,
                        impacto.OrigenDespues,
                        impacto.DestinoAntes,
                        impacto.DestinoDespues);

                    db.Transans.Add(transan);
                    audit = BuildTransferAudit(
                        request.Transan,
                        accessContext,
                        "Created",
                        impacto,
                        request.VendedorId,
                        request.CompradorId,
                        "Pendiente",
                        null);
                    db.TransanTransferAudits.Add(audit);
                    var writes1 = await db.SaveChangesAsync();
                    _logger.LogInformation("Transan.Guardar primer SaveChanges completado. Writes={Writes} TransanId={TransanId} AuditId={AuditId}",
                        writes1,
                        transan.Id,
                        audit.Id);

                    audit.TransanId = transan.Id;
                    db.TransanTransferAudits.Update(audit);
                    var writes2 = await db.SaveChangesAsync();
                    _logger.LogInformation("Transan.Guardar audit enlazada a transan. Writes={Writes} TransanId={TransanId} AuditId={AuditId}",
                        writes2,
                        transan.Id,
                        audit.Id);

                    await QueueTransferNotificationAsync(request, transan, false);
                    audit.MailEstado = "Pendiente";
                    audit.MailError = null;
                    db.TransanTransferAudits.Update(audit);
                    var writes3 = await db.SaveChangesAsync();
                    _logger.LogInformation("Transan.Guardar outbox registrada. Writes={Writes} TransanId={TransanId} AuditId={AuditId}",
                        writes3,
                        transan.Id,
                        audit.Id);

                    await transaction.CommitAsync();
                    _logger.LogInformation("Transan.Guardar commit completado. TransanId={TransanId}", transan.Id);
                });

                try
                {
                    LogTransferHistory(transan, "Created");
                }
                catch (Exception lex)
                {
                    Console.WriteLine($"Failed to write history log: {lex.Message}");
                }

                respuesta = new Respuesta<TransanDTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<TransanDTO>(transan) };
                _logger.LogInformation("Transan.Guardar finalizado correctamente. TransanId={TransanId}", transan.Id);
                return Ok(respuesta);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Transan.Guardar fallo por DbUpdateException.");
                respuesta = new Respuesta<TransanDTO> { Exito = 0, Mensaje = BuildDbUpdateMessage(ex) };
                return BadRequest(respuesta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transan.Guardar fallo por Exception general.");
                respuesta = new Respuesta<TransanDTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] TransanSaveRequestDTO request)
        {
            var respuesta = new Respuesta<TransanDTO>();
            try
            {
                if (request?.Transan == null)
                {
                    respuesta = new Respuesta<TransanDTO> { Exito = 0, Mensaje = "No se recibió la transferencia a editar." };
                    return BadRequest(respuesta);
                }

                var accessContext = await _userSocioContextService.ResolveAsync(User);

                NormalizeTransfer(request.Transan);

                var existing = await db.Transans.FirstOrDefaultAsync(u => u.Id == request.Transan.Id);
                if (existing == null)
                {
                    respuesta = new Respuesta<TransanDTO> { Exito = 0, Mensaje = "No se encontró la transferencia indicada." };
                    return NotFound(respuesta);
                }

                if (!CanAccessTransfer(accessContext, existing.Sven, existing.Scom) ||
                    !CanAccessTransfer(accessContext, request.Transan.Sven, request.Transan.Scom))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<TransanDTO>());
                }

                if (existing.FchUsu != null &&
                    (!string.Equals(existing.Sven, request.Transan.Sven) || !string.Equals(existing.Plant, request.Transan.Plant)))
                {
                    respuesta = new Respuesta<TransanDTO>
                    {
                        Exito = 0,
                        Mensaje = "No se puede cambiar el socio vendedor ni el plantel de origen de una transferencia ya procesada."
                    };
                    return BadRequest(respuesta);
                }

                var errores = await ValidateTransferRequestAsync(request, true);
                if (errores.Count > 0)
                {
                    respuesta = new Respuesta<TransanDTO> { Exito = 0, Mensaje = string.Join("<br/>", errores) };
                    return BadRequest(respuesta);
                }

                var strategy = db.Database.CreateExecutionStrategy();
                TransanTransferAudit? audit = null;
                await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await db.Database.BeginTransactionAsync();
                    _logger.LogInformation("Transan.Editar transacción abierta. TransanId={TransanId}", request.Transan.Id);

                    await RevertTransferChangesAsync(existing);
                    _logger.LogInformation("Transan.Editar stock anterior revertido. TransanId={TransanId}", existing.Id);
                    var impacto = await ApplyTransferChangesAsync(request.Transan);
                    var sellerPlant = await ResolveTransferPlantelAsync(request.Transan.PlantOrigenId, request.Transan.Plant, "de origen");
                    var buyerPlant = await ResolveTransferPlantelAsync(request.Transan.PlantDestinoId, request.Transan.NvoPla, "de destino");
                    ApplyTransferPlantelMetadata(existing, sellerPlant, buyerPlant);
                    _logger.LogInformation("Transan.Editar nuevo impacto calculado. TransanId={TransanId} Bucket={Bucket} OrigenAntes={OrigenAntes} OrigenDespues={OrigenDespues} DestinoAntes={DestinoAntes} DestinoDespues={DestinoDespues}",
                        request.Transan.Id,
                        impacto.FieldName,
                        impacto.OrigenAntes,
                        impacto.OrigenDespues,
                        impacto.DestinoAntes,
                        impacto.DestinoDespues);
                    ApplyTransferEntityChanges(existing, request.Transan);
                    db.Transans.Update(existing);
                    audit = BuildTransferAudit(
                        request.Transan,
                        accessContext,
                        "Edited",
                        impacto,
                        request.VendedorId,
                        request.CompradorId,
                        "Pendiente",
                        "Se revirtió el movimiento anterior y se aplicó el nuevo.");
                    db.TransanTransferAudits.Add(audit);
                    var writes1 = await db.SaveChangesAsync();
                    _logger.LogInformation("Transan.Editar primer SaveChanges completado. Writes={Writes} TransanId={TransanId} AuditId={AuditId}",
                        writes1,
                        existing.Id,
                        audit.Id);

                    await QueueTransferNotificationAsync(request, existing, true);
                    audit.MailEstado = "Pendiente";
                    audit.MailError = null;
                    db.TransanTransferAudits.Update(audit);
                    var writes2 = await db.SaveChangesAsync();
                    _logger.LogInformation("Transan.Editar outbox registrada. Writes={Writes} TransanId={TransanId} AuditId={AuditId}",
                        writes2,
                        existing.Id,
                        audit.Id);

                    await transaction.CommitAsync();
                    _logger.LogInformation("Transan.Editar commit completado. TransanId={TransanId}", existing.Id);
                });

                try
                {
                    LogTransferHistory(existing, "Edited");
                }
                catch (Exception lex)
                {
                    Console.WriteLine($"Failed to write history log: {lex.Message}");
                }

                respuesta = new Respuesta<TransanDTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<TransanDTO>(existing) };
                _logger.LogInformation("Transan.Editar finalizado correctamente. TransanId={TransanId}", existing.Id);
                return Ok(respuesta);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Transan.Editar falló por DbUpdateException. TransanId={TransanId}", request?.Transan?.Id);
                respuesta = new Respuesta<TransanDTO> { Exito = 0, Mensaje = BuildDbUpdateMessage(ex) };
                return BadRequest(respuesta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transan.Editar falló por Exception general. TransanId={TransanId}", request?.Transan?.Id);
                respuesta = new Respuesta<TransanDTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }

        [HttpGet]
        [Route("Auditoria/{transanId:int}")]
        public async Task<IActionResult> Auditoria(int transanId)
        {
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                var transfer = await db.Transans.AsNoTracking().FirstOrDefaultAsync(x => x.Id == transanId);

                if (transfer == null)
                {
                    return NotFound(new Respuesta<List<TransanTransferAudit>>
                    {
                        Exito = 0,
                        Mensaje = "No se encontró la transferencia solicitada."
                    });
                }

                if (!CanAccessTransfer(accessContext, transfer.Sven, transfer.Scom))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<TransanTransferAudit>>());
                }

                var audits = await db.TransanTransferAudits
                    .AsNoTracking()
                    .Where(x => x.TransanId == transanId)
                    .OrderByDescending(x => x.FechaEvento)
                    .ToListAsync();

                return Ok(new Respuesta<List<TransanTransferAudit>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = audits
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<List<TransanTransferAudit>>
                {
                    Exito = 0,
                    Mensaje = ex.Message
                });
            }
        }

        private void NormalizeTransfer(TransanDTO transan)
        {
            transan.NroCert = transan.NroCert?.Trim();
            transan.Plant = transan.Plant?.Trim();
            transan.NvoPla = transan.NvoPla?.Trim();
            transan.Sven = transan.Sven?.Trim();
            transan.Scom = transan.Scom?.Trim();
            transan.Vnom = transan.Vnom?.Trim();
            transan.Cnom = transan.Cnom?.Trim();
            transan.Tiphac = transan.Tiphac?.Trim();
            transan.Hemsta = transan.Hemsta?.Trim();
            transan.Tipani = transan.Tipani?.Trim();
            transan.Tipohem = transan.Tipohem?.Trim();
            transan.Incorp = 1;
        }

        private async Task<List<string>> ValidateTransferRequestAsync(TransanSaveRequestDTO request, bool isEdit)
        {
            var errores = new List<string>();
            var transan = request.Transan;

            if (transan == null)
            {
                errores.Add("No se recibió la transferencia.");
                return errores;
            }

            if (isEdit && transan.Id <= 0)
                errores.Add("La transferencia a editar no tiene un identificador válido.");

            if (transan.Fecvta == null)
                errores.Add("La fecha de venta es obligatoria.");

            if (request.VendedorId <= 0)
                errores.Add("Debés seleccionar un socio vendedor válido.");

            if (string.IsNullOrWhiteSpace(transan.Sven) || string.IsNullOrWhiteSpace(transan.Vnom))
                errores.Add("Faltan los datos del socio vendedor.");

            if (string.IsNullOrWhiteSpace(transan.Plant))
                errores.Add("Debés seleccionar un plantel de origen.");

            if (string.IsNullOrWhiteSpace(transan.NvoPla))
                errores.Add("Debés seleccionar un plantel de destino.");
            else if (transan.NvoPla.Length > 20)
                errores.Add("El plantel de destino supera el largo permitido (máx 20 caracteres).");

            if (!string.IsNullOrWhiteSpace(transan.Plant) && transan.Plant.Length > 20)
                errores.Add("El plantel de origen supera el largo permitido (máx 20 caracteres).");

            if (!string.IsNullOrWhiteSpace(transan.Plant) &&
                !string.IsNullOrWhiteSpace(transan.NvoPla) &&
                string.Equals(transan.Plant, transan.NvoPla, StringComparison.OrdinalIgnoreCase))
            {
                errores.Add("El plantel de origen y el plantel de destino no pueden ser el mismo.");
            }

            try
            {
                await ResolveTransferPlantelAsync(
                    request.PlantOrigenId ?? transan.PlantOrigenId,
                    transan.Plant,
                    "de origen");
            }
            catch (Exception ex)
            {
                errores.Add(ex.Message);
            }

            try
            {
                await ResolveTransferPlantelAsync(
                    request.PlantDestinoId ?? transan.PlantDestinoId,
                    transan.NvoPla,
                    "de destino");
            }
            catch (Exception ex)
            {
                errores.Add(ex.Message);
            }

            var cantidades = new[]
            {
                transan.CantHem ?? 0,
                transan.CantMach ?? 0,
                transan.CantChem ?? 0,
                transan.CantCmach ?? 0
            };
            var totalHembras = (transan.CantHem ?? 0) + (transan.CantChem ?? 0);

            if (cantidades.Any(x => x < 0))
                errores.Add("Las cantidades no pueden ser negativas.");

            if (cantidades.Sum() == 0)
                errores.Add("Debés ingresar al menos un animal para transferir.");

            if (string.IsNullOrWhiteSpace(transan.Tiphac))
                errores.Add("Debés seleccionar un tipo de hacienda.");

            if (string.IsNullOrWhiteSpace(transan.Tipani))
                errores.Add("Debés seleccionar el sexo de los animales.");

            if (totalHembras > 0)
            {
                var fieldName = TransferenciaAnimalImpactoHelper.ResolveFieldName(transan.Tiphac, transan.Hemsta, transan.Tipohem, out var fieldError);
                if (string.IsNullOrWhiteSpace(fieldName))
                    errores.Add(fieldError ?? "No se pudo determinar la categoría de hembras a transferir.");
            }

            if (!string.IsNullOrWhiteSpace(request.MailComprador) && !IsValidEmail(request.MailComprador))
                errores.Add("El mail del comprador no tiene un formato válido.");

            if (request.VendedorId > 0)
            {
                var vendedor = await db.Socios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.VendedorId);
                if (vendedor == null)
                {
                    errores.Add("El socio vendedor seleccionado no existe.");
                }
                else if (!string.Equals(vendedor.Scod, transan.Sven, StringComparison.OrdinalIgnoreCase))
                {
                    errores.Add("El socio vendedor seleccionado no coincide con la transferencia.");
                }
            }

            if (request.CompradorId.HasValue && request.CompradorId.Value > 0)
            {
                var comprador = await db.Socios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.CompradorId.Value);
                if (comprador == null)
                {
                    errores.Add("El socio comprador seleccionado no existe.");
                }
                else if (!string.Equals(comprador.Scod, transan.Scom, StringComparison.OrdinalIgnoreCase))
                {
                    errores.Add("El socio comprador seleccionado no coincide con la transferencia.");
                }
            }

            return errores;
        }

        private static string BuildDbUpdateMessage(DbUpdateException ex)
        {
            var innerMessage = ex.InnerException?.Message ?? ex.Message;

            if (innerMessage.Contains("TRANSAN_TRANSFER_AUDITS", StringComparison.OrdinalIgnoreCase) &&
                (innerMessage.Contains("doesn't exist", StringComparison.OrdinalIgnoreCase) ||
                 innerMessage.Contains("does not exist", StringComparison.OrdinalIgnoreCase) ||
                 innerMessage.Contains("doesn't exists", StringComparison.OrdinalIgnoreCase)))
            {
                return "Falta la tabla de auditoría de transferencias en la base de datos. Ejecutá el script TRANSAN_TRANSFER_AUDITS antes de volver a intentar.";
            }

            if (innerMessage.Contains("TRANSAN_MAIL_OUTBOX", StringComparison.OrdinalIgnoreCase) &&
                (innerMessage.Contains("doesn't exist", StringComparison.OrdinalIgnoreCase) ||
                 innerMessage.Contains("does not exist", StringComparison.OrdinalIgnoreCase) ||
                 innerMessage.Contains("doesn't exists", StringComparison.OrdinalIgnoreCase)))
            {
                return "Falta la tabla de cola de mails de transferencias en la base de datos. Ejecutá el script TRANSAN_MAIL_OUTBOX antes de volver a intentar.";
            }

            if (innerMessage.Contains("Data too long for column 'NVO_PLA'", StringComparison.OrdinalIgnoreCase))
            {
                return "El plantel de destino supera el largo permitido (máx 20 caracteres).";
            }

            if (innerMessage.Contains("Data too long for column 'PLANT'", StringComparison.OrdinalIgnoreCase))
            {
                return "El plantel de origen supera el largo permitido (máx 20 caracteres).";
            }

            if (innerMessage.Contains("bucket impactado", StringComparison.OrdinalIgnoreCase))
            {
                return "Faltan datos para determinar el bucket impactado. Revisá el tipo de hacienda, el estado de la hembra y el tipo de hembra.";
            }

            return "No se pudo guardar la transferencia por un error de datos. Revisá los campos ingresados.";
        }

        private TransanTransferAudit BuildTransferAudit(
            TransanDTO transan,
            UserSocioAccessContext accessContext,
            string accion,
            TransferenciaAnimalImpacto? impacto,
            int? vendedorId,
            int? compradorId,
            string mailEstado,
            string? detalle)
        {
            return new TransanTransferAudit
            {
                TransanId = transan.Id,
                Accion = accion,
                FechaEvento = DateTime.Now,
                UsuarioId = accessContext.CurrentUser?.Id,
                UsuarioNombre = accessContext.CurrentUser is null
                    ? null
                    : $"{accessContext.CurrentUser.Names} {accessContext.CurrentUser.LastNames}".Trim(),
                UsuarioRol = accessContext.CurrentUser?.Rol,
                VendedorId = vendedorId,
                CompradorId = compradorId,
                PlantelOrigenId = transan.PlantOrigenId,
                PlantelDestinoId = transan.PlantDestinoId,
                VendedorCodigo = transan.Sven,
                CompradorCodigo = transan.Scom,
                PlantelOrigen = FormatPlantSnapshot(transan.PlantOrigenCodigo, transan.PlantOrigenAnioex, transan.Plant),
                PlantelDestino = FormatPlantSnapshot(transan.PlantDestinoCodigo, transan.PlantDestinoAnioex, transan.NvoPla),
                PlantelOrigenCodigo = transan.PlantOrigenCodigo,
                PlantelOrigenAnioex = transan.PlantOrigenAnioex,
                PlantelDestinoCodigo = transan.PlantDestinoCodigo,
                PlantelDestinoAnioex = transan.PlantDestinoAnioex,
                BucketCampo = impacto?.FieldName,
                BucketEtiqueta = impacto?.BucketLabel,
                Tiphac = transan.Tiphac,
                Hemsta = transan.Hemsta,
                Tipohem = transan.Tipohem,
                CantHem = transan.CantHem,
                CantMach = transan.CantMach,
                CantChem = transan.CantChem,
                CantCmach = transan.CantCmach,
                OrigenAntes = impacto?.OrigenAntes,
                OrigenDespues = impacto?.OrigenDespues,
                DestinoAntes = impacto?.DestinoAntes,
                DestinoDespues = impacto?.DestinoDespues,
                MailEstado = mailEstado,
                Detalle = detalle
            };
        }

        private TransanTransferAudit BuildTransferAudit(
            Transan transan,
            UserSocioAccessContext accessContext,
            string accion,
            TransferenciaAnimalImpacto? impacto,
            string mailEstado,
            string? detalle)
        {
            return BuildTransferAudit(_mapper.Map<TransanDTO>(transan), accessContext, accion, impacto, null, null, mailEstado, detalle);
        }

        private static string? FormatPlantSnapshot(string? codigo, string? anioex, string? fallbackCodigo)
        {
            var code = !string.IsNullOrWhiteSpace(codigo) ? codigo : fallbackCodigo;
            if (string.IsNullOrWhiteSpace(code))
                return null;

            return string.IsNullOrWhiteSpace(anioex)
                ? code
                : $"{code} - {anioex}";
        }

        private bool ShouldSendTransferEmails()
        {
            return true;
        }

        private async Task<TransferenciaAnimalImpacto> ApplyTransferChangesAsync(TransanDTO request)
        {
            var sellerPlant = await ResolveTransferPlantelAsync(request.PlantOrigenId, request.Plant, "de origen");
            var buyerPlant = await ResolveTransferPlantelAsync(request.PlantDestinoId, request.NvoPla, "de destino");

            var sellerSnapshot = _mapper.Map<PlantelDTO>(sellerPlant);
            var buyerSnapshot = _mapper.Map<PlantelDTO>(buyerPlant);

            if (!TransferenciaAnimalImpactoHelper.TryBuildImpacto(request, sellerSnapshot, buyerSnapshot, out var impacto, out var error))
                throw new Exception(error ?? "No se pudo determinar el impacto de la transferencia.");

            if (impacto == null)
                throw new Exception("No se pudo construir el impacto de la transferencia.");

            var cantHem = request.CantHem ?? 0;
            _logger.LogInformation("Transan.Impacto calculado. PlantelOrigenId={OrigenId} PlantelDestinoId={DestinoId} Campo={Campo} Cantidad={Cantidad} OrigenAntes={OrigenAntes} DestinoAntes={DestinoAntes}",
                sellerPlant.Id,
                buyerPlant.Id,
                impacto.FieldName,
                cantHem,
                impacto.OrigenAntes,
                impacto.DestinoAntes);

            if (cantHem > 0)
            {
                var sellerValue = TransferenciaAnimalImpactoHelper.GetPlantelFieldValue(sellerSnapshot, impacto.FieldName);
                if (sellerValue < cantHem)
                {
                    throw new Exception($"El plantel de origen no tiene stock suficiente para transferir {cantHem} hembras. Disponible: {sellerValue}.");
                }

                // Solo la cantidad de hembras adultas mueve stock entre planteles.
                SetPlantelFieldValue(sellerPlant, impacto.FieldName, sellerValue - cantHem);
                sellerPlant.FchUsu = DateTime.Now;
                db.Planteles.Update(sellerPlant);
                _logger.LogInformation("Transan.Impacto origen aplicado. PlantelId={PlantelId} Campo={Campo} Antes={Antes} Despues={Despues}",
                    sellerPlant.Id, impacto.FieldName, sellerValue, sellerValue - cantHem);

                var buyerValue = TransferenciaAnimalImpactoHelper.GetPlantelFieldValue(buyerSnapshot, impacto.FieldName);
                SetPlantelFieldValue(buyerPlant, impacto.FieldName, buyerValue + cantHem);
                buyerPlant.FchUsu = DateTime.Now;
                db.Planteles.Update(buyerPlant);
                _logger.LogInformation("Transan.Impacto destino aplicado. PlantelId={PlantelId} Campo={Campo} Antes={Antes} Despues={Despues}",
                    buyerPlant.Id, impacto.FieldName, buyerValue, buyerValue + cantHem);

                var writes = await db.SaveChangesAsync();
                _logger.LogInformation("Transan.Impacto SaveChanges completado. Writes={Writes} OrigenPlantelId={OrigenPlantelId} DestinoPlantelId={DestinoPlantelId}",
                    writes, sellerPlant.Id, buyerPlant.Id);
            }

            return impacto;
        }

        private async Task RevertTransferChangesAsync(Transan existing)
        {
            var prevCantHem = existing.CantHem ?? 0;
            if (prevCantHem <= 0)
                return;

            var sellerPlant = await ResolveTransferPlantelAsync(existing.PlantOrigenId, existing.Plant, "de origen");
            var buyerPlant = !string.IsNullOrWhiteSpace(existing.NvoPla) || existing.PlantDestinoId.HasValue
                ? await ResolveTransferPlantelAsync(existing.PlantDestinoId, existing.NvoPla, "de destino")
                : null;

            var field = TransferenciaAnimalImpactoHelper.ResolveFieldName(existing.Tiphac, existing.Hemsta, existing.Tipohem, out var error);
            if (string.IsNullOrWhiteSpace(field))
                throw new Exception(error ?? "No se pudo revertir la transferencia anterior porque su categoría de hembras es inválida.");

            _logger.LogInformation("Transan.Revertir impacto anterior. TransanId={TransanId} Campo={Campo} Cantidad={Cantidad} OrigenPlantelId={OrigenPlantelId} DestinoPlantelId={DestinoPlantelId}",
                existing.Id,
                field,
                prevCantHem,
                sellerPlant.Id,
                buyerPlant?.Id);

            var sellerValue = TransferenciaAnimalImpactoHelper.GetPlantelFieldValue(_mapper.Map<PlantelDTO>(sellerPlant), field);
            SetPlantelFieldValue(sellerPlant, field, sellerValue + prevCantHem);
            sellerPlant.FchUsu = DateTime.Now;
            db.Planteles.Update(sellerPlant);
            _logger.LogInformation("Transan.Revertir origen aplicado. PlantelId={PlantelId} Campo={Campo} Antes={Antes} Despues={Despues}",
                sellerPlant.Id, field, sellerValue, sellerValue + prevCantHem);

            if (buyerPlant != null)
            {
                var buyerValue = TransferenciaAnimalImpactoHelper.GetPlantelFieldValue(_mapper.Map<PlantelDTO>(buyerPlant), field);
                SetPlantelFieldValue(buyerPlant, field, Math.Max(0, buyerValue - prevCantHem));
                buyerPlant.FchUsu = DateTime.Now;
                db.Planteles.Update(buyerPlant);
                _logger.LogInformation("Transan.Revertir destino aplicado. PlantelId={PlantelId} Campo={Campo} Antes={Antes} Despues={Despues}",
                    buyerPlant.Id, field, buyerValue, Math.Max(0, buyerValue - prevCantHem));
            }

            var writes = await db.SaveChangesAsync();
            _logger.LogInformation("Transan.Revertir SaveChanges completado. Writes={Writes} TransanId={TransanId}", writes, existing.Id);
        }

        private async Task<Plantel> ResolveTransferPlantelAsync(int? plantelId, string? plantelCodigo, string descripcion)
        {
            if (plantelId.HasValue && plantelId.Value > 0)
            {
                var plantel = await db.Planteles.FirstOrDefaultAsync(p => p.Id == plantelId.Value);
                if (plantel == null)
                    throw new Exception($"No se encontró el plantel {descripcion} seleccionado.");

                if (!string.IsNullOrWhiteSpace(plantelCodigo) &&
                    !string.Equals(plantel.Placod?.Trim(), plantelCodigo.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception($"El plantel {descripcion} seleccionado no coincide con el código informado.");
                }

                _logger.LogInformation("Transan.Plantel resuelto por Id. Descripcion={Descripcion} PlantelId={PlantelId} Codigo={Codigo}",
                    descripcion, plantel.Id, plantel.Placod);
                return plantel;
            }

            if (string.IsNullOrWhiteSpace(plantelCodigo))
                throw new Exception($"Debés seleccionar el plantel {descripcion}.");

            var matches = await db.Planteles.Where(p => p.Placod == plantelCodigo).ToListAsync();
            if (matches.Count == 0)
                throw new Exception($"No se encontró el plantel {descripcion} seleccionado.");

            if (matches.Count > 1)
                throw new Exception($"El plantel {descripcion} está duplicado en la base de datos. Debe corregirse antes de guardar la transferencia.");

            _logger.LogInformation("Transan.Plantel resuelto por código. Descripcion={Descripcion} PlantelId={PlantelId} Codigo={Codigo}",
                descripcion, matches[0].Id, matches[0].Placod);
            return matches[0];
        }

        private void ApplyTransferEntityChanges(Transan existing, TransanDTO request)
        {
            existing.NroCert = request.NroCert;
            existing.Fecvta = request.Fecvta;
            existing.Sven = request.Sven;
            existing.CategSv = request.CategSv;
            existing.Vnom = request.Vnom;
            existing.Scom = request.Scom;
            existing.CategSc = request.CategSc;
            existing.Cnom = request.Cnom;
            existing.Plant = request.Plant;
            existing.NvoPla = request.NvoPla;
            existing.PlantOrigenId = request.PlantOrigenId;
            existing.PlantDestinoId = request.PlantDestinoId;
            existing.CantHem = request.CantHem;
            existing.CantMach = request.CantMach;
            existing.Tiphac = request.Tiphac;
            existing.Hemsta = request.Hemsta;
            existing.Tipani = request.Tipani;
            existing.Incorp = 1;
            existing.Tipohem = request.Tipohem;
            existing.CantChem = request.CantChem;
            existing.CantCmach = request.CantCmach;
            existing.FchUsu = request.FchUsu;
            existing.CodUsu = request.CodUsu;
        }

        private static void ApplyTransferPlantelMetadata(Transan target, Plantel sellerPlant, Plantel buyerPlant)
        {
            // Guardamos un snapshot mínimo del plantel asociado para que la transferencia
            // conserve el código y el año aunque luego cambie el registro original.
            target.PlantOrigenCodigo = sellerPlant.Placod;
            target.PlantOrigenAnioex = sellerPlant.Anioex;
            target.PlantDestinoCodigo = buyerPlant.Placod;
            target.PlantDestinoAnioex = buyerPlant.Anioex;
        }

        private async Task QueueTransferNotificationAsync(TransanSaveRequestDTO request, Transan transan, bool isEdit)
        {
            var mailVendedor = await ResolveEmailAsync(request.VendedorId);
            var mailComprador = !string.IsNullOrWhiteSpace(request.MailComprador) && IsValidEmail(request.MailComprador)
                ? request.MailComprador
                : await ResolveEmailAsync(request.CompradorId);

            var destinatarios = new List<string> { "planteles@hereford.org.ar" };
            if (IsValidEmail(mailVendedor))
                destinatarios.Add(mailVendedor!);
            if (IsValidEmail(mailComprador))
                destinatarios.Add(mailComprador!);

            var outbox = new TransanMailOutbox
            {
                TransanId = transan.Id,
                Accion = isEdit ? "Edited" : "Created",
                Asunto = isEdit
                    ? $"El socio {transan.Vnom ?? "N/D"} ha modificado una transferencia"
                    : "Nueva transferencia animal",
                CuerpoHtml = BuildTransferEmailHtml(_mapper.Map<TransanDTO>(transan), isEdit),
                Destinatarios = string.Join(";", destinatarios.Distinct(StringComparer.OrdinalIgnoreCase)),
                MailVendedor = mailVendedor,
                MailComprador = mailComprador,
                Estado = "Pendiente",
                Intentos = 0,
                FechaCreacion = DateTime.UtcNow,
                PlantelOrigenId = transan.PlantOrigenId,
                PlantelDestinoId = transan.PlantDestinoId,
                PlantelOrigenCodigo = transan.Plant,
                PlantelDestinoCodigo = transan.NvoPla
            };

            db.TransanMailOutboxes.Add(outbox);
            _logger.LogInformation("Transan.Outbox creada. TransanId={TransanId} Estado={Estado} Destinatarios={Destinatarios} PlantelOrigenId={OrigenId} PlantelDestinoId={DestinoId}",
                transan.Id,
                outbox.Estado,
                outbox.Destinatarios,
                outbox.PlantelOrigenId,
                outbox.PlantelDestinoId);
        }

        private async Task<string?> ResolveEmailAsync(int? socioId)
        {
            if (!socioId.HasValue || socioId.Value <= 0)
                return null;

            var socio = await db.Socios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == socioId.Value);
            if (socio != null && IsValidEmail(socio.Mail))
                return socio.Mail;

            return null;
        }

        private string BuildTransferEmailHtml(TransanDTO transan, bool isEdit)
        {
            var total = (transan.CantHem ?? 0) + (transan.CantMach ?? 0) + (transan.CantChem ?? 0) + (transan.CantCmach ?? 0);
            var titulo = isEdit ? "Transferencia animal modificada" : "Nueva transferencia animal";
            var subtitulo = isEdit
                ? "Se registraron cambios en la siguiente transferencia."
                : "Se registró la siguiente transferencia en el sistema.";

            return $@"
<!DOCTYPE html>
<html lang=""es"">
<head>
  <meta charset=""utf-8"" />
  <title>{titulo}</title>
</head>
<body style=""margin:0;padding:24px;background-color:#f3f5f7;font-family:Segoe UI, Arial, sans-serif;color:#1f2937;"">
  <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"" style=""max-width:760px;margin:0 auto;background:#ffffff;border:1px solid #dfe3e8;border-radius:12px;overflow:hidden;"">
    <tr>
      <td style=""background:#1f5f3b;color:#ffffff;padding:22px 28px;"">
        <div style=""font-size:24px;font-weight:700;"">Asociacion Criadores Hereford</div>
        <div style=""font-size:18px;margin-top:6px;"">{titulo}</div>
      </td>
    </tr>
    <tr>
      <td style=""padding:24px 28px 10px 28px;font-size:15px;line-height:1.5;"">
        <p style=""margin:0 0 14px 0;"">{subtitulo}</p>
        <p style=""margin:0;color:#4b5563;"">Este correo puede conservarse como constancia de la operación informada.</p>
      </td>
    </tr>
    <tr>
      <td style=""padding:10px 28px 6px 28px;"">
        <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"" style=""border-collapse:collapse;"">
          {BuildEmailRow("Nro. Certificado", string.IsNullOrWhiteSpace(transan.NroCert) ? "-" : transan.NroCert)}
          {BuildEmailRow("Fecha de venta", transan.Fecvta?.ToString("dd/MM/yyyy") ?? "-")}
          {BuildEmailRow("Socio vendedor", transan.Vnom)}
          {BuildEmailRow("Plantel origen", transan.Plant)}
          {BuildEmailRow("Socio comprador", string.IsNullOrWhiteSpace(transan.Cnom) ? "(No informado)" : transan.Cnom)}
          {BuildEmailRow("Plantel destino", transan.NvoPla)}
          {BuildEmailRow("Tipo de hacienda", FormatTiphac(transan.Tiphac))}
          {BuildEmailRow("Sexo de los animales", FormatTipani(transan.Tipani))}
          {BuildEmailRow("Estado de las hembras", FormatHemsta(transan.Hemsta))}
          {BuildEmailRow("Tipo de hembras", string.IsNullOrWhiteSpace(transan.Tipohem) ? "-" : transan.Tipohem)}
          {BuildEmailRow("Hembras", (transan.CantHem ?? 0).ToString())}
          {BuildEmailRow("Machos", (transan.CantMach ?? 0).ToString())}
          {BuildEmailRow("Crias hembras", (transan.CantChem ?? 0).ToString())}
          {BuildEmailRow("Crias machos", (transan.CantCmach ?? 0).ToString())}
          {BuildEmailRow("Cantidad total", total.ToString())}
          {BuildEmailRow("Animales incorporados", "SI")}
        </table>
      </td>
    </tr>
    <tr>
      <td style=""padding:20px 28px 28px 28px;color:#6b7280;font-size:13px;border-top:1px solid #e5e7eb;"">
        Mensaje automatico generado por el sistema de transferencias de Hereford.
      </td>
    </tr>
  </table>
</body>
</html>";
        }

        private string BuildEmailRow(string label, string? value)
        {
            var safeLabel = System.Net.WebUtility.HtmlEncode(label);
            var safeValue = System.Net.WebUtility.HtmlEncode(string.IsNullOrWhiteSpace(value) ? "-" : value);

            return $@"
<tr>
  <td style=""padding:10px 12px;border-bottom:1px solid #eef2f7;background:#f9fafb;font-weight:600;width:38%;"">{safeLabel}</td>
  <td style=""padding:10px 12px;border-bottom:1px solid #eef2f7;"">{safeValue}</td>
</tr>";
        }

        private string FormatTiphac(string? value)
        {
            return value?.Trim().ToUpperInvariant() switch
            {
                "PHPR" => "PHPR (Puro Registrado)",
                "HPR" => "HPR (Puro Registrado)",
                "PHVIP" => "PHVIP (VIP)",
                "HVIP" => "HVIP (VIP)",
                _ => string.IsNullOrWhiteSpace(value) ? "-" : value
            };
        }

        private string FormatTipani(string? value)
        {
            return value?.Trim().ToUpperInvariant() switch
            {
                "H" => "Hembras",
                "M" => "Machos",
                _ => string.IsNullOrWhiteSpace(value) ? "-" : value
            };
        }

        private string FormatHemsta(string? value)
        {
            return value?.Trim().ToUpperInvariant() switch
            {
                "CC" => "Con Cria (CC)",
                "CCP" => "Con Cria y Prenada (CCP)",
                "SS" => "Sin Servicio (SS)",
                "PR" => "Prenada (PR)",
                _ => string.IsNullOrWhiteSpace(value) ? "-" : value
            };
        }

        // Write transfer history to a local JSON-lines log file (non-DB)
        private void LogTransferHistory(Transan transan, string action)
        {
            try
            {
                var historyDir = Path.Combine(_env.ContentRootPath, "Logs");
                if (!Directory.Exists(historyDir)) Directory.CreateDirectory(historyDir);

                var file = Path.Combine(historyDir, "transan_history.log");

                var entry = new
                {
                    Timestamp = DateTime.UtcNow,
                    Action = action,
                    Id = transan?.Id,
                    NroCert = transan?.NroCert,
                    Fecha = transan?.Fecvta,
                    Seller = transan?.Sven,
                    Buyer = transan?.Scom,
                    PlantOrigen = transan?.Plant,
                    PlantDestino = transan?.NvoPla,
                    PlantOrigenId = transan?.PlantOrigenId,
                    PlantDestinoId = transan?.PlantDestinoId,
                    CantHem = transan?.CantHem,
                    CantMach = transan?.CantMach,
                    CantChem = transan?.CantChem,
                    CantCmach = transan?.CantCmach,
                    Tiphac = transan?.Tiphac,
                    Hemsta = transan?.Hemsta,
                    Tipohem = transan?.Tipohem,
                    FchUsu = transan?.FchUsu,
                    CodUsu = transan?.CodUsu
                };

                var json = JsonSerializer.Serialize(entry);
                System.IO.File.AppendAllText(file, json + Environment.NewLine);
            }
            catch
            {
                // don't fail on logging
            }
        }

        private void SetPlantelFieldValue(Plantel plantel, string field, double value)
        {
            switch (field)
            {
                case "Varede":
                    plantel.Varede = value;
                    break;
                case "Vqcsrd":
                    plantel.Vqcsrd = value;
                    break;
                case "Vqssrd":
                    plantel.Vqssrd = value;
                    break;
                case "Varepr":
                    plantel.Varepr = value;
                    break;
                case "Vqcsrp":
                    plantel.Vqcsrp = value;
                    break;
                case "Vqssrp":
                    plantel.Vqssrp = value;
                    break;
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

        private static bool CanAccessTransfer(UserSocioAccessContext accessContext, int? sellerId, int? buyerId)
        {
            if (!RequiresActiveSocioScope(accessContext))
            {
                return true;
            }

            if (!accessContext.ActiveSocioId.HasValue)
            {
                return false;
            }

            return sellerId == accessContext.ActiveSocioId.Value || buyerId == accessContext.ActiveSocioId.Value;
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

        public class ContenidoMails2
        {
            public int Tipo { get; set; }
            public int Clase { get; set; }
            public string Mail { get; set; }
            public string? Nombre { get; set; }

            public string? MailCompra {  get; set; }

            public string MailVendedor { get; set; }

        }
        public class ContenidoMails
        {
            public int Vendedor { get; set; }
            public int Comprador { get; set; }
            public string Mail { get; set; }
            public string? Direccion { get; set; }

        }
    }
}
