using System;
using System.Collections.Generic;

namespace PaginaToros.Server.ModelsTemp;

public partial class SolicitudInspeccion
{
    public int Id { get; set; }

    public int? NroSolicitud { get; set; }

    public int? NroSocio { get; set; }

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

    public string? ToroPr { get; set; }

    public string? VcPr { get; set; }

    public string? VcVip { get; set; }

    public string? VqVip { get; set; }

    public int? CodEstablecimiento { get; set; }

    public string? FechaSolTemp { get; set; }

    public string? FechaAuTemp { get; set; }
}
