using System;
using System.Collections.Generic;

namespace PaginaToros.Server.Models
{
    public partial class Centrosium
    {
        public string Nrocen { get; set; } = null!;
        public string? Nombre { get; set; }
        public string? NroCSayg { get; set; }
        public DateTime? FchUsu { get; set; }
        public int? CodUsu { get; set; }
        public int Id { get; set; }
    }
}
