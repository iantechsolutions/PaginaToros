using System;

namespace PaginaToros.Shared.Models
{
    public partial class TransanMailOutbox
    {
        public int Id { get; set; }
        public int TransanId { get; set; }
        public string Accion { get; set; } = string.Empty;
        public string Asunto { get; set; } = string.Empty;
        public string CuerpoHtml { get; set; } = string.Empty;
        public string Destinatarios { get; set; } = string.Empty;
        public string? MailVendedor { get; set; }
        public string? MailComprador { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public int Intentos { get; set; }
        public string? UltimoError { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? UltimoIntento { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public DateTime? ProximoIntento { get; set; }
        public int? PlantelOrigenId { get; set; }
        public int? PlantelDestinoId { get; set; }
        public string? PlantelOrigenCodigo { get; set; }
        public string? PlantelDestinoCodigo { get; set; }
    }
}
