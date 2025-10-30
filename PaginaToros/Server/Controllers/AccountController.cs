using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using PaginaToros.Server.Context;
using PaginaToros.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using PaginaToros.Client.Pages.Socios;
using System.Net.Mime;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Asn1.Ocsp;
using PaginaToros.Client.Pages.Auth;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.Globalization;
using System.Net;
namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
    //    Roles = "USUARIOMAESTRO,ADMINISTRADOR")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly hereford_prContext _hfdb;

        public AccountController(ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            hereford_prContext hfdb)
        {
            this.db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _hfdb = hfdb;
        }

        //Metodos dmuu kdke jobp bhgo

        [HttpPost("EditUser")]
        public async Task<ActionResult> EditUser([FromBody] User model)
        {
            Console.WriteLine("Entró a EditUser");
            try
            {
                // 0) Traer tu usuario de dominio
                var dbUser = await db.User.FirstOrDefaultAsync(u => u.Id == model.Id);
                if (dbUser == null)
                    return NotFound("Usuario no encontrado.");

                // 1) Buscar el AspNetUser por el email actual guardado en tu tabla (mail anterior)
                //    Si por algún motivo no está, intento por el nuevo email.
                var aspUser = await _userManager.FindByNameAsync(dbUser.Email)
                            ?? await _userManager.FindByEmailAsync(dbUser.Email)
                            ?? await _userManager.FindByEmailAsync(model.Email);

                if (aspUser == null)
                    return BadRequest("No se pudo encontrar el usuario de acceso (AspNetUsers).");

                // 2) Si el email cambia, validar duplicados
                if (!string.Equals(aspUser.Email, model.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var conflict = await _userManager.FindByEmailAsync(model.Email);
                    if (conflict != null && conflict.Id != aspUser.Id)
                        return BadRequest("El email ya está en uso por otro usuario.");

                    // 2.a) Actualizar Email y UserName con UserManager
                    var setEmail = await _userManager.SetEmailAsync(aspUser, model.Email);
                    if (!setEmail.Succeeded) return BadRequest(setEmail.Errors);

                    var setUserName = await _userManager.SetUserNameAsync(aspUser, model.Email);
                    if (!setUserName.Succeeded) return BadRequest(setUserName.Errors);

                    // 2.b) Asegurar normalizados (algunos stores no lo hacen solos)
                    aspUser.NormalizedEmail = model.Email.ToUpperInvariant();
                    aspUser.NormalizedUserName = model.Email.ToUpperInvariant();
                }

                // 3) Teléfono (si cambió)
                if (!string.Equals(aspUser.PhoneNumber, model.Phone, StringComparison.Ordinal))
                {
                    var setPhone = await _userManager.SetPhoneNumberAsync(aspUser, model.Phone);
                    if (!setPhone.Succeeded) return BadRequest(setPhone.Errors);
                }

                // 4) Persistir cambios en AspNetUsers (trae ConcurrencyStamp correcto → NO hay excepción)
                var upd = await _userManager.UpdateAsync(aspUser);
                if (!upd.Succeeded) return BadRequest(upd.Errors);

                // 5) Actualizar tu tabla User
                dbUser.Names = model.Names;
                dbUser.LastNames = model.LastNames;
                dbUser.Dni = model.Dni?.ToUpperInvariant();
                dbUser.Rol = model.Rol?.ToUpperInvariant();
                dbUser.Status = model.Status?.ToUpperInvariant();
                dbUser.Phone = model.Phone;
                dbUser.Email = model.Email;            
                                                       

                await db.SaveChangesAsync();

                Console.WriteLine("Usuario actualizado correctamente.");
                return Ok("Usuario actualizado correctamente.");
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Conflicto de concurrencia. El registro fue modificado por otro proceso.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUser([FromBody] User model, string password)
        {
            try
            {
                model.Dni = model.Dni?.ToUpperInvariant();
                model.Rol = model.Rol?.ToUpperInvariant();
                model.Status = model.Status?.ToUpperInvariant() ?? "ACTIVO";
                model.Created = DateTime.UtcNow;

                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true,
                    PhoneNumber = model.Phone,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };
                user.NormalizedUserName = model.Email.ToUpperInvariant();
                user.NormalizedEmail = model.Email.ToUpperInvariant();

                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded) return BadRequest(result.Errors);

                if (!string.IsNullOrWhiteSpace(model.Rol))
                {
                    result = await _userManager.AddToRoleAsync(user, model.Rol);
                    if (!result.Succeeded) return BadRequest(result.Errors);
                }

                await db.User.AddAsync(model);
                await db.SaveChangesAsync();

                return Ok(user.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("SendRegistrationMail")]
        public async Task<ActionResult> SendRegistrationMail([FromBody] User model, string password)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("planteles@hereford.org.ar");
                    mail.To.Add(model.Email);
                    mail.Subject = "Confirmación de Registro en Hereford";

                    Console.WriteLine("LLego");

                   





                    string body = $@"
            <html>
            <body style='margin:0;padding:0;'>
                <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                    <tr>
                        <td>
                            <table width='600' border='0' cellspacing='0' cellpadding='0' align='center' style='background-repeat:no-repeat;background-image:url(cid:backgroundImage);background-size:cover;'>
                                <tr>
                                    <td style='padding: 20px; padding-top: 90px; padding-bottom: 300px; color: #000;'>
                                        <h2>Estimado socio.</h2>
                                        <p>Nos complace confirmar que su registro en nuestro sitio ha sido exitoso. A partir de ahora, tiene acceso completo a todos los servicios y funcionalidades disponibles.</p>
                                        <p><strong>Detalles de inicio de sesión:</strong></p>
                                        <p>Correo electrónico registrado: {model.Email}<br>Contraseña: {password}</p>
                                        <p>Recuerde mantener segura esta información y no compartirla. Gracias por su tiempo y ante cualquier consulta no dude en comunicarse por mail a planteles@hereford.org.ar</p>
                                        <p>Saludos cordiales,<br>Equipo de Puro Registrado Hereford.</p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>";

                    // Configura la vista HTML del correo con la imagen como recurso vinculado
                    string projectRoot = Directory.GetCurrentDirectory(); // Obtiene la raíz del proyecto
                    string imagePath = Path.Combine("wwwroot", "images", "background.jpg");
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

                    Console.WriteLine(model.Email);
                    Console.WriteLine(password);

                    //string filePath = Path.Combine("wwwroot", "images", "Tarifas Registros 2025.docx");
                    //if (System.IO.File.Exists(filePath))
                    //{
                    //    Attachment attachment = new Attachment(filePath);
                    //    mail.Attachments.Add(attachment);

                    //    Console.WriteLine("Adjunto añadido.");
                    //}
                    //else
                    //{
                    //    Console.WriteLine("El archivo no se encuentra en la ruta especificada.");
                    //    return BadRequest("Archivo no encontrado.");
                    //}



                    Console.WriteLine(mail.Attachments);

                    LinkedResource background = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg)
                    {
                        ContentId = "backgroundImage",
                        TransferEncoding = TransferEncoding.Base64
                    };
                    htmlView.LinkedResources.Add(background);

                    mail.AlternateViews.Add(htmlView);
                    mail.IsBodyHtml = true;

                    // Configuración del cliente SMTP
                    using (SmtpClient smtp = new SmtpClient("mail.hereford.org.ar", 587)) // Cambia el host y el puerto según tu proveedor
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("planteles@hereford.org.ar", "Hereford.2033"); // Coloca tu contraseña aquí
                        smtp.EnableSsl = true; // Cambia a false si el servidor no usa SSL
                        smtp.Send(mail);
                    }
                }

                return Ok("Correo enviado correctamente.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return BadRequest($"Error al enviar el correo: {e.Message}");
            }
        }

        //Hereford.2033 Hereford.2033

        [HttpPost("SendMail2025")]
        public async Task<ActionResult> SendMailInfo([FromBody] User model, string password)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    Console.WriteLine("Entro por lo mens");

                    mail.From = new MailAddress("planteles@hereford.org.ar");
                    mail.To.Add(model.Email);
                    mail.Subject = "Hereford - Puro Registrado.";

                    string projectRoot = Directory.GetCurrentDirectory();
                    string imagePath = Path.Combine(projectRoot, "wwwroot", "images", "backgroundEnvio.jpg");
                    string logoPath = Path.Combine(projectRoot, "wwwroot", "images", "LOGO.jpg");

                    // Verifica si el email es de Outlook, en tal caso no carga las imágenes
                    if (model.Email.Contains("outlook"))
                    {
                        logoPath = null;
                        imagePath = null; // No cargamos la imagen de fondo
                    }

                    if (!string.IsNullOrEmpty(imagePath) && !System.IO.File.Exists(imagePath))
                    {
                        Console.WriteLine("Imagen de fondo no encontrada.");
                        return BadRequest("Imagen de fondo no encontrada.");
                    }

                    if (!string.IsNullOrEmpty(logoPath) && !System.IO.File.Exists(logoPath))
                    {
                        Console.WriteLine("Logo no encontrado.");
                        return BadRequest("Logo no encontrado.");
                    }

                    string logoHtml = string.IsNullOrEmpty(logoPath) ? "" : $"<img src='cid:logoImage' alt='Hereford Logo' style='width:150px; height:auto;' />";

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
                                        <h2>Buenos Aires, {DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"))}</h2>
                                        <p>Señor {model.Names ?? "criador"} {model.LastNames}:</p>
                                        <p>Les informamos que, a partir de este momento, el sistema de autogestión anterior ya no estará en funcionamiento. Hemos implementado una nueva plataforma para mejorar la gestión y facilitarles el acceso a los servicios. Puede acceder a su perfil <a href='https://herefordapp.com.ar:1050/'>aquí</a>.</p>
                                        <p><strong>Detalles de inicio de sesión:</strong></p>
                                        <p>Correo electrónico registrado: {model.Email}<br>Contraseña: '{password}'</p>
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

                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        LinkedResource background = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg)
                        {
                            ContentId = "backgroundImage",
                            TransferEncoding = System.Net.Mime.TransferEncoding.Base64
                        };
                        htmlView.LinkedResources.Add(background);
                    }

                    if (!string.IsNullOrEmpty(logoPath))
                    {
                        LinkedResource logo = new LinkedResource(logoPath, MediaTypeNames.Image.Jpeg)
                        {
                            ContentId = "logoImage",
                            TransferEncoding = System.Net.Mime.TransferEncoding.Base64
                        };
                        htmlView.LinkedResources.Add(logo);
                    }

                    mail.AlternateViews.Add(htmlView);
                    mail.IsBodyHtml = true;

                    //string filePath = Path.Combine(projectRoot, "wwwroot", "images", "Tarifas Registros 2025.docx");
                    //if (System.IO.File.Exists(filePath))
                    //{
                    //    Attachment attachment = new Attachment(filePath);
                    //    mail.Attachments.Add(attachment);
                    //}

                    using (SmtpClient smtp = new SmtpClient("mail.hereford.org.ar", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("planteles@hereford.org.ar", "Hereford.2033");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }

                Console.WriteLine("Correo enviado correctamente.");
                return Ok("Correo enviado correctamente.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                Console.WriteLine(model.Email); 
                Console.WriteLine(password);
                return BadRequest($"Error al enviar el correo: {e.Message}");
            }
        }



        //Hereford.2033 Hereford.2033


        [HttpPost("SendChangeNotificationMail")]
        public async Task<ActionResult> SendChangeNotificationMail([FromBody] ChangeNotificationModel model)
        {
            Console.WriteLine(model.Id);

            var socio = db.User.FirstOrDefault(x => x.Id == model.Id);
            if (socio == null)
            {
                return BadRequest($"No se encontró un socio con el ID proporcionado ({model.Id}).");
            }

            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("planteles@hereford.org.ar");
                    mail.To.Add("planteles@hereford.org.ar");
                    mail.Subject = "Notificación de Cambio en el Sistema de Hereford";


                  



                    string body = $@"
            <html>
            <body style='margin:0;padding:0;'>
                <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                    <tr>
                        <td>
                            <table width='600' border='0' cellspacing='0' cellpadding='0' align='center'>
                                <tr>
                                    <td style='padding: 20px; color: #000;'>
                                        <h2>Notificación de cambio realizado por el socio</h2>
                                        <p><strong>Usuario:</strong> {socio.Names} {socio.LastNames}</p>
                                        <p><strong>Email:</strong> {socio.Email}</p>
                                        <p><strong>Clase:</strong> {model.Clase}</p>
                                        <p><strong>Tipo de acción:</strong> {model.Tipo}</p>
                                        <p><strong>Detalle:</strong> {model.Detalle}</p>
                                        <p>Saludos cordiales,<br>Equipo de Puro Registrado Hereford.</p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>";

                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    string filePath = @"C:\Users\Maxim\source\repos\PaginaToros\PaginaToros\Server\wwwroot\images\Mail.docx";
                    if (!System.IO.File.Exists(filePath))
                    {
                        Attachment attachment = new Attachment(filePath);
                        mail.Attachments.Add(attachment);
                    }

                    Console.WriteLine(filePath);
                    Console.WriteLine(mail.Attachments);



                    using (SmtpClient smtp = new SmtpClient("mail.hereford.org.ar", 587)) // Cambia esto al servidor SMTP correcto
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("planteles@hereford.org.ar", "Hereford.2033"); // Coloca la contraseña correcta aquí
                        smtp.EnableSsl = true; // Cambia a false si el hosting no requiere SSL/TLS en este puerto
                        smtp.Send(mail);
                    }
                }

                Console.WriteLine("Correo enviado.");
                return Ok("Correo enviado correctamente.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest($"Error al enviar el correo: {e.Message}");
            }
        }

        //Hereford.2033
        //dmuu kdke jobp bhgo
        //dmuu kdke jobp bhgo
        public class ChangeNotificationModel
        {
            public int Id { get; set; }

            public string? Tipo { get; set; }
            public string? Clase { get; set; }
            public string? Detalle { get; set; }
        }



        public class SendResetMailRequest
        {
            public User Usuario { get; set; }
            public string NuevaContrasena { get; set; }
        }

        [HttpPost("SendResetMail")]
        public async Task<ActionResult> SendResetMail([FromBody] SendResetMailRequest req)
        {
            if (req is null || req.Usuario is null || string.IsNullOrWhiteSpace(req.NuevaContrasena))
                return BadRequest("Payload inválido.");

            var model = req.Usuario;
            // IMPORTANTE: encode por si la contraseña tiene caracteres que rompan HTML
            var nuevaContraseña = WebUtility.HtmlEncode(req.NuevaContrasena);

            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress("planteles@hereford.org.ar");
                    mail.To.Add(model.Email);
                    mail.Subject = "Hereford - Restablecimiento de contraseña.";

                    string projectRoot = Directory.GetCurrentDirectory();
                    string imagePath = Path.Combine(projectRoot, "wwwroot", "images", "backgroundEnvio.jpg");
                    string logoPath = Path.Combine(projectRoot, "wwwroot", "images", "LOGO.jpg");

                    // Outlook: no cargar imágenes (case-insensitive)
                    bool esOutlook = model.Email?.IndexOf("outlook", StringComparison.OrdinalIgnoreCase) >= 0;
                    if (esOutlook)
                    {
                        logoPath = null;
                        imagePath = null;
                    }

                    if (!string.IsNullOrEmpty(imagePath) && !System.IO.File.Exists(imagePath))
                        return BadRequest("Imagen de fondo no encontrada.");

                    if (!string.IsNullOrEmpty(logoPath) && !System.IO.File.Exists(logoPath))
                        return BadRequest("Logo no encontrado.");

                    string logoHtml = string.IsNullOrEmpty(logoPath)
                        ? ""
                        : "<img src='cid:logoImage' alt='Hereford Logo' style='width:150px; height:auto;' />";

                    var fecha = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));

                    string body = $@"
                        <html>
                        <body style='margin:0;padding:0;'>
                          <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                            <tr>
                              <td>
                                <table width='600' border='0' cellspacing='0' cellpadding='0' align='center' style='background-repeat:no-repeat;background-image:url({(string.IsNullOrEmpty(imagePath) ? "" : "cid:backgroundImage")});background-size:cover;'>
                                  <tr>
                                    <td style='padding: 20px; text-align: left;'>
                                      {logoHtml}
                                    </td>
                                  </tr>
                                  <tr>
                                    <td style='padding: 20px; padding-top: 10px; color: #000;'>
                                      <h2>Buenos Aires, {fecha}</h2>
                                      <p>Señor {(!string.IsNullOrWhiteSpace(model.Names) ? model.Names : "criador")}:</p>
                                      <p>Les informamos que, a partir de este momento, el sistema de autogestión anterior ya no estará en funcionamiento. Hemos implementado una nueva plataforma para mejorar la gestión y facilitarles el acceso a los servicios. Puede acceder a su perfil <a href='https://herefordapp.com.ar:1050/'>aquí</a>.</p>
                                      <p><strong>Detalles de inicio de sesión:</strong></p>
                                      <p>Correo electrónico registrado: {model.Email}<br>Contraseña: <code>'{nuevaContraseña}'</code></p>
                                      <p>Recuerde mantener segura esta información y no compartirla. Ante cualquier consulta escriba a <a href='mailto:planteles@hereford.org.ar'>planteles@hereford.org.ar</a>.</p>
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
                        Credentials = new System.Net.NetworkCredential("planteles@hereford.org.ar", "Hereford.2033"),
                        EnableSsl = true
                    };

                    smtp.Send(mail);
                }

                Console.WriteLine("Correo enviado correctamente.");
                return Ok("Correo enviado correctamente.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error SendResetMail: {e.Message}");
                Console.WriteLine(model.Email);
                Console.WriteLine(nuevaContraseña);
                return BadRequest($"Error al enviar el correo: {e.Message}");
            }
        }


        [HttpPost("SendReinicioMail")]
        public async Task<ActionResult> SendReinicioMail([FromBody] User model, string password)
        {
            try
            {
                Console.WriteLine(model.Email);

                using (SmtpClient client = new SmtpClient("mail.hereford.org.ar", 587))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("planteles@hereford.org.ar", "Hereford.2033");
                    client.EnableSsl = true;


                    //Console.WriteLine("cAMBIO DE MAIL");
                    //Console.Write(Usuario.PasswordHash);

                    //var Usuario = _userManager.Users.FirstOrDefault(x => x.Email == model.Email);
                    var user = db.Users.FirstOrDefault(x => x.Email == password);

                    Console.WriteLine(model.Email);
                    Console.WriteLine(user.Email);
                    //Console.WriteLine(Usuario.PasswordHash);
                    Console.WriteLine(password);
                    Console.WriteLine("Cambio de mail y contra");



                    //var result = await _userManager.ChangePasswordAsync(user, Usuario.PasswordHash, password);


                    //if (!result.Succeeded)
                    //{
                    //    return BadRequest(result.Errors);
                    //}

                    MailMessage mail = new MailMessage
                    {
                        From = new MailAddress("planteles@hereford.org.ar"),
                        Subject = "Restablecimiento de mail para Hereford",
                        Body = $"Estimado,\nHemos recibido una solicitud para restablecer su contraseña. Los detalles de su nueva contraseña son los siguientes:\n" +
                               $"Correo electrónico registrado: {model.Email}\n" +
                               "Recuerde mantener esta información segura y no compartirla con nadie más. Gracias por estar con nosotros y esperamos que continúe disfrutando de nuestros servicios.\n" +
                               "Saludos cordiales,\nHereford",
                        IsBodyHtml = false
                    };
                    mail.To.Add(new MailAddress(model.Email));


                    Console.WriteLine("Fin envio de mail y contra");

                    //\nNueva contraseña: { Usuario.PasswordHash}
                    client.Send(mail);
                }

                return Ok("Correo enviado correctamente.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest($"Error al enviar el correo: {e.Message}");
            }
        }

        //dmuu kdke jobp bhgo


        

        [HttpPut("ChangePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePassModel model)
        {
            try
            {
                if (model.NewPass != model.ConfirmPass)
                {
                    return BadRequest("No se pudo confirmar el nuevo password");
                }

                var user = db.Users.FirstOrDefault(x => x.UserName == model.UserName);
                if (user == null)
                {
                    return BadRequest("No se pudo encontrar un usuario de acceso");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPass, model.NewPass);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return Ok("ok");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        public class ResetPasswordModel
            {
                public int UserId { get; set; }

                public string Email { get; set; }
                public string Password { get; set; }
            }

        [HttpPut("ResetPassword")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            try
            {
                var usuario = db.User.FirstOrDefault(x => x.Id == model.UserId)
                           ?? db.User.FirstOrDefault(x => x.Email == model.Email);

                if (usuario == null) return BadRequest("No se pudo encontrar un usuario");

                var email = usuario.Email;
                var user = db.Users.FirstOrDefault(x => x.UserName == email)
                        ?? db.Users.FirstOrDefault(x => x.Email == email)
                        ?? db.Users.FirstOrDefault(x => x.NormalizedEmail == email.ToUpper());

                if (user == null) return BadRequest("No se pudo encontrar un usuario de acceso");

                // Validar password contra políticas
                foreach (var validator in _userManager.PasswordValidators)
                {
                    var v = await validator.ValidateAsync(_userManager, user, model.Password);
                    if (!v.Succeeded) return BadRequest(v.Errors);
                }

                // Remover si tiene y luego agregar
                if (await _userManager.HasPasswordAsync(user))
                {
                    var remove = await _userManager.RemovePasswordAsync(user);
                    if (!remove.Succeeded) return BadRequest(remove.Errors);
                }

                var add = await _userManager.AddPasswordAsync(user, model.Password);
                if (!add.Succeeded) return BadRequest(add.Errors);

                return Ok("ok");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }


        public class ChangeEmailAndResetRequest
        {
            public int UserId { get; set; }
            public string NewEmail { get; set; } = "";
        }

        [HttpPost("ChangeEmailAndReset")]
        public async Task<ActionResult> ChangeEmailAndReset([FromBody] ChangeEmailAndResetRequest req)
        {
            Console.WriteLine("=== ChangeEmailAndReset INICIO ===");
            Console.WriteLine($"UserId: {req.UserId}");
            Console.WriteLine($"NewEmail (raw): '{req.NewEmail}'");

            try
            {
                // 1) Usuario de dominio
                var dbUser = await db.User.FirstOrDefaultAsync(u => u.Id == req.UserId);
                if (dbUser == null)
                {
                    Console.WriteLine("dbUser == null");
                    return BadRequest("Usuario no encontrado.");
                }

                Console.WriteLine($"dbUser.Email (actual): {dbUser.Email}");

                // 2) AspNetUser (por email actual)
                var aspUser =
                      await _userManager.FindByEmailAsync(dbUser.Email)
                   ?? await _userManager.FindByNameAsync(dbUser.Email);

                if (aspUser == null)
                {
                    Console.WriteLine("aspUser == null (no existe en AspNetUsers con el mail actual)");
                    // Intento por el mail nuevo (por si ya migró a mano)
                    aspUser = await _userManager.FindByEmailAsync(req.NewEmail)
                           ?? await _userManager.FindByNameAsync(req.NewEmail);

                    if (aspUser == null)
                        return BadRequest("Usuario de acceso no encontrado (AspNetUsers).");
                    Console.WriteLine($"Encontrado aspUser por NewEmail: {aspUser.Id}");
                }
                else
                {
                    Console.WriteLine($"aspUser encontrado: Id={aspUser.Id}, UserName={aspUser.UserName}, Email={aspUser.Email}");
                }

                var newEmail = (req.NewEmail ?? "").Trim();
                if (string.IsNullOrWhiteSpace(newEmail))
                {
                    Console.WriteLine("newEmail inválido (vacío)");
                    return BadRequest("Email inválido.");
                }

                // 3) Si cambia el email, validar duplicados y actualizar
                if (!string.Equals(aspUser.Email, newEmail, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Cambio de email detectado: '{aspUser.Email}' -> '{newEmail}'");

                    var conflict = await _userManager.FindByEmailAsync(newEmail);
                    if (conflict != null && conflict.Id != aspUser.Id)
                    {
                        Console.WriteLine($"CONFLICTO: {newEmail} ya está en uso por AspNetUser.Id={conflict.Id}");
                        return BadRequest("El email ya está en uso por otro usuario.");
                    }

                    var se = await _userManager.SetEmailAsync(aspUser, newEmail);
                    Console.WriteLine($"SetEmailAsync => {se.Succeeded}");
                    if (!se.Succeeded) return BadRequest(se.Errors);

                    var su = await _userManager.SetUserNameAsync(aspUser, newEmail);
                    Console.WriteLine($"SetUserNameAsync => {su.Succeeded}");
                    if (!su.Succeeded) return BadRequest(su.Errors);

                    // Normalizados + confirmado (evita problemas al loguear por NormalizedUserName)
                    aspUser.NormalizedEmail = newEmail.ToUpperInvariant();
                    aspUser.NormalizedUserName = newEmail.ToUpperInvariant();
                    aspUser.EmailConfirmed = true;

                    var upd = await _userManager.UpdateAsync(aspUser);
                    Console.WriteLine($"UpdateAsync(aspUser) => {upd.Succeeded}");
                    if (!upd.Succeeded) return BadRequest(upd.Errors);

                    Console.WriteLine($"aspUser.UserName NOW: {aspUser.UserName}");
                    Console.WriteLine($"aspUser.Email NOW: {aspUser.Email}");
                    Console.WriteLine($"aspUser.NormalizedUserName NOW: {aspUser.NormalizedUserName}");
                    Console.WriteLine($"aspUser.NormalizedEmail NOW: {aspUser.NormalizedEmail}");
                }
                else
                {
                    Console.WriteLine("No hay cambio de email (mismo valor).");
                }

                // 4) Generar UNA contraseña (solo A-Z y 0-9)
                var newPassword = GenerateServerPassword(12);
                Console.WriteLine($"Password generado (masked): {Mask(newPassword)}");

                // 5) Validar y setear password en Identity
                foreach (var validator in _userManager.PasswordValidators)
                {
                    var v = await validator.ValidateAsync(_userManager, aspUser, newPassword);
                    Console.WriteLine($"PasswordValidator => {v.Succeeded}");
                    if (!v.Succeeded) return BadRequest(v.Errors);
                }

                if (await _userManager.HasPasswordAsync(aspUser))
                {
                    var rm = await _userManager.RemovePasswordAsync(aspUser);
                    Console.WriteLine($"RemovePasswordAsync => {rm.Succeeded}");
                    if (!rm.Succeeded) return BadRequest(rm.Errors);
                }

                var add = await _userManager.AddPasswordAsync(aspUser, newPassword);
                Console.WriteLine($"AddPasswordAsync => {add.Succeeded}");
                if (!add.Succeeded) return BadRequest(add.Errors);

                // 6) Actualizar tu tabla User
                Console.WriteLine($"dbUser.Email '{dbUser.Email}' -> '{newEmail}'");
                dbUser.Email = newEmail;
                await db.SaveChangesAsync();
                Console.WriteLine("dbUser guardado (SaveChanges OK)");

                // 7) Enviar mail con la MISMA contraseña
                var mailReq = new SendResetMailRequest { Usuario = dbUser, NuevaContrasena = newPassword };

                Console.WriteLine("Enviando correo de reset...");
                var mailResult = await SendResetMail(mailReq); // misma acción del controller

                if (mailResult is ObjectResult obj && obj.StatusCode.HasValue && obj.StatusCode.Value >= 400)
                {
                    Console.WriteLine($"ERROR al enviar correo. Status: {obj.StatusCode}");
                    return BadRequest("Se cambió email y contraseña, pero falló el envío de mail.");
                }

                Console.WriteLine("=== ChangeEmailAndReset FIN OK ===");
                return Ok("Email y contraseña actualizados, mail enviado.");
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPCIÓN ChangeEmailAndReset:");
                Console.WriteLine(e.ToString());
                return BadRequest(e.Message);
            }
        }

        private static string Mask(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            if (s.Length <= 4) return "****";
            return s.Substring(0, 2) + new string('*', s.Length - 4) + s.Substring(s.Length - 2, 2);
        }

        // Generador server-side: SOLO MAYÚSCULAS + DÍGITOS
        private static string GenerateServerPassword(int length)
        {
            const string UPPER = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string DIGITS = "0123456789";
            string ALL = UPPER + DIGITS;

            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();

            int Next(int max)
            {
                Span<byte> b = stackalloc byte[4];
                rng.GetBytes(b);
                return (int)(BitConverter.ToUInt32(b) % (uint)max);
            }

            var sb = new StringBuilder();
            sb.Append(UPPER[Next(UPPER.Length)]);
            sb.Append(DIGITS[Next(DIGITS.Length)]);

            while (sb.Length < Math.Max(length, 8))
                sb.Append(ALL[Next(ALL.Length)]);

            // mezclar in-place
            var chars = sb.ToString().ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                int j = Next(chars.Length);
                (chars[i], chars[j]) = (chars[j], chars[i]);
            }
            return new string(chars);
        }




        [HttpGet("GetUsers/{search}/{status}/{actualPage}")]
        public async Task<ActionResult> GetUsers(string search, string status, int actualPage)
        {
            try
            {
                if (search == "TODO") search = "";
                if (status == "TODO") status = "";

                search = (search ?? "").ToUpper();
                status = (status ?? "");

                var query = db.User
                    .Where(x =>
                       (((x.Names ?? "").ToUpper().Contains(search)) ||
                        ((x.LastNames ?? "").ToUpper().Contains(search)) ||
                        ((x.Dni ?? "").Contains(search)) ||
                        ((x.Email ?? "").ToUpper().Contains(search))) &&
                       ((x.Status ?? "").Contains(status)) &&
                       x.Rol != "USUARIOMAESTRO");

                int allRegisters = query.Count();
                if (allRegisters <= 0)
                    return NotFound(Utilities.MSGNODATA);

                IList<User> entities = query
                    .OrderByDescending(x => x.Id)
                    .Skip((actualPage - 1) * Utilities.REGISTERSPERPAGE)
                    .Take(Utilities.REGISTERSPERPAGE)
                    .ToList();

                ResponseForList response = new ResponseForList()
                {
                    EntitiesPricipal = JsonSerializer.Serialize(entities),
                    AllRegisters = allRegisters,
                    ActualPage = actualPage
                };

                return Ok(JsonSerializer.Serialize(response));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetUserById/{id}")]
        public ActionResult GetUserById(int id)
        {
            try
            {
                var Usuario = db.User.FirstOrDefault(x => x.Id == id);
                if (Usuario == null)
                {
                    return BadRequest("No se pudo encontrar un usuario");
                }
                return Ok(JsonSerializer.Serialize(Usuario));
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet("GetUserByMail/{email}")]
        public ActionResult GetUserByMail(string email)
        {
            try
            {
                Console.WriteLine(email);
                Console.WriteLine("Por lo menos entro");
                var Usuario = db.User.FirstOrDefault(x => x.Email == email);
                if (Usuario == null)
                {
                    return BadRequest("No se pudo encontrar un usuario");
                }
                return Ok(JsonSerializer.Serialize(Usuario));
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var entities = await _hfdb.User.Include(x=>x.Socio).ToListAsync();

                ResponseForList response = new ResponseForList()
                {
                    EntitiesPricipal = JsonSerializer.Serialize(entities),
                };
                return Ok(JsonSerializer.Serialize(response));

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }


        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] UserLogin userInfo)
        {
            var Usuario = db.User.FirstOrDefault(x => x.Email == userInfo.UserName);
            if (Usuario != null)
            {
                if (Usuario != null && Usuario.Status != "ACTIVO")
                {
                    return BadRequest(Utilities.MSGSUSPENDEDUSER);
                }

                var result = await _signInManager.PasswordSignInAsync(userInfo.UserName, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return Ok(BuildToken(userInfo.UserName));
                }
                else
                {
                    return BadRequest(Utilities.MSGCREDENTIALSFAILS);
                }
            }
            else
            {
                return BadRequest(Utilities.MSGCREDENTIALSFAILS);
            }

        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var Usuario = db.User.FirstOrDefault(x => x.Id == id);
                if (Usuario == null)
                {
                    return BadRequest("No se pudo encontrar un usuario");
                }

                var user = db.Users.FirstOrDefault(x => x.UserName == Usuario.Email);
                if (user == null)
                {
                    return BadRequest("No se pudo encontrar un usuario de acceso");
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                db.Remove(Usuario);

                await db.SaveChangesAsync();
                return Ok("ok");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        private string BuildToken(string username)
        {
            var user = db.Users.First(x => x.UserName == username);
            string role = db.UserRoles.First(x => x.UserId == user.Id).RoleId;
            string completeName = "USUARIO MAESTRO";
            int UsuarioId = 0;
            int? SocioId = 0;
            if (role != "USUARIOMAESTRO")
            {
                var Usuario = db.User.First(x => x.Email == user.UserName);
                completeName = $"{Usuario.Names} {Usuario.LastNames}";
                UsuarioId = Usuario.Id;
                SocioId = Usuario.SocioId;
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim("userNM", username),
                new Claim(ClaimTypes.Name, completeName),
                new Claim("userId", user.Id),
                new Claim("UsuarioId", UsuarioId.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("SocioId",SocioId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(10);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
}
}
