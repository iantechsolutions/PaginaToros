using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using PaginaToros.Server.Context;
using PaginaToros.Server.Controllers;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Repositorio.Implementacion;
using PaginaToros.Server.Services;
using PaginaToros.Server.Utilidades;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Security.Claims;

namespace PaginaToros.Tests;

public class SocioControllerTests
{
    [Fact]
    public async Task RepairMissingScod_UsesCodpos2_WhenItDoesNotConflict()
    {
        using var scope = CreateContext();
        SeedForRepair(scope.Context);

        var controller = CreateController(scope.Context);

        var result = await controller.RepairMissingScod();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<Respuesta<SocioRepairResult>>(okResult.Value);

        Assert.Equal(1, response.Exito);
        Assert.Equal(3, response.List.TotalSocios);
        Assert.Equal(2, response.List.SociosConScodFaltante);
        Assert.Equal(2, response.List.SociosReparados);
        Assert.Equal(1, response.List.SociosReparadosConCodpos2);
        Assert.Equal(1, response.List.SociosReparadosConScodGenerado);
        Assert.Equal(0, response.List.SociosSinResolver);

        var socioConCodpos2 = await scope.Context.Socios.FirstAsync(x => x.Id == 2);
        var socioGenerado = await scope.Context.Socios.FirstAsync(x => x.Id == 3);

        Assert.Equal("0200", socioConCodpos2.Scod);
        Assert.Equal("1001", socioGenerado.Scod);
    }

    [Fact]
    public async Task RepairMissingScod_DoesNotChangeExistingScodValues()
    {
        using var scope = CreateContext();
        SeedForRepair(scope.Context);

        var controller = CreateController(scope.Context);

        await controller.RepairMissingScod();

        var validSocio = await scope.Context.Socios.FirstAsync(x => x.Id == 1);

        Assert.Equal("1000", validSocio.Scod);
    }

    private static SocioController CreateController(hereford_prContext context)
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = mapperConfig.CreateMapper();
        var repo = new SocioRepositorio(context);
        var identityContext = CreateIdentityContext();
        var accessService = new TestUserSocioContextService(new UserSocioAccessContext
        {
            IsAuthenticated = true,
            IsSocioUser = false,
            IsPrivilegedUser = true,
            AllowedSocioIds = Array.Empty<int>()
        });

        var controller = new SocioController(
            repo,
            mapper,
            context,
            identityContext,
            CreateUserManager(),
            accessService)
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
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<hereford_prContext>()
            .UseSqlite(connection)
            .Options;

        var context = new hereford_prContext(options);
        context.Database.ExecuteSqlRaw(@"
            CREATE TABLE SOCIOS (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                SCOD TEXT NOT NULL UNIQUE,
                CATEGO TEXT NULL,
                CUENTA TEXT NULL,
                PRENOM TEXT NULL,
                NOMBRE TEXT NULL,
                POSNOM TEXT NULL,
                DIRECC1 TEXT NULL,
                LOCALI1 TEXT NULL,
                CODPOS1 TEXT NULL,
                CODPRO1 TEXT NULL,
                ORDALF TEXT NULL,
                TELEFO1 TEXT NULL,
                mail TEXT NULL,
                CRIADOR TEXT NULL,
                DIRECC2 TEXT NULL,
                LOCALI2 TEXT NULL,
                CODPOS2 TEXT NULL,
                CODPRO2 TEXT NULL,
                TELEFO2 TEXT NULL,
                FECING TEXT NULL,
                VTOSUS TEXT NULL,
                ENVIO TEXT NULL,
                FCH_USU TEXT NULL,
                COD_USU INTEGER NULL,
                PLACOD TEXT NULL,
                mailreg TEXT NULL,
                diaregautog TEXT NULL
            );");

        return new TestContextScope(connection, context);
    }

    private static ApplicationDbContext CreateIdentityContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static UserManager<IdentityUser> CreateUserManager()
    {
        return new UserManager<IdentityUser>(
            new NoopUserStore(),
            Microsoft.Extensions.Options.Options.Create(new IdentityOptions()),
            new PasswordHasher<IdentityUser>(),
            Array.Empty<IUserValidator<IdentityUser>>(),
            Array.Empty<IPasswordValidator<IdentityUser>>(),
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            null!,
            null!);
    }

    private static void SeedForRepair(hereford_prContext context)
    {
        context.Socios.AddRange(
            new Socio
            {
                Id = 1,
                Scod = "1000",
                Codpos2 = "0100",
                Nombre = "Socio Vigente"
            },
            new Socio
            {
                Id = 2,
                Scod = string.Empty,
                Codpos2 = "0200",
                Nombre = "Socio A"
            },
            new Socio
            {
                Id = 3,
                Scod = "   ",
                Codpos2 = "1000",
                Nombre = "Socio B"
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

    private sealed class NoopUserStore : IUserStore<IdentityUser>
    {
        public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(IdentityResult.Success);

        public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(IdentityResult.Success);

        public Task<IdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
            => Task.FromResult<IdentityUser?>(null);

        public Task<IdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
            => Task.FromResult<IdentityUser?>(null);

        public Task<string?> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult<string?>(user.NormalizedUserName);

        public Task<string?> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult<string?>(user.Id);

        public Task<string?> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult<string?>(user.UserName);

        public Task SetNormalizedUserNameAsync(IdentityUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(IdentityUser user, string? userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(IdentityResult.Success);

        public void Dispose()
        {
        }
    }

    private sealed class TestContextScope : IDisposable
    {
        private readonly SqliteConnection _connection;

        public TestContextScope(SqliteConnection connection, hereford_prContext context)
        {
            _connection = connection;
            Context = context;
        }

        public hereford_prContext Context { get; }

        public void Dispose()
        {
            Context.Dispose();
            _connection.Dispose();
        }
    }
}
