﻿using System;
using System.Collections.Generic;

namespace PaginaToros.Server.ModelsTempp;

public partial class AspNetUserToken
{
    public string UserId { get; set; } = null!;

    public string LoginProvider { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Value { get; set; }
}