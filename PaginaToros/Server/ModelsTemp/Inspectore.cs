using System;
using System.Collections.Generic;

namespace PaginaToros.Server.ModelsTemp;

public partial class Inspectore
{
    public int Id { get; set; }

    public int? Codigo { get; set; }

    public string? Nombre { get; set; }

    public string? Direccion { get; set; }

    public string? Localidad { get; set; }

    public int? CodPostal { get; set; }

    public int? CodProvincia { get; set; }

    public string? Telefono { get; set; }

    public string? Mail { get; set; }

    public string? Provincia { get; set; }
}
