using System;
using System.Collections.Generic;

namespace PaginaToros.Server.ModelsTempp;

public partial class AspNetUserRole
{
    public string UserId { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    public virtual AspNetRole Role { get; set; } = null!;
}
