using Microsoft.EntityFrameworkCore;
using PaginaToros.Client.Shared.Filters;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Implementacion;
using PaginaToros.Shared.Models;

namespace PaginaToros.Tests;

public class TorosRepositorioTests
{
    [Fact]
    public async Task SearchAsync_AppliesDefaultOrder_ActiveFirstThenCreatedAtDescending()
    {
        using var context = CreateContext();
        Seed(context);
        var repo = new TorosRepositorio(context);

        var result = await repo.SearchAsync(new TorosFilterRequest { Skip = 0, Take = 10 });

        Assert.Equal(new[] { 2, 1, 4, 3 }, result.Items.Select(t => t.Id));
    }

    [Theory]
    [InlineData("bravo", 1)]
    [InlineData("HBA-222", 2)]
    [InlineData("TAT-333", 3)]
    [InlineData("ADN-444", 4)]
    [InlineData("1002", 2)]
    [InlineData("Socio Norte", 2)]
    public async Task SearchAsync_FiltersByText_OnSupportedFields(string searchText, int expectedId)
    {
        using var context = CreateContext();
        Seed(context);
        var repo = new TorosRepositorio(context);

        var result = await repo.SearchAsync(new TorosFilterRequest
        {
            Skip = 0,
            Take = 10,
            SearchText = searchText
        });

        Assert.Contains(result.Items, item => item.Id == expectedId);
    }

    [Fact]
    public async Task SearchAsync_FiltersBySocio()
    {
        using var context = CreateContext();
        Seed(context);
        var repo = new TorosRepositorio(context);

        var result = await repo.SearchAsync(new TorosFilterRequest
        {
            Skip = 0,
            Take = 10,
            SocioId = 20
        });

        Assert.Equal(new[] { 4, 3 }, result.Items.Select(t => t.Id));
    }

    [Fact]
    public async Task SearchAsync_FiltersByEstablecimiento()
    {
        using var context = CreateContext();
        Seed(context);
        var repo = new TorosRepositorio(context);

        var result = await repo.SearchAsync(new TorosFilterRequest
        {
            Skip = 0,
            Take = 10,
            EstablecimientoId = 200
        });

        Assert.Equal(new[] { 4, 3 }, result.Items.Select(t => t.Id));
    }

    [Fact]
    public async Task SearchAsync_SortsManuallyByColumn()
    {
        using var context = CreateContext();
        Seed(context);
        var repo = new TorosRepositorio(context);

        var result = await repo.SearchAsync(new TorosFilterRequest
        {
            Skip = 0,
            Take = 10,
            SortBy = "nombreToro",
            SortDirection = "asc"
        });

        Assert.Equal(new[] { "Alfa", "Bravo", "Delta", "Zonda" }, result.Items.Select(t => t.NomDad));
    }

    [Fact]
    public async Task GetFilterMetadataAsync_ReturnsEstablecimientosForSelectedSocio()
    {
        using var context = CreateContext();
        Seed(context);
        var repo = new TorosRepositorio(context);

        var metadata = await repo.GetFilterMetadataAsync(new TorosFilterRequest { SocioId = 20 });

        Assert.Single(metadata.Establecimientos);
        Assert.Equal(200, metadata.Establecimientos[0].IntValue);
        Assert.False(metadata.HasSinEstablecimientoOption);
    }

    [Fact]
    public async Task SearchAsync_ReturnsEmptyPage_WhenNoResultsMatch()
    {
        using var context = CreateContext();
        Seed(context);
        var repo = new TorosRepositorio(context);

        var result = await repo.SearchAsync(new TorosFilterRequest
        {
            Skip = 0,
            Take = 10,
            SearchText = "no-existe"
        });

        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }

