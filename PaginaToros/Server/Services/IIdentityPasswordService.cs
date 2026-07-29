using Microsoft.AspNetCore.Identity;

namespace PaginaToros.Server.Services
{
    public interface IIdentityPasswordService
    {
        Task<IdentityResult> ResetPasswordAsync(IdentityUser user, string password);
    }
}
