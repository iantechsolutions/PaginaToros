using System.Net.Mail;

namespace PaginaToros.Server.Services
{
    /// <summary>
    /// Único punto de envío de mails de la aplicación. Los controllers y servicios
    /// arman el <see cref="MailMessage"/> y lo mandan por acá; ninguno debe crear
    /// su propio <see cref="SmtpClient"/> ni conocer la cuenta SMTP.
    /// </summary>
    public interface IMailSender
    {
        /// <summary>Hay servidor, cuenta, contraseña y casilla de Planteles configurados.</summary>
        bool IsConfigured { get; }

        /// <summary>Casilla de Planteles configurada (<c>Mail:PlantelesAddress</c>), o vacío si falta.</summary>
        string PlantelesAddress { get; }

        /// <summary>
        /// Envía el mensaje por el servidor SMTP configurado. Si el mensaje no trae
        /// remitente, usa <c>Mail:From</c>. Falla con un mensaje claro si falta
        /// configuración o si el servidor rechaza la cuenta.
        /// </summary>
        Task SendAsync(MailMessage message, CancellationToken cancellationToken = default);
    }
}
