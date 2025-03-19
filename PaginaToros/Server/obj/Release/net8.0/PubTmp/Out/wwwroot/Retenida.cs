using System;
using System.Collections.Generic;

namespace PaginaToros.Server.Models
{
    public partial class Retenida
    {
        public string Nrodec { get; set; } = null!;
        public string? Nroplan { get; set; }
        public string? Desde { get; set; }
        public string? Hasta { get; set; }
        public string? Fecdecl { get; set; }
        public string? Fchrecepcion { get; set; }
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
        public string? NrFolio { get; set; }
        public DateTime? FchUsu { get; set; }
        public int? CodUsu { get; set; }
        public int Id { get; set; }
        public string Reten { get; set; } = null!;
    }
}
