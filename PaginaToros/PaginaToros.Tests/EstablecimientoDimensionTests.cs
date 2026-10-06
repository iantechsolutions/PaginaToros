using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PaginaPlantels.Server.Cont;
using PaginaToros.Server.Context;
using PaginaToros.Server.Controllers;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Repositorio.Implementacion;
using PaginaToros.Server.Services;
using PaginaToros.Server.Utilidades;
using PaginaToros.Shared.Helpers;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Tests;

/// <summary>
/// Reglas de la dimension "establecimiento" dentro de una razon social: planteles, inspecciones,
/// certificados, declaraciones y transferencias de toros no pueden mezclar campos.
/// </summary>
public class EstablecimientoPlantelHelperTests
{
    private static List<PlantelDTO> Planteles() => new()
    {
        new PlantelDTO { Id = 1, Placod = "5RA01", Anioex = "2025", Nrocri = "S1", EstablecimientoId = 100 },
        new PlantelDTO { Id = 2, Placod = "6RA01", Anioex = "2026", Nrocri = "S1", EstablecimientoId = 100 },
        new PlantelDTO { Id = 3, Placod = "6RA02", Anioex = "2026", Nrocri = "S1", EstablecimientoId = 200 },
        new PlantelDTO { Id = 4, Placod = "4RA01", Anioex = "2024", Nrocri = "S1", EstablecimientoId = null },
        new PlantelDTO { Id = 5, Placod = "6ZZ01", Anioex = "2026", Nrocri = "S9", EstablecimientoId = 100 },
    };

    [Fact]
    public void FiltrarPorEstablecimiento_NullDevuelveTodos_MenosUnoSoloEsePlantel_SinAsignarSoloLosHistoricos()
    {
        var planteles = Planteles();

        Assert.Equal(5, EstablecimientoPlantelHelper.FiltrarPorEstablecimiento(planteles, null).Count());
        Assert.Equal(new[] { 1, 2, 5 }, EstablecimientoPlantelHelper.FiltrarPorEstablecimiento(planteles, 100).Select(p => p.Id).OrderBy(x => x));
        Assert.Equal(new[] { 4 }, EstablecimientoPlantelHelper.FiltrarPorEstablecimiento(planteles, EstablecimientoPlantelHelper.FiltroSinEstablecimiento).Select(p => p.Id));
        Assert.Empty(EstablecimientoPlantelHelper.FiltrarPorEstablecimiento(planteles, 999));
        Assert.Empty(EstablecimientoPlantelHelper.FiltrarPorEstablecimiento(null, 100));
    }

    [Fact]
    public void EsCompatible_AdmiteMismoEstablecimientoYSinAsignar()
    {
        Assert.True(EstablecimientoPlantelHelper.EsCompatible(100, 100));
        Assert.True(EstablecimientoPlantelHelper.EsCompatible(null, 100));
        Assert.True(EstablecimientoPlantelHelper.EsCompatible(100, null));
        Assert.False(EstablecimientoPlantelHelper.EsCompatible(200, 100));
    }

    [Fact]
    public void ElegirPlantelPorDefecto_PrefiereElDelEstablecimientoYDelAnio()
    {
        var elegido = EstablecimientoPlantelHelper.ElegirPlantelPorDefecto(Planteles(), "S1", 100, 2, 2026);
        Assert.Equal(2, elegido!.Id);
    }

    [Fact]
    public void ElegirPlantelPorDefecto_NuncaTomaElDeOtroEstablecimiento()
    {
        var elegido = EstablecimientoPlantelHelper.ElegirPlantelPorDefecto(Planteles(), "S1", 200, 2, 2026);
        Assert.Equal(3, elegido!.Id);
    }

    [Fact]
    public void ElegirPlantelPorDefecto_ConVariosEstablecimientosYSoloHistoricos_NoEligeNada()
    {
        var soloHistoricos = Planteles().Where(p => p.EstablecimientoId == null).ToList();
        var elegido = EstablecimientoPlantelHelper.ElegirPlantelPorDefecto(soloHistoricos, "S1", 300, 2, 2026);
        Assert.Null(elegido);
    }

    [Fact]
    public void ElegirPlantelPorDefecto_SinEstablecimientoElegidoYVariosEstablecimientos_NoEligeNada()
    {
        // Todavia no se dijo en que establecimiento se inspecciona: no hay existencia anterior que mostrar.
        Assert.Null(EstablecimientoPlantelHelper.ElegirPlantelPorDefecto(Planteles(), "S1", null, 2, 2026));
        // Con un solo establecimiento (o ninguno cargado) se mantiene el comportamiento historico:
        // el plantel mas reciente del año (Id mas alto).
        Assert.Equal(3, EstablecimientoPlantelHelper.ElegirPlantelPorDefecto(Planteles(), "S1", null, 1, 2026)!.Id);
        Assert.Equal(3, EstablecimientoPlantelHelper.ElegirPlantelPorDefecto(Planteles(), "S1", null, 0, 2026)!.Id);
    }

    [Fact]
    public void ElegirPlantelPorDefecto_ConUnSoloEstablecimiento_AceptaHistoricos()
    {
        var soloHistoricos = Planteles().Where(p => p.EstablecimientoId == null).ToList();
        var elegido = EstablecimientoPlantelHelper.ElegirPlantelPorDefecto(soloHistoricos, "S1", 300, 1, 2026);
        Assert.Equal(4, elegido!.Id);
    }

    [Fact]
    public void PlantelesCompatibles_ExcluyeOtroEstablecimientoYOtroSocio()
    {
        var compatibles = EstablecimientoPlantelHelper.PlantelesCompatibles(Planteles(), "S1", 100);
        Assert.Equal(new[] { 2, 1, 4 }, compatibles.Select(p => p.Id));
    }

