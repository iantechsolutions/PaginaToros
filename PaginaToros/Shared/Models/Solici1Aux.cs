using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaginaToros.Shared.Models
{
    public partial class Solici1Aux
    {
        public int Id { get; set; }
        public int IdSoli { get; set; }
        public string? Anio { get; set; }
        public double? Cantor { get; set; }
        public double? Cantvq { get; set; }
        public double? Canvac { get; set; }
        public double? Canvaq { get; set; }
        public Solici1? Solicitud { get; set; }
    }
}
