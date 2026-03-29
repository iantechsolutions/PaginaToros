using System;

namespace PaginaToros.Shared.Models
{
    public class UserSocio
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SocioId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
