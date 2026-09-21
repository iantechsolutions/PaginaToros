using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Implementacion;
using PaginaToros.Shared.Models;

namespace PaginaToros.Tests;

/// <summary>
/// Control de duplicados del alta/edicion de certificados. La numeracion la lleva cada
/// centro de IA, asi que el duplicado se busca dentro del mismo NROCEN: el mismo
/// NRO_CERT + HBA emitido por otro centro no es el mismo certificado.
/// </summary>
public class CertifsemanRepositorioTests
{
    [Fact]
    public async Task ObtenerPorClave_WhenSameCentroNroCertAndHba_ReturnsDuplicate()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new CertifsemanRepositorio(scope.Context);

        var result = await repo.ObtenerPorClave(" B-70 ", " 16074 ", " 445912 ");

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
    }

    [Fact]
    public async Task ObtenerPorClave_WhenSameNroCertAndHbaButOtherCentro_ReturnsNull()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new CertifsemanRepositorio(scope.Context);

        var result = await repo.ObtenerPorClave("B-71", "16074", "445912");

        Assert.Null(result);
    }

    [Fact]
    public async Task ObtenerPorClave_WhenExcludingOwnId_ReturnsNull()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new CertifsemanRepositorio(scope.Context);

        var result = await repo.ObtenerPorClave("B-70", "16074", "445912", excludeId: 1);

        Assert.Null(result);
    }

    [Fact]
    public async Task ObtenerPorClave_LoadsSocioAndCentro_ForTheErrorMessage()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new CertifsemanRepositorio(scope.Context);

        var result = await repo.ObtenerPorClave("B-70", "16074", "445912");

        Assert.NotNull(result);
        Assert.Equal("SILENKA S.A.", result!.Socio?.Nombre);
        Assert.Equal("ESTANCIAS Y CABAÑA LAS LILAS S.A.", result.Centro?.Nombre);
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
            Id = 376,
            Scod = "004503",
            Nombre = "SILENKA S.A."
        });

        context.Centrosia.Add(new Centrosium
        {
            Id = 1,
            Nrocen = "B-70",
            Nombre = "ESTANCIAS Y CABAÑA LAS LILAS S.A."
        });

        context.Certifsemen.AddRange(
            new Certifseman
            {
                Id = 1,
                Nrocen = "B-70",
                Nrocri = "004503",
                NroCert = "16074",
                Hba = "445912",
                NomDad = "LAS LILAS X3349 ENDURE KRISTOFF",
                Fecvta = new DateTime(2025, 9, 26)
            },
            new Certifseman
            {
                Id = 2,
                Nrocen = "B-70",
                Nrocri = "004503",
                NroCert = "16080",
                Hba = "438613",
                NomDad = "LAS LILAS X2985 YA ESTA! S FANATICO",
                Fecvta = new DateTime(2025, 5, 5)
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
