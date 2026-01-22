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
            if (IsValidEmail(email))
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
        public async Task<IActionResult> Guardar([FromBody] TransanDTO request)
        {
            Respuesta<TransanDTO> _Respuesta = new Respuesta<TransanDTO>();
            try
            {
                Transan _Transan = _mapper.Map<Transan>(request);

                // Use execution strategy to support MySQL retry policy with transactions
                var strategy = db.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await db.Database.BeginTransactionAsync();

                    // --- Stock adjustments for hembras ---
                    var cantHem = request.CantHem ?? 0;
                    if (cantHem > 0)
                    {
                        var sellerPlant = await db.Planteles.FirstOrDefaultAsync(p => p.Placod == request.Plant);
                        if (sellerPlant == null)
                        {
                            throw new Exception("Plantel de origen no encontrado");
                        }

                        var field = GetHembraField(request.Tiphac, request.Hemsta);
                        if (string.IsNullOrEmpty(field))
                        {
                            throw new Exception("Estado de hembra inválido");
                        }

                        double sellerValue = GetPlantelFieldValue(sellerPlant, field);
                        if (sellerValue < cantHem)
                        {
                            throw new Exception("No hay suficiente stock en el plantel del vendedor");
                        }

                        SetPlantelFieldValue(sellerPlant, field, sellerValue - cantHem);
                        sellerPlant.FchUsu = DateTime.Now;
                        db.Planteles.Update(sellerPlant);

                        if (!string.IsNullOrWhiteSpace(request.NvoPla))
                        {
                            var buyerPlant = await db.Planteles.FirstOrDefaultAsync(p => p.Placod == request.NvoPla);
                            if (buyerPlant != null)
                            {
                                double buyerValue = GetPlantelFieldValue(buyerPlant, field);
                                SetPlantelFieldValue(buyerPlant, field, buyerValue + cantHem);
                                buyerPlant.FchUsu = DateTime.Now;
                                db.Planteles.Update(buyerPlant);
                            }
                        }

                        await db.SaveChangesAsync();
                    }

                    // Save transfer using same DbContext
                    db.Transans.Add(_Transan);
                    await db.SaveChangesAsync();

                    await transaction.CommitAsync();
                });

                // Log history to file (non-DB) for audit
                try
                {
                    LogTransferHistory(_Transan, "Created");
                }
                catch (Exception lex)
                {
                    Console.WriteLine($"Failed to write history log: {lex.Message}");
                }

                if (_Transan.Id != 0)
                    _Respuesta = new Respuesta<TransanDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<TransanDTO>(_Transan) };
                else
                    _Respuesta = new Respuesta<TransanDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<TransanDTO>() { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] TransanDTO request)
        {
            Respuesta<TransanDTO> _Respuesta = new Respuesta<TransanDTO>();
            try
            {
                // Load existing transfer from same DbContext
                var existing = await db.Transans.FirstOrDefaultAsync(u => u.Id == request.Id);
                if (existing == null)
                {
                    _Respuesta = new Respuesta<TransanDTO>() { Exito = 0, Mensaje = "No se encontró el identificador" };
                    return StatusCode(StatusCodes.Status404NotFound, _Respuesta);
                }

                // If transfer already processed (FchUsu not null), disallow changing seller or origin plant
                if (existing.FchUsu != null)
                {
                    if (!string.Equals(existing.Sven, request.Sven) || !string.Equals(existing.Plant, request.Plant))
                    {
                        _Respuesta = new Respuesta<TransanDTO>() { Exito = 0, Mensaje = "No se puede cambiar vendedor o plantel de origen de una transferencia ya procesada." };
                        return StatusCode(StatusCodes.Status400BadRequest, _Respuesta);
                    }
                }

                var strategy = db.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await db.Database.BeginTransactionAsync();

                    // Revert previous hembras movement
                    var prevCantHem = existing.CantHem ?? 0;
                    if (prevCantHem > 0)
                    {
                        var prevSeller = await db.Planteles.FirstOrDefaultAsync(p => p.Placod == existing.Plant);
                        if (prevSeller != null)
                        {
                            var prevField = GetHembraField(existing.Tiphac, existing.Hemsta);
                            if (!string.IsNullOrEmpty(prevField))
                            {
                                double val = GetPlantelFieldValue(prevSeller, prevField);
                                SetPlantelFieldValue(prevSeller, prevField, val + prevCantHem);
                                prevSeller.FchUsu = DateTime.Now;
                                db.Planteles.Update(prevSeller);
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(existing.NvoPla))
                        {
                            var prevBuyer = await db.Planteles.FirstOrDefaultAsync(p => p.Placod == existing.NvoPla);
                            if (prevBuyer != null)
                            {
                                var prevField = GetHembraField(existing.Tiphac, existing.Hemsta);
                                if (!string.IsNullOrEmpty(prevField))
                                {
                                    double val = GetPlantelFieldValue(prevBuyer, prevField);
                                    SetPlantelFieldValue(prevBuyer, prevField, val - prevCantHem);
                                    prevBuyer.FchUsu = DateTime.Now;
                                    db.Planteles.Update(prevBuyer);
                                }
                            }
                        }

                        await db.SaveChangesAsync();
                    }

                    // Apply new adjustments
                    var cantHem = request.CantHem ?? 0;
                    if (cantHem > 0)
                    {
                        var sellerPlant = await db.Planteles.FirstOrDefaultAsync(p => p.Placod == request.Plant);
                        if (sellerPlant == null)
                        {
                            throw new Exception("Plantel de origen no encontrado");
                        }

                        string field = GetHembraField(request.Tiphac, request.Hemsta);
                        if (string.IsNullOrEmpty(field))
                        {
                            throw new Exception("Estado de hembra inválido");
                        }

                        double sellerValue = GetPlantelFieldValue(sellerPlant, field);
                        if (sellerValue < cantHem)
                        {
                            throw new Exception("No hay suficiente stock en el plantel del vendedor");
                        }

                        SetPlantelFieldValue(sellerPlant, field, sellerValue - cantHem);
                        sellerPlant.FchUsu = DateTime.Now;
                        db.Planteles.Update(sellerPlant);

                        if (!string.IsNullOrWhiteSpace(request.NvoPla))
                        {
                            var buyerPlant = await db.Planteles.FirstOrDefaultAsync(p => p.Placod == request.NvoPla);
                            if (buyerPlant != null)
                            {
                                double buyerValue = GetPlantelFieldValue(buyerPlant, field);
                                SetPlantelFieldValue(buyerPlant, field, buyerValue + cantHem);
                                buyerPlant.FchUsu = DateTime.Now;
                                db.Planteles.Update(buyerPlant);
                            }
                        }

                        await db.SaveChangesAsync();
                    }

                    // Update transfer record using same DbContext
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
                    existing.Incorp = request.Incorp;
                    existing.Tipohem = request.Tipohem;
                    existing.CantChem = request.CantChem;
                    existing.CantCmach = request.CantCmach;
                    existing.FchUsu = request.FchUsu;
                    existing.CodUsu = request.CodUsu;

                    db.Transans.Update(existing);
                    await db.SaveChangesAsync();

                    await transaction.CommitAsync();

                    // Log history
                    try
                    {
                        LogTransferHistory(existing, "Edited");
                    }
                    catch (Exception lex)
                    {
                        Console.WriteLine($"Failed to write history log: {lex.Message}");
                    }
                });

                _Respuesta = new Respuesta<TransanDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<TransanDTO>(request) };
                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<TransanDTO>() { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
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
