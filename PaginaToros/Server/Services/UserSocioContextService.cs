using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using PaginaToros.Server.Context;
using PaginaToros.Shared.Models;
using System.Security.Claims;

namespace PaginaToros.Server.Services
{
    public class UserSocioContextService : IUserSocioContextService
    {
        private readonly hereford_prContext _db;

        public UserSocioContextService(hereford_prContext db)
        {
            _db = db;
        }

        public async Task<UserSocioAccessContext> ResolveAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
        {
            if (principal?.Identity?.IsAuthenticated != true)
            {
                return new UserSocioAccessContext();
            }

            var usuarioIdClaim = principal.Claims.FirstOrDefault(x => x.Type == "UsuarioId")?.Value;
            var identityUserId = principal.Claims.FirstOrDefault(x => x.Type == "userId")?.Value
                ?? principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = principal.Claims.FirstOrDefault(x => x.Type == "userNM")?.Value
                ?? principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.UniqueName)?.Value
                ?? principal.FindFirstValue(ClaimTypes.Email);

            User? currentUser = null;

            if (int.TryParse(usuarioIdClaim, out var usuarioId) && usuarioId > 0)
            {
                currentUser = await _db.User.FirstOrDefaultAsync(
                    x => x.Id == usuarioId,
                    cancellationToken);
            }

            if (currentUser is null && !string.IsNullOrWhiteSpace(identityUserId))
            {
                currentUser = await _db.User.FirstOrDefaultAsync(
                    x => x.IdentityUserId == identityUserId,
                    cancellationToken);
            }

            if (currentUser is null && !string.IsNullOrWhiteSpace(email))
            {
                currentUser = await _db.User.FirstOrDefaultAsync(
                    x => x.Email == email,
                    cancellationToken);
            }

            if (currentUser is null)
            {
                return new UserSocioAccessContext
                {
                    IsAuthenticated = true
                };
            }

            if (string.IsNullOrWhiteSpace(currentUser.IdentityUserId) && !string.IsNullOrWhiteSpace(identityUserId))
            {
                currentUser.IdentityUserId = identityUserId;
                await _db.SaveChangesAsync(cancellationToken);
            }

            var allowedSocioIds = await _db.UserSocios
                .AsNoTracking()
                .Where(x => x.UserId == currentUser.Id)
                .Select(x => x.SocioId)
                .Distinct()
                .ToListAsync(cancellationToken);

            if (allowedSocioIds.Count == 0 && currentUser.SocioId.HasValue && currentUser.SocioId.Value > 0)
            {
                allowedSocioIds.Add(currentUser.SocioId.Value);
            }

            allowedSocioIds = allowedSocioIds
                .Where(x => x > 0)
                .Distinct()
                .ToList();

            var isSocioUser = IsSocioRole(currentUser.Rol);
            var isPrivilegedUser = IsPrivilegedRole(currentUser.Rol);

            if (isSocioUser && allowedSocioIds.Count > 0 &&
                (!currentUser.SocioId.HasValue || !allowedSocioIds.Contains(currentUser.SocioId.Value)))
            {
                currentUser.SocioId = allowedSocioIds[0];
                await _db.SaveChangesAsync(cancellationToken);
            }

            string? activeSocioCode = null;
            if (currentUser.SocioId.HasValue && currentUser.SocioId.Value > 0)
            {
                activeSocioCode = await _db.Socios
                    .AsNoTracking()
                    .Where(x => x.Id == currentUser.SocioId.Value)
                    .Select(x => x.Scod)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            return new UserSocioAccessContext
            {
                IsAuthenticated = true,
                CurrentUser = currentUser,
                IsSocioUser = isSocioUser,
                IsPrivilegedUser = isPrivilegedUser,
                AllowedSocioIds = allowedSocioIds,
                ActiveSocioId = currentUser.SocioId,
                ActiveSocioCode = activeSocioCode
            };
        }

        public async Task<bool> CanAccessSocioAsync(ClaimsPrincipal principal, int socioId, CancellationToken cancellationToken = default)
        {
            if (socioId <= 0)
            {
                return false;
            }

            var context = await ResolveAsync(principal, cancellationToken);
            if (!context.IsAuthenticated || context.CurrentUser is null)
            {
                return false;
            }

            if (context.IsPrivilegedUser || !context.IsSocioUser)
            {
                return true;
            }

            return context.AllowedSocioIds.Contains(socioId);
        }

        private static bool IsSocioRole(string? role)
            => string.Equals(role, "SOCIO", StringComparison.OrdinalIgnoreCase)
               || string.Equals(role, "Socio", StringComparison.OrdinalIgnoreCase);

        private static bool IsPrivilegedRole(string? role)
            => string.Equals(role, "ADMINISTRADOR", StringComparison.OrdinalIgnoreCase)
               || string.Equals(role, "USUARIOMAESTRO", StringComparison.OrdinalIgnoreCase);
    }
}
