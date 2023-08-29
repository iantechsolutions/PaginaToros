using System;
using System.Collections.Generic;

namespace PaginaToros.Shared.Models;

public partial class ResultadoInspeccion
{
    public int Id { get; set; }

    public int? NroSolicitud { get; set; }

    public int? NroSocio { get; set; }

    public string? NombreSocio { get; set; }

    public int? NroResultado { get; set; }

    public string? Plantel { get; set; }

    public DateTime? AñoExistenciaPlantel { get; set; }

    public DateTime? FechaDeSolicitud { get; set; }

    public DateTime? FechaDeInspeccion { get; set; }

    public string? Inspector { get; set; }

    public string? Establecimiento { get; set; }

    public string? Localidad { get; set; }

    public bool? Reinspeccion { get; set; }
}
