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
        public async Task<ActionResult> EditUser([FromBody] User model, string? password)
        {
            Console.WriteLine("Entró a EditUser");
            try
            {
                

                Console.WriteLine("Entro 2");

                var user = new IdentityUser
                {
                    UserName = model.Email,
                    NormalizedUserName = model.Email,
                    PhoneNumber = model.Phone,
                    Email = model.Email,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };
                if (user.Email != null)
                    user.NormalizedEmail = model.Email.ToUpper();

                var result = await _userManager.UpdateAsync(user);

                var dbUser = await db.User.FirstOrDefaultAsync(u => u.Id == model.Id);
                if (dbUser != null)
                {
                    Console.WriteLine("Existe");
                    Console.WriteLine(dbUser.Email);

                    dbUser.Dni = model.Dni.ToUpper();
                    dbUser.Names = model.Names;
                    dbUser.LastNames = model.LastNames;
                    dbUser.Rol = model.Rol.ToUpper();
                    dbUser.Status = model.Status.ToUpper();
                    dbUser.Created = DateTime.UtcNow;

                    dbUser.Email = model.Email;

                    Console.WriteLine(dbUser.Email);


                    db.User.Update(dbUser);
                    await db.SaveChangesAsync();
                }
                Console.WriteLine("Entro 3");

                Console.WriteLine("Usuario actualizado correctamente.");
                return Ok("Usuario actualizado correctamente.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUser([FromBody] User model, string password)
        {

            Console.WriteLine("Entro");
            try
            {
                model.Dni = model.Dni.ToUpper();
                model.Names = model.Names;
                model.LastNames = model.LastNames;
                model.Rol = model.Rol.ToUpper();
                model.Status = model.Status.ToUpper();
                model.Created = DateTime.UtcNow;

                var user = new IdentityUser
                {
                    UserName = model.Email,
                    NormalizedUserName = model.Email,
                    PhoneNumber = model.Phone,
                    Email = model.Email,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };
                if (user.Email != null)
                    user.NormalizedEmail = model.Email.ToUpper();

                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                result = await _userManager.AddToRoleAsync(user, model.Rol);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                await db.User.AddAsync(model);
                await db.SaveChangesAsync();

                return Ok("ok");
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

                    string filePath = Path.Combine("wwwroot", "images", "Tarifas Registros 2025.docx");
                    if (System.IO.File.Exists(filePath))
                    {
                        Attachment attachment = new Attachment(filePath);
                        mail.Attachments.Add(attachment);

                        Console.WriteLine("Adjunto añadido.");
                    }
                    else
                    {
                        Console.WriteLine("El archivo no se encuentra en la ruta especificada.");
                        return BadRequest("Archivo no encontrado.");
                    }



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
                                        <h2>Buenos Aires, 26 de enero de 2025</h2>
                                        <p>Señor Criador:</p>
                                        <p>Tenemos el agrado de dirigirnos a usted con el objeto de enviarle la documentación relacionada con los controles de procreos y las nuevas tarifas vigentes a partir del 1 de enero de 2025.</p>
                                        <p>Recuerde que, para una mejor organización de los controles, las <strong>SOLICITUDES DE INSPECCIÓN</strong> junto con el correspondiente <strong>ADELANTO</strong> deberán enviarse <strong>HASTA EL 15 DE MARZO PRÓXIMO</strong>, para evitar recargos por presentación fuera de término. Además, es importante regularizar cualquier saldo pendiente para evitar inconvenientes.</p>
                                        <p>En la <strong>SOLICITUD DE INSPECCIÓN</strong> adjunta, el adelanto se calcula automáticamente, por lo que sólo se debe completar los campos requeridos en cantidad de animales solicitados y su año de nacimiento.</p>
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

                    string filePath = Path.Combine(projectRoot, "wwwroot", "images", "Tarifas Registros 2025.docx");
                    if (System.IO.File.Exists(filePath))
                    {
                        Attachment attachment = new Attachment(filePath);
                        mail.Attachments.Add(attachment);
                    }

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
            //public string? Apellido { get; set; }
            //public string Email { get; set; }

            public string? Tipo { get; set; }
            public string? Clase { get; set; }
            public string? Detalle { get; set; }
        }
        [HttpPost("SendResetMail")]
        public async Task<ActionResult> SendResetMail([FromBody] User model, string nuevaContraseña)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    Console.WriteLine("Entro por lo mens");

                    mail.From = new MailAddress("planteles@hereford.org.ar");
                    mail.To.Add(model.Email);
                    mail.Subject = "Hereford - Restablecimiento de contraseña.";

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
                                        <h2>Buenos Aires, 26 de enero de 2025</h2>
                                        <p>Señor Criador:</p>
                                        <p>Tenemos el agrado de dirigirnos a usted con el objeto de enviarle la documentación relacionada con los controles de procreos y las nuevas tarifas vigentes a partir del 1 de enero de 2025.</p>
                                        <p>Recuerde que, para una mejor organización de los controles, las <strong>SOLICITUDES DE INSPECCIÓN</strong> junto con el correspondiente <strong>ADELANTO</strong> deberán enviarse <strong>HASTA EL 15 DE MARZO PRÓXIMO</strong>, para evitar recargos por presentación fuera de término. Además, es importante regularizar cualquier saldo pendiente para evitar inconvenientes.</p>
                                        <p>En la <strong>SOLICITUD DE INSPECCIÓN</strong> adjunta, el adelanto se calcula automáticamente, por lo que sólo se debe completar los campos requeridos en cantidad de animales solicitados y su año de nacimiento.</p>
                                        <p>Les informamos que, a partir de este momento, el sistema de autogestión anterior ya no estará en funcionamiento. Hemos implementado una nueva plataforma para mejorar la gestión y facilitarles el acceso a los servicios. Puede acceder a su perfil <a href='https://herefordapp.com.ar:1050/'>aquí</a>.</p>
                                        <p><strong>Detalles de inicio de sesión:</strong></p>
                                        <p>Correo electrónico registrado: {model.Email}<br>Contraseña: '{nuevaContraseña}'</p>
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

                    string filePath = Path.Combine(projectRoot, "wwwroot", "images", "Tarifas Registros 2025.docx");
                    if (System.IO.File.Exists(filePath))
                    {
                        Attachment attachment = new Attachment(filePath);
                        mail.Attachments.Add(attachment);
                    }

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


        //[HttpPut("SaveUser")]
        //public async Task<ActionResult> SaveUser([FromBody] User model)
        //{
        //    try
        //    {
        //        var Usuario = db.User.FirstOrDefault(x => x.Id == model.Id);
        //        if (Usuario == null)
        //        {
        //            return BadRequest("No se pudo encontrar un usuario");
        //        }
        //        string oldDni = Usuario.Dni;

        //        Usuario.Dni = model.Dni.ToUpper();
        //        Usuario.Names = model.Names.ToUpper();
        //        Usuario.LastNames = model.LastNames.ToUpper();
        //        Usuario.Rol = model.Rol.ToUpper();
        //        Usuario.Status = model.Status.ToUpper();
        //        Usuario.Phone = model.Phone.ToUpper(); ;
        //        Usuario.Email = model.Email.ToUpper(); ;

        //        var user = db.Users.FirstOrDefault(x => x.UserName == oldDni);
        //        if (user == null)
        //        {
        //            return BadRequest("No se pudo encontrar un usuario de acceso");
        //        }

        //        user.UserName = model.Email.ToUpper(); ;
        //        user.NormalizedUserName = model.Email.ToUpper(); ;
        //        user.PhoneNumber = model.Email.ToUpper(); ;
        //        user.Email = model.Email.ToUpper(); ;
        //        if (user.Email != null)
        //            user.NormalizedEmail = Usuario.Email.ToUpper();

               

        //        var result = await _userManager.UpdateAsync(user);
        //        if (!result.Succeeded)
        //        {
        //            return BadRequest(result.Errors);
        //        }

        //        var resultRole = await _userManager.RemoveFromRoleAsync(user, Usuario.Rol);
        //        if (!result.Succeeded)
        //        {
        //            return BadRequest(result.Errors);
        //        }

        //        resultRole = await _userManager.AddToRoleAsync(user, model.Rol);
        //        if (!resultRole.Succeeded)
        //        {
        //            return BadRequest(resultRole.Errors);
        //        }

        //        await db.SaveChangesAsync();
        //        return Ok("ok");
        //    }
        //    catch (Exception e)
        //    {

        //        return BadRequest(e.Message);
        //    }
        //}

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
                public string Password { get; set; }
            }

[HttpPut("ResetPassword")]
public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordModel model)
{
    try
    {
        var Usuario = db.User.FirstOrDefault(x => x.Id == model.UserId);
        if (Usuario == null)
        {
            return BadRequest("No se pudo encontrar un usuario");
        }

        var user = db.Users.FirstOrDefault(x => x.UserName == Usuario.Email);
        if (user == null)
        {
            return BadRequest("No se pudo encontrar un usuario de acceso");
        }

        var result = await _userManager.RemovePasswordAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        result = await _userManager.AddPasswordAsync(user, model.Password);
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

        [HttpGet("GetUsers/{search}/{status}/{actualPage}")]
        public async Task<ActionResult> GetUsers(string search, string status, int actualPage)
        {
            try
            {


                if (search == "TODO")
                    search = "";
                if (status == "TODO")
                    status = "";

                search = search.ToUpper();
                int allRegisters = db.User.Where(x => (x.Names.Contains(search) || x.LastNames.Contains(search) || x.Dni.Contains(search)) && x.Status.Contains(status) && x.Rol != "USUARIOMAESTRO").Count();
                if (allRegisters <= 0)
                {
                    return NotFound(Utilities.MSGNODATA);
                }

                IList<User> entities = db.User.Where(x => (x.Names.Contains(search) || x.LastNames.Contains(search) || x.Dni.Contains(search)) && x.Status.Contains(status) && x.Rol != "USUARIOMAESTRO")
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
