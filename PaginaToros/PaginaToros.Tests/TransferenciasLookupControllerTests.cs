using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using PaginaToros.Server.Context;
using PaginaToros.Server.Controllers;
using PaginaToros.Server.Services;
using PaginaToros.Server.Utilidades;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Tests;

/// <summary>
/// Autorizacion de los lookups que usan las transferencias.
///
/// Regla: un usuario socio consulta datos COMPLETOS (planteles con existencias, establecimientos,
/// toros) solo de su razon social activa; la contraparte de una transferencia se consulta por
/// DestinoTransferencia, que exige que la razon social activa sea vendedor o comprador (misma
/// regla con la que Transan/Transsb/Futcontrol aceptan guardar) y devuelve datos minimos. Los
/// roles administrativos consultan cualquiera con datos completos.
///
/// Datos: TestDb (S1 con establecimientos 100/101 y planteles 1/2; S2 con establecimiento 200)
/// mas <see cref="SeedDestino"/> (planteles y un segundo establecimiento para S2, un tercer socio
/// S3 ajeno a toda transferencia, y toros transferibles de S1 y S2).
/// </summary>
public class TransferenciasLookupControllerTests
{
    // ----- PlantelesBySocioCode: origen de la transferencia, con existencias -----

    [Fact]
    public async Task PlantelesBySocioCode_UsuarioSocioConAccesoASuRazonSocial_DevuelvePlanteles()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        var controller = CreateController(ctx, SocioActivo(socioId: 1, scod: "S1"));

        var planteles = AssertOk<List<PlantelDTO>>(await controller.PlantelesBySocioCode("S1"));