    [Fact]
    public void GenerarCodigoDisponible_DosEstablecimientosDelMismoAnio_DanCodigosDistintosSinTruncar()
    {
        // Codigos de la razon social: el plantel 2025 y el 2026 del primer establecimiento.
        var ocupados = new List<string?> { "5RA01", "6RA01" };

        var segundoEstablecimiento = EstablecimientoPlantelHelper.GenerarCodigoDisponible("2026", "RA01", ocupados);
        ocupados.Add(segundoEstablecimiento);
        var tercerEstablecimiento = EstablecimientoPlantelHelper.GenerarCodigoDisponible("2026", "RA01", ocupados);

        Assert.Equal("6RA011", segundoEstablecimiento);
        Assert.Equal("6RA012", tercerEstablecimiento);
        Assert.Equal(3, ocupados.Concat(new[] { tercerEstablecimiento }).Count(c => c!.StartsWith("6RA01")));
        Assert.All(new[] { segundoEstablecimiento, tercerEstablecimiento },
            c => Assert.True(c.Length <= EstablecimientoPlantelHelper.CodigoMaxLength));
        Assert.Null(EstablecimientoPlantelHelper.ValidarCodigo(segundoEstablecimiento));
    }

    [Fact]
    public void GenerarCodigoDisponible_SinColision_DevuelveElCodigoBase()
    {
        var codigo = EstablecimientoPlantelHelper.GenerarCodigoDisponible("2026", " RA01 ", new[] { "5RA01" });
        Assert.Equal("6RA01", codigo);
    }

    [Fact]
    public void GenerarCodigoDisponible_NuncaTrunca_RechazaCodigoMasLargoQuePlacod()
    {
        // Cola de 9 caracteres: con el digito del año llega a 10 (entra), con sufijo ya no.
        var cola = "ABCDEFGHI";
        var base10 = EstablecimientoPlantelHelper.GenerarCodigoDisponible("2026", cola, Array.Empty<string>());
        Assert.Equal(10, base10.Length);

        var ex = Assert.Throws<InvalidOperationException>(() =>
            EstablecimientoPlantelHelper.GenerarCodigoDisponible("2026", cola, new[] { base10 }));
        Assert.Contains("máximo es 10", ex.Message);

        Assert.NotNull(EstablecimientoPlantelHelper.ValidarCodigo("6ABCDEFGHI1"));
        Assert.Null(EstablecimientoPlantelHelper.ValidarCodigo("6ABCDEFGHI"));
    }

    [Fact]
    public void BuscarPlantelDestino_SeparaElPropioDelHistorico()
    {
        var planteles = Planteles();
        planteles.Add(new PlantelDTO { Id = 6, Placod = "6RA09", Anioex = "2026", Nrocri = "S1", EstablecimientoId = null });

        var destino = EstablecimientoPlantelHelper.BuscarPlantelDestino(planteles, "S1", 300, "2026", out var historico);

        Assert.Null(destino);
        Assert.Equal(6, historico!.Id);
    }
}

public class PlantelControllerEstablecimientoTests
{
    [Fact]
    public async Task Guardar_RechazaEstablecimientoDeOtraRazonSocial()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        var controller = CreateController(ctx);

