using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Utilidades;
using PaginaToros.Shared.Models;

namespace PaginaToros.Tests;

public class AccountEmailSyncHelperTests
{
    [Fact]
    public async Task ResolveAndSyncAsync_LeavesEmailUntouched_WhenEmailMatches()
    {
        using var scope = CreateScope();
        SeedDomainUsers(scope.DomainContext);

        var result = await AccountEmailSyncHelper.ResolveAndSyncAsync(
            scope.DomainContext,
            scope.UserManager,
            1,
            "juan.perez@users.com");

        Assert.True(result.Success);
        Assert.False(result.EmailChanged);
        Assert.Equal("juan.perez@users.com", result.Email);

        var domainUser = await scope.DomainContext.User.FirstAsync(x => x.Id == 1);
        Assert.Equal("juan.perez@users.com", domainUser.Email);

        var identityUser = scope.FindIdentityUser("ident-1");
        Assert.NotNull(identityUser);
        Assert.Equal("juan.perez@users.com", identityUser!.Email);
    }

    [Fact]
    public async Task ResolveAndSyncAsync_UpdatesEmail_WhenNewEmailIsAvailable()
    {
        using var scope = CreateScope();
        SeedDomainUsers(scope.DomainContext);

        var result = await AccountEmailSyncHelper.ResolveAndSyncAsync(
            scope.DomainContext,
            scope.UserManager,
            1,
            "nuevo@mail.com");

        Assert.True(result.Success);
        Assert.True(result.EmailChanged);
        Assert.Equal("nuevo@mail.com", result.Email);

        var domainUser = await scope.DomainContext.User.FirstAsync(x => x.Id == 1);
        Assert.Equal("nuevo@mail.com", domainUser.Email);
        Assert.Equal("ident-1", domainUser.IdentityUserId);

        var identityUser = scope.FindIdentityUser("ident-1");
        Assert.NotNull(identityUser);
        Assert.Equal("nuevo@mail.com", identityUser!.Email);
        Assert.Equal("nuevo@mail.com", identityUser.UserName);
        Assert.Equal("NUEVO@MAIL.COM", identityUser.NormalizedEmail);
        Assert.Equal("NUEVO@MAIL.COM", identityUser.NormalizedUserName);
        Assert.True(identityUser.EmailConfirmed);
    }

    [Fact]
    public async Task ResolveAndSyncAsync_Fails_WhenNewEmailIsAlreadyInUse()
    {
        using var scope = CreateScope();
        SeedDomainUsers(scope.DomainContext);

        var result = await AccountEmailSyncHelper.ResolveAndSyncAsync(
            scope.DomainContext,
            scope.UserManager,
            1,
            "maria.lopez@users.com");

        Assert.False(result.Success);
        Assert.Equal("El email ya está en uso por otro usuario.", result.ErrorMessage);

        var domainUser = await scope.DomainContext.User.FirstAsync(x => x.Id == 1);
        Assert.Equal("juan.perez@users.com", domainUser.Email);

        var identityUser = scope.FindIdentityUser("ident-1");
        Assert.NotNull(identityUser);
        Assert.Equal("juan.perez@users.com", identityUser!.Email);
    }

    [Fact]
    public async Task ResolveAndSyncAsync_Fails_WhenUserDoesNotExist()
    {
        using var scope = CreateScope();
        SeedDomainUsers(scope.DomainContext);

        var result = await AccountEmailSyncHelper.ResolveAndSyncAsync(
            scope.DomainContext,
            scope.UserManager,
            999,
            "otro@mail.com");

        Assert.False(result.Success);
        Assert.Equal("Usuario no encontrado.", result.ErrorMessage);
    }

    private static TestScope CreateScope()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var domainContext = new ApplicationDbContext(options);
        var store = new InMemoryIdentityStore(new[]
        {
            new IdentityUser
            {
                Id = "ident-1",
                UserName = "juan.perez@users.com",
                Email = "juan.perez@users.com",
                NormalizedUserName = "JUAN.PEREZ@USERS.COM",
                NormalizedEmail = "JUAN.PEREZ@USERS.COM",
                EmailConfirmed = true
            },
            new IdentityUser
            {
                Id = "ident-2",
                UserName = "maria.lopez@users.com",
                Email = "maria.lopez@users.com",
                NormalizedUserName = "MARIA.LOPEZ@USERS.COM",
                NormalizedEmail = "MARIA.LOPEZ@USERS.COM",
                EmailConfirmed = true
            }
        });

        var userManager = CreateUserManager(store);

        return new TestScope(domainContext, userManager, store);
    }

    private static UserManager<IdentityUser> CreateUserManager(InMemoryIdentityStore store)
    {
        return new UserManager<IdentityUser>(
            store,
            Microsoft.Extensions.Options.Options.Create(new IdentityOptions()),
            new PasswordHasher<IdentityUser>(),
            Array.Empty<IUserValidator<IdentityUser>>(),
            Array.Empty<IPasswordValidator<IdentityUser>>(),
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            null!,
            null!);
    }

    private static void SeedDomainUsers(ApplicationDbContext context)
    {
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
                IdentityUserId = "ident-1"
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
                IdentityUserId = "ident-2"
            });

        context.SaveChanges();
        context.ChangeTracker.Clear();
    }

    private sealed class TestScope : IDisposable
    {
        public TestScope(ApplicationDbContext domainContext, UserManager<IdentityUser> userManager, InMemoryIdentityStore identityStore)
        {
            DomainContext = domainContext;
            UserManager = userManager;
            IdentityStore = identityStore;
        }

        public ApplicationDbContext DomainContext { get; }
        public UserManager<IdentityUser> UserManager { get; }
        public InMemoryIdentityStore IdentityStore { get; }

        public IdentityUser? FindIdentityUser(string id) => IdentityStore.FindById(id);

        public void Dispose()
        {
            DomainContext.Dispose();
            UserManager.Dispose();
        }
    }

    private sealed class InMemoryIdentityStore : IUserEmailStore<IdentityUser>
    {
        private readonly Dictionary<string, IdentityUser> _users;

        public InMemoryIdentityStore(IEnumerable<IdentityUser> users)
        {
            _users = users.ToDictionary(x => x.Id, x => x);
        }

        public IdentityUser? FindById(string id)
            => _users.TryGetValue(id, out var user) ? user : null;

        public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            _users[user.Id] = user;
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            _users.Remove(user.Id);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
            => Task.FromResult(FindById(userId));

        public Task<IdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
            => Task.FromResult(_users.Values.FirstOrDefault(x => string.Equals(x.NormalizedUserName, normalizedUserName, StringComparison.OrdinalIgnoreCase)));

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
        {
            _users[user.Id] = user;
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<string?> GetEmailAsync(IdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult<string?>(user.Email);

        public Task SetEmailAsync(IdentityUser user, string? email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.EmailConfirmed);

        public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task<string?> GetNormalizedEmailAsync(IdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult<string?>(user.NormalizedEmail);

        public Task SetNormalizedEmailAsync(IdentityUser user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task<IdentityUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
            => Task.FromResult(_users.Values.FirstOrDefault(x =>
                string.Equals(x.NormalizedEmail, normalizedEmail, StringComparison.OrdinalIgnoreCase)));

        public void Dispose()
        {
        }
    }
}
