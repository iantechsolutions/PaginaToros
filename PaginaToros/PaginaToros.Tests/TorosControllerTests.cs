using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Controllers;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Implementacion;
using PaginaToros.Server.Services;
using PaginaToros.Server.Utilidades;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Security.Claims;

namespace PaginaToros.Tests;

public class TorosControllerTests
{
    [Fact]
    public async Task Editar_IgnoresStaleSocioNavigation_AndKeepsNewCriador()
    {
        using var context = CreateContext();
        Seed(context);

        var controller = CreateController(context);

        var request = new TorosuniDTO
        {
            Id = 1,
            Criador = "S20",
            Socio = new Socio
            {
                Id = 10,
                Scod = "S10",
                Nombre = "Socio Norte",
                Codpos2 = "010"
            },
            EstablecimientoId = 200,
            CodEstado = "1",
            NomDad = "Bravo",
            Hba = "HBA-111",
            Tatpart = "TAT-111",
            NrTsan = "ADN-111",
            Sbcod = 1001,
            Nacido = new DateTime(2024, 1, 1),
            Fechasba = "01/02/2026",
            Variedad = "H"
        };

        var result = await controller.Editar(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<Respuesta<TorosuniDTO>>(okResult.Value);
        Assert.True(response.Exito == 1, response.Mensaje);

        var persisted = await context.Torosunis
            .Include(t => t.Socio)
            .Include(t => t.Establecimiento)
            .FirstAsync(t => t.Id == 1);

        Assert.Equal("S20", persisted.Criador);
        Assert.NotNull(persisted.Socio);
        Assert.Equal(20, persisted.Socio!.Id);
        Assert.Equal("S20", persisted.Socio.Scod);
        Assert.Equal(200, persisted.EstablecimientoId);
    }

    private static TorosController CreateController(hereford_prContext context)
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = mapperConfig.CreateMapper();
        var repo = new TorosRepositorio(context);
        var accessService = new TestUserSocioContextService(new UserSocioAccessContext
        {
            IsAuthenticated = true,
            IsSocioUser = false,
            IsPrivilegedUser = true,
            AllowedSocioIds = Array.Empty<int>()
        });

        var controller = new TorosController(repo, mapper, accessService, context)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity())
                }
            }
        };

        return controller;
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
        context.Torosunis.Add(
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
            });

        context.SaveChanges();
        context.ChangeTracker.Clear();
    }

    private sealed class TestUserSocioContextService : IUserSocioContextService
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
}
