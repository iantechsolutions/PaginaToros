using System;
using System.Collections.Generic;

namespace PaginaToros.Shared.Models.Response
{
    public class Desepla1PlantelAmbiguityRepairResult
    {
        public int TotalDeclaraciones { get; set; }
        public int DeclaracionesConPlantel { get; set; }
        public int DeclaracionesCorregidas { get; set; }
        public int DeclaracionesPendientes { get; set; }
        public List<string> Observaciones { get; set; } = new();
        public List<Desepla1PlantelAmbiguityPendingItem> Pendientes { get; set; } = new();
    }

    public class Desepla1PlantelAmbiguityPendingItem
    {
        public int Id { get; set; }
        public string? Nrodec { get; set; }
        public string? Nroplan { get; set; }
        public DateTime? FechaDeclaracion { get; set; }
        public string? Nrocri { get; set; }
        public string? SocioNombre { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}
