using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Controllers;
using PaginaToros.Shared.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace PaginaToros.Tests;

public class AccountControllerSearchTests
{
    [Theory]
    [InlineData("juan", 1)]
    [InlineData("perez", 1)]
    [InlineData("juan perez", 1)]
    [InlineData("socio sur", 2)]
    [InlineData("sur@socio.com", 2)]
    public async Task SearchUsersPaged_FindsUsers_ByUserAndSocioFields(string searchText, int expectedUserId)
    {
        using var scope = CreateContext();
        Seed(scope.DomainContext);

        var controller = CreateController(scope.DomainContext);

        var result = await controller.SearchUsersPaged(0, 10, searchText, null);
        var wrapper = DeserializeResponse(result);
        var users = JsonSerializer.Deserialize<List<User>>(wrapper.EntitiesPricipal) ?? new List<User>();

        Assert.Contains(users, user => user.Id == expectedUserId);
    }

    [Fact]
    public async Task SearchUsersPaged_AppliesPaging_AndReturnsTotalCount()
    {
        using var scope = CreateContext();
        Seed(scope.DomainContext);

        var controller = CreateController(scope.DomainContext);

        var result = await controller.SearchUsersPaged(0, 1, null, null);
        var wrapper = DeserializeResponse(result);
        var users = JsonSerializer.Deserialize<List<User>>(wrapper.EntitiesPricipal) ?? new List<User>();

        Assert.Equal(3, wrapper.AllRegisters);
        Assert.Single(users);
    }

    [Fact]
    public async Task SearchUsersPaged_ReturnsNotFoundWhenNoMatch()
    {
        using var scope = CreateContext();
        Seed(scope.DomainContext);

        var controller = CreateController(scope.DomainContext);

        var result = await controller.SearchUsersPaged(0, 10, "no-existe", null);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(Utilities.MSGNODATA, notFound.Value);
    }

    private static AccountController CreateController(hereford_prContext domainContext)
    {
        var identityOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var identityContext = new ApplicationDbContext(identityOptions);

        return new AccountController(
            identityContext,
            CreateUserManager(),
            null!,
            new ConfigurationBuilder().Build(),
            domainContext);
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
                Id INTEGER PRIMARY KEY,
                SCOD TEXT NOT NULL,
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

        context.Database.ExecuteSqlRaw(@"
            CREATE TABLE User (
                Id INTEGER PRIMARY KEY,
                Names TEXT NOT NULL,
                LastNames TEXT NOT NULL,
                Dni TEXT NOT NULL,
                Phone TEXT NOT NULL,
                Email TEXT NOT NULL,
                Rol TEXT NOT NULL,
                Status TEXT NOT NULL,
                Created TEXT NOT NULL,
                IdentityUserId TEXT NULL,
                SocioId INTEGER NULL,
                FOREIGN KEY (SocioId) REFERENCES SOCIOS(Id)
            );");

        context.Database.ExecuteSqlRaw(@"
            CREATE TABLE user_socios (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                user_id INTEGER NOT NULL,
                socio_id INTEGER NOT NULL,
                created_at TEXT NOT NULL
            );");

        return new TestContextScope(connection, context);
    }

    private static void Seed(hereford_prContext context)
    {
        context.Socios.AddRange(
            new Socio
            {
                Id = 1,
                Scod = "S1",
                Nombre = "Socio Norte",
                Mail = "norte@socio.com"
            },
            new Socio
            {
                Id = 2,
                Scod = "S2",
                Nombre = "Socio Sur",
                Mail = "sur@socio.com"
            });

        context.User.AddRange(
            new User
            {
                Id = 1,
                Names = "Juan",
                LastNames = "Perez",
                Dni = "1",
                Phone = "111",
                Email = "juan.perez@users.com",
                Rol = "SOCIO",
                Status = "ACTIVO",
                Created = DateTime.UtcNow,
                SocioId = 1
            },
            new User
            {
                Id = 2,
                Names = "Maria",
                LastNames = "Lopez",
                Dni = "2",
                Phone = "222",
                Email = "maria.lopez@users.com",
                Rol = "SOCIO",
                Status = "ACTIVO",
                Created = DateTime.UtcNow,
                SocioId = 1
            },
            new User
            {
                Id = 3,
                Names = "Pedro",
                LastNames = "Gomez",
                Dni = "3",
                Phone = "333",
                Email = "pedro.gomez@users.com",
                Rol = "SOCIO",
                Status = "INACTIVO",
                Created = DateTime.UtcNow,
                SocioId = null
            });

        context.UserSocios.AddRange(
            new UserSocio
            {
                Id = 1,
                UserId = 1,
                SocioId = 1,
                CreatedAt = DateTime.UtcNow
            },
            new UserSocio
            {
                Id = 2,
                UserId = 2,
                SocioId = 2,
                CreatedAt = DateTime.UtcNow
            },
            new UserSocio
            {
                Id = 3,
                UserId = 3,
                SocioId = 2,
                CreatedAt = DateTime.UtcNow
            });

        context.SaveChanges();
        context.ChangeTracker.Clear();
    }

    private static ResponseForList DeserializeResponse(ActionResult result)
    {
        var ok = Assert.IsType<OkObjectResult>(result);
        var payload = Assert.IsType<string>(ok.Value);
        var wrapper = JsonSerializer.Deserialize<ResponseForList>(payload);

        Assert.NotNull(wrapper);
        return wrapper!;
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

        public TestContextScope(SqliteConnection connection, hereford_prContext domainContext)
        {
            _connection = connection;
            DomainContext = domainContext;
        }

        public hereford_prContext DomainContext { get; }

        public void Dispose()
        {
            DomainContext.Dispose();
            _connection.Dispose();
        }
    }
}
