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
