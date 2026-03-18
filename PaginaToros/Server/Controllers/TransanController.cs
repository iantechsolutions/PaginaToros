using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Client.Pages.Socios;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using System.IO;
using Microsoft.EntityFrameworkCore;

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
        public TransanController(hereford_prContext db, ITransanRepositorio TransanRepositorio, IMapper mapper, IWebHostEnvironment env)
        {
            this.db = db;
            _mapper = mapper;
            _TransanRepositorio = TransanRepositorio;
            _env = env;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<TransanDTO>> _ResponseDTO = new Respuesta<List<TransanDTO>>();

            try
            {
                List<TransanDTO> listaPedido = new List<TransanDTO>();
                var a = await _TransanRepositorio.Lista(skip, take);


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
                var a = await _TransanRepositorio.CantidadTotal();

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
                var a = await _TransanRepositorio.LimitadosFiltrados(skip, take, expression);

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
        public IActionResult ExampleMethod([FromBody] ContenidoMails request)
        {
            using (MailMessage mail = new MailMessage())
            {

                Console.Write("Envio de mail");

                try
                {
                    // Obtener correo del socio vendedor
                    var socioVendedor = db.User.FirstOrDefault(x => x.SocioId == request.Vendedor);
                    if (socioVendedor != null && IsValidEmail(socioVendedor.Email))
                    {
                        mail.To.Add(socioVendedor.Email); // Se agrega el correo del vendedor si es válido
                    }
                    else
                    {
                        Console.WriteLine("Socio vendedor sin cuenta o correo inválido");
                    }


                    // Obtener correo del socio comprador o verificar si hay una dirección
                    if (string.IsNullOrEmpty(request.Direccion))
                    {
                        var socioComprador = db.User.FirstOrDefault(x => x.SocioId == request.Comprador);
                        if (socioComprador != null && IsValidEmail(socioComprador.Email))
                        {
                            mail.To.Add(socioComprador.Email); // Se agrega el correo del comprador si es válido
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
                    mail.From = new MailAddress(correoPuroRegistrado);
                    mail.Subject = "Nueva transferencia animal";
                    mail.Body = request.Mail;

                    // Configurar SMTP y enviar
                    using (SmtpClient smtp = new SmtpClient("mail.hereford.org.ar", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential(correoPuroRegistrado, "Hereford.2033"); // Agrega tu contraseña aquí
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
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
                    string correoPuroRegistrado = "planteles@hereford.org.ar";
                    string smtpHost = "mail.hereford.org.ar";
                    int smtpPort = 587;
                    string smtpUsername = "planteles@hereford.org.ar";
                    string smtpPassword = "Hereford.2033"; // ideal: usar variable de entorno

                    // Siempre agregar planteles
                    TryAddRecipient(mail, correoPuroRegistrado);

                    // Vendedor: si vino MailVendedor y es válido, usarlo. Si no, resolver por SocioId (Tipo)
                    if (!string.IsNullOrWhiteSpace(request.MailVendedor) && IsValidEmail(request.MailVendedor))
                    {
                        TryAddRecipient(mail, request.MailVendedor);
                    }
                    else if (request.Tipo > 0)
                    {
                        var socioVendedor = db.User.FirstOrDefault(x => x.SocioId == request.Tipo);
                        if (socioVendedor != null && IsValidEmail(socioVendedor.Email))
                            TryAddRecipient(mail, socioVendedor.Email);
                        else
                            Console.WriteLine("No se pudo resolver mail del vendedor.");
                    }

                    // Comprador: idem con MailCompra o SocioId (Clase)
                    if (!string.IsNullOrWhiteSpace(request.MailCompra) && IsValidEmail(request.MailCompra))
                    {
                        TryAddRecipient(mail, request.MailCompra);
                    }
                    else if (request.Clase > 0)
                    {
                        var socioComprador = db.User.FirstOrDefault(x => x.SocioId == request.Clase);
                        if (socioComprador != null && IsValidEmail(socioComprador.Email))
                            TryAddRecipient(mail, socioComprador.Email);
                        else
                            Console.WriteLine("No se pudo resolver mail del comprador.");
                    }

                    // Mensaje
                    mail.From = new MailAddress(correoPuroRegistrado);
                    mail.Subject = "El socio " + (request.Nombre ?? "N/D") + " ha modificado una transferencia";
                    mail.Body = request.Mail ?? "(sin cuerpo)";

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
                // Load existing transfer using DbContext
                var existing = await db.Transans.FirstOrDefaultAsync(u => u.Id == id);
                if (existing == null)
                {
                    _Respuesta = new Respuesta<string>() { Exito = 0, Mensaje = "No se encontró el identificador" };
                    return StatusCode(StatusCodes.Status404NotFound, _Respuesta);
                }

                var strategy = db.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await db.Database.BeginTransactionAsync();

                        // Revert hembras: add back to seller
                        var prevCantHem = existing.CantHem ?? 0;
                        if (prevCantHem > 0)
                        {
                            var seller = await db.Planteles.FirstOrDefaultAsync(p => p.Placod == existing.Plant);
                            if (seller != null)
                            {
                                var field = GetHembraField(existing.Tiphac, existing.Hemsta);
                                if (!string.IsNullOrEmpty(field))
                                {
                                    double val = GetPlantelFieldValue(seller, field);
                                    SetPlantelFieldValue(seller, field, val + prevCantHem);
                                    seller.FchUsu = DateTime.Now;
                                    db.Planteles.Update(seller);
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(existing.NvoPla))
                            {
                                var buyer = await db.Planteles.FirstOrDefaultAsync(p => p.Placod == existing.NvoPla);
                                if (buyer != null)
                                {
                                    var field = GetHembraField(existing.Tiphac, existing.Hemsta);
                                    if (!string.IsNullOrEmpty(field))
                                    {
                                        double val = GetPlantelFieldValue(buyer, field);
                                        double newVal = Math.Max(0, val - prevCantHem);
                                        SetPlantelFieldValue(buyer, field, newVal);
                                        buyer.FchUsu = DateTime.Now;
                                        db.Planteles.Update(buyer);
                                    }
                                }
                            }

                            await db.SaveChangesAsync();
                        }

                        // Remove transfer record
                        db.Transans.Remove(existing);
                        await db.SaveChangesAsync();

                        await transaction.CommitAsync();
                });

                // Log history
                try
                {
                    LogTransferHistory(existing, "Deleted");
                }
                catch { }

                _Respuesta = new Respuesta<string>() { Exito = 1, Mensaje = "ok", List = "" };
             
             
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
        public async Task<IActionResult> Guardar([FromBody] TransanSaveRequestDTO request)
        {
            var respuesta = new Respuesta<TransanDTO>();
            try
            {
                if (request?.Transan == null)
                {
                    respuesta = new Respuesta<TransanDTO> { Exito = 0, Mensaje = "No se recibió la transferencia a guardar." };
                    return BadRequest(respuesta);
                }

                NormalizeTransfer(request.Transan);

                var errores = await ValidateTransferRequestAsync(request, false);
                if (errores.Count > 0)
                {
                    respuesta = new Respuesta<TransanDTO> { Exito = 0, Mensaje = string.Join("<br/>", errores) };
                    return BadRequest(respuesta);
                }

                var transan = _mapper.Map<Transan>(request.Transan);
                var strategy = db.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await db.Database.BeginTransactionAsync();

                    await ApplyTransferChangesAsync(request.Transan);
                    db.Transans.Add(transan);
                    await db.SaveChangesAsync();
                    await SendTransferNotificationAsync(request, false);

                    await transaction.CommitAsync();
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
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
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

                NormalizeTransfer(request.Transan);

                var existing = await db.Transans.FirstOrDefaultAsync(u => u.Id == request.Transan.Id);
                if (existing == null)
                {
                    respuesta = new Respuesta<TransanDTO> { Exito = 0, Mensaje = "No se encontró la transferencia indicada." };
                    return NotFound(respuesta);
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
                await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await db.Database.BeginTransactionAsync();

                    await RevertTransferChangesAsync(existing);
                    await ApplyTransferChangesAsync(request.Transan);
                    ApplyTransferEntityChanges(existing, request.Transan);
                    db.Transans.Update(existing);
                    await db.SaveChangesAsync();
                    await SendTransferNotificationAsync(request, true);

                    await transaction.CommitAsync();
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
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta = new Respuesta<TransanDTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
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

            if (!string.IsNullOrWhiteSpace(transan.Plant) &&
                !string.IsNullOrWhiteSpace(transan.NvoPla) &&
                string.Equals(transan.Plant, transan.NvoPla, StringComparison.OrdinalIgnoreCase))
            {
                errores.Add("El plantel de origen y el plantel de destino no pueden ser el mismo.");
            }

            if (string.IsNullOrWhiteSpace(transan.Tiphac))
                errores.Add("Debés seleccionar un tipo de hacienda.");

            if (string.IsNullOrWhiteSpace(transan.Tipani))
                errores.Add("Debés seleccionar el sexo de los animales.");

            if (string.IsNullOrWhiteSpace(transan.Hemsta))
                errores.Add("Debés seleccionar el estado de las hembras.");

            if (string.IsNullOrWhiteSpace(transan.Tipohem))
                errores.Add("Debés seleccionar el tipo de hembras.");

            var cantidades = new[]
            {
                transan.CantHem ?? 0,
                transan.CantMach ?? 0,
                transan.CantChem ?? 0,
                transan.CantCmach ?? 0
            };
            var totalHembras = (transan.CantHem ?? 0) + (transan.CantChem ?? 0);
            var totalMachos = (transan.CantMach ?? 0) + (transan.CantCmach ?? 0);

            if (cantidades.Any(x => x < 0))
                errores.Add("Las cantidades no pueden ser negativas.");

            if (cantidades.Sum() == 0)
                errores.Add("Debés ingresar al menos un animal para transferir.");

            if (totalHembras > 0)
            {
                if (string.IsNullOrWhiteSpace(transan.Hemsta))
                    errores.Add("Debés informar el estado de las hembras cuando la transferencia incluye hembras.");

                if (string.IsNullOrWhiteSpace(transan.Tipohem))
                    errores.Add("Debés informar el tipo de hembras cuando la transferencia incluye hembras.");
            }

            if (!string.IsNullOrWhiteSpace(request.MailComprador) && !IsValidEmail(request.MailComprador))
                errores.Add("El mail del comprador no tiene un formato válido.");

            if (!string.IsNullOrWhiteSpace(transan.Plant))
            {
                var plantelOrigen = await db.Planteles.AsNoTracking().FirstOrDefaultAsync(p => p.Placod == transan.Plant);
                if (plantelOrigen == null)
                    errores.Add("El plantel de origen seleccionado no existe.");
            }

            if (!string.IsNullOrWhiteSpace(transan.NvoPla))
            {
                var plantelDestino = await db.Planteles.AsNoTracking().FirstOrDefaultAsync(p => p.Placod == transan.NvoPla);
                if (plantelDestino == null)
                    errores.Add("El plantel de destino seleccionado no existe.");
            }

            if (request.VendedorId > 0)
            {
                var vendedor = await db.Socios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.VendedorId);
                if (vendedor == null)
                    errores.Add("El socio vendedor seleccionado no existe.");
            }

            if (request.CompradorId.HasValue && request.CompradorId.Value > 0)
            {
                var comprador = await db.Socios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.CompradorId.Value);
                if (comprador == null)
                    errores.Add("El socio comprador seleccionado no existe.");
            }

            return errores;
        }

        private async Task ApplyTransferChangesAsync(TransanDTO request)
        {
            var cantHem = request.CantHem ?? 0;
            if (cantHem <= 0)
                return;

            var sellerPlant = await db.Planteles.FirstOrDefaultAsync(p => p.Placod == request.Plant);
            if (sellerPlant == null)
                throw new Exception("No se encontró el plantel de origen seleccionado.");

            var field = GetHembraField(request.Tiphac, request.Hemsta);
            if (string.IsNullOrWhiteSpace(field))
                throw new Exception("No se pudo determinar la categoría de hembras a transferir.");

            var sellerValue = GetPlantelFieldValue(sellerPlant, field);
            if (sellerValue < cantHem)
            {
                throw new Exception($"El plantel de origen no tiene stock suficiente para transferir {cantHem} hembras. Disponible: {sellerValue}.");
            }

            SetPlantelFieldValue(sellerPlant, field, sellerValue - cantHem);
            sellerPlant.FchUsu = DateTime.Now;
            db.Planteles.Update(sellerPlant);

            var buyerPlant = await db.Planteles.FirstOrDefaultAsync(p => p.Placod == request.NvoPla);
            if (buyerPlant == null)
                throw new Exception("No se encontró el plantel de destino seleccionado.");

            var buyerValue = GetPlantelFieldValue(buyerPlant, field);
            SetPlantelFieldValue(buyerPlant, field, buyerValue + cantHem);
            buyerPlant.FchUsu = DateTime.Now;
            db.Planteles.Update(buyerPlant);

            await db.SaveChangesAsync();
        }

        private async Task RevertTransferChangesAsync(Transan existing)
        {
            var prevCantHem = existing.CantHem ?? 0;
            if (prevCantHem <= 0)
                return;

            var field = GetHembraField(existing.Tiphac, existing.Hemsta);
            if (string.IsNullOrWhiteSpace(field))
                throw new Exception("No se pudo revertir la transferencia anterior porque su categoría de hembras es inválida.");

            var prevSeller = await db.Planteles.FirstOrDefaultAsync(p => p.Placod == existing.Plant);
            if (prevSeller != null)
            {
                var sellerValue = GetPlantelFieldValue(prevSeller, field);
                SetPlantelFieldValue(prevSeller, field, sellerValue + prevCantHem);
                prevSeller.FchUsu = DateTime.Now;
                db.Planteles.Update(prevSeller);
            }

            if (!string.IsNullOrWhiteSpace(existing.NvoPla))
            {
                var prevBuyer = await db.Planteles.FirstOrDefaultAsync(p => p.Placod == existing.NvoPla);
                if (prevBuyer != null)
                {
                    var buyerValue = GetPlantelFieldValue(prevBuyer, field);
                    SetPlantelFieldValue(prevBuyer, field, Math.Max(0, buyerValue - prevCantHem));
                    prevBuyer.FchUsu = DateTime.Now;
                    db.Planteles.Update(prevBuyer);
                }
            }

            await db.SaveChangesAsync();
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

        private async Task SendTransferNotificationAsync(TransanSaveRequestDTO request, bool isEdit)
        {
            var transan = request.Transan;
            var asunto = isEdit
                ? $"El socio {transan.Vnom ?? "N/D"} ha modificado una transferencia"
                : "Nueva transferencia animal";

            using var mail = new MailMessage();

            const string correoSistema = "planteles@hereford.org.ar";
            const string smtpHost = "mail.hereford.org.ar";
            const int smtpPort = 587;
            const string smtpUsername = "planteles@hereford.org.ar";
            const string smtpPassword = "Hereford.2033";

            mail.From = new MailAddress(correoSistema);
            mail.Subject = asunto;
            mail.Body = BuildTransferEmailHtml(transan, isEdit);
            mail.IsBodyHtml = true;

            TryAddRecipient(mail, correoSistema);

            var mailVendedor = await ResolveEmailAsync(request.VendedorId);
            if (!string.IsNullOrWhiteSpace(mailVendedor))
                TryAddRecipient(mail, mailVendedor);

            var mailComprador = !string.IsNullOrWhiteSpace(request.MailComprador)
                ? request.MailComprador
                : await ResolveEmailAsync(request.CompradorId);

            if (!string.IsNullOrWhiteSpace(mailComprador))
                TryAddRecipient(mail, mailComprador);

            if (mail.To.Count == 0)
                throw new Exception("No se encontró ningún destinatario válido para enviar el mail de la transferencia.");

            using var smtp = new SmtpClient(smtpHost, smtpPort)
            {
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
        }

        private async Task<string?> ResolveEmailAsync(int? socioId)
        {
            if (!socioId.HasValue || socioId.Value <= 0)
                return null;

            var usuario = await db.User.AsNoTracking().FirstOrDefaultAsync(x => x.SocioId == socioId.Value);
            if (usuario != null && IsValidEmail(usuario.Email))
                return usuario.Email;

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

        // Helper: decide which plantel field to use for hembras based on tipo hacienda and estado
        private string GetHembraField(string tiphac, string hemsta)
        {
            if (string.IsNullOrWhiteSpace(hemsta) || string.IsNullOrWhiteSpace(tiphac)) return null;
            hemsta = hemsta.Trim().ToUpper();
            tiphac = tiphac.Trim().ToUpper();

            bool isPR = tiphac.Contains("PR");
            bool isVIP = tiphac.Contains("VIP");

            if (hemsta == "CC" || hemsta == "CCP")
            {
                return isPR ? "Varede" : "Varepr"; // vacas
            }
            else if (hemsta == "PR")
            {
                return isPR ? "Vqcsrd" : "Vqcsrp"; // vaquillonas con servicio
            }
            else if (hemsta == "SS")
            {
                return isPR ? "Vqssrd" : "Vqssrp"; // vaquillonas sin servicio
            }

            return null;
        }

        private double GetPlantelFieldValue(Plantel plantel, string field)
        {
            return field switch
            {
                "Varede" => plantel.Varede ?? 0,
                "Vqcsrd" => plantel.Vqcsrd ?? 0,
                "Vqssrd" => plantel.Vqssrd ?? 0,
                "Varepr" => plantel.Varepr ?? 0,
                "Vqcsrp" => plantel.Vqcsrp ?? 0,
                "Vqssrp" => plantel.Vqssrp ?? 0,
                _ => 0
            };
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
