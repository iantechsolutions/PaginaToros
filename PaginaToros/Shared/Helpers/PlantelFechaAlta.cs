using System;
using System.Globalization;

namespace PaginaToros.Shared.Helpers
{
    /// <summary>
    /// Fecha de alta de un plantel tal como la usa el orden por defecto del listado:
    /// FECING (texto, varios formatos históricos) y, si no se puede leer, FCH_USU.
    /// </summary>
    public static class PlantelFechaAlta
    {
        private static readonly string[] Formatos = { "yyyy/MM/dd", "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy" };

        public static DateTime? Obtener(string? fecing, DateTime? fchUsu)
        {
            if (!string.IsNullOrWhiteSpace(fecing))
            {
                if (DateTime.TryParseExact(fecing, Formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha))
                {
                    return fecha;
                }

                if (DateTime.TryParse(fecing, out fecha))
                {
                    return fecha;
                }
            }

            return fchUsu;
        }
    }
}
