using System;
using System.Collections.Generic;

namespace PaginaToros.Shared.Models
{
    public partial class Desepla1DTO
    {
        public string Nrodec { get; set; } = null!;
        public string? Nroplan { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        public DateTime? Fecdecl { get; set; }
        public DateTime? Fchrecepcion { get; set; }
        public string? Tipse { get; set; }
        public string? Semen { get; set; }
        public double? Cantv { get; set; }
        public double? Cantb { get; set; }
        public double? Remba { get; set; }
        public double? Rempr { get; set; }
        public bool? Semprop { get; set; }
        public double? Cantor { get; set; }
        public double? Remmpr { get; set; }
        public bool? Ctrlu { get; set; }
        public bool? Ctrlm { get; set; }
        public double? CoefAutoSn { get; set; }
        public double? CoefAutoIa { get; set; }
        public double? CoefAutoIar { get; set; }
        public float? IaSincro { get; set; }
        public int? PastillasSincro { get; set; }
        public string? Fecret { get; set; }
        public int? NrFolio { get; set; }
        public DateTime? FchUsu { get; set; }
        public int? CodUsu { get; set; }
        public int Id { get; set; }
        public string Reten { get; set; } = "0";
        public string Edicion { get; set; } = "0";
        public string Nrocri { get; set; } = null!;
        public Socio Socio { get; set; }
        public Plantel Plantel { get; set; }

    }
}
