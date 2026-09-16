using System.Net;

namespace PaginaToros.Client.Helpers
{
    /// <summary>
    /// Detalle de un campo que cambió entre lo que estaba guardado y lo que se guardó.
    /// </summary>
    public sealed class CambioCertificado
    {
        public CambioCertificado(string campo, string? antes, string? ahora)
        {
            Campo = campo;
            Antes = antes;
            Ahora = ahora;
        }

        public string Campo { get; }
        public string? Antes { get; }
        public string? Ahora { get; }
    }

    /// <summary>
    /// Datos del certificado que viajan al mail de aviso.
    /// </summary>
    public sealed class CertificadoEmailDatos
    {
        public string? NroCert { get; set; }
        public string? TipoCert { get; set; }
        public string? SocioNombre { get; set; }
        public string? SocioCodigo { get; set; }
        public string? CentroNombre { get; set; }
        public string? ToroNombre { get; set; }
        public string? ToroHba { get; set; }
        public string? ToroTatuaje { get; set; }
        public string? DosisOriginales { get; set; }
        public string? DosisRemanentes { get; set; }
        public string? TipoEnvase { get; set; }
    }

    public static class CertificadoEmailTemplates
    {
        public static string BuildAsunto(string? nroCert)
            => string.IsNullOrWhiteSpace(nroCert)
                ? "Edición de certificado de semen"
                : $"Edición de certificado de semen {nroCert.Trim()}";

        public static string BuildEdicionCertificadoHtml(
            CertificadoEmailDatos datos,
            IReadOnlyCollection<CambioCertificado> cambios,
            DateTime fecha)
        {
            var nroCert = string.IsNullOrWhiteSpace(datos.NroCert) ? "sin número" : datos.NroCert.Trim();

            return $@"
<!DOCTYPE html>
<html lang=""es"">
<head>
  <meta charset=""utf-8"" />
  <title>Certificado de semen modificado</title>
</head>
<body style=""margin:0;padding:24px;background-color:#f3f5f7;font-family:Segoe UI, Arial, sans-serif;color:#1f2937;"">
  <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"" style=""max-width:760px;margin:0 auto;background:#ffffff;border:1px solid #dfe3e8;border-radius:12px;overflow:hidden;"">
    <tr>
      <td style=""background:#1f5f3b;color:#ffffff;padding:22px 28px;"">
        <div style=""font-size:24px;font-weight:700;"">Asociacion Criadores Hereford</div>
        <div style=""font-size:18px;margin-top:6px;"">Certificado de semen modificado</div>
      </td>
    </tr>
    <tr>
      <td style=""padding:18px 28px 8px 28px;font-size:15px;line-height:1.5;"">
        <p style=""margin:0 0 14px 0;"">Se editó el certificado de semen <strong>{Enc(nroCert)}</strong>. Abajo están los datos actualizados.</p>
      </td>
    </tr>
    <tr>
      <td style=""padding:6px 28px 20px 28px;"">
        <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"" style=""border-collapse:collapse;"">
          {BuildEmailRow("Nro. Certificado", datos.NroCert)}
          {BuildEmailRow("Tipo de certificado", datos.TipoCert)}
          {BuildEmailRow("Socio", BuildSocio(datos))}
          {BuildEmailRow("Centro", datos.CentroNombre)}
          {BuildEmailRow("Toro - Nombre", datos.ToroNombre)}
          {BuildEmailRow("Toro - HBA", datos.ToroHba)}
          {BuildEmailRow("Toro - Tatuaje", datos.ToroTatuaje)}
          {BuildEmailRow("Cantidad original de dosis", datos.DosisOriginales)}
          {BuildEmailRow("Dosis remanentes", datos.DosisRemanentes)}
          {BuildEmailRow("Tipo de envase", datos.TipoEnvase)}
        </table>
      </td>
    </tr>
    {BuildCambiosSection(cambios)}
    <tr>
      <td style=""padding:16px 28px 22px 28px;border-top:1px solid #e5e7eb;color:#6b7280;font-size:13px;line-height:1.5;"">
        Aviso automático generado el {fecha:dd/MM/yyyy} a las {fecha:HH:mm} hs. No respondas a este correo.
      </td>
    </tr>
  </table>
</body>
</html>";
        }

        private static string BuildCambiosSection(IReadOnlyCollection<CambioCertificado> cambios)
        {
            if (cambios == null || cambios.Count == 0)
            {
                return string.Empty;
            }

            var filas = string.Concat(cambios.Select(BuildCambioRow));

            return $@"
<tr>
  <td style=""padding:0 28px 22px 28px;"">
    <div style=""font-size:15px;font-weight:700;margin:0 0 10px 0;"">Qué se modificó</div>
    <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"" style=""border-collapse:collapse;font-size:14px;"">
      <tr>
        <th align=""left"" style=""padding:8px 10px;background:#eef2f1;color:#374151;border-bottom:1px solid #d7dedb;width:34%;"">Campo</th>
        <th align=""left"" style=""padding:8px 10px;background:#eef2f1;color:#374151;border-bottom:1px solid #d7dedb;width:33%;"">Antes</th>
        <th align=""left"" style=""padding:8px 10px;background:#eef2f1;color:#374151;border-bottom:1px solid #d7dedb;width:33%;"">Ahora</th>
      </tr>
      {filas}
    </table>
  </td>
</tr>";
        }

        private static string BuildCambioRow(CambioCertificado cambio)
        {
            // Tachar el guion de "estaba vacío" se lee como si se hubiera borrado algo.
            var tachado = string.IsNullOrWhiteSpace(cambio.Antes) ? string.Empty : "text-decoration:line-through;";

            return $@"
<tr>
  <td style=""padding:8px 10px;border-bottom:1px solid #e5e7eb;color:#6b7280;font-weight:600;"">{Enc(cambio.Campo)}</td>
  <td style=""padding:8px 10px;border-bottom:1px solid #e5e7eb;color:#9ca3af;{tachado}"">{Valor(cambio.Antes)}</td>
  <td style=""padding:8px 10px;border-bottom:1px solid #e5e7eb;color:#1f5f3b;font-weight:600;"">{Valor(cambio.Ahora)}</td>
</tr>";
        }

        private static string BuildEmailRow(string label, string? value)
            => $@"
<tr>
  <td style=""padding:8px 0;border-bottom:1px solid #e5e7eb;color:#6b7280;width:35%;font-weight:600;"">{Enc(label)}</td>
  <td style=""padding:8px 0;border-bottom:1px solid #e5e7eb;color:#111827;"">{Valor(value)}</td>
</tr>";

        private static string? BuildSocio(CertificadoEmailDatos datos)
        {
            var nombre = datos.SocioNombre?.Trim();
            var codigo = datos.SocioCodigo?.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                return codigo;
            }

            return string.IsNullOrWhiteSpace(codigo) ? nombre : $"{nombre} ({codigo})";
        }

        // Los nombres de socios y centros traen "&", "<" y comillas; sin escapar
        // rompen el HTML del mail.
        private static string Valor(string? value)
            => string.IsNullOrWhiteSpace(value) ? "-" : Enc(value.Trim());

        private static string Enc(string? value)
            => WebUtility.HtmlEncode(value ?? string.Empty);
    }
}
