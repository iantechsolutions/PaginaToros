using System;
using System.Collections.Generic;

namespace PaginaToros.Server.Models
{
    public partial class Inspect
    {
        public string Icod { get; set; } = null!;
        public string? Nombre { get; set; }
        public string? Direcc { get; set; }
        public string? Locali { get; set; }
        public string? Codpos { get; set; }
        public string? Codpro { get; set; }
        public string? Telefo { get; set; }
        public DateTime? FchUsu { get; set; }
        public int? CodUsu { get; set; }
        public int Id { get; set; }
        public string? Mail { get; set; }
    }
}
