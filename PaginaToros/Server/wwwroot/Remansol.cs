using System;
using System.Collections.Generic;

namespace PaginaToros.Server.Models
{
    public partial class Remansol
    {
        public string Nrodec { get; set; } = null!;
        public string? Nroplan { get; set; }
        public string? Nrosol { get; set; }
        public string? Remantoropr { get; set; }
        public string? Remanvqpr { get; set; }
        public string? Remanvqvip { get; set; }
        public DateTime? FchUsu { get; set; }
        public int? CodUsu { get; set; }
        public int Id { get; set; }
        public int Nrosolanual { get; set; }
        public string? Anio { get; set; }
        public string? Disptoropr { get; set; }
        public string? Dispvqpr { get; set; }
        public string? Dispvqvip { get; set; }
        public string? Auttoropr { get; set; }
        public string? Autvqpr { get; set; }
        public string? Autvqvip { get; set; }
        public string? Pedtoropr { get; set; }
        public string? Pedvqpr { get; set; }
        public string? Pedvqvip { get; set; }
    }
}
