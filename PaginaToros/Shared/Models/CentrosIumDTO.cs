using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PaginaToros.Shared.Models
{
    public partial class CentrosiumDTO
    {
        public string Nrocen { get; set; } = null!;
        public string? Nombre { get; set; }
        public string? NroCSayg { get; set; }
        public DateTime? FchUsu { get; set; }
        public int? CodUsu { get; set; }
        public int Id { get; set; }
        [JsonIgnore]
        public List<Certifseman>? Certificados { get; set; }
    }
}