using PaginaToros.Shared.Helpers;

namespace PaginaToros.Client.Shared.Sorting
{
    public static class ListSortExtensions
    {
        /// <summary>
        /// Ordena en el cliente un listado que ya tiene todos sus registros, con las mismas reglas
        /// que el servidor (<see cref="InMemorySort"/>): vacíos al final y empates por id descendente.
        /// <paramref name="keys"/> asocia cada <see cref="SortField.Key"/> con el valor a comparar
        /// (texto, número o fecha). Si el campo actual no tiene clave, devuelve el listado tal cual.
        /// </summary>
        public static List<T> ApplyTo<T>(
            this ListSort sort,
            IEnumerable<T> items,
            IReadOnlyDictionary<string, Func<T, object?>> keys,
            Func<T, int> id)
            => keys.TryGetValue(sort.Key, out var key)
                ? InMemorySort.OrderNullsLast(items, key, sort.Descending, id)
                : items.ToList();
    }
}
