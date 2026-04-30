using Microsoft.AspNetCore.WebUtilities;
using PaginaToros.Client.Shared.Filters;

namespace PaginaToros.Client.Helpers
{
    public static class FilterQueryStateHelper
    {
        public static string BuildTorosUrl(string basePath, TorosFilterState state)
        {
            var query = new Dictionary<string, string?>();

            Add(query, "q", state.SearchText);
            Add(query, "socioId", state.SocioId?.ToString());
            Add(query, "establecimientoId", state.EstablecimientoId?.ToString());
            Add(query, "sinEst", state.IncluirSinEstablecimiento ? "1" : null);
            Add(query, "estado", state.Estado);
            Add(query, "tipoToro", state.TipoToro);
            Add(query, "variedad", state.Variedad);
            Add(query, "nombreToro", state.NombreToro);
            Add(query, "nroOrden", state.NroOrden?.ToString());
            Add(query, "hba", state.Hba);
            Add(query, "tatuaje", state.Tatuaje);
            Add(query, "tipAdn", state.TipAdn);
            Add(query, "fechaSDesde", FormatDate(state.FechaSDesde));
            Add(query, "fechaSHasta", FormatDate(state.FechaSHasta));
            Add(query, "fechaNacimientoDesde", FormatDate(state.FechaNacimientoDesde));
            Add(query, "fechaNacimientoHasta", FormatDate(state.FechaNacimientoHasta));

            return QueryHelpers.AddQueryString(basePath, query!);
        }

        public static void ApplyTorosQuery(TorosFilterState state, string uri)
        {
            var target = new Uri(uri);
            var query = QueryHelpers.ParseQuery(target.Query);

            state.SearchText = Get(query, "q");
            state.SocioId = ParseInt(Get(query, "socioId"));
            state.EstablecimientoId = ParseInt(Get(query, "establecimientoId"));
            state.IncluirSinEstablecimiento = Get(query, "sinEst") == "1";
            state.Estado = Get(query, "estado");
            state.TipoToro = Get(query, "tipoToro");
            state.Variedad = Get(query, "variedad");
            state.NombreToro = Get(query, "nombreToro");
            state.NroOrden = ParseInt(Get(query, "nroOrden"));
            state.Hba = Get(query, "hba");
            state.Tatuaje = Get(query, "tatuaje");
            state.TipAdn = Get(query, "tipAdn");
            state.FechaSDesde = ParseDate(Get(query, "fechaSDesde"));
            state.FechaSHasta = ParseDate(Get(query, "fechaSHasta"));
            state.FechaNacimientoDesde = ParseDate(Get(query, "fechaNacimientoDesde"));
            state.FechaNacimientoHasta = ParseDate(Get(query, "fechaNacimientoHasta"));
        }

        private static void Add(IDictionary<string, string?> query, string key, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                query[key] = value;
            }
        }

        private static string Get(IDictionary<string, Microsoft.Extensions.Primitives.StringValues> query, string key)
            => query.TryGetValue(key, out var value) ? value.ToString() : string.Empty;

        private static int? ParseInt(string? value)
            => int.TryParse(value, out var parsed) ? parsed : null;

        private static DateTime? ParseDate(string? value)
            => DateTime.TryParse(value, out var parsed) ? parsed.Date : null;

        private static string? FormatDate(DateTime? value)
            => value?.ToString("yyyy-MM-dd");
    }
}
