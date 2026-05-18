using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Controllers;
using PaginaToros.Server.Repositorio.Implementacion;
using PaginaToros.Server.Services;
using PaginaToros.Server.Utilidades;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Tests;

public class Desepla1ControllerTests
{
    [Fact]
    public async Task RepairAmbiguousPlantels_AutoCorrectsUniqueHistoricalMatch_AndKeepsAmbiguousPending()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var controller = CreateController(scope.Context);

        var result = await controller.RepairAmbiguousPlantels();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<Respuesta<Desepla1PlantelAmbiguityRepairResult>>(okResult.Value);

        Assert.Equal(1, response.Exito);
        Assert.NotNull(response.List);
        Assert.Equal(3, response.List!.TotalDeclaraciones);
        Assert.Equal(2, response.List.DeclaracionesConPlantel);
        Assert.Equal(1, response.List.DeclaracionesCorregidas);
        Assert.Equal(1, response.List.DeclaracionesPendientes);
        Assert.Single(response.List.Pendientes);

        var corregida = await scope.Context.Desepla1s.FirstAsync(x => x.Id == 1);
        var pendiente = await scope.Context.Desepla1s.FirstAsync(x => x.Id == 2);

        Assert.Equal("9R976", corregida.Nroplan);
        Assert.Equal("R976", pendiente.Nroplan);
        Assert.Equal(2, response.List.Pendientes[0].Id);
    }

    private static Desepla1Controller CreateController(hereford_prContext context)
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = mapperConfig.CreateMapper();

        var controller = new Desepla1Controller(
            new Desepla1Repositorio(context),
            mapper,
            new TestUserSocioContextService(),
            context)
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

        return controller;
    }

    private static TestContextScope CreateContext()
    {
        var options = new DbContextOptionsBuilder<hereford_prContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString("N"))
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
                Placod = "8R976",
                Anioex = "2018",
                Fecing = "2018/01/01",
                Nrocri = "004476",
                Estado = "A"
            },
            new Plantel
            {
                Id = 2,
                Placod = "9R976",
                Anioex = "2019",
                Fecing = "2019/01/01",
                Nrocri = "004476",
                Estado = "A"
            });

        context.Desepla1s.AddRange(
            new Desepla1
            {
                Id = 1,
                Nrodec = "10001",
                Nroplan = "R976",
                Fecdecl = new DateTime(2019, 4, 12),
                Nrocri = "004476"
            },
            new Desepla1
            {
                Id = 2,
                Nrodec = "10002",
                Nroplan = "R976",
                Fecdecl = new DateTime(2017, 4, 12),
                Nrocri = "004476"
            },
            new Desepla1
            {
                Id = 3,
                Nrodec = "10003",
                Nroplan = null,
                Fecdecl = new DateTime(2020, 1, 1),
                Nrocri = "004476"
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
                IsPrivilegedUser = true,
                IsSocioUser = false,
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
}
