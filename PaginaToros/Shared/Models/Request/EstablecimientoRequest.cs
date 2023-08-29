namespace PaginaToros.Shared.Models.Request
{
    public class EstablecimientoRequest
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public int? ConsultasRealizadas { get; set; }

        public DateTime? Fecha { get; set; }
    }
}
