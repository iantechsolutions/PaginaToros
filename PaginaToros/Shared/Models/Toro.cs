using System;
using System.Collections.Generic;

namespace PaginaToros.Shared.Models;

public partial class Toro
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public int? Calidad { get; set; }

    public int? IdEst { get; set; }

    public string? NombreEst { get; set; }
}
