using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Shared.Models;

namespace PaginaToros.Server.Utilidades
{
    public static class AccountEmailSyncHelper
    {
        public static async Task<EmailSyncResult> ResolveAndSyncAsync(
            ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            int userId,
            string requestedEmail)
        {
            var newEmail = (requestedEmail ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(newEmail))
            {
                return EmailSyncResult.Fail("Email inválido.");
            }

            var domainUser = await db.User.FirstOrDefaultAsync(x => x.Id == userId);
            if (domainUser == null)
            {
                return EmailSyncResult.Fail("Usuario no encontrado.");
            }

            var identityUser = await FindIdentityUserAsync(userManager, domainUser.IdentityUserId)
                ?? await FindIdentityUserAsync(userManager, domainUser.Email)
                ?? await FindIdentityUserAsync(userManager, newEmail);

            if (identityUser == null)
            {
                return EmailSyncResult.Fail("Usuario de acceso no encontrado (AspNetUsers).");
            }

            var currentEmail = (domainUser.Email ?? string.Empty).Trim();
            var emailChanged = !string.Equals(currentEmail, newEmail, StringComparison.OrdinalIgnoreCase);

            if (emailChanged)
            {
                var domainConflict = await db.User.AnyAsync(x =>
                    x.Id != domainUser.Id &&
                    x.Email == newEmail);

                if (domainConflict)
                {
                    return EmailSyncResult.Fail("El email ya está en uso por otro usuario.");
                }

                var identityConflict = await userManager.FindByEmailAsync(newEmail);
                if (identityConflict != null && identityConflict.Id != identityUser.Id)
                {
                    return EmailSyncResult.Fail("El email ya está en uso por otro usuario.");
                }

                var setEmail = await userManager.SetEmailAsync(identityUser, newEmail);
                if (!setEmail.Succeeded)
                {
                    return EmailSyncResult.Fail(ToErrorMessage(setEmail.Errors));
                }

                var setUserName = await userManager.SetUserNameAsync(identityUser, newEmail);
                if (!setUserName.Succeeded)
                {
                    return EmailSyncResult.Fail(ToErrorMessage(setUserName.Errors));
                }

                identityUser.NormalizedEmail = newEmail.ToUpperInvariant();
                identityUser.NormalizedUserName = newEmail.ToUpperInvariant();
                identityUser.EmailConfirmed = true;

                var update = await userManager.UpdateAsync(identityUser);
                if (!update.Succeeded)
                {
                    return EmailSyncResult.Fail(ToErrorMessage(update.Errors));
                }

                domainUser.Email = newEmail;
                if (string.IsNullOrWhiteSpace(domainUser.IdentityUserId))
                {
                    domainUser.IdentityUserId = identityUser.Id;
                }

                await db.SaveChangesAsync();
            }

            return EmailSyncResult.Ok(domainUser, identityUser, newEmail, emailChanged);
        }

        private static async Task<IdentityUser?> FindIdentityUserAsync(UserManager<IdentityUser> userManager, string? emailOrId)
        {
            if (string.IsNullOrWhiteSpace(emailOrId))
            {
                return null;
            }

            return await userManager.FindByIdAsync(emailOrId)
                ?? await userManager.FindByEmailAsync(emailOrId)
                ?? await userManager.FindByNameAsync(emailOrId);
        }

        private static string ToErrorMessage(IEnumerable<IdentityError> errors)
            => string.Join(Environment.NewLine, errors.Where(x => !string.IsNullOrWhiteSpace(x.Description)).Select(x => x.Description));

        public sealed record EmailSyncResult(bool Success, string? ErrorMessage, User? DomainUser, IdentityUser? IdentityUser, string Email, bool EmailChanged)
        {
            public static EmailSyncResult Ok(User domainUser, IdentityUser identityUser, string email, bool emailChanged)
                => new(true, null, domainUser, identityUser, email, emailChanged);

            public static EmailSyncResult Fail(string errorMessage)
                => new(false, errorMessage, null, null, string.Empty, false);
        }
    }
}
