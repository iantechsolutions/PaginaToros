﻿using Microsoft.AspNetCore.Identity;
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

        //Metodos

        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUser([FromBody] User model, string password)
        {
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
                    mail.From = new MailAddress("puroregistrado@hotmail.com");
                    mail.To.Add(model.Email);
                    mail.Subject = "Confirmación de Registro en Hereford";

                    // Path to the image
                    string currentDirectory = AppContext.BaseDirectory;
                    Console.WriteLine(currentDirectory);
                    string imagePath = Path.Combine(currentDirectory,"background.jpg");
                    Console.WriteLine(imagePath);
                    Console.WriteLine(model.Email);
                    // HTML body with background image and text on top
                    string body = $@"
                    <html>
                    <body style='margin:0;padding:0;'>
                        <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                            <tr>
                                <td>
                                    <table width='600' border='0' cellspacing='0' cellpadding='0' align='center' style='background-repeat:no-repeat;background-image:url(cid:backgroundImage);background-size:cover;'>
                                        <tr>
                                            <td style='padding: 20px;padding-top: 90px; padding-bottom:300px; color: #000;'>
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

                    // Create an alternate view to hold the HTML content
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

                    // Add the background image as a linked resource
                    LinkedResource background = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg);
                    background.ContentId = "backgroundImage";
                    background.TransferEncoding = TransferEncoding.Base64;
                    htmlView.LinkedResources.Add(background);

                    // Add the HTML view to the email message
                    mail.AlternateViews.Add(htmlView);
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("puroregistrado@hotmail.com", "puro2025");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                return Ok("ok");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("SendResetMail")]
        public async Task<ActionResult> SendResetMail([FromBody] User model, string nuevaContraseña)
        {
            try
            {
                try { 
                Console.WriteLine(model.Email);


                    SmtpClient client = new SmtpClient();
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("puroregistrado@hotmail.com", "puro2025", "hotmail.com");
                    client.Port = 587; // 25 587
                    client.Host = "smtp.office365.com";
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.EnableSsl = true;



                    Console.WriteLine(nuevaContraseña);
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("puroregistrado@hotmail.com");
                    mail.To.Add(new MailAddress(model.Email));
                    mail.Subject = $"Restablecimiento de contraseña para Hereford";
                    string body = $"Estimado,\nHemos recibido una solicitud para restablecer su contraseña. Los detalles de su nueva contraseña son los siguientes:\n";
                    body += $"Correo electrónico registrado: {model.Email}\nNueva contraseña: {nuevaContraseña}\n";
                    body += $"Recuerde mantener esta información segura y no compartirla con nadie más. Gracias por estar con nosotros y esperamos que continúe disfrutando de nuestros servicios.\n";
                    body += $"Saludos cordiales,\n Hereford";

                    mail.Body = body;

                    client.Send(mail);

                return Ok("ok");
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        



        [HttpPut("SaveUser")]
        public async Task<ActionResult> SaveUser([FromBody] User model)
        {
            try
            {
                var Usuario = db.User.FirstOrDefault(x => x.Id == model.Id);
                if (Usuario == null)
                {
                    return BadRequest("No se pudo encontrar un usuario");
                }
                string oldDni = Usuario.Dni;

                Usuario.Dni = model.Dni.ToUpper();
                Usuario.Names = model.Names.ToUpper();
                Usuario.LastNames = model.LastNames.ToUpper();
                Usuario.Rol = model.Rol.ToUpper();
                Usuario.Status = model.Status.ToUpper();
                Usuario.Phone = model.Phone;
                Usuario.Email = model.Email;

                var user = db.Users.FirstOrDefault(x => x.UserName == oldDni);
                if (user == null)
                {
                    return BadRequest("No se pudo encontrar un usuario de acceso");
                }

                user.UserName = model.Dni;
                user.NormalizedUserName = model.Dni;
                user.PhoneNumber = model.Phone;
                user.Email = model.Email;
                if (user.Email != null)
                    user.NormalizedEmail = Usuario.Email.ToUpper();

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                var resultRole = await _userManager.RemoveFromRoleAsync(user, Usuario.Rol);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                resultRole = await _userManager.AddToRoleAsync(user, model.Rol);
                if (!resultRole.Succeeded)
                {
                    return BadRequest(resultRole.Errors);
                }

                await db.SaveChangesAsync();
                return Ok("ok");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

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
