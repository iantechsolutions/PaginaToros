using System;
using System.Collections.Generic;

namespace PaginaToros.Shared.Models;

public partial class SolicitudInspeccion
{
    public int Id { get; set; }

    public string? NroSolicitud { get; set; }

    public string? NroSocio { get; set; }

    public bool? Activo { get; set; }

    public string? NombreSocio { get; set; }

    public string? Establecimiento { get; set; }

    public DateTime? FechaSolicitud { get; set; }

    public DateTime? FechaAutorizacion { get; set; }

    public bool? Reinspeccion { get; set; }

    public bool? ControlProduccion { get; set; }

    public bool? Completada { get; set; }

    public string? Min { get; set; }

    public string? Max { get; set; }

    public DateTime? Ano { get; set; }

    public double? ToroPr { get; set; }

    public double? VcPr { get; set; }

    public double? VcVip { get; set; }

    public double? VqVip { get; set; }

    public string? CodEstablecimiento { get; set; }

    public string? FechaSolTemp { get; set; }

    public string? FechaAuTemp { get; set; }
}
