using System;
using System.Collections.Generic;

namespace PaginaToros.Shared.Models;

public partial class InspRe
{
    public int Id { get; set; }

    public string? Nrores { get; set; }

    public string? Nropla { get; set; }

    public string? Observ { get; set; }

    public int? Ppajust { get; set; }

    public int? Epromedio { get; set; }

    public int? Emax { get; set; }

    public int? Emin { get; set; }

    public int? Tortot { get; set; }

    public int? Torsb { get; set; }

    public string? FchUsu { get; set; }

    public int? CodUsu { get; set; }

    public int? Editar { get; set; }

    public string? Icod { get; set; }

    public string? Scod { get; set; }

    public string? Estcod { get; set; }

    public DateTime? FechaInspeccion { get; set; }

    public string? NombreSocio { get; set; }
}
