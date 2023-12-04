using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaginaToros.Shared.Models
{
    public partial class PlantelDTO
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
        public string? Estado { get; set; } = null!;
        public string? Fecing { get; set; } = null!;
        public int Id { get; set; }
        public Socio? Socio { get; set; }
    }
}
