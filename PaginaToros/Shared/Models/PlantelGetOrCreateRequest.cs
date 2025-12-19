namespace PaginaToros.Shared.Models
{
    public class PlantelGetOrCreateRequest
    {
        public string? Placod { get; set; }
        public string? BasePlacod { get; set; }
        public string Anioex { get; set; } = string.Empty;
        public int Nrocri { get; set; }

        public int Varede { get; set; }
        public int Vqcsrd { get; set; }
        public int Vqssrd { get; set; }
        public int Varepr { get; set; }
        public int Vqcsrp { get; set; }
        public int Vqssrp { get; set; }
    }
}