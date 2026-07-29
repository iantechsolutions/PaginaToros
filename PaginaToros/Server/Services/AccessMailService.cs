using PaginaToros.Shared.Models;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace PaginaToros.Server.Services
{
    public class AccessMailService : IAccessMailService
    {
        public Task<(bool Success, string? ErrorMessage)> SendAccessMailAsync(User model, string password, AccessMailTemplate template)
        {
            try
            {
                using var mail = new MailMessage();
                mail.From = new MailAddress("planteles@hereford.org.ar");
                mail.To.Add(model.Email);
                mail.Subject = template == AccessMailTemplate.Registration
                    ? "Hereford - Puro Registrado."
                    : "Hereford - Restablecimiento de contraseña.";

                string projectRoot = Directory.GetCurrentDirectory();
                string imagePath = Path.Combine(projectRoot, "wwwroot", "images", "backgroundEnvio.jpg");
                string logoPath = Path.Combine(projectRoot, "wwwroot", "images", "LOGO.jpg");

                bool esOutlook = model.Email?.IndexOf("outlook", StringComparison.OrdinalIgnoreCase) >= 0;
                if (esOutlook)
                {
                    logoPath = string.Empty;
                    imagePath = string.Empty;
                }

                if (!string.IsNullOrEmpty(imagePath) && !File.Exists(imagePath))
                {
                    return Task.FromResult<(bool Success, string? ErrorMessage)>((false, "No se encontró la imagen de fondo del correo."));
                }

                if (!string.IsNullOrEmpty(logoPath) && !File.Exists(logoPath))
                {
                    return Task.FromResult<(bool Success, string? ErrorMessage)>((false, "No se encontró el logo del correo."));
                }

                var htmlView = AlternateView.CreateAlternateViewFromString(
                    BuildBody(model, password, imagePath, logoPath),
                    null,
                    MediaTypeNames.Text.Html);

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
                return Task.FromResult<(bool Success, string? ErrorMessage)>((true, null));
            }
            catch (Exception ex)
            {
                return Task.FromResult<(bool Success, string? ErrorMessage)>((false, ex.Message));
            }
        }

        private static string BuildBody(User model, string password, string imagePath, string logoPath)
        {
            var logoHtml = string.IsNullOrEmpty(logoPath)
                ? string.Empty
                : "<img src='cid:logoImage' alt='Hereford Logo' style='width:150px; height:auto; display:block;' />";

            var fecha = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));
            var nombre = WebUtility.HtmlEncode(model.Names ?? "criador");
            var apellido = WebUtility.HtmlEncode(model.LastNames ?? string.Empty);
            var email = WebUtility.HtmlEncode(model.Email);
            var safePassword = WebUtility.HtmlEncode(password);
            var backgroundRef = string.IsNullOrEmpty(imagePath) ? "" : "cid:backgroundImage";

            return $@"
                <html>
                <body style='margin:0;padding:0;'>
                  <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                    <tr>
                      <td>
                        <table width='600' border='0' cellspacing='0' cellpadding='0' align='center' style='background-color:#ffffff;background-repeat:no-repeat;background-image:url({backgroundRef});background-size:cover;'>
                          <tr>
                            <td style='padding:20px; text-align:left;'>
                              {logoHtml}
                            </td>
                          </tr>
                          <tr>
                            <td style='padding:20px; padding-top:10px; color:#000000; font-family:Arial, Helvetica, sans-serif; line-height:1.5;'>
                              <h2 style='margin:0 0 16px 0; color:#000000;'>Buenos Aires, {fecha}</h2>
                              <p style='margin:0 0 14px 0; color:#000000;'>Señor {nombre} {apellido}:</p>
                              <p style='margin:0 0 14px 0; color:#000000;'>Les informamos que, a partir de este momento, el sistema de autogestión anterior ya no estará en funcionamiento. Hemos implementado una nueva plataforma para mejorar la gestión y facilitarles el acceso a los servicios. Puede acceder a su perfil <a href='https://herefordapp.com.ar:1050/'>aquí</a>.</p>
                              <p style='margin:0 0 14px 0; color:#000000;'><strong>Detalles de inicio de sesión:</strong></p>
                              <p style='margin:0 0 14px 0; color:#000000;'>Correo electrónico registrado: {email}<br />Contraseña: <code style='font-size:16px; color:#000000;'>{safePassword}</code></p>
                              <p style='margin:0 0 14px 0; color:#000000;'>Recuerde mantener segura esta información y no compartirla. Ante cualquier consulta escriba a <a href='mailto:planteles@hereford.org.ar'>planteles@hereford.org.ar</a>.</p>
                              <p style='margin:0; color:#000000;'>Gracias por su comprensión y colaboración.</p>
                            </td>
                          </tr>
                          <tr>
                            <td style='padding:20px; padding-top:25px; color:#777777; font-family:Arial, Helvetica, sans-serif;'>
                              <p>Paz Hernández (Encargada Registros) - <a href='mailto:planteles@hereford.org.ar'>planteles@hereford.org.ar</a></p>
                            </td>
                          </tr>
                          <tr>
                            <td style='height:200px;'></td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </body>
                </html>";
        }
    }
}
