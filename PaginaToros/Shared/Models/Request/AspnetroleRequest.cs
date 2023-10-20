using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaginaToros.Shared.Models.Request
{
    public partial class AspnetuserRequest
    {


        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public virtual ICollection<AspNetUserClaim> Aspnetuserclaims { get; set; }
        public virtual ICollection<AspNetUserLogin> Aspnetuserlogins { get; set; }
        public virtual ICollection<AspNetUserRole> Aspnetuserroles { get; set; }
        public virtual ICollection<AspNetUserToken> Aspnetusertokens { get; set; }
    }
}
