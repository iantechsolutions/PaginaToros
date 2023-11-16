using System;
using System.Collections.Generic;

namespace PaginaToros.Server.Models
{
    public partial class Archrural
    {
        public int Id { get; set; }
        public string? Tat { get; set; }
        public string? Hba { get; set; }
        public string? Tipo { get; set; }
        public string? Nombre { get; set; }
        public DateTime? Nac { get; set; }
    }
}
