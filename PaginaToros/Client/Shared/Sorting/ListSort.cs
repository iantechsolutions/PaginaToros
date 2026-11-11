namespace PaginaToros.Client.Shared.Sorting
{
    /// <summary>Tipo de dato de un campo ordenable: define las etiquetas de dirección y la dirección del primer clic.</summary>
    public enum SortValueKind
    {
        Text,
        Number,
        Date
    }

    /// <summary>
    /// Campo ordenable de un listado. <see cref="Key"/> es lo que viaja al backend (que lo valida
    /// contra su propia lista de campos permitidos) y, si el campo tiene columna, coincide con su
    /// <c>Property</c>.
    /// </summary>
    public sealed record SortField(
        string Key,
        string Label,
        SortValueKind Kind,
        string? AscendingLabel = null,
        string? DescendingLabel = null,
        bool? FirstDescending = null)
    {
        /// <summary>
        /// Dirección al elegir el campo (en el selector o con el primer clic en su encabezado):
        /// fechas y números arrancan por el mayor/más reciente; textos de la A a la Z.
        /// </summary>
        public bool StartsDescending => FirstDescending ?? Kind != SortValueKind.Text;

        public string DirectionLabel(bool descending) => descending
            ? DescendingLabel ?? Kind switch
            {
                SortValueKind.Date => "Más recientes primero",
                SortValueKind.Number => "Mayor a menor",
                _ => "Z a A"
            }
            : AscendingLabel ?? Kind switch
            {
                SortValueKind.Date => "Más antiguos primero",
                SortValueKind.Number => "Menor a mayor",
                _ => "A a Z"
            };
    }

    /// <summary>
    /// Estado de orden de un listado (un solo campo). Lo comparten el selector
    /// (<see cref="SortSelector"/>) y los encabezados (<see cref="SortableHeader"/>), así que
    /// cambiar uno actualiza el otro. Cada página define sus campos, etiquetas y orden inicial.
    /// </summary>
    public sealed class ListSort
    {
        private readonly Dictionary<string, SortField> _fields;

        public ListSort(IEnumerable<SortField> fields, string defaultKey, bool defaultDescending)
        {
            Fields = fields.ToList();
            if (Fields.Count == 0)
            {
                throw new ArgumentException("El listado necesita al menos un campo ordenable.", nameof(fields));
            }

            _fields = Fields.ToDictionary(x => x.Key, StringComparer.Ordinal);
            if (!_fields.TryGetValue(defaultKey, out var current))
            {
                throw new ArgumentException($"El orden inicial '{defaultKey}' no está entre los campos ordenables.", nameof(defaultKey));
            }

            Current = current;
            Descending = defaultDescending;
        }

        public IReadOnlyList<SortField> Fields { get; }

        public SortField Current { get; private set; }

        public bool Descending { get; private set; }

        public string Key => Current.Key;

        public bool IsSortedBy(string key) => string.Equals(Current.Key, key, StringComparison.Ordinal);

        public SortField? Find(string key) => _fields.TryGetValue(key, out var field) ? field : null;

        /// <summary>Fija campo y dirección. Devuelve false si no cambió nada o el campo no existe.</summary>
        public bool Set(string key, bool descending)
        {
            if (!_fields.TryGetValue(key, out var field))
            {
                return false;
            }

            if (ReferenceEquals(field, Current) && descending == Descending)
            {
                return false;
            }

            Current = field;
            Descending = descending;
            return true;
        }

        /// <summary>Elige otro campo conservando la dirección "natural" de su tipo.</summary>
        public bool SetField(string key)
        {
            var field = Find(key);
            return field != null && Set(key, field.StartsDescending);
        }

        /// <summary>Clic en un encabezado: si ya ordena por ese campo invierte la dirección; si no, lo elige.</summary>
        public bool Toggle(string key)
            => IsSortedBy(key) ? Set(key, !Descending) : SetField(key);
    }
}
