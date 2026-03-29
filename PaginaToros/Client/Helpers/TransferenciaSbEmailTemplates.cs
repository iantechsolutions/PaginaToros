namespace PaginaToros.Client.Helpers
{
    public static class TransferenciaSbEmailTemplates
    {
        public static string BuildTransferenciaSbEmailHtml(
            bool esAlta,
            string? nroCert,
            string? nroOrden,
            string? socioVendedor,
            string? socioComprador,
            string? toroNombre,
            string? toroHba,
            string? toroTatuaje)
        {
            var titulo = esAlta ? "Nueva transferencia S/" : "Transferencia S/ modificada";
            return $@"
<!DOCTYPE html>
<html lang=""es"">
<head>
  <meta charset=""utf-8"" />
  <title>{titulo}</title>
</head>
<body style=""margin:0;padding:24px;background-color:#f3f5f7;font-family:Segoe UI, Arial, sans-serif;color:#1f2937;"">
  <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"" style=""max-width:760px;margin:0 auto;background:#ffffff;border:1px solid #dfe3e8;border-radius:12px;overflow:hidden;"">
    <tr>
      <td style=""background:#1f5f3b;color:#ffffff;padding:22px 28px;"">
        <div style=""font-size:24px;font-weight:700;"">Asociacion Criadores Hereford</div>
        <div style=""font-size:18px;margin-top:6px;"">{titulo}</div>
      </td>
    </tr>
    <tr>
      <td style=""padding:18px 28px 8px 28px;font-size:15px;line-height:1.5;"">
        <p style=""margin:0 0 14px 0;"">Se registró la siguiente transferencia S/ en el sistema.</p>
      </td>
    </tr>
    <tr>
      <td style=""padding:6px 28px 20px 28px;"">
        <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"" style=""border-collapse:collapse;"">
          {BuildEmailRow("Nro. Certificado", nroCert)}
          {BuildEmailRow("Nro. Orden", nroOrden)}
          {BuildEmailRow("Socio vendedor", socioVendedor)}
          {BuildEmailRow("Socio comprador", socioComprador)}
          {BuildEmailRow("Toro - Nombre", toroNombre)}
          {BuildEmailRow("Toro - HBA", toroHba)}
          {BuildEmailRow("Toro - Tatuaje", toroTatuaje)}
        </table>
      </td>
    </tr>
  </table>
</body>
</html>";
        }

        private static string BuildEmailRow(string label, string? value)
        {
            var safeValue = string.IsNullOrWhiteSpace(value) ? "-" : value;
            return $@"
<tr>
  <td style=""padding:8px 0;border-bottom:1px solid #e5e7eb;color:#6b7280;width:35%;font-weight:600;"">{label}</td>
  <td style=""padding:8px 0;border-bottom:1px solid #e5e7eb;color:#111827;"">{safeValue}</td>
</tr>";
        }
    }
}
