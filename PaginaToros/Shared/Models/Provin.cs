using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PaginaToros.Shared.Models
{
    public partial class Provin
    {
        public string? Pcod { get; set; }
        public string? Nombre { get; set; }

        public DateTime? FchUsu { get; set; }
        public int? CodUsu { get; set; }
        public int Id { get; set; }
        [JsonIgnore]
        public List<Estable>? Establecimientos { get; set; }
        [JsonIgnore]
        public List<Inspect>? Inspectores { get; set; }
        [JsonIgnore]
        public List<Socio>? Socios { get; set; }
    }
}
