using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace PaginaToros.Server.Services
{
    public sealed class SmtpMailSender : IMailSender
    {
        private readonly IOptionsMonitor<MailOptions> _options;
        private readonly ILogger<SmtpMailSender> _logger;

        public SmtpMailSender(IOptionsMonitor<MailOptions> options, ILogger<SmtpMailSender> logger)
        {
            _options = options;
            _logger = logger;
        }

        public bool IsConfigured => _options.CurrentValue.GetMissingSettings().Count == 0;

        public string PlantelesAddress => _options.CurrentValue.PlantelesAddress?.Trim() ?? string.Empty;

        public async Task SendAsync(MailMessage message, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(message);

            var options = _options.CurrentValue;
            var missing = options.GetMissingSettings();
            if (missing.Count > 0)
            {
                throw new InvalidOperationException(
                    $"El envío de mails no está configurado: falta {string.Join(", ", missing)}.");
            }

            message.From ??= new MailAddress(options.SenderAddress);

            using var smtp = new SmtpClient(options.Host!.Trim(), options.Port)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(options.UserName!.Trim(), options.Password),
                EnableSsl = options.EnableSsl
            };

            try
            {
                await smtp.SendMailAsync(message, cancellationToken);
            }
            catch (SmtpException ex) when (IsMissingAuthentication(ex))
            {
                // SmtpClient no avisa cuando el servidor rechaza el login (535): sigue
                // sin autenticar y el error recién aparece al declarar el remitente, como
                // "SMTP AUTH is required...". Sin esta traducción parece un bug de código.
                _logger.LogWarning(ex,
                    "El servidor SMTP {Host}:{Port} no aceptó la cuenta {UserName}.",
                    options.Host, options.Port, options.UserName);
                throw new SmtpException(
                    $"El servidor de correo no aceptó la cuenta {options.UserName}: revisar {MailOptions.SectionName}:{nameof(MailOptions.UserName)} " +
                    $"y {MailOptions.SectionName}:{nameof(MailOptions.Password)}. Respuesta del servidor: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "No se pudo enviar el mail \"{Subject}\" por {Host}:{Port}.",
                    message.Subject, options.Host, options.Port);
                throw;
            }
        }

        private static bool IsMissingAuthentication(SmtpException ex) =>
            ex.Message.Contains("AUTH", StringComparison.OrdinalIgnoreCase);
    }
}
