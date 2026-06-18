using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Implementacion;
using PaginaToros.Shared.Models;

namespace PaginaToros.Tests;

public class TransanRepositorioTests
{
    [Fact]
    public async Task Lista_OrdersByProcessDateDescending_WithIdTieBreaker_AndNullsLast()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new TransanRepositorio(scope.Context);

        var result = await repo.Lista(0, 10);

        Assert.Equal(new[] { 2, 1, 3, 4 }, result.Select(x => x.Id));
    }

    [Fact]
    public async Task LimitadosFiltrados_AppliesOrderingAndPaging()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new TransanRepositorio(scope.Context);

        var result = await repo.LimitadosFiltrados(1, 2, string.Empty);

        Assert.Equal(new[] { 1, 3 }, result.Select(x => x.Id));
    }

    private static TestContextScope CreateContext()
    {
        var options = new DbContextOptionsBuilder<hereford_prContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;

        var context = new hereford_prContext(options);
        context.Database.EnsureCreated();
        return new TestContextScope(context);
    }

    private static void Seed(hereford_prContext context)
    {
        context.Transans.AddRange(
            new Transan
            {
                Id = 1,
                NroCert = "001",
                FchUsu = new DateTime(2024, 1, 2, 8, 0, 0),
                Fecvta = new DateTime(2024, 1, 1),
                Sven = "1000",
                Vnom = "Vendedor A",
                Plant = "P001",
                NvoPla = "P002"
            },
            new Transan
            {
                Id = 2,
                NroCert = "002",
                FchUsu = new DateTime(2024, 1, 2, 8, 0, 0),
                Fecvta = new DateTime(2024, 1, 2),
                Sven = "1000",
                Vnom = "Vendedor B",
                Plant = "P001",
                NvoPla = "P002"
            },
            new Transan
            {
                Id = 3,
                NroCert = "003",
                FchUsu = new DateTime(2024, 1, 1, 12, 0, 0),
                Fecvta = new DateTime(2024, 1, 3),
                Sven = "1000",
                Vnom = "Vendedor C",
                Plant = "P001",
                NvoPla = "P002"
            },
            new Transan
            {
                Id = 4,
                NroCert = "004",
                FchUsu = null,
                Fecvta = new DateTime(2024, 1, 4),
                Sven = "1000",
                Vnom = "Vendedor D",
                Plant = "P001",
                NvoPla = "P002"
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
