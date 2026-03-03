using System;
using System.Collections.Generic;

namespace PaginaToros.Server.Models
{
    public partial class TorosRural
    {
        public string? Hba { get; set; }
        public string? Tatpart { get; set; }
        public string? Nombre { get; set; }
        public DateTime? FecNac { get; set; }
        public sbyte? Procesado { get; set; }
        public string? TipToro { get; set; }
        public DateTime? FchUsu { get; set; }
        public int? CodUsu { get; set; }
        public int Id { get; set; }
    }
}
