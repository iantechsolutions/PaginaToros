using Microsoft.AspNetCore.Identity;

namespace PaginaToros.Server.Services
{
    public class IdentityPasswordService : IIdentityPasswordService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public IdentityPasswordService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> ResetPasswordAsync(IdentityUser user, string password)
        {
            foreach (var validator in _userManager.PasswordValidators)
            {
                var validation = await validator.ValidateAsync(_userManager, user, password);
                if (!validation.Succeeded)
                {
                    return validation;
                }
            }

            if (await _userManager.HasPasswordAsync(user))
            {
                var remove = await _userManager.RemovePasswordAsync(user);
                if (!remove.Succeeded)
                {
                    return remove;
                }
            }

            return await _userManager.AddPasswordAsync(user, password);
        }
    }
}
