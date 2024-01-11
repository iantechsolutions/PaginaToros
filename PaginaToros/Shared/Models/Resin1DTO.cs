using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PaginaToros.Shared.Models
{
    public partial class Resin1DTO
    {
        public string? Nrores { get; set; }
        public string? Nropla { get; set; }
        public DateTime? Freali { get; set; }
        public string? Observ { get; set; }
        public int? Ppajust { get; set; }
        public int? Epromedio { get; set; }
        public int? Emax { get; set; }
        public int? Emin { get; set; }
        public int? Tortot { get; set; }
        public int? Torsb { get; set; }
        public DateTime? FchUsu { get; set; }
        public int? CodUsu { get; set; }
        public int Id { get; set; }
        public int Editar { get; set; }
        public string? Icod { get; set; } = null!;
        public string? Scod { get; set; } = null!;
        public string? Estcod { get; set; }
        public Socio? Socio { get; set; }
        public Estable? Establecimiento { get; set; }
        [JsonIgnore]
        public List<Resin6>? Resin6s { get; set; }
    }
}