        var result = await controller.Guardar(new PlantelDTO
        {
            Placod = "6RA01", Anioex = "2026", Nrocri = "S1", EstablecimientoId = 200, Estado = "S"
        });

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        var body = Assert.IsType<Respuesta<PlantelDTO>>(bad.Value);
        Assert.Contains("no pertenece", body.Mensaje);
        Assert.False(await ctx.Planteles.AnyAsync(p => p.Placod == "6RA01"));
    }

    [Fact]
    public async Task Guardar_AceptaEstablecimientoPropioYLoPersiste()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        var controller = CreateController(ctx);

        var result = await controller.Guardar(new PlantelDTO
        {
            Placod = "6RA01", Anioex = "2026", Nrocri = "S1", EstablecimientoId = 101, Estado = "S"
        });

        var created = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status201Created, created.StatusCode);
        var guardado = await ctx.Planteles.AsNoTracking().SingleAsync(p => p.Placod == "6RA01");
        Assert.Equal(101, guardado.EstablecimientoId);
    }

    [Fact]
    public async Task GetOrCreate_CreaUnPlantelPorEstablecimientoParaElMismoSocioYAnio()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        // El request legacy lleva el codigo de socio como int: socio "77" con dos establecimientos.
        ctx.Socios.Add(new Socio { Id = 3, Scod = "77", Nombre = "Socio Setenta" });
        ctx.Estables.AddRange(
            new Estable { Id = 300, Ecod = "E300", Codsoc = "77", Nombre = "Campo A", Activo = "S", Fecing = "" },
            new Estable { Id = 301, Ecod = "E301", Codsoc = "77", Nombre = "Campo B", Activo = "S", Fecing = "" });
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
        var controller = CreateController(ctx);

        // Cada llamada simula un request distinto (DbContext scoped): se limpia el tracker entre ellas.
        var primero = await controller.GetOrCreate(new PlantelGetOrCreateRequest
        {
            Anioex = "2026", Nrocri = 77, BasePlacod = "RA01", EstablecimientoId = 300, Varede = 10
        });
        ctx.ChangeTracker.Clear();
        var segundo = await controller.GetOrCreate(new PlantelGetOrCreateRequest
        {
            Anioex = "2026", Nrocri = 77, BasePlacod = "RA01", EstablecimientoId = 301, Varede = 5
        });
        ctx.ChangeTracker.Clear();
        var repetido = await controller.GetOrCreate(new PlantelGetOrCreateRequest
        {
            Anioex = "2026", Nrocri = 77, BasePlacod = "RA01", EstablecimientoId = 300, Varede = 12
        });
        ctx.ChangeTracker.Clear();
        var ajeno = await controller.GetOrCreate(new PlantelGetOrCreateRequest
        {
            Anioex = "2026", Nrocri = 77, BasePlacod = "RA01", EstablecimientoId = 100, Varede = 1
        });
        ctx.ChangeTracker.Clear();

        Assert.Equal(StatusCodes.Status201Created, Assert.IsType<ObjectResult>(primero).StatusCode);
        Assert.Equal(StatusCodes.Status201Created, Assert.IsType<ObjectResult>(segundo).StatusCode);
        Assert.IsType<OkObjectResult>(repetido);
        Assert.IsType<BadRequestObjectResult>(ajeno);

        var del2026 = await ctx.Planteles.AsNoTracking().Where(p => p.Anioex == "2026" && p.Nrocri == "77").ToListAsync();
        Assert.Equal(2, del2026.Count);
        Assert.Equal(2, del2026.Select(p => p.Placod).Distinct().Count());
        Assert.Equal(12, del2026.Single(p => p.EstablecimientoId == 300).Varede);
        Assert.Equal(5, del2026.Single(p => p.EstablecimientoId == 301).Varede);

        // Sin colisiones ni truncamiento: cada establecimiento tiene su codigo completo, dentro del
        // ancho de PLACOD, y el codigo identifica al plantel de forma univoca dentro del socio.
        Assert.Equal("6RA01", del2026.Single(p => p.EstablecimientoId == 300).Placod);
        Assert.Equal("6RA011", del2026.Single(p => p.EstablecimientoId == 301).Placod);
        Assert.All(del2026, p => Assert.True(p.Placod!.Length <= EstablecimientoPlantelHelper.CodigoMaxLength));
        Assert.Equal(300, (await ctx.Planteles.AsNoTracking().SingleAsync(p => p.Nrocri == "77" && p.Placod == "6RA01")).EstablecimientoId);
        Assert.Equal(301, (await ctx.Planteles.AsNoTracking().SingleAsync(p => p.Nrocri == "77" && p.Placod == "6RA011")).EstablecimientoId);
    }

    [Fact]
    public async Task Guardar_RechazaCodigoMasLargoQuePlacodEnVezDeTruncar()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        var controller = CreateController(ctx);

        var result = await controller.Guardar(new PlantelDTO
        {
            Placod = "6RA01234567", Anioex = "2026", Nrocri = "S1", EstablecimientoId = 100, Estado = "S"
        });

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        var body = Assert.IsType<Respuesta<PlantelDTO>>(bad.Value);
        Assert.Equal(0, body.Exito);
        Assert.Contains("máximo es 10", body.Mensaje);
        Assert.False(await ctx.Planteles.AnyAsync(p => p.Anioex == "2026"));
    }

    [Fact]
    public async Task GetOrCreate_RechazaEnVezDeTruncarCuandoElSufijoNoEntraEnPlacod()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        ctx.Socios.Add(new Socio { Id = 3, Scod = "77", Nombre = "Socio Setenta" });
        ctx.Estables.AddRange(
            new Estable { Id = 300, Ecod = "E300", Codsoc = "77", Nombre = "Campo A", Activo = "S", Fecing = "" },
            new Estable { Id = 301, Ecod = "E301", Codsoc = "77", Nombre = "Campo B", Activo = "S", Fecing = "" });
        // Un plantel con el codigo base de 10 caracteres ya ocupado por el primer establecimiento.
        ctx.Planteles.Add(new Plantel { Id = 50, Placod = "6ABCDEFGHI", Anioex = "2026", Nrocri = "77", EstablecimientoId = 300, Estado = "A" });
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
        var controller = CreateController(ctx);

        var result = await controller.GetOrCreate(new PlantelGetOrCreateRequest
        {
            Anioex = "2026", Nrocri = 77, BasePlacod = "ABCDEFGHI", EstablecimientoId = 301, Varede = 1
        });

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("máximo es 10", Assert.IsType<Respuesta<PlantelDTO>>(bad.Value).Mensaje);
        Assert.Equal(1, await ctx.Planteles.CountAsync(p => p.Nrocri == "77"));
    }

    [Fact]
    public async Task ExportarExcel_ConEstablecimientoId_SoloIncluyeLosPlantelesDeEseEstablecimiento()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx); // S1: 5RA01 (2025, establecimiento 100) y 4RA01 (2024, sin asignar)
        var controller = CreateController(ctx);

        var hojasTodos = await HojasDelExcel(controller, establecimientoId: null);
        var hojasNorte = await HojasDelExcel(controller, establecimientoId: 100);
        var hojasSinAsignar = await HojasDelExcel(controller, establecimientoId: EstablecimientoPlantelHelper.FiltroSinEstablecimiento);

        Assert.Contains("Año 2025", hojasTodos.Keys);
        Assert.Contains("Año 2024", hojasTodos.Keys);

        Assert.Contains("Año 2025", hojasNorte.Keys);
        Assert.DoesNotContain("Año 2024", hojasNorte.Keys);
        Assert.Equal("5RA01", hojasNorte["Año 2025"].Single());

        Assert.Contains("Año 2024", hojasSinAsignar.Keys);
        Assert.DoesNotContain("Año 2025", hojasSinAsignar.Keys);
        Assert.Equal("4RA01", hojasSinAsignar["Año 2024"].Single());
    }

    /// <summary>Nombre de hoja -> códigos de plantel listados (sin la fila de total).</summary>
    private static async Task<Dictionary<string, List<string>>> HojasDelExcel(PlantelController controller, int? establecimientoId)
    {
        var result = await controller.ExportarExcel("2020-01-01", "2026-12-31", "Todos", establecimientoId);
        var file = Assert.IsType<FileContentResult>(result);

        OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        using var package = new OfficeOpenXml.ExcelPackage(new MemoryStream(file.FileContents));
        var hojas = new Dictionary<string, List<string>>();
        foreach (var ws in package.Workbook.Worksheets)
        {
            var codigos = new List<string>();
            for (var r = 2; r <= (ws.Dimension?.End.Row ?? 1); r++)
            {
                var valor = ws.Cells[r, 1].Text;
                if (string.IsNullOrWhiteSpace(valor) || valor.StartsWith("TOTAL", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                codigos.Add(valor);
            }
            hojas[ws.Name] = codigos;
        }

        return hojas;
    }

    private static PlantelController CreateController(hereford_prContext ctx)
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();
        return new PlantelController(
            new PlantelRepositorio(ctx),
            new Resin1Repositorio(ctx),
            new EstableRepositorio(ctx),
            mapper,
            new TestUserSocioContextService(TestDb.Privilegiado()))
        {
            ControllerContext = TestDb.AdminContext()
        };
    }
}

