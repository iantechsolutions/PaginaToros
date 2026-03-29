using PaginaToros.Shared.Models;

namespace PaginaToros.Server.Services
{
    public class UserSocioAccessContext
    {
        public bool IsAuthenticated { get; init; }
        public User? CurrentUser { get; init; }
        public bool IsSocioUser { get; init; }
        public bool IsPrivilegedUser { get; init; }
        public IReadOnlyList<int> AllowedSocioIds { get; init; } = Array.Empty<int>();
        public int? ActiveSocioId { get; init; }
        public string? ActiveSocioCode { get; init; }
    }
}
