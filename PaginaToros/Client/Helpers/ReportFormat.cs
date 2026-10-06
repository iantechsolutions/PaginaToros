using System.Globalization;

namespace PaginaToros.Client.Helpers
{
    /// <summary>
    /// Formato de números para los informes (Inspecciones y Planteles). Independiente de la
    /// cultura del navegador: separador de miles "." y decimales "," como se leen en Argentina.
    /// Solo cambia la presentación; los valores se muestran tal cual se calculan.
    /// </summary>
    public static class ReportFormat
    {
        public static string N0(int value)
            => value.ToString("#,0", CultureInfo.InvariantCulture).Replace(',', '.');

        public static string N0(long value)
            => value.ToString("#,0", CultureInfo.InvariantCulture).Replace(',', '.');

        /// <summary>
        /// Enteros con separador de miles. Si el valor trae decimales (stock cargado a mano) se
        /// muestran tal cual, con coma decimal, para no redondear un dato de la base.
        /// </summary>
        public static string N0(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                return "-";
            }

            if (Math.Abs(value - Math.Round(value)) < 1e-9)
            {
                return N0((long)Math.Round(value));
            }

            return value.ToString("#,0.##", CultureInfo.InvariantCulture)
                .Replace(',', '\u0001')
                .Replace('.', ',')
                .Replace('\u0001', '.');
        }

        /// <summary>Porcentaje de <paramref name="parte"/> sobre <paramref name="total"/>, con un decimal.</summary>
        public static string Pct(double parte, double total)
        {
            if (total <= 0)
            {
                return "-";
            }

            return (parte / total * 100).ToString("0.0", CultureInfo.InvariantCulture).Replace('.', ',') + " %";
        }

        public static string Fecha(DateTime? value)
            => value.HasValue ? value.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "-";

        /// <summary>
        /// Texto en minúsculas y sin acentos para los buscadores locales de los informes
        /// ("cordoba" encuentra "Córdoba"). Si el runtime no puede normalizar, compara tal cual.
        /// </summary>
        public static string Normalize(string? value)
        {
            var text = (value ?? string.Empty).Trim().ToLowerInvariant();
            if (text.Length == 0)
            {
                return text;
            }

            try
            {
                var decomposed = text.Normalize(System.Text.NormalizationForm.FormD);
                var builder = new System.Text.StringBuilder(decomposed.Length);
                foreach (var ch in decomposed)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                    {
                        builder.Append(ch);
                    }
                }

                return builder.ToString();
            }
            catch
            {
                return text;
            }
        }
    }
}