public class Resin1ControllerEstablecimientoTests
{
    [Fact]
    public async Task Guardar_RechazaPlantelDeOtroEstablecimientoDeLaMismaRazonSocial()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        var controller = CreateController(ctx);

        // Inspeccion en Campo Sur (E101) que intenta escribir el plantel de Campo Norte (100).
        var result = await controller.Guardar(new Resin1DTO
        {
            Scod = "S1", Icod = "I1", Estcod = "E101", Nropla = "5RA01", Freali = new DateTime(2026, 3, 1)
        });

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        var body = Assert.IsType<Respuesta<Resin1DTO>>(bad.Value);
        Assert.Contains("pertenece al establecimiento", body.Mensaje);
        Assert.False(await ctx.Resin1s.AnyAsync());
    }

    [Fact]
    public async Task Guardar_AceptaPlantelDelMismoEstablecimientoYPlantelSinAsignar()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        var controller = CreateController(ctx);

        var propio = await controller.Guardar(new Resin1DTO
        {
            Scod = "S1", Icod = "I1", Estcod = "E100", Nropla = "5RA01", Freali = new DateTime(2026, 3, 1)
        });
        var historico = await controller.Guardar(new Resin1DTO
        {
            Scod = "S1", Icod = "I1", Estcod = "E101", Nropla = "4RA01", Freali = new DateTime(2026, 3, 1)
        });

        Assert.IsType<OkObjectResult>(propio);
        Assert.IsType<OkObjectResult>(historico);
        Assert.Equal(2, await ctx.Resin1s.CountAsync());
    }

    private static Resin1Controller CreateController(hereford_prContext ctx)
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();
        return new Resin1Controller(
            new Resin1Repositorio(ctx),
            mapper,
            new TestUserSocioContextService(TestDb.Privilegiado()),
            ctx)
        {
            ControllerContext = TestDb.AdminContext()
        };
    }
}

public class CertifsemanControllerEstablecimientoTests
{
    [Fact]
    public async Task Guardar_RechazaEstablecimientoDeOtraRazonSocial()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        var controller = CreateController(ctx);

        var result = await controller.Guardar(new CertifsemanDTO
        {
            NroCert = "77", Hba = "HBA77", Nrocen = "C1", Nrocri = "S1", EstablecimientoId = 200, NrDosi = 10, NrDosiOr = "10"
        });

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        var body = Assert.IsType<Respuesta<CertifsemanDTO>>(bad.Value);
        Assert.Contains("no pertenece", body.Mensaje);
        Assert.False(await ctx.Certifsemen.AnyAsync(c => c.NroCert == "77"));
    }

    [Fact]
    public async Task Guardar_PersisteElEstablecimientoDelStock()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        var controller = CreateController(ctx);

        var result = await controller.Guardar(new CertifsemanDTO
        {
            NroCert = "78", Hba = "HBA78", Nrocen = "C1", Nrocri = "S1", EstablecimientoId = 101, NrDosi = 10, NrDosiOr = "10"
        });

        Assert.IsType<OkObjectResult>(result);
        var guardado = await ctx.Certifsemen.AsNoTracking().SingleAsync(c => c.NroCert == "78");
        Assert.Equal(101, guardado.EstablecimientoId);
    }

    private static CertifsemanController CreateController(hereford_prContext ctx)
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();
        return new CertifsemanController(
            new CertifsemanRepositorio(ctx),
            new EstableRepositorio(ctx),
            mapper,
            new TestUserSocioContextService(TestDb.Privilegiado()))
        {
            ControllerContext = TestDb.AdminContext()
        };
    }
}

public class Desepla3ControllerEstablecimientoTests
{
    [Fact]
    public async Task Guardar_RechazaToroDeOtroEstablecimientoQueElPlantel()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        ctx.Desepla1s.Add(new Desepla1 { Id = 1, Nrodec = "900", Nrocri = "S1", Nroplan = "5RA01", Tipse = "NA" });
        ctx.Torosunis.AddRange(
            new Torosuni { Id = 1, Hba = "T-NORTE", Criador = "S1", EstablecimientoId = 100, CodEstado = "1", Fecing = "" },
            new Torosuni { Id = 2, Hba = "T-SUR", Criador = "S1", EstablecimientoId = 101, CodEstado = "1", Fecing = "" },
            new Torosuni { Id = 3, Hba = "T-LIBRE", Criador = "S1", EstablecimientoId = null, CodEstado = "1", Fecing = "" });
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
        var controller = CreateController(ctx);

        var deOtroCampo = await controller.Guardar(new Desepla3DTO { Nrodec = "900", Tipo = "toro", Hardb = "T-SUR", Cantv = 1 });
        var propio = await controller.Guardar(new Desepla3DTO { Nrodec = "900", Tipo = "toro", Hardb = "T-NORTE", Cantv = 1 });
        var sinAsignar = await controller.Guardar(new Desepla3DTO { Nrodec = "900", Tipo = "toro", Hardb = "T-LIBRE", Cantv = 1 });

        var bad = Assert.IsType<BadRequestObjectResult>(deOtroCampo);
        var body = Assert.IsType<Respuesta<Desepla3DTO>>(bad.Value);
        Assert.Contains("mismo establecimiento", body.Mensaje);
        Assert.Equal(StatusCodes.Status200OK, Assert.IsType<ObjectResult>(propio).StatusCode);
        Assert.Equal(StatusCodes.Status200OK, Assert.IsType<ObjectResult>(sinAsignar).StatusCode);
        Assert.Equal(2, await ctx.Desepla3s.CountAsync());
    }

    [Fact]
    public async Task Guardar_RechazaSemenDeOtroEstablecimientoQueElPlantel()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        ctx.Desepla1s.Add(new Desepla1 { Id = 1, Nrodec = "901", Nrocri = "S1", Nroplan = "5RA01", Tipse = "IA" });
        ctx.Certifsemen.Add(new Certifseman { Id = 1, Hba = "SEMEN-SUR", Nrocri = "S1", NroCert = "1", Nrocen = "C1", EstablecimientoId = 101, NrDosi = 5 });
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
        var controller = CreateController(ctx);

        var result = await controller.Guardar(new Desepla3DTO { Nrodec = "901", Tipo = "insem", Hardb = "SEMEN-SUR", Cantv = 1 });

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("El semen", Assert.IsType<Respuesta<Desepla3DTO>>(bad.Value).Mensaje);
        Assert.Equal(0, await ctx.Desepla3s.CountAsync());
    }

    private static Desepla3Controller CreateController(hereford_prContext ctx)
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();
        return new Desepla3Controller(
            new Desepla3Repositorio(ctx),
            mapper,
            new TestUserSocioContextService(TestDb.Privilegiado()),
            ctx)
        {
            ControllerContext = TestDb.AdminContext()
        };
    }
}

