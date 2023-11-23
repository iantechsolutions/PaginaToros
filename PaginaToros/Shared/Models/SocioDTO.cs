using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PaginaToros.Shared.Models
{
    public partial class SocioDTO
    {
        public int Id { get; set; }
        public string Scod { get; set; } = null!;
        public string? Catego { get; set; }
        public string? Cuenta { get; set; }
        public string? Prenom { get; set; }
        public string? Nombre { get; set; }
        public string? Posnom { get; set; }
        public string? Direcc1 { get; set; }
        public string? Locali1 { get; set; }
        public string? Codpos1 { get; set; }
        public string? Codpro1 { get; set; }
        public string? Ordalf { get; set; }
        public string? Telefo1 { get; set; }
        public string? Mail { get; set; }
        public string? Criador { get; set; }
        public string? Direcc2 { get; set; }
        public string? Locali2 { get; set; }
        public string? Codpos2 { get; set; }
        public string? Codpro2 { get; set; }
        public string? Telefo2 { get; set; }
        public DateTime? Fecing { get; set; }
        public DateTime? Vtosus { get; set; }
        public string? Envio { get; set; }
        public DateTime? FchUsu { get; set; }
        public int? CodUsu { get; set; }
        public string? Placod { get; set; }
        public string? Mailreg { get; set; }
        public string? Diaregautog { get; set; }
        [JsonIgnore]
        public List<Torosuni>? Torosunis { get; set; }
        [JsonIgnore]
        public List<Plantel>? Planteles { get; set; }
        [JsonIgnore]
        public List<Estable>? Establecimientos { get; set; }
        [JsonIgnore]
        public List<Certifseman>? Certificados { get; set; }
    }
}
