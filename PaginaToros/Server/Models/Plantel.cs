using System;
using System.Collections.Generic;

namespace PaginaToros.Server.Models
{
    public partial class Plantel
    {
        public string? Placod { get; set; }
        public string? Anioex { get; set; }
        public double? Varede { get; set; }
        public double? Vqcsrd { get; set; }
        public double? Vqssrd { get; set; }
        public double? Varepr { get; set; }
        public double? Vqcsrp { get; set; }
        public double? Vqssrp { get; set; }
        public DateTime? Feulti { get; set; }
        public string? Nroins { get; set; }
        public string? Nrocri { get; set; }
        public string? Catego { get; set; }
        public string? Aniopa { get; set; }
        public DateTime? Urein { get; set; }
        public DateTime? FchUsu { get; set; }
        public int? CodUsu { get; set; }
        public string? Comment { get; set; }
        public string Estado { get; set; } = null!;
        public string Fecing { get; set; } = null!;
        public int Id { get; set; }
    }
}
