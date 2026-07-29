using PaginaToros.Shared.Models;

namespace PaginaToros.Server.Services
{
    public enum AccessMailTemplate
    {
        Registration,
        Reset
    }

    public interface IAccessMailService
    {
        Task<(bool Success, string? ErrorMessage)> SendAccessMailAsync(User model, string password, AccessMailTemplate template);
    }
}
