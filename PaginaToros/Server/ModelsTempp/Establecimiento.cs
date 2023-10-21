using System;
using System.Collections.Generic;

namespace PaginaToros.Server.ModelsTempp;

public partial class Establecimiento
{
    public int Id { get; set; }

    public int? Codigo { get; set; }

    public int? CodigoSocio { get; set; }

    public bool? Activo { get; set; }

    public string? Nombre { get; set; }

    public string? Encargado { get; set; }

    public string? Domicilio { get; set; }

    public string? Telefono { get; set; }

    public string? Localidad { get; set; }

    public string? CodPostal { get; set; }

    public string? CodProvincia { get; set; }

    public string? Informacion { get; set; }

    public int? CodZona { get; set; }

    public DateTime? FechaExistencia { get; set; }

    public string? NombreSocio { get; set; }

    public string? Provincia { get; set; }
}
