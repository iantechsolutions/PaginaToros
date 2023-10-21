using System;
using System.Collections.Generic;

namespace PaginaToros.Shared.Models;

public partial class Zona
{
    public int Id { get; set; }

    public string? CodigoZona { get; set; }

    public string? Meses { get; set; }

    public string? CodigoInspector { get; set; }

    public string? Provincia { get; set; }

    public string? Localidad { get; set; }

    public string? NombreInspector { get; set; }
}
