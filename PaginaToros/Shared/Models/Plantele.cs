using System;
using System.Collections.Generic;

namespace PaginaToros.Shared.Models;

public partial class Plantele
{
    public int Id { get; set; }

    public string? NroPlantel { get; set; }

    public bool? Activo { get; set; }

    public string? FechaExistencia { get; set; }

    public string? NroUltInspeccion { get; set; }

    public DateTime? UltimaInspeccion { get; set; }

    public DateTime? UltimaReinspeccion { get; set; }

    public string? CodSocio { get; set; }

    public double? Vacas { get; set; }

    public double? VaquillServicio { get; set; }

    public double? VaquillNoServicio { get; set; }

    public double? VacasVip { get; set; }

    public double? PrenadasVip { get; set; }

    public double? VaquillNoServicioVip { get; set; }

    public string? Comentarios { get; set; }

    public string? NombreSocio { get; set; }
}
