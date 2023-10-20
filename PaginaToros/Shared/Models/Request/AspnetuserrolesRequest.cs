namespace PaginaToros.Shared.Models.Request
{
    public class AspnetuserrolesRequest
    {

        public string UserId { get; set; }
        public string RoleId { get; set; }

        public virtual AspNetRole Role { get; set; }
        public virtual AspNetUser User { get; set; }

    }
}