public class TranssbControllerEstablecimientoTests
{
    [Fact]
    public async Task Guardar_TransferenciaEntreRazonesSociales_CambiaDueñoYEstablecimiento()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        ctx.Torosunis.Add(ToroDeS1EnCampoNorte());
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
        var controller = CreateController(ctx);

        var result = await controller.Guardar(new TranssbDTO
        {
            NroTrans = "T-0", Sven = "S1", Scom = "S2", Vnom = "Socio Uno", Cnom = "Socio Dos", Ecod = "E200", Torovendido = 1
        });

        var ok = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        var body = Assert.IsType<Respuesta<TranssbDTO>>(ok.Value);
        Assert.Equal(1, body.Exito);
        Assert.NotNull(body.List);
        var toro = await ctx.Torosunis.AsNoTracking().SingleAsync(t => t.Id == 1);
        Assert.Equal("S2", toro.Criador);
        Assert.Equal(200, toro.EstablecimientoId);
        Assert.Equal("E200", toro.Estcod);
        Assert.Equal(1, await ctx.Transsbs.CountAsync());
    }

    [Fact]
    public async Task Guardar_PermiteMovimientoInternoEntreEstablecimientosDeLaMismaRazonSocial()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        ctx.Torosunis.Add(ToroDeS1EnCampoNorte());
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
        var controller = CreateController(ctx);

        var result = await controller.Guardar(new TranssbDTO
        {
            NroTrans = "T-1", Sven = "S1", Scom = "S1", Vnom = "Socio Uno", Cnom = "Socio Uno", Ecod = "E101", Torovendido = 1
        });

        var ok = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.Equal(1, Assert.IsType<Respuesta<TranssbDTO>>(ok.Value).Exito);
        var toro = await ctx.Torosunis.AsNoTracking().SingleAsync(t => t.Id == 1);
        Assert.Equal(101, toro.EstablecimientoId);
        Assert.Equal("E101", toro.Estcod);
        Assert.Equal("S1", toro.Criador);
        Assert.Equal(1, await ctx.Transsbs.CountAsync());
    }

    [Fact]
    public async Task Guardar_RechazaMismaRazonSocialSiElToroYaEstaEnEseEstablecimiento()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        ctx.Torosunis.Add(ToroDeS1EnCampoNorte());
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
        var controller = CreateController(ctx);

        var result = await controller.Guardar(new TranssbDTO
        {
            NroTrans = "T-2", Sven = "S1", Scom = "S1", Vnom = "Socio Uno", Cnom = "Socio Uno", Ecod = "E100", Torovendido = 1
        });

        var error = AssertRechazo(result, StatusCodes.Status400BadRequest);
        Assert.Contains("ya esta en ese establecimiento", error.Mensaje);
        await AssertNadaCambioAsync(ctx);
    }

    [Fact]
    public async Task Guardar_RechazaEstablecimientoQueNoEsDelComprador()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        ctx.Torosunis.Add(ToroDeS1EnCampoNorte());
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
        var controller = CreateController(ctx);

        // E101 es de S1, no del comprador S2.
        var result = await controller.Guardar(new TranssbDTO
        {
            NroTrans = "T-3", Sven = "S1", Scom = "S2", Vnom = "Socio Uno", Cnom = "Socio Dos", Ecod = "E101", Torovendido = 1
        });

        var error = AssertRechazo(result, StatusCodes.Status400BadRequest);
        Assert.Contains("no pertenece al socio comprador", error.Mensaje);
        await AssertNadaCambioAsync(ctx);
    }

    [Fact]
    public async Task Guardar_RechazaToroQueNoEsDelVendedor()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        ctx.Torosunis.Add(ToroDeS1EnCampoNorte());
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
        var controller = CreateController(ctx);

        // El toro es de S1 pero el vendedor declarado es S2.
        var result = await controller.Guardar(new TranssbDTO
        {
            NroTrans = "T-4", Sven = "S2", Scom = "S1", Vnom = "Socio Dos", Cnom = "Socio Uno", Ecod = "E100", Torovendido = 1
        });

        var error = AssertRechazo(result, StatusCodes.Status400BadRequest);
        Assert.Contains("no pertenece al socio vendedor", error.Mensaje);
        await AssertNadaCambioAsync(ctx);
    }

    [Fact]
    public async Task Guardar_ErrorDePersistencia_Devuelve500ConMensajeSeguroYDetalleSoloEnElLog()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        ctx.Torosunis.Add(ToroDeS1EnCampoNorte());
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
        var log = new CapturaLogger<TranssbController>();
        var controller = CreateController(ctx, new TranssbRepositorioQueFalla(new TranssbRepositorio(ctx)), log);

        var result = await controller.Guardar(new TranssbDTO
        {
            NroTrans = "T-5", Sven = "S1", Scom = "S2", Vnom = "Socio Uno", Cnom = "Socio Dos", Ecod = "E200", Torovendido = 1
        });

        // Status 500 + Exito 0 (el cliente nunca lo lee como exito) y un mensaje funcional sin la base adentro.
        var error = AssertRechazo(result, StatusCodes.Status500InternalServerError);
        AssertMensajeSeguro(error.Mensaje, "guardar la transferencia");

        // El motivo real ("Data too long for column 'ECOD'") queda unicamente en el log del servidor.
        var registro = Assert.Single(log.Errores);
        Assert.Contains("Transsb.Guardar", registro.Mensaje);
        Assert.Contains("Data too long for column 'ECOD'", registro.Excepcion!.GetBaseException().Message);
        await AssertNadaCambioAsync(ctx);
    }

    [Fact]
    public async Task Editar_ErrorDePersistencia_Devuelve500ConMensajeSeguroYNoTocaElToro()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        ctx.Torosunis.Add(ToroDeS1EnCampoNorte());
        ctx.Transsbs.Add(new Transsb { Id = 1, NroTrans = "T-6", Sven = "S1", Scom = "S2", Vnom = "Socio Uno", Cnom = "Socio Dos", Ecod = "E200", Torovendido = 1 });
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
        var log = new CapturaLogger<TranssbController>();
        var controller = CreateController(ctx, new TranssbRepositorioQueFalla(new TranssbRepositorio(ctx)), log);

        var result = await controller.Editar(new TranssbDTO
        {
            Id = 1, NroTrans = "T-6", Sven = "S1", Scom = "S2", Vnom = "Socio Uno", Cnom = "Socio Dos", Ecod = "E200", Torovendido = 1
        });

        var error = AssertRechazo(result, StatusCodes.Status500InternalServerError);
        AssertMensajeSeguro(error.Mensaje, "editar la transferencia");
        var registro = Assert.Single(log.Errores);
        Assert.Contains("Transsb.Editar", registro.Mensaje);
        Assert.Contains("Data too long", registro.Excepcion!.GetBaseException().Message);
        // La transaccion no se confirmo: el toro sigue siendo de S1 en Campo Norte.
        var toro = await ctx.Torosunis.AsNoTracking().SingleAsync(t => t.Id == 1);
        Assert.Equal("S1", toro.Criador);
        Assert.Equal(100, toro.EstablecimientoId);
    }

    [Fact]
    public async Task Guardar_ExcepcionInternaNoControlada_NoSeExponeComo400()
    {
        // Una InvalidOperationException que NO es un rechazo de negocio (EF las usa para errores de
        // tracking, conexion, etc.) no puede terminar en un 400 con su texto: es un 500 con mensaje seguro.
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        ctx.Torosunis.Add(ToroDeS1EnCampoNorte());
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
        var log = new CapturaLogger<TranssbController>();
        var interna = new InvalidOperationException("The instance of entity type 'Transsb' cannot be tracked because another instance with the key value '{Id: 7}' is already being tracked.");
        var controller = CreateController(ctx, new TranssbRepositorioQueFalla(new TranssbRepositorio(ctx), interna), log);

        var result = await controller.Guardar(new TranssbDTO
        {
            NroTrans = "T-7", Sven = "S1", Scom = "S2", Vnom = "Socio Uno", Cnom = "Socio Dos", Ecod = "E200", Torovendido = 1
        });

        var error = AssertRechazo(result, StatusCodes.Status500InternalServerError);
        AssertMensajeSeguro(error.Mensaje, "guardar la transferencia");
        Assert.DoesNotContain("cannot be tracked", error.Mensaje);
        Assert.Same(interna, Assert.Single(log.Errores).Excepcion);
        await AssertNadaCambioAsync(ctx);
    }

    [Fact]
    public async Task Guardar_RechazaNumeroDeCertificadoMasLargoQueLaColumna()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        ctx.Torosunis.Add(ToroDeS1EnCampoNorte());
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
        var controller = CreateController(ctx);

        // TRANSSB.NRO_TRANS es varchar(6): antes esto terminaba en "Data too long" desde la base.
        var result = await controller.Guardar(new TranssbDTO
        {
            NroTrans = "QA-MOV-1", Sven = "S1", Scom = "S2", Vnom = "Socio Uno", Cnom = "Socio Dos", Ecod = "E200", Torovendido = 1
        });

        var error = AssertRechazo(result, StatusCodes.Status400BadRequest);
        Assert.Contains("6 caracteres", error.Mensaje);
        await AssertNadaCambioAsync(ctx);
    }

    [Fact]
    public async Task Editar_TransferenciaInexistente_DevuelveExito0()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        var controller = CreateController(ctx);

        var result = await controller.Editar(new TranssbDTO { Id = 999, NroTrans = "T-9", Sven = "S1", Scom = "S2", Ecod = "E200", Torovendido = 1 });

        var error = AssertRechazo(result, StatusCodes.Status404NotFound);
        Assert.Contains("No se encontró", error.Mensaje);
    }

    private static Torosuni ToroDeS1EnCampoNorte()
        => new() { Id = 1, Hba = "T1", Criador = "S1", EstablecimientoId = 100, Estcod = "E100", TipToro = "S", CodEstado = "1", Fecing = "" };

    /// <summary>Un rechazo SIEMPRE llega con status de error y Exito = 0: el cliente nunca puede leerlo como exito.</summary>
    private static Respuesta<TranssbDTO> AssertRechazo(IActionResult result, int statusEsperado)
    {
        var objectResult = Assert.IsAssignableFrom<ObjectResult>(result);
        Assert.Equal(statusEsperado, objectResult.StatusCode);
        var body = Assert.IsType<Respuesta<TranssbDTO>>(objectResult.Value);
        Assert.Equal(0, body.Exito);
        Assert.False(string.IsNullOrWhiteSpace(body.Mensaje));
        return body;
    }

    /// <summary>
    /// El usuario recibe un mensaje funcional y entendible; nada de SQL, columnas, excepciones EF/MariaDB
    /// ni stack traces (eso vive solo en el log del servidor).
    /// </summary>
    private static void AssertMensajeSeguro(string mensaje, string accionFallida)
    {
        Assert.Contains($"No se pudo {accionFallida}", mensaje);
        Assert.Contains("error interno", mensaje);
        foreach (var fragmentoTecnico in new[] { "Data too long", "ECOD", "column", "DbUpdateException", "MySql", "MariaDB", "entity changes", "SaveChanges", "Exception", "   at " })
        {
            Assert.DoesNotContain(fragmentoTecnico, mensaje, StringComparison.OrdinalIgnoreCase);
        }
    }

    private static async Task AssertNadaCambioAsync(hereford_prContext ctx)
    {
        Assert.Equal(0, await ctx.Transsbs.CountAsync());
        var toro = await ctx.Torosunis.AsNoTracking().SingleAsync(t => t.Id == 1);
        Assert.Equal("S1", toro.Criador);
        Assert.Equal(100, toro.EstablecimientoId);
        Assert.Equal("E100", toro.Estcod);
    }

    private static TranssbController CreateController(
        hereford_prContext ctx,
        ITranssbRepositorio? transsbRepositorio = null,
        ILogger<TranssbController>? logger = null)
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();
        return new TranssbController(
            transsbRepositorio ?? new TranssbRepositorio(ctx),
            new TorosRepositorio(ctx),
            mapper,
            ctx,
            new TestUserSocioContextService(TestDb.Privilegiado()),
            logger ?? NullLogger<TranssbController>.Instance)
        {
            ControllerContext = TestDb.AdminContext()
        };
    }

    /// <summary>
    /// Simula que la base rechaza el INSERT/UPDATE de la transferencia. Por defecto lanza el
    /// DbUpdateException tipico ("Data too long"); se puede pasar otra excepcion para probar
    /// errores internos que no son de negocio.
    /// </summary>
    private sealed class TranssbRepositorioQueFalla : ITranssbRepositorio
    {
        private readonly ITranssbRepositorio _inner;
        private readonly Exception _falla;

        public TranssbRepositorioQueFalla(ITranssbRepositorio inner, Exception? falla = null)
        {
            _inner = inner;
            _falla = falla ?? new DbUpdateException("An error occurred while saving the entity changes.", new InvalidOperationException("Data too long for column 'ECOD' at row 1"));
        }

        public Task<Transsb> Crear(Transsb entidad) => throw _falla;
        public Task<bool> Editar(Transsb entidad) => throw _falla;

        public Task<List<Transsb>> Lista(int page, int count) => _inner.Lista(page, count);
        public Task<Transsb> Obtener(System.Linq.Expressions.Expression<Func<Transsb, bool>> filtro = null!) => _inner.Obtener(filtro);
        public Task<List<Transsb>> LimitadosFiltrados(int skip, int take, string filtro = null!) => _inner.LimitadosFiltrados(skip, take, filtro);
        public Task<List<Transsb>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null!) => _inner.LimitadosFiltradosNoInclude(skip, take, filtro);
        public Task<bool> Eliminar(Transsb entidad) => _inner.Eliminar(entidad);
        public Task<IQueryable<Transsb>> Consultar(System.Linq.Expressions.Expression<Func<Transsb, bool>> filtro = null!) => _inner.Consultar(filtro);
        public Task<int> CantidadTotal() => _inner.CantidadTotal();
    }
}

