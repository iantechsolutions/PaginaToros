using System;
using System.Collections.Generic;

namespace PaginaToros.Shared.Models
{
    public partial class FutcontrolDTO
    {
        public string? NroTrans { get; set; }
        public DateTime? Fectrans { get; set; }
        public string? Sven { get; set; }
        public string? CategSv { get; set; }
        public string? Vnom { get; set; }
        public string? Scom { get; set; }
        public string? CategSc { get; set; }
        public string? Cnom { get; set; }
        public string? Plantel { get; set; }
        public int? EdadCrias { get; set; }
        public int? CantHem { get; set; }
        public int? CantMach { get; set; }
        public string? PlantDest { get; set; }
        public sbyte? Incorp { get; set; }
        public string? Hemsta { get; set; }
        public DateTime? FchUsu { get; set; }
        public int? CodUsu { get; set; }
        public int Id { get; set; }
    }
}
