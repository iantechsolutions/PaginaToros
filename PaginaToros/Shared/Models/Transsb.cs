using System;
using System.Collections.Generic;

namespace PaginaToros.Shared.Models
{
    public partial class Transsb
    {
        public string NroTrans { get; set; } = null!;
        public DateTime? Fectrans { get; set; }
        public string? NroOrden { get; set; }
        public string? Sven { get; set; }
        public string? CategSv { get; set; }
        public string? Vnom { get; set; }
        public string? Scom { get; set; }
        public string? CategSc { get; set; }
        public string? Cnom { get; set; }
        public string? Ecod { get; set; }
        public DateTime? FchUsu { get; set; }
        public int? CodUsu { get; set; }
        public int Id { get; set; }
        public Estable? Establecimiento { get; set; }
    }
}
