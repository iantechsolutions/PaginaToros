namespace PaginaToros.Shared.Models
{
    public class TransanSaveRequestDTO
    {
        public TransanDTO Transan { get; set; } = new();
        public int VendedorId { get; set; }
        public int? CompradorId { get; set; }
        public string? MailComprador { get; set; }
        public int? PlantOrigenId { get; set; }
        public int? PlantDestinoId { get; set; }
    }
}
