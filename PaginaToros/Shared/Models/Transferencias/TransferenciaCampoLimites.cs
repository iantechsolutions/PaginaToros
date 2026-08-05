namespace PaginaToros.Shared.Models
{
    /// <summary>
    /// Largos maximos reales de las columnas involucradas en transferencias.
    /// Estos valores deben coincidir con el esquema de la base (ver
    /// Server/Sql/20260805_widen_transan_futcontrol_plantel_columns.sql) y con
    /// el mapeo de hereford_prContext. Se centralizan aca para que la validacion
    /// y los mensajes de error no puedan volver a divergir del esquema.
    /// </summary>
    public static class TransferenciaCampoLimites
    {
        /// <summary>PLANTEL.PLACOD, TRANSAN.PLANT, TRANSAN.NVO_PLA, FUT_CONTROL.PLANTEL/PLANT_DEST.</summary>
        public const int PlantelCodigo = 10;

        /// <summary>TRANSAN.TIPHAC (necesita 5 para 'PHVIP').</summary>
        public const int TipoHacienda = 8;

        /// <summary>TRANSAN.HEMSTA / FUT_CONTROL.HEMSTA.</summary>
        public const int EstadoHembras = 3;

        /// <summary>TRANSAN.TIPOHEM.</summary>
        public const int TipoHembras = 2;

        /// <summary>SOCIOS.SCOD, TRANSAN.SVEN/SCOM, FUT_CONTROL.SVEN/SCOM.</summary>
        public const int SocioCodigo = 6;

        /// <summary>SOCIOS.NOMBRE, TRANSAN.VNOM/CNOM, FUT_CONTROL.VNOM/CNOM.</summary>
        public const int SocioNombre = 100;

        /// <summary>TRANSAN.NRO_CERT.</summary>
        public const int NroCertificado = 10;

        /// <summary>FUT_CONTROL.NRO_TRANS.</summary>
        public const int NroTransferencia = 6;
    }
}
