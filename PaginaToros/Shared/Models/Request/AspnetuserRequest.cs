namespace PaginaToros.Shared.Models.Request
{
    public partial class AspnetroleRequest
    {


        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }

        public virtual ICollection<AspNetRoleClaim> Aspnetroleclaims { get; set; }
        public virtual ICollection<AspNetUserRole> Aspnetuserroles { get; set; }
    }
}
