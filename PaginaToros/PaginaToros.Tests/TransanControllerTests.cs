using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging.Abstractions;
using PaginaToros.Server.Context;
using PaginaToros.Server.Controllers;
using PaginaToros.Server.Repositorio.Implementacion;
using PaginaToros.Server.Services;
using PaginaToros.Server.Utilidades;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Security.Claims;

namespace PaginaToros.Tests;

public class TransanControllerTests
{
    [Fact]
    public async Task Guardar_SetsProcessDateOnCreate()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var controller = CreateController(scope.Context);
        var request = BuildCreateRequest();

        var result = await controller.Guardar(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        var response = Assert.IsType<Respuesta<TransanDTO>>(okResult.Value);

        Assert.Equal(1, response.Exito);
        Assert.NotNull(response.List);
        Assert.True(response.List!.FchUsu.HasValue);

        var persisted = await scope.Context.Transans.FirstAsync();
        Assert.True(persisted.FchUsu.HasValue);
    }

    [Fact]
    public async Task Editar_KeepsOriginalProcessDate()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var controller = CreateController(scope.Context);

        var createResult = await controller.Guardar(BuildCreateRequest());
        var createOk = Assert.IsType<OkObjectResult>(createResult);
        Assert.Equal(StatusCodes.Status200OK, createOk.StatusCode);
        var createResponse = Assert.IsType<Respuesta<TransanDTO>>(createOk.Value);
        Assert.NotNull(createResponse.List);

        var original = createResponse.List!;
        Assert.True(original.FchUsu.HasValue);
        var originalDate = original.FchUsu.Value;

        var editRequest = BuildEditRequest(original.Id, originalDate.AddDays(5));
        var editResult = await controller.Editar(editRequest);

        var editOk = Assert.IsType<OkObjectResult>(editResult);
        Assert.Equal(StatusCodes.Status200OK, editOk.StatusCode);
        var editResponse = Assert.IsType<Respuesta<TransanDTO>>(editOk.Value);

        Assert.Equal(1, editResponse.Exito);