public class Desepla1RepairAmbiguousPlantelsTests
{
    [Fact]
    public async Task RepairAmbiguousPlantels_NuncaTomaElPlantelDeOtraRazonSocial()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        // Codigos equivalentes en dos razones sociales: S1 tiene el RE06 de 2024, S2 solo el de 2023.
        // Sin filtrar por socio, la declaracion de S2 (2024) se iba al 4RE06 de S1 (año mas cercano).
        ctx.Planteles.AddRange(
            new Plantel { Id = 10, Placod = "4RE06", Anioex = "2024", Fecing = "2024/01/01", Nrocri = "S1", Estado = "A" },
            new Plantel { Id = 11, Placod = "3RE06", Anioex = "2023", Fecing = "2023/01/01", Nrocri = "S2", Estado = "A" });
        ctx.Desepla1s.Add(new Desepla1 { Id = 1, Nrodec = "20001", Nroplan = "RE06", Fecdecl = new DateTime(2024, 5, 1), Nrocri = "S2" });
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
        var controller = CreateController(ctx);

        var result = await controller.RepairAmbiguousPlantels();

        var body = Assert.IsType<Respuesta<Desepla1PlantelAmbiguityRepairResult>>(Assert.IsType<OkObjectResult>(result).Value);
        Assert.Equal(1, body.List!.DeclaracionesCorregidas);
        Assert.Equal(0, body.List.DeclaracionesPendientes);
        Assert.Equal("3RE06", (await ctx.Desepla1s.AsNoTracking().SingleAsync(x => x.Id == 1)).Nroplan);
    }

    [Fact]
    public async Task RepairAmbiguousPlantels_DosPlantelesDelMismoAnioEnDistintosEstablecimientos_QuedaPendiente()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        ctx.Planteles.AddRange(
            new Plantel { Id = 20, Placod = "6RA01", Anioex = "2026", Fecing = "2026/01/01", Nrocri = "S1", EstablecimientoId = 100, Estado = "A" },
            new Plantel { Id = 21, Placod = "6RA011", Anioex = "2026", Fecing = "2026/02/01", Nrocri = "S1", EstablecimientoId = 101, Estado = "A" });
        // "6RA0" no coincide exacto ni como sufijo con ninguno: ambos lo contienen y son del mismo año.
        ctx.Desepla1s.Add(new Desepla1 { Id = 2, Nrodec = "20002", Nroplan = "6RA0", Fecdecl = new DateTime(2026, 3, 1), Nrocri = "S1" });
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
        var controller = CreateController(ctx);

        var result = await controller.RepairAmbiguousPlantels();

        var body = Assert.IsType<Respuesta<Desepla1PlantelAmbiguityRepairResult>>(Assert.IsType<OkObjectResult>(result).Value);
        Assert.Equal(0, body.List!.DeclaracionesCorregidas);
        Assert.Equal(1, body.List.DeclaracionesPendientes);
        Assert.Contains("coincide con varios", body.List.Pendientes.Single().Motivo);
        Assert.Equal("6RA0", (await ctx.Desepla1s.AsNoTracking().SingleAsync(x => x.Id == 2)).Nroplan);
    }

    [Fact]
    public async Task RepairAmbiguousPlantels_DeclaracionSinRazonSocial_QuedaPendiente()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        ctx.Desepla1s.Add(new Desepla1 { Id = 3, Nrodec = "20003", Nroplan = "RA01", Fecdecl = new DateTime(2025, 3, 1), Nrocri = " " });
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
        var controller = CreateController(ctx);

        var result = await controller.RepairAmbiguousPlantels();

        var body = Assert.IsType<Respuesta<Desepla1PlantelAmbiguityRepairResult>>(Assert.IsType<OkObjectResult>(result).Value);
        Assert.Equal(1, body.List!.DeclaracionesPendientes);
        Assert.Contains("razón social", body.List.Pendientes.Single().Motivo);
        Assert.Equal("RA01", (await ctx.Desepla1s.AsNoTracking().SingleAsync(x => x.Id == 3)).Nroplan);
    }

    private static Desepla1Controller CreateController(hereford_prContext ctx)
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();
        return new Desepla1Controller(
            new Desepla1Repositorio(ctx),
            mapper,
            new TestUserSocioContextService(TestDb.Privilegiado()),
            ctx)
        {
            ControllerContext = TestDb.AdminContext()
        };
    }
}

