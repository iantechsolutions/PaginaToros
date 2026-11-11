using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PaginaToros.Shared.Helpers
{
    /// <summary>
    /// Orden en memoria con las reglas comunes de los listados ordenables:
    /// los valores vacíos (null o texto en blanco) van siempre al final, en ambas direcciones;
    /// los textos se comparan sin distinguir mayúsculas ni acentos; y los empates se resuelven
    /// por el id más alto primero (el último registro cargado).
    /// Lo usan el servidor (listados que ya se ordenan en memoria) y el cliente (listados que
    /// tienen todos los registros), para que el mismo campo ordene igual en los dos lados.
    /// </summary>
    public static class InMemorySort
    {
        private static readonly CompareInfo TextCompare = CultureInfo.InvariantCulture.CompareInfo;

        public static List<T> OrderNullsLast<T>(
            IEnumerable<T> items,
            Func<T, object?> key,
            bool descending,
            Func<T, int> id)
        {
            var comparer = new ValueComparer(descending);
            return items
                .Select(item => (Item: item, Value: Normalize(key(item))))
                .OrderBy(x => x.Value is null)
                .ThenBy(x => x.Value, comparer)
                .ThenByDescending(x => id(x.Item))
                .Select(x => x.Item)
                .ToList();
        }

        /// <summary>Año de 4 dígitos guardado como texto (p. ej. PLANTEL.ANIOEX); null si no es un número.</summary>
        public static int? ParseYear(string? value)
            => int.TryParse(value?.Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out var year) ? year : null;

        private static object? Normalize(object? value) => value switch
        {
            null => null,
            string text when string.IsNullOrWhiteSpace(text) => null,
            string text => text.Trim(),
            _ => value
        };

        private sealed class ValueComparer : IComparer<object?>
        {
            private readonly int _sign;

            public ValueComparer(bool descending) => _sign = descending ? -1 : 1;

            public int Compare(object? x, object? y)
            {
                if (x is null || y is null)
                {
                    return 0;
                }

                var result = x is string a && y is string b
                    ? TextCompare.Compare(a, b, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace)
                    : Comparer<object>.Default.Compare(x, y);

                return _sign * result;
            }
        }
    }
}
