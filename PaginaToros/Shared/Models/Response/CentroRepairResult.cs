namespace PaginaToros.Shared.Models.Response
{
    /// <summary>
    /// Resultado del diagnóstico / reparación de centros sin NROCEN cargado.
    /// NROCEN es la clave con la que CERTIFSEMEN referencia a CENTROSIA, así que
    /// un centro sin ese dato no se puede usar para dar de alta certificados.
    /// </summary>
    public class CentroRepairResult
    {
        /// <summary>True cuando se corrió en modo análisis y no se tocó nada.</summary>
        public bool SoloDiagnostico { get; set; }
        public int TotalCentros { get; set; }
        public int CentrosSinNrocen { get; set; }
        public int CentrosReparados { get; set; }
        public int CertificadosHuerfanos { get; set; }
        public int CertificadosRevinculados { get; set; }
        public List<CentroRepairItem> Detalle { get; set; } = new();
        public List<string> Observaciones { get; set; } = new();
    }

    public class CentroRepairItem
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? NroCSayg { get; set; }
        public string? NrocenNuevo { get; set; }
        public string Origen { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