public class PlantelDiagnosticoEstablecimientoTests
{
    [Fact]
    public async Task ListarGruposDuplicados_NoMezclaPlantelesDeDistintoEstablecimiento()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        // Mismo socio y año en dos establecimientos: legitimo. Dos del mismo establecimiento: duplicado.
        ctx.Planteles.AddRange(
            new Plantel { Id = 20, Placod = "6RA01", Anioex = "2026", Nrocri = "S1", EstablecimientoId = 100, Estado = "S" },
            new Plantel { Id = 21, Placod = "6RA02", Anioex = "2026", Nrocri = "S1", EstablecimientoId = 101, Estado = "S" },
            new Plantel { Id = 22, Placod = "6RA011", Anioex = "2026", Nrocri = "S1", EstablecimientoId = 100, Estado = "S" });
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();

        var repo = new PlantelDiagnosticoRepositorio(ctx, NullLogger<PlantelDiagnosticoRepositorio>.Instance);
        var grupos = await repo.ListarGruposDuplicadosAsync();

        var grupo = Assert.Single(grupos);
        Assert.Equal(100, grupo.EstablecimientoId);
        Assert.Equal(new[] { 20, 22 }, grupo.Planteles.Select(p => p.Id).OrderBy(x => x));
        Assert.Equal("6RA01", grupo.PlacodSugerido);
    }
}

