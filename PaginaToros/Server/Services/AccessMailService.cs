using PaginaToros.Shared.Models;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace PaginaToros.Server.Services
{
    /// <summary>
    /// Mail de acceso (alta / reinicio de contraseña).
    ///
    /// El texto NUNCA va encima de una imagen de fondo: los clientes en modo oscuro
    /// (Apple Mail, Gmail app, Outlook) aclaran el texto pero no la imagen, y el texto
    /// quedaba blanco sobre blanco. Las imágenes son bloques propios (cabecera y pie)
    /// y el texto va sobre un color sólido, que el cliente invierte junto con el texto.
    /// Las imágenes van embebidas en el ensamblado (Resources/Mail) para no depender
    /// del directorio de trabajo del proceso.
    /// </summary>
    public class AccessMailService : IAccessMailService
    {
        public const string HeaderContentId = "mailHeader";
        public const string FooterContentId = "mailFooter";

        private const string AppUrl = "https://herefordapp.com.ar:1050/";

        private static readonly Lazy<byte[]> HeaderImage = new(() => LoadImage("mail-header.jpg"));
        private static readonly Lazy<byte[]> FooterImage = new(() => LoadImage("mail-footer.jpg"));

        private readonly IMailSender _mailSender;

        public AccessMailService(IMailSender mailSender)
        {
            _mailSender = mailSender;
        }

        public async Task<(bool Success, string? ErrorMessage)> SendAccessMailAsync(User model, string password, AccessMailTemplate template)
        {
            try
            {
                using var mail = new MailMessage();
                mail.To.Add(model.Email);
                mail.Subject = template == AccessMailTemplate.Registration
                    ? "Hereford - Puro Registrado."
                    : "Hereford - Restablecimiento de contraseña.";
                mail.SubjectEncoding = Encoding.UTF8;
                mail.BodyEncoding = Encoding.UTF8;

                var fecha = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));

                // Texto plano primero y HTML al final: los clientes muestran la última alternativa que soportan.
                var textView = AlternateView.CreateAlternateViewFromString(
                    BuildTextBody(model, password, fecha, _mailSender.PlantelesAddress),
                    Encoding.UTF8,
                    MediaTypeNames.Text.Plain);

                var htmlView = AlternateView.CreateAlternateViewFromString(
                    BuildHtmlBody(model, password, fecha, _mailSender.PlantelesAddress),
                    Encoding.UTF8,
                    MediaTypeNames.Text.Html);
                htmlView.LinkedResources.Add(CreateImage(HeaderImage.Value, HeaderContentId));
                htmlView.LinkedResources.Add(CreateImage(FooterImage.Value, FooterContentId));

                mail.AlternateViews.Add(textView);
                mail.AlternateViews.Add(htmlView);

                await _mailSender.SendAsync(mail);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        private static LinkedResource CreateImage(byte[] content, string contentId) =>
            new(new MemoryStream(content, writable: false), MediaTypeNames.Image.Jpeg)
            {
                ContentId = contentId,
                TransferEncoding = TransferEncoding.Base64
            };

        private static byte[] LoadImage(string fileName)
        {
            var resourceName = $"PaginaToros.Server.Mail.{fileName}";
            using var stream = typeof(AccessMailService).Assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException($"No se encontró la imagen del correo '{resourceName}' en el ensamblado.");
            using var buffer = new MemoryStream();
            stream.CopyTo(buffer);
            return buffer.ToArray();
        }

        private static string BuildTextBody(User model, string password, string fecha, string plantelesAddress)
        {
            var nombre = $"{model.Names ?? "criador"} {model.LastNames ?? string.Empty}".Trim();
            return $@"Buenos Aires, {fecha}

Señor {nombre}:

Les informamos que, a partir de este momento, el sistema de autogestión anterior ya no estará en funcionamiento. Hemos implementado una nueva plataforma para mejorar la gestión y facilitarles el acceso a los servicios. Puede acceder a su perfil en {AppUrl}

Detalles de inicio de sesión:
Correo electrónico registrado: {model.Email}
Contraseña: {password}

Recuerde mantener segura esta información y no compartirla. Ante cualquier consulta escriba a {plantelesAddress}.

Gracias por su comprensión y colaboración.

Paz Hernández (Encargada Registros) - {plantelesAddress}
";
        }

        private static string BuildHtmlBody(User model, string password, string fecha, string plantelesAddress)
        {
            var nombre = WebUtility.HtmlEncode(model.Names ?? "criador");
            var apellido = WebUtility.HtmlEncode(model.LastNames ?? string.Empty);
            var email = WebUtility.HtmlEncode(model.Email);
            var safePassword = WebUtility.HtmlEncode(password);
            var contacto = WebUtility.HtmlEncode(plantelesAddress);
            var safeFecha = WebUtility.HtmlEncode(fecha);

            // Colores siempre explícitos (texto, fondo y links) y fondo sólido con bgcolor + CSS.
            const string text = "color:#1a1a1a;";
            const string link = "color:#0b5229;text-decoration:underline;";
            const string p = "margin:0 0 14px 0;" + text;

            return $@"<!DOCTYPE html>
<html lang='es'>
<head>
<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
<meta name='viewport' content='width=device-width, initial-scale=1' />
<meta name='color-scheme' content='light only' />
<meta name='supported-color-schemes' content='light only' />
<style>:root {{ color-scheme: light only; supported-color-schemes: light only; }}</style>
</head>
<body style='margin:0;padding:0;background-color:#f2f2f2;'>
<table role='presentation' width='100%' border='0' cellspacing='0' cellpadding='0' bgcolor='#f2f2f2' style='background-color:#f2f2f2;'>
  <tr>
    <td align='center' style='padding:16px 0;'>
      <table role='presentation' width='600' border='0' cellspacing='0' cellpadding='0' bgcolor='#ffffff' style='width:100%;max-width:600px;background-color:#ffffff;'>
        <tr>
          <td style='padding:0;line-height:0;font-size:0;'>
            <img src='cid:{HeaderContentId}' width='600' alt='Hereford Argentina' style='display:block;width:100%;max-width:600px;height:auto;border:0;' />
          </td>
        </tr>
        <tr>
          <td bgcolor='#ffffff' style='padding:16px 32px 8px 32px;background-color:#ffffff;{text}font-family:Arial, Helvetica, sans-serif;font-size:15px;line-height:1.5;'>
            <h2 style='margin:0 0 16px 0;{text}font-size:20px;'>Buenos Aires, {safeFecha}</h2>
            <p style='{p}'>Señor {nombre} {apellido}:</p>
            <p style='{p}'>Les informamos que, a partir de este momento, el sistema de autogestión anterior ya no estará en funcionamiento. Hemos implementado una nueva plataforma para mejorar la gestión y facilitarles el acceso a los servicios. Puede acceder a su perfil <a href='{AppUrl}' style='{link}'>aquí</a>.</p>
            <p style='{p}'><strong style='{text}'>Detalles de inicio de sesión:</strong></p>
            <p style='{p}'>Correo electrónico registrado: <span style='{text}'>{email}</span><br />Contraseña: <span style='{text}font-family:Consolas, &quot;Courier New&quot;, monospace;font-size:16px;background-color:#eeeeee;padding:2px 6px;'>{safePassword}</span></p>
            <p style='{p}'>Recuerde mantener segura esta información y no compartirla. Ante cualquier consulta escriba a <a href='mailto:{contacto}' style='{link}'>{contacto}</a>.</p>
            <p style='margin:0;{text}'>Gracias por su comprensión y colaboración.</p>
          </td>
        </tr>
        <tr>
          <td bgcolor='#ffffff' style='padding:24px 32px 8px 32px;background-color:#ffffff;color:#555555;font-family:Arial, Helvetica, sans-serif;font-size:14px;line-height:1.5;'>
            Paz Hernández (Encargada Registros) - <a href='mailto:{contacto}' style='{link}'>{contacto}</a>
          </td>
        </tr>
        <tr>
          <td style='padding:0;line-height:0;font-size:0;'>
            <img src='cid:{FooterContentId}' width='600' alt='Hereford' style='display:block;width:100%;max-width:600px;height:auto;border:0;' />
          </td>
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
