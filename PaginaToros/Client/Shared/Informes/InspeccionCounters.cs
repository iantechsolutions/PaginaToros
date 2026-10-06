using PaginaToros.Shared.Models;

namespace PaginaToros.Client.Shared.Informes
{
    /// <summary>
    /// Acumulador de métricas del Resumen General de Inspecciones (PR / VIP / SCP / CP).
    /// Es la misma clase <c>Counters</c> que vivía dentro de MensualGeneral.razor: las reglas de
    /// cálculo (qué campos de RESIN6 suman a presentadas/marcadas y cómo se derivan las rechazadas)
    /// no cambian; se extrajo para que la tabla compartida de desgloses pueda leerla.
    /// </summary>
    public sealed class InspeccionCounters
    {
        // PR
        public int prPres, prMarc;
        // VIP
        public int vipPres, vipMarc;
        // SCP
        public int scpPres, scpMarc;
        // CP
        public int cpPres, cpMarc, cpSBarra;

        public int prRech => Math.Max(0, prPres - prMarc);
        public int vipRech => Math.Max(0, vipPres - vipMarc);
        public int scpRech => Math.Max(0, scpPres - scpMarc);
        public int cpRech => Math.Max(0, cpPres - cpMarc);

        public int TotalPresentadas => prPres + vipPres + scpPres + cpPres;
        public int TotalMarcadas => prMarc + vipMarc + scpMarc + cpMarc;
        public int TotalRechazadas => prRech + vipRech + scpRech + cpRech;

        public void AddFrom(Resin6DTO x)
        {
            // defensivo: convertir a int cualquier valor nullable
            prPres += Convert.ToInt32(x.Hdp ?? 0);
            prMarc += Convert.ToInt32((x.HdpM ?? 0) + (x.HdpAs ?? 0));

            vipPres += Convert.ToInt32((x.Hgvp ?? 0) + (x.Hgqp ?? 0));
            vipMarc += Convert.ToInt32((x.Hgvb ?? 0) + (x.Hgqb ?? 0));

            scpPres += Convert.ToInt32(x.Mcp ?? 0);
            scpMarc += Convert.ToInt32((x.McpM ?? 0) + (x.McpAs ?? 0));

            cpPres += Convert.ToInt32(x.Msp ?? 0);
            cpMarc += Convert.ToInt32((x.MspM ?? 0) + (x.MspAs ?? 0));
            cpSBarra += Convert.ToInt32(x.Mspsb ?? 0);
        }

        /// <summary>Suma otro acumulador (totales de una tabla filtrada).</summary>
        public void Add(InspeccionCounters other)
        {
            prPres += other.prPres; prMarc += other.prMarc;
            vipPres += other.vipPres; vipMarc += other.vipMarc;
            scpPres += other.scpPres; scpMarc += other.scpMarc;
            cpPres += other.cpPres; cpMarc += other.cpMarc; cpSBarra += other.cpSBarra;
        }

        public bool IsAllZero =>
            prPres == 0 && prMarc == 0 &&
            vipPres == 0 && vipMarc == 0 &&
            scpPres == 0 && scpMarc == 0 &&
            cpPres == 0 && cpMarc == 0 && cpSBarra == 0;
    }

    /// <summary>Fila de un desglose (por provincia, por inspector, por mes...).</summary>
    public sealed class InspeccionRow
    {
        public InspeccionRow(string key, string label, string? label2, InspeccionCounters counters)
        {
            Key = key;
            Label = label;
            Label2 = label2;
            C = counters;
        }

        public string Key { get; }
        public string Label { get; }
        public string? Label2 { get; }
        public InspeccionCounters C { get; }
    }
}
