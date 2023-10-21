using System;
using System.Collections.Generic;

namespace PaginaToros.Shared.Models;

public partial class Socio
{
    public int Id { get; set; }

    public string NroSocio { get; set; }

    public bool? Activo { get; set; }

    public string? NombreCompleto { get; set; }

    public string? Domicilio { get; set; }

    public string? Localidad { get; set; }

    public string? CodPostal { get; set; }

    public string? Provincia { get; set; }

    public string? Telefono { get; set; }

    public string? Telefono2 { get; set; }

    public string? Mail { get; set; }

    public string? UltimoPlantel { get; set; }

    public DateTime? FechaExistencia { get; set; }

    public string? Prenom { get; set; }

    public string? Postnom { get; set; }
}
