using System;
using System.Collections.Generic;

namespace PaginaToros.Server.Models
{
    public partial class Usuario
    {
        public string User { get; set; } = null!;
        public string Pass { get; set; } = null!;
        public string Passmd5 { get; set; } = null!;
        public string Nrousu { get; set; } = null!;
        public string Ultimavis { get; set; } = null!;
        public string Ultimahora { get; set; } = null!;
        public int Id { get; set; }
    }
}