        var persisted = await scope.Context.Transans.FirstAsync(x => x.Id == original.Id);
        Assert.Equal(originalDate, persisted.FchUsu);
    }

    [Fact]
    public async Task Eliminar_RevertsStock_AndDeletesTransfer()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var controller = CreateController(scope.Context);
        var createResult = await controller.Guardar(BuildCreateRequest());
        var createOk = Assert.IsType<OkObjectResult>(createResult);
        var createResponse = Assert.IsType<Respuesta<TransanDTO>>(createOk.Value);
        Assert.NotNull(createResponse.List);

        var created = createResponse.List!;
        var deleteResult = await controller.Eliminar(created.Id);
        var deleteOk = Assert.IsType<ObjectResult>(deleteResult);
        Assert.Equal(StatusCodes.Status200OK, deleteOk.StatusCode);
        var deleteResponse = Assert.IsType<Respuesta<string>>(deleteOk.Value);
        Assert.Equal(1, deleteResponse.Exito);

        var seller = await scope.Context.Planteles.FirstAsync(x => x.Id == 1);
        var buyer = await scope.Context.Planteles.FirstAsync(x => x.Id == 2);

        Assert.Equal(10, seller.Varede);
        Assert.Equal(5, buyer.Varede);
        Assert.False(await scope.Context.Transans.AnyAsync(x => x.Id == created.Id));
    }

    private static TransanController CreateController(hereford_prContext context)
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = mapperConfig.CreateMapper();
        var repo = new TransanRepositorio(context);
        var accessService = new TestUserSocioContextService();

        return new TransanController(
            context,
            repo,
            mapper,
            new TestWebHostEnvironment(),
            accessService,
            NullLogger<TransanController>.Instance)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, "admin"),
                        new Claim(ClaimTypes.Role, "ADMINISTRADOR")
                    }, "TestAuth"))
                }
            }
        };
    }

    private static TransanSaveRequestDTO BuildCreateRequest()
    {
        return new TransanSaveRequestDTO
        {
            Transan = new TransanDTO
            {
                NroCert = "1234",
                Fecvta = new DateTime(2024, 1, 15),
                CantHem = 2,
                CantMach = 0,
                CantChem = 0,
                CantCmach = 0,
                Tiphac = "PHPR",
                Hemsta = "CC",
                Tipani = "H",
                Tipohem = "VA",
                Sven = "1000",
                Vnom = "Vendedor A",
                Cnom = "Comprador B",
                Scom = "2000",
                Plant = "P001",
                NvoPla = "P002",
                PlantOrigenId = 1,
                PlantDestinoId = 2
            },
            VendedorId = 1,
            CompradorId = 2,
            MailComprador = "comprador@test.com",
            PlantOrigenId = 1,
            PlantDestinoId = 2
        };
    }

    private static TransanSaveRequestDTO BuildEditRequest(int id, DateTime originalDate)
    {
        return new TransanSaveRequestDTO
        {
            Transan = new TransanDTO
            {
                Id = id,
                NroCert = "1234",
                Fecvta = new DateTime(2024, 1, 15),
                FchUsu = originalDate,
                CantHem = 3,
                CantMach = 0,
                CantChem = 0,
                CantCmach = 0,
                Tiphac = "PHPR",
                Hemsta = "CC",
                Tipani = "H",
                Tipohem = "VA",
                Sven = "1000",
                Vnom = "Vendedor A",
                Cnom = "Comprador B",
                Scom = "2000",
                Plant = "P001",
                NvoPla = "P002",
                PlantOrigenId = 1,
                PlantDestinoId = 2
            },
            VendedorId = 1,
            CompradorId = 2,
            MailComprador = "comprador@test.com",
            PlantOrigenId = 1,
            PlantDestinoId = 2
        };
    }

    private static TestContextScope CreateContext()
    {
        var options = new DbContextOptionsBuilder<hereford_prContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new hereford_prContext(options);
        context.Database.EnsureCreated();
        return new TestContextScope(context);
    }

    private static void Seed(hereford_prContext context)
    {
        context.Socios.AddRange(
            new Socio
            {
                Id = 1,
                Scod = "1000",
                Nombre = "Vendedor A",
                Mail = "vendedor@test.com"
            },
            new Socio
            {
                Id = 2,
                Scod = "2000",
                Nombre = "Comprador B",
                Mail = "comprador@test.com"
            });

        context.Planteles.AddRange(
            new Plantel
            {
                Id = 1,
                Placod = "P001",
                Anioex = "2024",
                Nrocri = "1000",
                Estado = "A",
                Fecing = "2024/01/01",
                Varede = 10
            },
            new Plantel
            {
                Id = 2,
                Placod = "P002",
                Anioex = "2024",
                Nrocri = "2000",
                Estado = "A",
                Fecing = "2024/01/01",
                Varede = 5
            });

        context.Transans.Add(new Transan
        {
            Id = 1,
            NroCert = "0001",
            Fecvta = new DateTime(2024, 1, 10),
            FchUsu = new DateTime(2024, 1, 11),
            Sven = "1000",
            Vnom = "Vendedor A",
            Scom = "2000",
            Cnom = "Comprador B",
            Plant = "P001",
            NvoPla = "P002",
            PlantOrigenId = 1,
            PlantDestinoId = 2,
            CantHem = 2,
            CantMach = 0,
            CantChem = 0,
            CantCmach = 0,
            Tiphac = "PHPR",
            Hemsta = "CC",
            Tipani = "H",
            Tipohem = "VA",
            Incorp = 1
        });

        context.SaveChanges();
        context.ChangeTracker.Clear();
    }

    private sealed class TestUserSocioContextService : IUserSocioContextService
    {
        public Task<UserSocioAccessContext> ResolveAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
            => Task.FromResult(new UserSocioAccessContext
            {
                IsAuthenticated = true,
                IsSocioUser = false,
                IsPrivilegedUser = true,
                AllowedSocioIds = Array.Empty<int>()
            });

        public Task<bool> CanAccessSocioAsync(ClaimsPrincipal principal, int socioId, CancellationToken cancellationToken = default)
            => Task.FromResult(true);
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

    private sealed class TestWebHostEnvironment : IWebHostEnvironment
    {
        public string ApplicationName { get; set; } = "PaginaToros.Tests";
        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
        public string WebRootPath { get; set; } = string.Empty;
        public string EnvironmentName { get; set; } = "Test";
        public string ContentRootPath { get; set; } = Path.GetTempPath();
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
