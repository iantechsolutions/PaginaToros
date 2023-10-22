using System;
using System.Collections.Generic;

namespace PaginaToros.Server.Models
{
    public partial class Zona
    {
        public string Zcod { get; set; } = null!;
        public string? Meses { get; set; }
        public string? Inspec { get; set; }
        public string? Codpro { get; set; }
        public string? Locali { get; set; }
        public int? CodUsu { get; set; }
        public int Id { get; set; }
        public DateTime? FchUsu { get; set; }
    }
}
