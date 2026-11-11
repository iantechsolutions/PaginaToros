using PaginaToros.Client.Shared.Sorting;

namespace PaginaToros.Tests;

public class ListSortTests
{
    private static ListSort NewSort() => new(
        new[]
        {
            new SortField("Id", "Orden de carga", SortValueKind.Date),
            new SortField("Nombre", "Nombre", SortValueKind.Text),
            new SortField("Cantidad", "Cantidad", SortValueKind.Number),
        },
        defaultKey: "Id",
        defaultDescending: true);

    [Fact]
    public void StartsWithTheDefinedInitialOrder()
    {
        var sort = NewSort();

        Assert.Equal("Id", sort.Key);
        Assert.True(sort.Descending);
    }

    [Fact]
    public void Toggle_OnAnotherHeader_SelectsItWithItsNaturalDirection_AndAgainInvertsIt()
    {
        var sort = NewSort();

        Assert.True(sort.Toggle("Nombre"));
        Assert.Equal(("Nombre", false), (sort.Key, sort.Descending));

        Assert.True(sort.Toggle("Nombre"));
        Assert.Equal(("Nombre", true), (sort.Key, sort.Descending));

        Assert.True(sort.Toggle("Cantidad"));
        Assert.Equal(("Cantidad", true), (sort.Key, sort.Descending));
    }

    [Fact]
    public void Set_IgnoresUnknownFieldsAndReportsWhenNothingChanged()
    {
        var sort = NewSort();

        Assert.False(sort.Set("NoExiste", false));
        Assert.False(sort.Set("Id", true));
        Assert.Equal(("Id", true), (sort.Key, sort.Descending));
    }

    [Fact]
    public void ApplyTo_KeepsEmptyValuesLastAndBreaksTiesByHighestId()
    {
        var sort = NewSort();
        sort.Set("Nombre", descending: false);
        var items = new[]
        {
            (Id: 1, Nombre: "beta"),
            (Id: 2, Nombre: " "),
            (Id: 3, Nombre: "Álamo"),
            (Id: 4, Nombre: "Beta"),
        };
        var keys = new Dictionary<string, Func<(int Id, string Nombre), object?>> { ["Nombre"] = x => x.Nombre };

        var asc = sort.ApplyTo(items, keys, x => x.Id).Select(x => x.Id);
        sort.Set("Nombre", descending: true);
        var desc = sort.ApplyTo(items, keys, x => x.Id).Select(x => x.Id);

        // Sin distinguir mayúsculas ni acentos; "beta"/"Beta" empatan → id más alto primero.
        Assert.Equal(new[] { 3, 4, 1, 2 }, asc);
        Assert.Equal(new[] { 4, 1, 3, 2 }, desc);
    }
}
