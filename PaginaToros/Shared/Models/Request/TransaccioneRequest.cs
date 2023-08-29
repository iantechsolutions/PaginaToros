namespace PaginaToros.Shared.Models.Request
{
    public class TransaccioneRequest
    {
        public int Id { get; set; }

        public string? NombreVendedor { get; set; } = null!;

        public string? NombreComprador { get; set; } = null!;

        public DateTime? Fecha { get; set; }

        public int? TotalVaquillonas { get; set; }

        public int? TotalToros { get; set; }

        public string? Toros { get; set; }
    }
}
