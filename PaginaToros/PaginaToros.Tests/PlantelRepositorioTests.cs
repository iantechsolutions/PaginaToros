using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Implementacion;
using PaginaToros.Shared.Models;

namespace PaginaToros.Tests;

public class PlantelRepositorioTests
{
    [Fact]
    public async Task Lista_OrdersByCreationDate_ThenYear_ThenId()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new PlantelRepositorio(scope.Context);

        var result = await repo.Lista(0, 4);

        Assert.Equal(new[] { 2, 1, 3, 4 }, result.Select(x => x.Id));
    }

    [Fact]
    public async Task LimitadosFiltrados_AppliesOrderingAndPaging()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new PlantelRepositorio(scope.Context);

        var result = await repo.LimitadosFiltrados(1, 2, string.Empty);

        Assert.Equal(new[] { 1, 3 }, result.Select(x => x.Id));
    }

    [Fact]
    public async Task LimitadosFiltrados_WhenFilteringBySocio_UsesYearOrdering()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new PlantelRepositorio(scope.Context);

        var result = await repo.LimitadosFiltrados(0, 0, "Socio.Id == 1");

        Assert.Equal(new[] { 3, 2, 1, 4 }, result.Select(x => x.Id));
    }

    [Fact]
    public async Task SearchPaged_WithoutSort_KeepsTheExistingDefaultOrderAndReturnsTheFilteredTotal()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new PlantelRepositorio(scope.Context);

        var (items, total) = await repo.SearchPagedAsync(1, 2);
        var (porSocio, totalSocio) = await repo.SearchPagedAsync(0, 15, "Socio.Id == 1", PlantelRepositorio.SortFechaAlta, sortDescending: true);

        Assert.Equal(new[] { 1, 3 }, items.Select(x => x.Id));
        Assert.Equal(4, total);
        Assert.Equal(new[] { 3, 2, 1, 4 }, porSocio.Select(x => x.Id));
        Assert.Equal(4, totalSocio);
    }

    [Theory]
    [InlineData(false, new[] { 4, 1, 2, 3 })]
    [InlineData(true, new[] { 3, 2, 1, 4 })]
    public async Task SearchPaged_SortsByYearAsNumber(bool descending, int[] expected)
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new PlantelRepositorio(scope.Context);

        var (items, _) = await repo.SearchPagedAsync(0, 15, null, PlantelRepositorio.SortAnioExistencia, descending);

        Assert.Equal(expected, items.Select(x => x.Id));
    }

    [Theory]
    [InlineData(false, new[] { 4, 2, 1, 3 })]
    [InlineData(true, new[] { 1, 4, 2, 3 })]
    public async Task SearchPaged_NumbersKeepEmptyLastAndBreakTiesByLastLoaded(bool descending, int[] expected)
    {
        using var scope = CreateContext();
        Seed(scope.Context, varede: new double?[] { 30, 10, null, 10 });

        var repo = new PlantelRepositorio(scope.Context);

        var (items, total) = await repo.SearchPagedAsync(0, 15, null, nameof(Plantel.Varede), descending);

        // Vacíos al final en ambas direcciones; empates (10 y 10) con el id más alto primero.
        Assert.Equal(expected, items.Select(x => x.Id));
        Assert.Equal(4, total);
    }

    [Fact]
    public async Task SearchPaged_OrdersTheWholeFilteredSetBeforePaging()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new PlantelRepositorio(scope.Context);

        var (pagina1, _) = await repo.SearchPagedAsync(0, 2, null, PlantelRepositorio.SortPlantel, sortDescending: true);
        var (pagina2, _) = await repo.SearchPagedAsync(2, 2, null, PlantelRepositorio.SortPlantel, sortDescending: true);

        Assert.Equal(new[] { "P004", "P003", "P002", "P001" }, pagina1.Concat(pagina2).Select(x => x.Placod));
    }

    [Fact]
    public async Task SearchPaged_UnknownSortField_FallsBackToTheDefaultOrder()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new PlantelRepositorio(scope.Context);

        var (items, _) = await repo.SearchPagedAsync(0, 15, null, "Comment); drop", sortDescending: false);

        Assert.Equal(new[] { 2, 1, 3, 4 }, items.Select(x => x.Id));
    }

    private static TestContextScope CreateContext()
    {
        var options = new DbContextOptionsBuilder<hereford_prContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new hereford_prContext(options);
        context.Database.EnsureCreated();
        return new TestContextScope(context);
    }

    private static void Seed(hereford_prContext context, double?[]? varede = null)
    {
        context.Socios.Add(new Socio
        {
            Id = 1,
            Scod = "004476",
            Nombre = "Socio Test"
        });

        context.Planteles.AddRange(
            new Plantel
            {
                Id = 1,
                Placod = "P001",
                Varede = varede?[0],
                Anioex = "2021",
                Fecing = "2024/01/01",
                FchUsu = new DateTime(2024, 1, 1),
                Nrocri = "004476",
                Estado = "S"
            },
            new Plantel
            {
                Id = 2,
                Placod = "P002",
                Varede = varede?[1],
                Anioex = "2023",
                Fecing = "2024/01/01",
                FchUsu = new DateTime(2024, 1, 1),
                Nrocri = "004476",
                Estado = "S"
            },
            new Plantel
            {
                Id = 3,
                Placod = "P003",
                Varede = varede?[2],
                Anioex = "2024",
                Fecing = "2023/12/31",
                FchUsu = new DateTime(2023, 12, 31),
                Nrocri = "004476",
                Estado = "S"
            },
            new Plantel
            {
                Id = 4,
                Placod = "P004",
                Varede = varede?[3],
                Anioex = "2020",
                Fecing = "2022/01/01",
                FchUsu = new DateTime(2022, 1, 1),
                Nrocri = "004476",
                Estado = "S"
            });

        context.SaveChanges();
        context.ChangeTracker.Clear();
    }

    private sealed class TestContextScope : IDisposable
    {
        public TestContextScope(hereford_prContext context)
        {
            Context = context;
        }

        public hereford_prContext Context { get; }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
