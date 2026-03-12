using System.Collections.Generic;

namespace PaginaToros.Shared.Models
{
    public class SocioRegistrationRequest
    {
        public SocioDTO Socio { get; set; } = new();
    }

    public class SocioRegistrationResult
    {
        public bool UsuarioCreado { get; set; }
        public bool UsuarioReiniciado { get; set; }
        public string? CodigoError { get; set; }
        public string? MensajeUsuario { get; set; }
        public int? SocioAsociadoId { get; set; }
        public string? SocioAsociadoCodigo { get; set; }
        public string? SocioAsociadoNombre { get; set; }
        public string? MailAsociado { get; set; }
        public SocioDTO? Socio { get; set; }
        public List<string> Errores { get; set; } = new();
    }
}
