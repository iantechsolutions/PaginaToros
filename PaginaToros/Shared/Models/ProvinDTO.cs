using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PaginaToros.Shared.Models
{
    public partial class ProvinDTO
    {
        public string? Pcod { get; set; }
        public string? Nombre { get; set; }
        [JsonIgnore]
        public List<Estable>? Establecimientos { get; set; }
        public int Id { get; set; }
        [JsonIgnore]
        public List<Inspect>? Inspectores { get; set; }
        [JsonIgnore]
        public List<Socio>? Socios { get; set; }
    }
}
