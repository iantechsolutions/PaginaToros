using System;
using System.Collections.Generic;

namespace PaginaToros.Server.ModelsTemp;

public partial class Zona
{
    public int Id { get; set; }

    public int? CodigoZona { get; set; }

    public string? Meses { get; set; }

    public string? CodigoInspector { get; set; }

    public string? Provincia { get; set; }

    public string? Localidad { get; set; }

    public string? NombreInspector { get; set; }
}
