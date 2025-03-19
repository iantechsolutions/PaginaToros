using System;
using System.Collections.Generic;

namespace PaginaToros.Server.Models
{
    public partial class Peg
    {
        public int Id { get; set; }
        public string Nrodec { get; set; } = null!;
        public string Nrosocio { get; set; } = null!;
        public string Socio { get; set; } = null!;
        public string Hba { get; set; } = null!;
        public string Desde { get; set; } = null!;
        public string Hasta { get; set; } = null!;
        public string Cvientres { get; set; } = null!;
        public string Servicio { get; set; } = null!;
        public string? Apodo { get; set; }
    }
}
