using System.Collections.Generic;

namespace PaginaToros.Shared.Models
{
    public class UserSocioOptionDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }

    public class UserContextDTO
    {
        public int UserId { get; set; }
        public bool IsSocioUser { get; set; }
        public int? ActiveSocioId { get; set; }
        public string? ActiveSocioDisplayName { get; set; }
        public List<UserSocioOptionDTO> AllowedSocios { get; set; } = new();
    }

    public class ChangeActiveSocioRequest
    {
        public int SocioId { get; set; }
    }
}
