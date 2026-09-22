using System.Collections.Generic;

namespace PaginaToros.Shared.Models
{
    /// <summary>Las seis categorias de existencia que guarda un plantel.</summary>
    public class PlantelTotalesDTO
    {
        public double Varede { get; set; }
        public double Vqcsrd { get; set; }
        public double Vqssrd { get; set; }
        public double Varepr { get; set; }
        public double Vqcsrp { get; set; }
        public double Vqssrp { get; set; }

        public bool TieneNegativos =>
            Varede < 0 || Vqcsrd < 0 || Vqssrd < 0 || Varepr < 0 || Vqcsrp < 0 || Vqssrp < 0;
    }

    /// <summary>
    /// Que cuelga de un plantel. Son cinco tablas que lo referencian por codigo de texto, sin FK:
    /// borrar el plantel sin repuntarlas deja esos registros huerfanos.
    /// </summary>
    public class PlantelReferenciasDTO
    {
        public int Resin1 { get; set; }
        public int Resin8 { get; set; }
        public int Desepla1 { get; set; }
        public int Remansol { get; set; }
        public int Retenidas { get; set; }
        public List<string> Detalle { get; set; } = new();

        public int Total => Resin1 + Resin8 + Desepla1 + Remansol + Retenidas;
    }

    public class PlantelDuplicadoDTO
    {
        public int Id { get; set; }
        public string? Placod { get; set; }
        public string? Anioex { get; set; }
        public string? Nrocri { get; set; }
        public string? Fecing { get; set; }

        public PlantelTotalesDTO Totales { get; set; } = new();
        public PlantelReferenciasDTO Referencias { get; set; } = new();

        /// <summary>Su codigo se deriva del de otro plantel del mismo grupo: huella de la cascada.</summary>
        public bool DerivadoDeOtroDelGrupo { get; set; }
        public string? DerivadoDe { get; set; }
        public bool EsSugerido { get; set; }
    }

    public class PlantelDuplicadoGrupoDTO
    {
        public string Nrocri { get; set; } = string.Empty;
        public string Anioex { get; set; } = string.Empty;
        public string? SocioNombre { get; set; }

        public List<PlantelDuplicadoDTO> Planteles { get; set; } = new();

        public string? PlacodSugerido { get; set; }
        public string? MotivoSugerencia { get; set; }

        /// <summary>Totales recalculados con la formula real del circuito. Null si no se pudieron calcular.</summary>
        public PlantelTotalesDTO? TotalesRecalculados { get; set; }
        public string? OrigenTotalesRecalculados { get; set; }

        public List<string> Advertencias { get; set; } = new();

        public string Clave => $"{Nrocri}|{Anioex}";
    }

    public class ConsolidarPlantelesRequest
    {
        public string Nrocri { get; set; } = string.Empty;
        public string Anioex { get; set; } = string.Empty;
        public string PlacodSobreviviente { get; set; } = string.Empty;
        public List<string> PlacodsAEliminar { get; set; } = new();

        public bool RepuntarReferencias { get; set; } = true;
        public bool RecalcularTotales { get; set; } = true;
        public bool EliminarSobrantes { get; set; } = true;

        /// <summary>Habilita grabar totales recalculados que den negativo. Por defecto se bloquea.</summary>
        public bool AceptarTotalesNegativos { get; set; }

        /// <summary>false = simulacion: calcula el plan y no toca nada.</summary>
        public bool Aplicar { get; set; }
    }

    public class ConsolidarPlantelesResultado
    {
        public bool Aplicado { get; set; }
        public List<string> Acciones { get; set; } = new();
        public List<string> Advertencias { get; set; } = new();
        public List<string> Errores { get; set; } = new();

        public bool PuedeAplicarse => Errores.Count == 0;
    }
}
