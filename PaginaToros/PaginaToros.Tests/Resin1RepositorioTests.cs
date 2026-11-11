using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Implementacion;
using PaginaToros.Shared.Models;

namespace PaginaToros.Tests;

public class Resin1RepositorioTests
{
    [Fact]
    public async Task Lista_OrdersByCreationDate_ThenInspectionYear_ThenId()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new Resin1Repositorio(scope.Context);

        var result = await repo.Lista(0, 4);

        Assert.Equal(new[] { 2, 1, 3, 4 }, result.Select(x => x.Id));
    }

    [Fact]
    public async Task LimitadosFiltrados_AppliesOrderingAndPaging()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new Resin1Repositorio(scope.Context);

        var result = await repo.LimitadosFiltrados(1, 2, string.Empty);

        Assert.Equal(new[] { 1, 3 }, result.Select(x => x.Id));
    }

    [Fact]
    public async Task SearchPaged_ByDefault_ShowsTheLastLoadedResultFirst_EvenWithoutCreationDate()
    {
        using var scope = CreateContext();
        Seed(scope.Context);
        AddResultWithoutCreationDate(scope.Context, id: 5);

        var repo = new Resin1Repositorio(scope.Context);

        var (items, total) = await repo.SearchPagedAsync(0, 15);

        // Antes se ordenaba por FCH_USU y los cargados sin esa fecha quedaban al final.
        Assert.Equal(new[] { 5, 4, 3, 2, 1 }, items.Select(x => x.Id));
        Assert.Equal(5, total);
    }

    [Theory]
    [InlineData(true, new[] { 3, 2, 1, 4 })]
    [InlineData(false, new[] { 1, 2, 3, 4 })]
    public async Task SearchPaged_ByInspectionDate_KeepsEmptyDatesLastInBothDirections(bool descending, int[] expected)
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new Resin1Repositorio(scope.Context);

        var (items, _) = await repo.SearchPagedAsync(0, 15, sortBy: Resin1Repositorio.SortFechaInspeccion, sortDescending: descending);

        Assert.Equal(expected, items.Select(x => x.Id));
    }

    [Theory]
    [InlineData(true, new[] { 2, 1, 3, 4, 5 })]
    [InlineData(false, new[] { 4, 3, 2, 1, 5 })]
    public async Task SearchPaged_ByCreationDate_BreaksTiesByLastLoadedAndLeavesEmptyLast(bool descending, int[] expected)
    {
        using var scope = CreateContext();
        Seed(scope.Context);
        AddResultWithoutCreationDate(scope.Context, id: 5);

        var repo = new Resin1Repositorio(scope.Context);

        var (items, _) = await repo.SearchPagedAsync(0, 15, sortBy: Resin1Repositorio.SortFechaCreacion, sortDescending: descending);

        // 1 y 2 comparten fecha de creación: primero el último cargado (2).
        Assert.Equal(expected, items.Select(x => x.Id));
    }

    [Fact]
    public async Task SearchPaged_ByResultNumber_SortsNumericallyAndPagesOverTheWholeSet()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new Resin1Repositorio(scope.Context);

        var (pagina1, total) = await repo.SearchPagedAsync(0, 2, sortBy: Resin1Repositorio.SortNroResultado, sortDescending: true);
        var (pagina2, _) = await repo.SearchPagedAsync(2, 2, sortBy: Resin1Repositorio.SortNroResultado, sortDescending: true);

        Assert.Equal(new[] { "10004", "10003", "10002", "10001" }, pagina1.Concat(pagina2).Select(x => x.Nrores));
        Assert.Equal(4, total);
    }

    [Fact]
    public async Task SearchPaged_UnknownSortField_FallsBackToLastLoadedFirst()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new Resin1Repositorio(scope.Context);

        var (items, _) = await repo.SearchPagedAsync(0, 15, sortBy: "Observ", sortDescending: false);

        Assert.Equal(new[] { 4, 3, 2, 1 }, items.Select(x => x.Id));
    }

    private static void AddResultWithoutCreationDate(hereford_prContext context, int id)
    {
        context.Resin1s.Add(new Resin1
        {
            Id = id,
            Nrores = "10005",
            Nropla = "P005",
            FchUsu = null,
            Freali = new DateTime(2026, 2, 1),
            Scod = "004476",
            Icod = "I005",
            Estcod = "E001"
        });
        context.SaveChanges();
        context.ChangeTracker.Clear();
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

    private static void Seed(hereford_prContext context)
    {
        var socio = new Socio
        {
            Id = 1,
            Scod = "004476",
            Nombre = "Socio Test"
        };

        var estable = new Estable
        {
            Id = 1,
            Ecod = "E001",
            Codsoc = socio.Scod,
            Nombre = "Estable Test",
            Fecing = "2024/01/01"
        };

        context.Socios.Add(socio);
        context.Estables.Add(estable);
        context.Resin1s.AddRange(
            new Resin1
            {
                Id = 1,
                Nrores = "10001",
                Nropla = "P001",
                FchUsu = new DateTime(2024, 1, 1),
                Freali = new DateTime(2023, 5, 1),
                Scod = socio.Scod,
                Icod = "I001",
                Estcod = estable.Ecod,
                Socio = socio,
                Establecimiento = estable
            },
            new Resin1
            {
                Id = 2,
                Nrores = "10002",
                Nropla = "P002",
                FchUsu = new DateTime(2024, 1, 1),
                Freali = new DateTime(2024, 5, 1),
                Scod = socio.Scod,
                Icod = "I002",
                Estcod = estable.Ecod,
                Socio = socio,
                Establecimiento = estable
            },
            new Resin1
            {
                Id = 3,
                Nrores = "10003",
                Nropla = "P003",
                FchUsu = new DateTime(2023, 12, 31),
                Freali = new DateTime(2025, 1, 1),
                Scod = socio.Scod,
                Icod = "I003",
                Estcod = estable.Ecod,
                Socio = socio,
                Establecimiento = estable
            },
            new Resin1
            {
                Id = 4,
                Nrores = "10004",
                Nropla = "P004",
                FchUsu = new DateTime(2022, 1, 1),
                Freali = null,
                Scod = socio.Scod,
                Icod = "I004",
                Estcod = estable.Ecod,
                Socio = socio,
                Establecimiento = estable
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
