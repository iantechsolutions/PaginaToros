using PaginaToros.Shared.Models;

namespace PaginaToros.Shared.Helpers
{
    public static class DeclaracionServicioHelper
    {
        private static readonly Dictionary<string, string> TipoLabelPorCodigo = new(StringComparer.OrdinalIgnoreCase)
        {
            ["NA"] = "Natural",
            ["NC"] = "Natural a Corral",
            ["IA"] = "Inseminacion c/celo",
            ["IAC"] = "Inseminacion c/celo y rep",
            ["IACR"] = "Inseminacion c/celo y rep",
            ["IR"] = "Inseminacion c/celo y rep",
            ["ANUAL"] = "Inseminacion c/celo y rep",
            ["IAPR"] = "Inseminacion c/p y rep",
            ["IATFR"] = "IATF c/rep"
        };

        private static readonly Dictionary<string, string> TipoCodigoPorLabel = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Natural"] = "NA",
            ["Natural a Corral"] = "NC",
            ["Inseminacion c/celo"] = "IA",
            ["Inseminacion c/celo y rep"] = "IACR",
            ["Inseminacion c/p y rep"] = "IAPR",
            ["IATF c/rep"] = "IATFR",
            ["Santa Fe"] = "IATFR"
        };

        private static readonly HashSet<string> TiposInseminacion = new(StringComparer.OrdinalIgnoreCase)
        {
            "Inseminacion c/celo",
            "Inseminacion c/celo y rep",
            "Inseminacion c/p y rep",
            "IATF c/rep"
        };

