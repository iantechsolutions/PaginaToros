using System;

namespace PaginaToros.Shared.Models
{
    public partial class TransanTransferAudit
    {
        public int Id { get; set; }
        public int TransanId { get; set; }
        public string Accion { get; set; } = string.Empty;
        public DateTime FechaEvento { get; set; }
        public int? UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }
        public string? UsuarioRol { get; set; }
        public int? VendedorId { get; set; }
        public int? CompradorId { get; set; }
        public int? PlantelOrigenId { get; set; }
        public int? PlantelDestinoId { get; set; }
        public string? VendedorCodigo { get; set; }
        public string? CompradorCodigo { get; set; }
        public string? PlantelOrigen { get; set; }
        public string? PlantelDestino { get; set; }
        public string? PlantelOrigenCodigo { get; set; }
        public string? PlantelOrigenAnioex { get; set; }
        public string? PlantelDestinoCodigo { get; set; }
        public string? PlantelDestinoAnioex { get; set; }
        public string? BucketCampo { get; set; }
        public string? BucketEtiqueta { get; set; }
        public string? Tiphac { get; set; }
        public string? Hemsta { get; set; }
        public string? Tipohem { get; set; }
        public int? CantHem { get; set; }
        public int? CantMach { get; set; }
        public int? CantChem { get; set; }
        public int? CantCmach { get; set; }
        public double? OrigenAntes { get; set; }
        public double? OrigenDespues { get; set; }
        public double? DestinoAntes { get; set; }
        public double? DestinoDespues { get; set; }
        public string? MailVendedor { get; set; }
        public string? MailComprador { get; set; }
        public string? MailEstado { get; set; }
        public string? MailError { get; set; }
        public string? Detalle { get; set; }
    }
}
