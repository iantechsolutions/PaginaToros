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
