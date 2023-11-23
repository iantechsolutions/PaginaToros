using System;
using System.Collections.Generic;

namespace PaginaToros.Shared.Models
{
    public partial class EstableDTO
    {
        public int Id { get; set; }
        public string? Ecod { get; set; }
        public string? Codsoc { get; set; }
        public string? Catego { get; set; }
        public string? Nombre { get; set; }
        public string? Direcc { get; set; }
        public string? Locali { get; set; }
        public string? Codpos { get; set; }
        public string? Codpro { get; set; }
        public string? Plano { get; set; }
        public string? Codzon { get; set; }
        public string? Activo { get; set; }
        public DateTime? FchUsu { get; set; }
        public int? CodUsu { get; set; }
        public string Fecing { get; set; } = null!;
        public string? Fechacreacion { get; set; }
        public string? Encargado { get; set; }
        public string? Tel { get; set; }
        public Socio Socio { get; set; }
    }
}
