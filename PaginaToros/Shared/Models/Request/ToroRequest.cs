namespace PaginaToros.Shared.Models.Request
{
    public class ToroRequest
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public int? Calidad { get; set; }

        public int? IdEst { get; set; }

        public string? NombreEst { get; set; }
    }
}
