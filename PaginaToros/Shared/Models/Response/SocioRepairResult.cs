using System.Collections.Generic;

namespace PaginaToros.Shared.Models.Response
{
    public class SocioRepairResult
    {
        public int TotalSocios { get; set; }
        public int SociosConScodFaltante { get; set; }
        public int SociosReparados { get; set; }
        public int SociosReparadosConCodpos2 { get; set; }
        public int SociosReparadosConScodGenerado { get; set; }
        public int SociosSinResolver { get; set; }
        public List<string> Observaciones { get; set; } = new();
    }
}
