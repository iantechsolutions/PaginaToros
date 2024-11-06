using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PaginaToros.Shared.Models
{
    public partial class ZonaDTO
    {
        public string Zcod { get; set; } = null!;
        public string? Meses { get; set; }
        public string? Inspec { get; set; }
        public string? Codpro { get; set; }
        public string? Locali { get; set; }
        public int? CodUsu { get; set; }
        public int Id { get; set; }
        public DateTime? FchUsu { get; set; }
        [JsonIgnore]
        public Inspect? Inspector { get; set; }
        [JsonIgnore]
        public List<Estable>? Estables { get; set; }
    }
}