    [Fact]
    public void TorosFilterState_Clear_ResetsSocioAndEstablecimiento()
    {
        var state = new TorosFilterState
        {
            SocioId = 10,
            SocioLabel = "Socio Norte",
            EstablecimientoId = 100,
            EstablecimientoLabel = "Campo Norte",
            IncluirSinEstablecimiento = true,
            SearchText = "bravo",
            SortBy = "hba",
            SortDirection = "desc"
        };

        state.Clear();

        Assert.Null(state.SocioId);
        Assert.Null(state.EstablecimientoId);
        Assert.False(state.IncluirSinEstablecimiento);
        Assert.Equal(string.Empty, state.SearchText);
        Assert.Equal(string.Empty, state.SortBy);
        Assert.Equal(string.Empty, state.SortDirection);
    }

    private static hereford_prContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<hereford_prContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new hereford_prContext(options);
    }

    private static void Seed(hereford_prContext context)
    {
        var socioNorte = new Socio { Id = 10, Scod = "S10", Nombre = "Socio Norte", Codpos2 = "010" };
        var socioSur = new Socio { Id = 20, Scod = "S20", Nombre = "Socio Sur", Codpos2 = "020" };
        var establecimientoNorte = new Estable { Id = 100, Codsoc = "S10", Ecod = "E10", Nombre = "Campo Norte", Fecing = string.Empty };
        var establecimientoSur = new Estable { Id = 200, Codsoc = "S20", Ecod = "E20", Nombre = "Campo Sur", Fecing = string.Empty };

        context.Socios.AddRange(socioNorte, socioSur);
        context.Estables.AddRange(establecimientoNorte, establecimientoSur);
        context.Torosunis.AddRange(
            new Torosuni
            {
                Id = 1,
                CodEstado = "1",
                CreatedAt = new DateTime(2026, 1, 10),
                Socio = socioNorte,
                Criador = socioNorte.Scod,
                Establecimiento = establecimientoNorte,
                EstablecimientoId = establecimientoNorte.Id,
                NomDad = "Bravo",
                Hba = "HBA-111",
                Tatpart = "TAT-111",
                NrTsan = "ADN-111",
                Sbcod = 1001,
                Nacido = new DateTime(2024, 1, 1),
                Fechasba = "01/02/2026"
            },
            new Torosuni
            {
                Id = 2,
                CodEstado = "1",
                CreatedAt = new DateTime(2026, 2, 10),
                Socio = socioNorte,
                Criador = socioNorte.Scod,
                Establecimiento = establecimientoNorte,
                EstablecimientoId = establecimientoNorte.Id,
                NomDad = "Alfa",
                Hba = "HBA-222",
                Tatpart = "TAT-222",
                NrTsan = "ADN-222",
                Sbcod = 1002,
                Nacido = new DateTime(2024, 2, 1),
                Fechasba = "02/02/2026"
            },
            new Torosuni
            {
                Id = 3,
                CodEstado = "4",
                CreatedAt = new DateTime(2026, 3, 10),
                Socio = socioSur,
                Criador = socioSur.Scod,
                Establecimiento = establecimientoSur,
                EstablecimientoId = establecimientoSur.Id,
                NomDad = "Zonda",
                Hba = "HBA-333",
                Tatpart = "TAT-333",
                NrTsan = "ADN-333",
                Sbcod = 1003,
                Nacido = new DateTime(2024, 3, 1),
                Fechasba = "03/02/2026"
            },
            new Torosuni
            {
                Id = 4,
                CodEstado = "2",
                CreatedAt = new DateTime(2026, 4, 10),
                Socio = socioSur,
                Criador = socioSur.Scod,
                Establecimiento = establecimientoSur,
                EstablecimientoId = establecimientoSur.Id,
                NomDad = "Delta",
                Hba = "HBA-444",
                Tatpart = "TAT-444",
                NrTsan = "ADN-444",
                Sbcod = 1004,
                Nacido = new DateTime(2024, 4, 1),
                Fechasba = "04/02/2026"
            });

        context.SaveChanges();
    }
}
