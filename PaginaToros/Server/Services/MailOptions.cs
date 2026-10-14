namespace PaginaToros.Server.Services
{
    /// <summary>
    /// Configuración del envío de mails (sección "Mail").
    /// Servidor, cuenta y casillas van en appsettings.json. La contraseña NO se
    /// versiona: en el servidor sale de la variable de entorno <c>Mail__Password</c>
    /// y en desarrollo puede ir en appsettings.Local.json.
    /// </summary>
    public sealed class MailOptions
    {
        public const string SectionName = "Mail";

        public string? Host { get; set; }
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;

        /// <summary>Cuenta con la que se autentica contra el servidor SMTP.</summary>
        public string? UserName { get; set; }

        public string? Password { get; set; }

        /// <summary>Remitente de los mails. Si queda vacío se usa <see cref="UserName"/>.</summary>
        public string? From { get; set; }

        /// <summary>
        /// Casilla de Planteles: recibe copia de los avisos (transferencias,
        /// solicitudes de inspección, cambios) y figura como contacto en los mails.
        /// </summary>
        public string? PlantelesAddress { get; set; }

        public string SenderAddress => string.IsNullOrWhiteSpace(From) ? UserName?.Trim() ?? string.Empty : From.Trim();

        /// <summary>Claves que faltan para poder enviar. Nunca incluye valores.</summary>
        public IReadOnlyList<string> GetMissingSettings()
        {
            var missing = new List<string>();

            if (string.IsNullOrWhiteSpace(Host))
                missing.Add($"{SectionName}:{nameof(Host)}");
            if (Port <= 0 || Port > 65535)
                missing.Add($"{SectionName}:{nameof(Port)}");
            if (string.IsNullOrWhiteSpace(UserName))
                missing.Add($"{SectionName}:{nameof(UserName)}");
            if (string.IsNullOrEmpty(Password))
                missing.Add($"{SectionName}:{nameof(Password)}");
            if (string.IsNullOrWhiteSpace(PlantelesAddress))
                missing.Add($"{SectionName}:{nameof(PlantelesAddress)}");

            return missing;
        }
    }
}
