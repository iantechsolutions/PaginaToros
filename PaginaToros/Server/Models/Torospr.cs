using System;
using System.Collections.Generic;

namespace PaginaToros.Server.Models
{
    public partial class Torospr
    {
        public string? Apodo { get; set; }
        public string? Nombre { get; set; }
        public string? EstDoc { get; set; }
        public string? ResInsp { get; set; }
        public string? Sbcod { get; set; }
        public string? TipToro { get; set; }
        public string? Tatpart { get; set; }
        public string? NomDad { get; set; }
        public string? NrInsc { get; set; }
        public string? NrTsan { get; set; }
        public string? NrInsd { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Hba { get; set; }
        public string? Variedad { get; set; }
        public string? Criador { get; set; }
        public string? Catego { get; set; }
        public string? Plantel { get; set; }
        public string? Estcod { get; set; }
        public DateTime? FchBaja { get; set; }
        public bool? Activo { get; set; }
        public string? MotivoBaj { get; set; }
        public DateTime? Nacido { get; set; }
        public bool? Actualizado { get; set; }
        public string? CircEscrotal { get; set; }
        public string? CodEstado { get; set; }
        public int? IdTipo { get; set; }
        public DateTime? FchUsu { get; set; }
        public int? CodUsu { get; set; }
        public string Fecing { get; set; } = null!;
        public int Id { get; set; }
    }
}
