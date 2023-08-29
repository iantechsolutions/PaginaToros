using System;
using System.Collections.Generic;

namespace PaginaToros.Shared.Models;

public partial class Plantele
{
    public int Id { get; set; }

    public string? NroPlantel { get; set; }

    public bool? Activo { get; set; }

    public DateTime? FechaExistencia { get; set; }

    public string? NroUltInspeccion { get; set; }

    public DateTime? UltimaInspeccion { get; set; }

    public DateTime? UltimaReinspeccion { get; set; }

    public string? Socio { get; set; }

    public int? Vacas { get; set; }

    public int? VaquillServicio { get; set; }

    public int? VaquillNoServicio { get; set; }

    public int? VacasVip { get; set; }

    public int? PrenadasVip { get; set; }

    public int? VaquillNoServicioVip { get; set; }
}