        public static string NormalizeTipoLabel(string? label)
        {
            var value = (label ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            if (TipoLabelPorCodigo.TryGetValue(value, out var mappedLabel))
            {
                return mappedLabel;
            }

            if (string.Equals(value, "Santa Fe", StringComparison.OrdinalIgnoreCase))
            {
                return "IATF c/rep";
            }

            return value;
        }

        public static string ToCode(string? label)
        {
            var normalized = NormalizeTipoLabel(label);
            if (TipoCodigoPorLabel.TryGetValue(normalized, out var code))
            {
                return code;
            }

            return normalized;
        }

        public static string ToLabel(string? code)
        {
            var normalizedCode = (code ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(normalizedCode))
            {
                return normalizedCode;
            }

            if (TipoLabelPorCodigo.TryGetValue(normalizedCode, out var label))
            {
                return label;
            }

            var pair = TipoCodigoPorLabel.FirstOrDefault(kv =>
                string.Equals(kv.Value, normalizedCode, StringComparison.OrdinalIgnoreCase));

            return string.IsNullOrWhiteSpace(pair.Key) ? normalizedCode : NormalizeTipoLabel(pair.Key);
        }

        public static bool IsNaturalTipo(string? tipo)
            => string.Equals(NormalizeTipoLabel(tipo), "Natural", StringComparison.Ordinal);

        public static bool IsNaturalCorralTipo(string? tipo)
            => string.Equals(NormalizeTipoLabel(tipo), "Natural a Corral", StringComparison.Ordinal);

        public static bool IsTipoInseminacion(string? tipo)
            => TiposInseminacion.Contains(NormalizeTipoLabel(tipo));

        public static bool IsTipoConToros(string? tipo)
        {
            var normalized = NormalizeTipoLabel(tipo);
            return normalized == "Natural a Corral"
                || normalized == "Inseminacion c/celo y rep"
                || normalized == "Inseminacion c/p y rep"
                || normalized == "IATF c/rep";
        }

        public static bool IsDetalleInseminacion(string? tipo)
            => string.Equals((tipo ?? string.Empty).Trim(), "insem", StringComparison.OrdinalIgnoreCase);

        public static double GetMultiplicador(string? tipo) => NormalizeTipoLabel(tipo) switch
        {
            "Inseminacion c/celo" => 1.5,
            "Inseminacion c/celo y rep" => 1.0,
            "IATF c/rep" => 1.0,
            "Inseminacion c/p y rep" => 0.8,
            _ => 0.0
        };

        public static double GetMultiplicadorForCode(string? tipse) => NormalizeTipoLabel(tipse) switch
        {
            "Inseminacion c/celo" => 1.5,
            "Inseminacion c/celo y rep" => 1.0,
            "IATF c/rep" => 1.0,
            "Inseminacion c/p y rep" => 0.8,
            _ => 0.0
        };

        public static PlantelDTO? ResolvePlantelHistorico(IEnumerable<PlantelDTO>? planteles, string? nroplan, DateTime? fechaDeclaracion, out string warning)
        {
            warning = string.Empty;
            var requested = (nroplan ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(requested) || planteles == null)
            {
                return null;
            }

            var list = planteles.Where(p => p != null).ToList();
            if (list.Count == 0)
            {
                return null;
            }

            var exactMatches = list
                .Where(p => string.Equals((p.Placod ?? string.Empty).Trim(), requested, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (exactMatches.Count == 1)
            {
                return exactMatches[0];
            }

            if (exactMatches.Count > 1)
            {
                return ResolveBestCandidate(exactMatches, requested, fechaDeclaracion, out warning, "registros exactos");
            }

            var suffixMatches = list
                .Where(p => !string.IsNullOrWhiteSpace(p.Placod) &&
                            (p.Placod ?? string.Empty).Trim().EndsWith(requested, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (suffixMatches.Count == 1)
            {
                return suffixMatches[0];
            }

            if (suffixMatches.Count > 1)
            {
                return ResolveBestCandidate(suffixMatches, requested, fechaDeclaracion, out warning, "códigos históricos");
            }

            var containsMatches = list
                .Where(p => !string.IsNullOrWhiteSpace(p.Placod) &&
                            (p.Placod ?? string.Empty).Trim().Contains(requested, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (containsMatches.Count == 1)
            {
                return containsMatches[0];
            }

            if (containsMatches.Count > 1)
            {
                return ResolveBestCandidate(containsMatches, requested, fechaDeclaracion, out warning, "coincidencias parciales");
            }

            return null;
        }

        private static PlantelDTO? ResolveBestCandidate(
            IReadOnlyCollection<PlantelDTO> matches,
            string requested,
            DateTime? fechaDeclaracion,
            out string warning,
            string reason)
        {
            warning = string.Empty;

            var scored = matches
                .Select(candidate => new CandidateScore(candidate, BuildScore(candidate, fechaDeclaracion)))
                .ToList();

            if (scored.Count == 0)
            {
                return null;
            }

            var best = scored
                .OrderByDescending(x => x.Score.IsOnOrBeforeDeclaration)
                .ThenBy(x => x.Score.YearDistance)
                .ThenByDescending(x => x.Score.HasYear)
                .ThenByDescending(x => x.Score.CandidateYear)
                .ThenByDescending(x => x.Score.CreationDate)
                .ThenByDescending(x => x.Score.Id)
                .ToList();

            var winner = best[0];
            var tied = best
                .Where(x => x.Score.Equals(winner.Score))
                .ToList();

            if (tied.Count > 1)
            {
                var preview = string.Join(", ", tied.Take(5).Select(x => x.Candidate.Placod).Where(x => !string.IsNullOrWhiteSpace(x)));
                warning = fechaDeclaracion.HasValue
                    ? $"El plantel {requested} coincide con varios {reason} para la declaración {fechaDeclaracion:yyyy}. Coincidencias: {preview}. Se seleccionó el más cercano al año, pero conviene revisarlo."
                    : $"El plantel {requested} coincide con varios {reason}. Coincidencias: {preview}. Se seleccionó el más cercano al año, pero conviene revisarlo.";
            }

            return winner.Candidate;
        }

        private static CandidateScoreDetails BuildScore(PlantelDTO candidate, DateTime? fechaDeclaracion)
        {
            var creationDate = GetPlantelCreationDate(candidate);
            var candidateYear = TryGetPlantelYear(candidate) ?? creationDate?.Year;
            var declarationYear = fechaDeclaracion?.Year;
            var yearDistance = candidateYear.HasValue && declarationYear.HasValue
                ? Math.Abs(candidateYear.Value - declarationYear.Value)
                : int.MaxValue;
            var effectiveCreationDate = creationDate
                ?? (candidateYear.HasValue ? new DateTime(candidateYear.Value, 1, 1) : (DateTime?)null);
            var isOnOrBeforeDeclaration = !fechaDeclaracion.HasValue
                || !effectiveCreationDate.HasValue
                || effectiveCreationDate.Value <= fechaDeclaracion.Value;

            return new CandidateScoreDetails(
                HasYear: candidateYear.HasValue,
                CandidateYear: candidateYear ?? int.MinValue,
                YearDistance: yearDistance,
                IsOnOrBeforeDeclaration: isOnOrBeforeDeclaration,
                CreationDate: creationDate ?? DateTime.MinValue,
                Id: candidate?.Id ?? 0);
        }

        private static int? TryGetPlantelYear(PlantelDTO? plantel)
        {
            if (plantel == null || string.IsNullOrWhiteSpace(plantel.Anioex))
            {
                return null;
            }

            return int.TryParse(plantel.Anioex, out var year) ? year : null;
        }

        private static DateTime? GetPlantelCreationDate(PlantelDTO? plantel)
        {
            if (plantel == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(plantel.Fecing))
            {
                var formatos = new[] { "yyyy/MM/dd", "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy" };
                if (DateTime.TryParseExact(plantel.Fecing, formatos, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var fechaCreacion))
                {
                    return fechaCreacion;
                }

                if (DateTime.TryParse(plantel.Fecing, out fechaCreacion))
                {
                    return fechaCreacion;
                }
            }

            return plantel.FchUsu;
        }

        private sealed record CandidateScore(PlantelDTO Candidate, CandidateScoreDetails Score);

        private sealed record CandidateScoreDetails(
            bool HasYear,
            int CandidateYear,
            int YearDistance,
            bool IsOnOrBeforeDeclaration,
            DateTime CreationDate,
            int Id);
    }
}