        Assert.Equal(new[] { 1, 2 }, planteles.Select(p => p.Id).OrderBy(x => x));
        Assert.All(planteles, p => Assert.Equal("S1", p.Nrocri));
    }

    [Fact]
    public async Task PlantelesBySocioCode_UsuarioSocioSinRazonSocialActiva_Rechazado()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        var controller = CreateController(ctx, SocioSinRazonSocial());

        AssertProhibido<List<PlantelDTO>>(await controller.PlantelesBySocioCode("S1"));
    }

    [Fact]
    public async Task PlantelesBySocioCode_UsuarioSocioConsultaOtraRazonSocial_NoExponePlanteles()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        // S2 no tiene planteles cargados; S1 tiene dos. Un usuario de S2 pide los de S1.
        var controller = CreateController(ctx, SocioActivo(socioId: 2, scod: "S2"));

        AssertProhibido<List<PlantelDTO>>(await controller.PlantelesBySocioCode("S1"));
        // Tampoco filtrando por un establecimiento ajeno.
        AssertProhibido<List<PlantelDTO>>(await controller.PlantelesBySocioCode("S1", establecimientoId: 100));
    }

    [Fact]
    public async Task PlantelesBySocioCode_Administrador_PuedeConsultarCualquierRazonSocial()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        var controller = CreateController(ctx, TestDb.Privilegiado());

        var planteles = AssertOk<List<PlantelDTO>>(await controller.PlantelesBySocioCode("S1"));

        Assert.Equal(2, planteles.Count);
    }

    [Fact]
    public async Task PlantelesBySocioCode_UsuarioNoAutenticado_Rechazado()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        var controller = CreateController(ctx, new UserSocioAccessContext());

        AssertProhibido<List<PlantelDTO>>(await controller.PlantelesBySocioCode("S1"));
    }

    [Fact]
    public async Task PlantelesBySocioCode_UsuarioSocio_FiltraPorEstablecimientoPropio()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        ctx.Planteles.Add(new Plantel { Id = 3, Placod = "5RA02", Anioex = "2025", Nrocri = "S1", EstablecimientoId = 101, Estado = "S" });
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
        var controller = CreateController(ctx, SocioActivo(socioId: 1, scod: "S1"));

        // Campo Norte (100): su plantel 5RA01 mas el historico sin establecimiento (4RA01); nunca el de Campo Sur.
        var planteles = AssertOk<List<PlantelDTO>>(await controller.PlantelesBySocioCode("S1", establecimientoId: 100));

        Assert.Equal(new[] { 1, 2 }, planteles.Select(p => p.Id).OrderBy(x => x));
    }

    // ----- EstablecimientosBySocioCode y TorosTransferiblesBySocioCode: mismo criterio -----

    [Fact]
    public async Task EstablecimientosBySocioCode_UsuarioSocio_SoloSuRazonSocial()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        SeedDestino(ctx);
        var controller = CreateController(ctx, SocioActivo(socioId: 1, scod: "S1"));

        var propios = AssertOk<List<EstableDTO>>(await controller.EstablecimientosBySocioCode("S1"));
        Assert.Equal(new[] { 100, 101 }, propios.Select(e => e.Id).OrderBy(x => x));

        AssertProhibido<List<EstableDTO>>(await controller.EstablecimientosBySocioCode("S2"));
        AssertProhibido<List<EstableDTO>>(await controller.EstablecimientosBySocioCode("S3"));
    }

    [Fact]
    public async Task TorosTransferiblesBySocioCode_UsuarioSocio_SoloSuRazonSocial()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        SeedDestino(ctx);
        var controller = CreateController(ctx, SocioActivo(socioId: 1, scod: "S1"));

        var propios = AssertOk<List<TorosuniDTO>>(await controller.TorosTransferiblesBySocioCode("S1"));
        Assert.Equal(new[] { 1 }, propios.Select(t => t.Id));

        // Los toros del comprador nunca hacen falta para una transferencia: 403 sin lista.
        AssertProhibido<List<TorosuniDTO>>(await controller.TorosTransferiblesBySocioCode("S2"));
    }

    [Fact]
    public async Task EstablecimientosYToros_Administrador_CualquierRazonSocial()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        SeedDestino(ctx);
        var controller = CreateController(ctx, TestDb.Privilegiado());

        var establecimientos = AssertOk<List<EstableDTO>>(await controller.EstablecimientosBySocioCode("S2"));
        Assert.Equal(new[] { 200, 201 }, establecimientos.Select(e => e.Id).OrderBy(x => x));

        var toros = AssertOk<List<TorosuniDTO>>(await controller.TorosTransferiblesBySocioCode("S2"));
        Assert.Equal(new[] { 2 }, toros.Select(t => t.Id));
    }

    [Fact]
    public async Task EstablecimientosYToros_UsuarioNoAutenticado_Rechazado()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        var controller = CreateController(ctx, new UserSocioAccessContext());

        AssertProhibido<List<EstableDTO>>(await controller.EstablecimientosBySocioCode("S1"));
        AssertProhibido<List<TorosuniDTO>>(await controller.TorosTransferiblesBySocioCode("S1"));
    }

    // ----- DestinoTransferencia: contraparte de una transferencia, datos minimos -----

    [Fact]
    public async Task DestinoTransferencia_SocioVendedor_ConsultaCompradorConDatosMinimos()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        SeedDestino(ctx);
        // S1 vende a S2: S1 necesita los planteles y establecimientos de S2 para elegir el destino.
        var controller = CreateController(ctx, SocioActivo(socioId: 1, scod: "S1"));

        var destino = AssertOk<TransferenciaDestinoDTO>(
            await controller.DestinoTransferencia(vendedorSocioCode: "S1", compradorSocioCode: "S2"));

        Assert.Equal("S2", destino.CompradorSocioCode);
        Assert.False(destino.IncluyeExistencias);

        Assert.Equal(new[] { 10, 11, 12 }, destino.Planteles.Select(p => p.Id).OrderBy(x => x));
        var enCampoDos = destino.Planteles.Single(p => p.Id == 10);
        Assert.Equal("6RB01", enCampoDos.Placod);
        Assert.Equal("2026", enCampoDos.Anioex);
        Assert.Equal("S2", enCampoDos.Nrocri);
        Assert.Equal(200, enCampoDos.EstablecimientoId);
        Assert.NotNull(enCampoDos.Establecimiento);
        Assert.Equal("Campo Dos", enCampoDos.Establecimiento!.Nombre);
        Assert.All(destino.Planteles, AssertPlantelSinExistencias);

        Assert.Equal(new[] { 200, 201 }, destino.Establecimientos.Select(e => e.Id).OrderBy(x => x));
        var campoDos = destino.Establecimientos.Single(e => e.Id == 200);
        Assert.Equal("E200", campoDos.Ecod);
        Assert.Equal("Campo Dos", campoDos.Nombre);
        Assert.All(destino.Establecimientos, AssertEstablecimientoSinDatosDeContacto);
    }

    [Fact]
    public async Task DestinoTransferencia_SocioQueNoParticipa_Rechazado()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        SeedDestino(ctx);
        // S1 intenta usar el lookup de destino para una transferencia entre S3 y S2, en la que no participa.
        var controller = CreateController(ctx, SocioActivo(socioId: 1, scod: "S1"));

        AssertProhibido<TransferenciaDestinoDTO>(
            await controller.DestinoTransferencia(vendedorSocioCode: "S3", compradorSocioCode: "S2"));
        // Sin vendedor tampoco: el unico contexto valido es "mi razon social es una de las partes".
        AssertProhibido<TransferenciaDestinoDTO>(
            await controller.DestinoTransferencia(vendedorSocioCode: null, compradorSocioCode: "S2"));
        AssertProhibido<TransferenciaDestinoDTO>(
            await controller.DestinoTransferencia(vendedorSocioCode: "", compradorSocioCode: "S3"));
    }

    [Fact]
    public async Task DestinoTransferencia_SocioComprador_VeSuPropioDestinoCompleto()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        SeedDestino(ctx);
        // S2 es el comprador de una transferencia de S1: su propio destino va con existencias.
        var controller = CreateController(ctx, SocioActivo(socioId: 2, scod: "S2"));

        var destino = AssertOk<TransferenciaDestinoDTO>(
            await controller.DestinoTransferencia(vendedorSocioCode: "S1", compradorSocioCode: "S2"));

        Assert.True(destino.IncluyeExistencias);
        Assert.Equal(50, destino.Planteles.Single(p => p.Id == 10).Varede);
    }

    [Fact]
    public async Task DestinoTransferencia_MovimientoInterno_DatosCompletos()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        var controller = CreateController(ctx, SocioActivo(socioId: 1, scod: "S1"));

        var destino = AssertOk<TransferenciaDestinoDTO>(
            await controller.DestinoTransferencia(vendedorSocioCode: "S1", compradorSocioCode: "S1"));

        Assert.True(destino.IncluyeExistencias);
        Assert.Equal(new[] { 1, 2 }, destino.Planteles.Select(p => p.Id).OrderBy(x => x));
        Assert.Equal(10, destino.Planteles.Single(p => p.Id == 1).Varede);
    }

    [Fact]
    public async Task DestinoTransferencia_FiltroPorEstablecimientoDelComprador_Permitido()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        SeedDestino(ctx);
        var controller = CreateController(ctx, SocioActivo(socioId: 1, scod: "S1"));

        // Campo Dos (200): su plantel 6RB01 mas el que no tiene establecimiento; nunca el de Campo Dos Sur (201).
        var destino = AssertOk<TransferenciaDestinoDTO>(
            await controller.DestinoTransferencia(vendedorSocioCode: "S1", compradorSocioCode: "S2", establecimientoId: 200));

        Assert.Equal(new[] { 10, 11 }, destino.Planteles.Select(p => p.Id).OrderBy(x => x));
        Assert.All(destino.Planteles, AssertPlantelSinExistencias);
    }

    [Fact]
    public async Task DestinoTransferencia_EstablecimientoDeOtraRazonSocial_Rechazado()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        SeedDestino(ctx);
        var controller = CreateController(ctx, SocioActivo(socioId: 1, scod: "S1"));

        // Contexto manipulado: el establecimiento 300 es de S3 y el 100 del propio vendedor; ninguno es del comprador S2.
        AssertProhibido<TransferenciaDestinoDTO>(
            await controller.DestinoTransferencia(vendedorSocioCode: "S1", compradorSocioCode: "S2", establecimientoId: 300));
        AssertProhibido<TransferenciaDestinoDTO>(
            await controller.DestinoTransferencia(vendedorSocioCode: "S1", compradorSocioCode: "S2", establecimientoId: 100));
        AssertProhibido<TransferenciaDestinoDTO>(
            await controller.DestinoTransferencia(vendedorSocioCode: "S1", compradorSocioCode: "S2", establecimientoId: 9999));
    }

    [Fact]
    public async Task DestinoTransferencia_CompradorInexistente_NoEncontrado()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        var controller = CreateController(ctx, SocioActivo(socioId: 1, scod: "S1"));

        var result = await controller.DestinoTransferencia(vendedorSocioCode: "S1", compradorSocioCode: "S9");

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        var body = Assert.IsType<Respuesta<TransferenciaDestinoDTO>>(notFound.Value);
        Assert.Equal(0, body.Exito);
        Assert.Null(body.List);
    }

    [Fact]
    public async Task DestinoTransferencia_SinComprador_BadRequest()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        var controller = CreateController(ctx, SocioActivo(socioId: 1, scod: "S1"));

        var result = await controller.DestinoTransferencia(vendedorSocioCode: "S1", compradorSocioCode: " ");

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var body = Assert.IsType<Respuesta<TransferenciaDestinoDTO>>(badRequest.Value);
        Assert.Equal(0, body.Exito);
    }

    [Fact]
    public async Task DestinoTransferencia_Administrador_CualquierRazonSocialConDatosCompletos()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        SeedDestino(ctx);
        var controller = CreateController(ctx, TestDb.Privilegiado());

        // El administrador carga transferencias entre terceros: sin restriccion de partes y con existencias.
        var destino = AssertOk<TransferenciaDestinoDTO>(
            await controller.DestinoTransferencia(vendedorSocioCode: "S3", compradorSocioCode: "S2"));

        Assert.True(destino.IncluyeExistencias);
        Assert.Equal(50, destino.Planteles.Single(p => p.Id == 10).Varede);
        Assert.Equal("Calle 2", destino.Establecimientos.Single(e => e.Id == 200).Direcc);

        // Y sin indicar vendedor (en el formulario el administrador puede elegir primero el comprador).
        AssertOk<TransferenciaDestinoDTO>(await controller.DestinoTransferencia(vendedorSocioCode: null, compradorSocioCode: "S2"));
    }

    [Fact]
    public async Task DestinoTransferencia_UsuarioNoAutenticado_Rechazado()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        var controller = CreateController(ctx, new UserSocioAccessContext());

        AssertProhibido<TransferenciaDestinoDTO>(
            await controller.DestinoTransferencia(vendedorSocioCode: "S1", compradorSocioCode: "S2"));
    }

    [Fact]
    public async Task DestinoTransferencia_UsuarioSocioSinRazonSocialActiva_Rechazado()
    {
        using var ctx = TestDb.Create();
        TestDb.Seed(ctx);
        var controller = CreateController(ctx, SocioSinRazonSocial());

        AssertProhibido<TransferenciaDestinoDTO>(
            await controller.DestinoTransferencia(vendedorSocioCode: "S1", compradorSocioCode: "S2"));
    }

    // ----- Datos de prueba y helpers -----

    /// <summary>
    /// S2 gana un segundo establecimiento (201 Campo Dos Sur) y tres planteles: 6RB01 (2026, en 200,
    /// con existencias), 5RB01 (2025, sin establecimiento) y 6RB02 (2026, en 201). Los codigos no se
    /// repiten entre socios porque el modelo EF usa PLACOD como clave de Plantel. S3 es un socio
    /// ajeno con su establecimiento 300. Toros transferibles: 1 de S1 y 2 de S2.
    /// </summary>
    private static void SeedDestino(hereford_prContext ctx)
    {
        ctx.Socios.Add(new Socio { Id = 3, Scod = "S3", Nombre = "Socio Tres" });
        ctx.Estables.AddRange(
            new Estable { Id = 201, Ecod = "E201", Codsoc = "S2", Nombre = "Campo Dos Sur", Activo = "S", Fecing = "", Direcc = "Calle 3", Tel = "333", Encargado = "Encargado 3" },
            new Estable { Id = 300, Ecod = "E300", Codsoc = "S3", Nombre = "Campo Tres", Activo = "S", Fecing = "" });
        var campoDos = ctx.Estables.Single(e => e.Id == 200);
        campoDos.Direcc = "Calle 2";
        campoDos.Tel = "222";
        campoDos.Encargado = "Encargado 2";
        ctx.Planteles.AddRange(
            new Plantel { Id = 10, Placod = "6RB01", Anioex = "2026", Nrocri = "S2", EstablecimientoId = 200, Estado = "S", Varede = 50, Vqcsrd = 5, Vqssrd = 4, Varepr = 3, Vqcsrp = 2, Vqssrp = 1, Comment = "privado" },
            new Plantel { Id = 11, Placod = "5RB01", Anioex = "2025", Nrocri = "S2", EstablecimientoId = null, Estado = "S", Varede = 20 },
            new Plantel { Id = 12, Placod = "6RB02", Anioex = "2026", Nrocri = "S2", EstablecimientoId = 201, Estado = "S", Varede = 30 },
            new Plantel { Id = 20, Placod = "6RC01", Anioex = "2026", Nrocri = "S3", EstablecimientoId = 300, Estado = "S", Varede = 70 });
        ctx.Torosunis.AddRange(
            new Torosuni { Id = 1, Hba = "T-UNO", NomDad = "Toro Uno", Criador = "S1", EstablecimientoId = 100, TipToro = "S", CodEstado = "1", Fecing = "" },
            new Torosuni { Id = 2, Hba = "T-DOS", NomDad = "Toro Dos", Criador = "S2", EstablecimientoId = 200, TipToro = "S", CodEstado = "1", Fecing = "" });
        ctx.SaveChanges();
        ctx.ChangeTracker.Clear();
    }

    /// <summary>Usuario con rol Socio cuya razon social activa (UserSocio.ActiveSocioId) es la indicada.</summary>
    private static UserSocioAccessContext SocioActivo(int socioId, string scod) => new()
    {
        IsAuthenticated = true,
        IsSocioUser = true,
        IsPrivilegedUser = false,
        CurrentUser = new User { Id = 10 + socioId, Rol = "Socio", SocioId = socioId },
        AllowedSocioIds = new[] { socioId },
        ActiveSocioId = socioId,
        ActiveSocioCode = scod
    };

    /// <summary>Usuario con rol Socio pero sin ninguna razon social asignada en UserSocio.</summary>
    private static UserSocioAccessContext SocioSinRazonSocial() => new()
    {
        IsAuthenticated = true,
        IsSocioUser = true,
        IsPrivilegedUser = false,
        CurrentUser = new User { Id = 7, Rol = "Socio" },
        AllowedSocioIds = Array.Empty<int>(),
        ActiveSocioId = null,
        ActiveSocioCode = null
    };

    private static T AssertOk<T>(IActionResult result) where T : class
    {
        var ok = Assert.IsType<OkObjectResult>(result);
        var body = Assert.IsType<Respuesta<T>>(ok.Value);
        Assert.Equal(1, body.Exito);
        Assert.NotNull(body.List);
        return body.List!;
    }

    /// <summary>403 + Exito 0 + sin lista: no se filtra ni un registro.</summary>
    private static void AssertProhibido<T>(IActionResult result)
    {
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
        var body = Assert.IsType<Respuesta<T>>(objectResult.Value);
        Assert.Equal(0, body.Exito);
        Assert.Null(body.List);
        Assert.False(string.IsNullOrWhiteSpace(body.Mensaje));
    }

    /// <summary>Datos minimos de un plantel destino: ni existencias, ni comentarios, ni el socio.</summary>
    private static void AssertPlantelSinExistencias(PlantelDTO plantel)
    {
        Assert.False(string.IsNullOrWhiteSpace(plantel.Placod));
        Assert.Null(plantel.Varede);
        Assert.Null(plantel.Vqcsrd);
        Assert.Null(plantel.Vqssrd);
        Assert.Null(plantel.Varepr);
        Assert.Null(plantel.Vqcsrp);
        Assert.Null(plantel.Vqssrp);
        Assert.Null(plantel.Comment);
        Assert.Null(plantel.Nroins);
        Assert.Null(plantel.Feulti);
        Assert.Null(plantel.Socio);
        if (plantel.Establecimiento != null)
        {
            Assert.Null(plantel.Establecimiento.Direcc);
            Assert.Null(plantel.Establecimiento.Tel);
            Assert.Null(plantel.Establecimiento.Encargado);
        }
    }

    /// <summary>Datos minimos de un establecimiento destino: codigo y nombre, sin contacto ni ubicacion.</summary>
    private static void AssertEstablecimientoSinDatosDeContacto(EstableDTO establecimiento)
    {
        Assert.False(string.IsNullOrWhiteSpace(establecimiento.Nombre));
        Assert.Null(establecimiento.Direcc);
        Assert.Null(establecimiento.Locali);
        Assert.Null(establecimiento.Codpos);
        Assert.Null(establecimiento.Tel);
        Assert.Null(establecimiento.Encargado);
        Assert.Null(establecimiento.Plano);
        Assert.Null(establecimiento.Socio);
    }

    private static TransferenciasLookupController CreateController(hereford_prContext ctx, UserSocioAccessContext accessContext)
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();
        return new TransferenciasLookupController(
            ctx,
            mapper,
            new TestUserSocioContextService(accessContext),
            NullLogger<TransferenciasLookupController>.Instance)
        {
            ControllerContext = TestDb.AdminContext()
        };
    }
}
