using System.Security.Claims;

namespace PaginaToros.Server.Services
{
    public interface IUserSocioContextService
    {
        Task<UserSocioAccessContext> ResolveAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
        Task<bool> CanAccessSocioAsync(ClaimsPrincipal principal, int socioId, CancellationToken cancellationToken = default);
    }
}
