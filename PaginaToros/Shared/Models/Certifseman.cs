using PaginaToros.Shared.JsonConversor;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PaginaToros.Shared.Models
{
    public partial class Certifseman
    {
        public string? TipoCert { get; set; }
        public string? NroConst { get; set; }
        public string? NroCert { get; set; }
        public string? Nrocen { get; set; }

        //[JsonConverter(typeof(DateOnlyJsonConverter))] 
        public DateTime? Fecvta { get; set; }
        public DateTime? FchConst { get; set; }
        public string? Nven { get; set; }
        public string? Nrocri { get; set; }
        public string? CategSc { get; set; }
        public string? Scod { get; set; }
        public string? Tatpart { get; set; }
        public string? Hba { get; set; }
        public string? NomDad { get; set; }
        public string? NrInsc { get; set; }
        public string? NrTsan { get; set; }
        public string? NrInsd { get; set; }
        public int? NrDosi { get; set; }
        public string? NrDosiOr { get; set; }
        public string? TipEnv { get; set; }
        public string? Variedad { get; set; }
        public DateTime? FchUsu { get; set; }
        public int? CodUsu { get; set; }
        public int Id { get; set; }
        public string? Apodo { get; set; }
        public Socio Socio { get; set; }
        public Centrosium Centro { get; set; }
    }
}
