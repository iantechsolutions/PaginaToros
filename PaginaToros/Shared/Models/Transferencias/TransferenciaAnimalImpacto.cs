using System;
using System.Collections.Generic;

namespace PaginaToros.Shared.Models
{
    public sealed class TransferenciaAnimalImpacto
    {
        public string FieldName { get; set; } = string.Empty;
        public string BucketLabel { get; set; } = string.Empty;
        public string OrigenLabel { get; set; } = string.Empty;
        public string DestinoLabel { get; set; } = string.Empty;
        public double OrigenAntes { get; set; }
        public double OrigenDespues { get; set; }
        public double DestinoAntes { get; set; }
        public double DestinoDespues { get; set; }
        public int Cantidad { get; set; }
    }

    public static class TransferenciaAnimalImpactoHelper
    {
        public static readonly IReadOnlyList<string> HembrasValidasParaVacas = new[] { "CC", "CCP" };
        public static readonly IReadOnlyList<string> HembrasValidasParaVaquillonas = new[] { "PR", "SS" };

        public static bool TryBuildImpacto(
            TransanDTO transan,
            PlantelDTO? origen,
            PlantelDTO? destino,
            out TransferenciaAnimalImpacto? impacto,
            out string? error)
        {
            impacto = null;
            error = null;

            if (transan is null)
            {
                error = "No se recibió la transferencia.";
                return false;
            }

            var fieldName = ResolveFieldName(transan.Tiphac, transan.Hemsta, transan.Tipohem, out var fieldError);
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                error = fieldError ?? "No se pudo determinar el bucket de stock impactado.";
                return false;
            }

            if (origen is null || string.IsNullOrWhiteSpace(origen.Placod))
            {
                error = "No hay plantel de origen seleccionado.";
                return false;
            }

            if (destino is null || string.IsNullOrWhiteSpace(destino.Placod))
            {
                error = "No hay plantel de destino seleccionado.";
                return false;
            }

            var cantidad = transan.CantHem ?? 0;
            var origenAntes = GetPlantelFieldValue(origen, fieldName);
            var destinoAntes = GetPlantelFieldValue(destino, fieldName);

            impacto = new TransferenciaAnimalImpacto
            {
                FieldName = fieldName,
                BucketLabel = GetBucketLabel(fieldName),
                OrigenLabel = $"{origen.Placod} - {origenAntes:0}",
                DestinoLabel = $"{destino.Placod} - {destinoAntes:0}",
                OrigenAntes = origenAntes,
                OrigenDespues = origenAntes - cantidad,
                DestinoAntes = destinoAntes,
                DestinoDespues = destinoAntes + cantidad,
                Cantidad = cantidad
            };

            return true;
        }

        public static string? ResolveFieldName(string? tiphac, string? hemsta, string? tipohem, out string? error)
        {
            error = null;

            var tiphacNormalizado = (tiphac ?? string.Empty).Trim().ToUpperInvariant();
            var hemstaNormalizada = (hemsta ?? string.Empty).Trim().ToUpperInvariant();
            var tipohemNormalizado = (tipohem ?? string.Empty).Trim().ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(tiphacNormalizado) ||
                string.IsNullOrWhiteSpace(hemstaNormalizada) ||
                string.IsNullOrWhiteSpace(tipohemNormalizado))
            {
                error = "Faltan datos para determinar el bucket impactado.";
                return null;
            }

            var esPR = tiphacNormalizado.Contains("PR", StringComparison.OrdinalIgnoreCase);
            var esVIP = tiphacNormalizado.Contains("VIP", StringComparison.OrdinalIgnoreCase);

            if (!esPR && !esVIP)
            {
                error = "El tipo de hacienda no es válido para determinar el impacto.";
                return null;
            }

            if (string.Equals(tipohemNormalizado, "VA", StringComparison.OrdinalIgnoreCase))
            {
                if (!HembrasValidasParaVacas.Contains(hemstaNormalizada))
                {
                    error = "La combinación elegida no corresponde a vacas. Para 'VA' solo se permiten estados CC o CCP.";
                    return null;
                }

                return esPR ? "Varede" : "Varepr";
            }

            if (string.Equals(tipohemNormalizado, "VQ", StringComparison.OrdinalIgnoreCase))
            {
                if (!HembrasValidasParaVaquillonas.Contains(hemstaNormalizada))
                {
                    error = "La combinación elegida no corresponde a vaquillonas. Para 'VQ' solo se permiten estados PR o SS.";
                    return null;
                }

                if (hemstaNormalizada == "PR")
                {
                    return esPR ? "Vqcsrd" : "Vqcsrp";
                }

                return esPR ? "Vqssrd" : "Vqssrp";
            }

            error = "El tipo de hembras no es válido.";
            return null;
        }

        public static double GetPlantelFieldValue(PlantelDTO plantel, string fieldName)
        {
            return fieldName switch
            {
                "Varede" => plantel.Varede ?? 0,
                "Vqcsrd" => plantel.Vqcsrd ?? 0,
                "Vqssrd" => plantel.Vqssrd ?? 0,
                "Varepr" => plantel.Varepr ?? 0,
                "Vqcsrp" => plantel.Vqcsrp ?? 0,
                "Vqssrp" => plantel.Vqssrp ?? 0,
                _ => 0
            };
        }

        public static string GetBucketLabel(string fieldName)
        {
            return fieldName switch
            {
                "Varede" => "Vacas PR",
                "Vqcsrd" => "Vaquillonas con servicio PR",
                "Vqssrd" => "Vaquillonas sin servicio PR",
                "Varepr" => "Vacas VIP",
                "Vqcsrp" => "Vaquillonas con servicio VIP",
                "Vqssrp" => "Vaquillonas sin servicio VIP",
                _ => fieldName
            };
        }

        public static string BuildChangeMessage(string label, double before, double after)
        {
            return $"La cantidad de {label} pasará de {before:0} a {after:0}.";
        }
    }
}
