namespace PaginaToros.Shared.Models
{
    public class TorosFilterRequest
    {
        public int Skip { get; set; }
        public int Take { get; set; } = 15;
        public string? SearchText { get; set; }
        public int? SocioId { get; set; }
        public List<int> SocioIds { get; set; } = new();
        public int? EstablecimientoId { get; set; }
        public bool IncluirSinEstablecimiento { get; set; }
        public string? Estado { get; set; }
        public string? TipoToro { get; set; }
        public string? Variedad { get; set; }
        public string? NombreToro { get; set; }
        public int? NroOrden { get; set; }
        public string? Hba { get; set; }
        public string? Tatuaje { get; set; }
        public string? TipAdn { get; set; }
        public DateTime? FechaSDesde { get; set; }
        public DateTime? FechaSHasta { get; set; }
        public DateTime? FechaNacimientoDesde { get; set; }
        public DateTime? FechaNacimientoHasta { get; set; }
    }

    public class TorosPagedResponse
    {
        public List<TorosuniDTO> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }

    public class TorosFilterOptionDTO
    {
        public string Value { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public int? IntValue { get; set; }
    }

    public class TorosFilterMetadataResponse
    {
        public List<SocioLookupItemDTO> Socios { get; set; } = new();
        public List<TorosFilterOptionDTO> Establecimientos { get; set; } = new();
        public List<TorosFilterOptionDTO> Estados { get; set; } = new();
        public List<TorosFilterOptionDTO> TiposToro { get; set; } = new();
        public List<TorosFilterOptionDTO> Variedades { get; set; } = new();
        public bool ShowSocioSelector { get; set; }
        public bool ShowEstablecimientoSelector { get; set; }
        public bool HasSinEstablecimientoOption { get; set; }
    }

    public class SocioLookupItemDTO
    {
        public int Id { get; set; }
        public string Scod { get; set; } = string.Empty;
        public string NumeroSocio { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
}