internal static class TestDb
{
    public static hereford_prContext Create()
    {
        var options = new DbContextOptionsBuilder<hereford_prContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new hereford_prContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    /// <summary>
    /// S1 tiene DOS establecimientos (100 Campo Norte / 101 Campo Sur); S2 tiene uno (200).
    /// Planteles de S1: 5RA01 (2025, Norte) y 4RA01 (2024, sin asignar).
    /// </summary>
    public static void Seed(hereford_prContext ctx)
    {
        ctx.Socios.AddRange(
            new Socio { Id = 1, Scod = "S1", Nombre = "Socio Uno", Mail = "s1@test.com" },
            new Socio { Id = 2, Scod = "S2", Nombre = "Socio Dos", Mail = "s2@test.com" });
        ctx.Estables.AddRange(
            new Estable { Id = 100, Ecod = "E100", Codsoc = "S1", Nombre = "Campo Norte", Activo = "S", Fecing = "" },
            new Estable { Id = 101, Ecod = "E101", Codsoc = "S1", Nombre = "Campo Sur", Activo = "S", Fecing = "" },
            new Estable { Id = 200, Ecod = "E200", Codsoc = "S2", Nombre = "Campo Dos", Activo = "S", Fecing = "" });
        ctx.Planteles.AddRange(
            new Plantel { Id = 1, Placod = "5RA01", Anioex = "2025", Nrocri = "S1", EstablecimientoId = 100, Estado = "S", Varede = 10 },
            new Plantel { Id = 2, Placod = "4RA01", Anioex = "2024", Nrocri = "S1", EstablecimientoId = null, Estado = "S", Varede = 8 });
        ctx.SaveChanges();
        ctx.ChangeTracker.Clear();
    }

    public static UserSocioAccessContext Privilegiado() => new()
    {
        IsAuthenticated = true,
        IsSocioUser = false,
        IsPrivilegedUser = true,
        CurrentUser = new User { Id = 1 },
        AllowedSocioIds = Array.Empty<int>()
    };

    public static ControllerContext AdminContext() => new()
    {
        HttpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Role, "ADMINISTRADOR")
            }, "TestAuth"))
        }
    };
}

/// <summary>
/// Logger de prueba: guarda lo que el controller manda al log para poder afirmar que el detalle
/// tecnico de un error llega al servidor aunque al cliente le viaje un mensaje generico.
/// </summary>
internal sealed class CapturaLogger<T> : ILogger<T>
{
    public List<(LogLevel Nivel, string Mensaje, Exception? Excepcion)> Registros { get; } = new();

    public IEnumerable<(LogLevel Nivel, string Mensaje, Exception? Excepcion)> Errores
        => Registros.Where(r => r.Nivel >= LogLevel.Error);

    public IDisposable BeginScope<TState>(TState state) => SinScope.Instancia;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        => Registros.Add((logLevel, formatter(state, exception), exception));

    private sealed class SinScope : IDisposable
    {
        public static readonly SinScope Instancia = new();

        public void Dispose()
        {
        }
    }
}

internal sealed class TestUserSocioContextService : IUserSocioContextService
{
    private readonly UserSocioAccessContext _context;

    public TestUserSocioContextService(UserSocioAccessContext context)
    {
        _context = context;
    }

    public Task<UserSocioAccessContext> ResolveAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
        => Task.FromResult(_context);

    public Task<bool> CanAccessSocioAsync(ClaimsPrincipal principal, int socioId, CancellationToken cancellationToken = default)
        => Task.FromResult(true);
}
