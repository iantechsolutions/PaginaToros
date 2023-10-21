using System;
using System.Collections.Generic;

namespace PaginaToros.Shared.Models;

public partial class Inspectore
{
    public int Id { get; set; }

    public string? Codigo { get; set; }

    public string? Nombre { get; set; }

    public string? Direccion { get; set; }

    public string? Localidad { get; set; }

    public string? CodPostal { get; set; }

    public string? CodProvincia { get; set; }

    public string? Telefono { get; set; }

    public string? Mail { get; set; }

    public string? Provincia { get; set; }
}
