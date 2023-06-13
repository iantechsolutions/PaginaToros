using System;
using System.Collections.Generic;

namespace PaginaToros.Shared.Models;

public partial class Transaccione
{
    public int Id { get; set; }

    public string? NombreVendedor { get; set; }

    public string? NombreComprador { get; set; }

    public DateTime? Fecha { get; set; }

    public int? Total { get; set; }

    public string? Toros { get; set; }
}
