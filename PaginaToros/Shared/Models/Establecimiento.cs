using System;
using System.Collections.Generic;

namespace PaginaToros.Shared.Models;

public partial class Establecimiento
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public int? ConsultasRealizadas { get; set; }

    public DateTime? Fecha { get; set; }
}
