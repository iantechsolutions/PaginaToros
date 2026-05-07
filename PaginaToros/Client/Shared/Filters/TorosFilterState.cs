using PaginaToros.Shared.Models;

namespace PaginaToros.Client.Shared.Filters
{
    public class TorosFilterState
    {
        public string SearchText { get; set; } = string.Empty;
        public int? SocioId { get; set; }
        public string SocioLabel { get; set; } = string.Empty;
        public int? EstablecimientoId { get; set; }
        public string EstablecimientoLabel { get; set; } = string.Empty;
        public bool IncluirSinEstablecimiento { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string TipoToro { get; set; } = string.Empty;
        public string Variedad { get; set; } = string.Empty;
        public string NombreToro { get; set; } = string.Empty;
        public int? NroOrden { get; set; }
        public string Hba { get; set; } = string.Empty;
        public string Tatuaje { get; set; } = string.Empty;
        public string TipAdn { get; set; } = string.Empty;
        public DateTime? FechaSDesde { get; set; }
        public DateTime? FechaSHasta { get; set; }
        public DateTime? FechaNacimientoDesde { get; set; }
        public DateTime? FechaNacimientoHasta { get; set; }
        public string SortBy { get; set; } = string.Empty;
        public string SortDirection { get; set; } = string.Empty;

        public TorosFilterRequest ToRequest(int skip, int take, IEnumerable<int>? socioIds = null)
        {
            return new TorosFilterRequest
            {
                Skip = skip,
                Take = take,
                SearchText = string.IsNullOrWhiteSpace(SearchText) ? null : SearchText.Trim(),
                SocioId = SocioId,
                SocioIds = socioIds?.Where(x => x > 0).Distinct().ToList() ?? new List<int>(),
                EstablecimientoId = EstablecimientoId,
                IncluirSinEstablecimiento = IncluirSinEstablecimiento,
                Estado = EmptyToNull(Estado),
                TipoToro = EmptyToNull(TipoToro),
                Variedad = EmptyToNull(Variedad),
                NombreToro = EmptyToNull(NombreToro),
                NroOrden = NroOrden,
                Hba = EmptyToNull(Hba),
                Tatuaje = EmptyToNull(Tatuaje),
                TipAdn = EmptyToNull(TipAdn),
                FechaSDesde = FechaSDesde,
                FechaSHasta = FechaSHasta,
                FechaNacimientoDesde = FechaNacimientoDesde,
                FechaNacimientoHasta = FechaNacimientoHasta,
                SortBy = EmptyToNull(SortBy),
                SortDirection = EmptyToNull(SortDirection)
            };
        }

        public bool HasActiveFilters()
        {
            return !string.IsNullOrWhiteSpace(SearchText)
                || SocioId.HasValue
                || EstablecimientoId.HasValue
                || IncluirSinEstablecimiento
                || !string.IsNullOrWhiteSpace(Estado)
                || !string.IsNullOrWhiteSpace(TipoToro)
                || !string.IsNullOrWhiteSpace(Variedad)
                || !string.IsNullOrWhiteSpace(NombreToro)
                || NroOrden.HasValue
                || !string.IsNullOrWhiteSpace(Hba)
                || !string.IsNullOrWhiteSpace(Tatuaje)
                || !string.IsNullOrWhiteSpace(TipAdn)
                || FechaSDesde.HasValue
                || FechaSHasta.HasValue
                || FechaNacimientoDesde.HasValue
                || FechaNacimientoHasta.HasValue;
        }

        public void Clear()
        {
            SearchText = string.Empty;
            SocioId = null;
            SocioLabel = string.Empty;
            EstablecimientoId = null;
            EstablecimientoLabel = string.Empty;
            IncluirSinEstablecimiento = false;
            Estado = string.Empty;
            TipoToro = string.Empty;
            Variedad = string.Empty;
            NombreToro = string.Empty;
            NroOrden = null;
            Hba = string.Empty;
            Tatuaje = string.Empty;
            TipAdn = string.Empty;
            FechaSDesde = null;
            FechaSHasta = null;
            FechaNacimientoDesde = null;
            FechaNacimientoHasta = null;
            SortBy = string.Empty;
            SortDirection = string.Empty;
        }

        public TorosFilterState Clone()
        {
            var copy = new TorosFilterState();
            copy.CopyFrom(this);
            return copy;
        }

        public void CopyFrom(TorosFilterState source)
        {
            SearchText = source.SearchText;
            SocioId = source.SocioId;
            SocioLabel = source.SocioLabel;
            EstablecimientoId = source.EstablecimientoId;
            EstablecimientoLabel = source.EstablecimientoLabel;
            IncluirSinEstablecimiento = source.IncluirSinEstablecimiento;
            Estado = source.Estado;
            TipoToro = source.TipoToro;
            Variedad = source.Variedad;
            NombreToro = source.NombreToro;
            NroOrden = source.NroOrden;
            Hba = source.Hba;
            Tatuaje = source.Tatuaje;
            TipAdn = source.TipAdn;
            FechaSDesde = source.FechaSDesde;
            FechaSHasta = source.FechaSHasta;
            FechaNacimientoDesde = source.FechaNacimientoDesde;
            FechaNacimientoHasta = source.FechaNacimientoHasta;
            SortBy = source.SortBy;
            SortDirection = source.SortDirection;
        }

        private static string? EmptyToNull(string? value)
            => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    public class FilterChipItem
    {
        public string Key { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
