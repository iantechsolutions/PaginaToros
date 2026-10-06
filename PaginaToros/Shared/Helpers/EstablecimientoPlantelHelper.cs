using System;
using System.Collections.Generic;
using System.Linq;
using PaginaToros.Shared.Models;

namespace PaginaToros.Shared.Helpers
{
    /// <summary>
    /// Reglas compartidas (cliente, servidor y tests) para decidir que plantel, toro o certificado
    /// puede usarse en una operacion que ocurre en un establecimiento concreto.
    ///
    /// Un registro "sin establecimiento" (EstablecimientoId == null) es un dato historico que no
    /// pudo asignarse con certeza. Se lo considera COMPATIBLE con cualquier establecimiento para no
    /// bloquear la operatoria, pero NUNCA se lo elige por defecto cuando la razon social tiene mas
    /// de un establecimiento: ahi la eleccion es del usuario.
    /// </summary>
    public static class EstablecimientoPlantelHelper
    {
        /// <summary>
        /// Largo maximo de un codigo de plantel: PLANTEL.PLACOD es varchar(10). Toda columna que
        /// referencia ese codigo (RESIN1/RESIN8.NROPLA, DESEPLA1/REMANSOL/retenidas.NROPLAN,
        /// FUT_CONTROL.PLANTEL y PLANT_DEST, TRANSAN.PLANT y NVO_PLA) tiene que admitir al menos
        /// este largo; ver Server/Sql/20261006_establecimiento_plantel_certifsemen.sql.
        /// Con un plantel por establecimiento, el segundo del mismo año lleva sufijo y ya no entra
        /// en los 4 o 5 caracteres de las columnas legacy.
        /// </summary>
        public const int CodigoMaxLength = 10;

        public static bool MismoCodigo(string? a, string? b)
            => string.Equals((a ?? string.Empty).Trim(), (b ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase);

        /// <summary>Mensaje de error si el codigo supera <see cref="CodigoMaxLength"/>; null si es valido.</summary>
        public static string? ValidarCodigo(string? placod)
        {
            var codigo = (placod ?? string.Empty).Trim();
            return codigo.Length > CodigoMaxLength
                ? $"El código de plantel {codigo} tiene {codigo.Length} caracteres y el máximo es {CodigoMaxLength}."
                : null;
        }

        /// <summary>
        /// Codigo para un plantel nuevo: ultimo digito del año + cola heredada del plantel de origen
        /// (o del socio). Si ese codigo ya existe para la razon social (por ejemplo, el plantel del
        /// mismo año en OTRO establecimiento) se agrega un sufijo numerico: 6RA01, 6RA011, 6RA012...
        /// Es la misma regla en el cliente (AddReporte) y en el servidor (PlantelController.GetOrCreate).
        /// Nunca trunca: si el resultado supera <see cref="CodigoMaxLength"/> lanza, porque un codigo
        /// recortado podria coincidir con el de otro plantel.
        /// </summary>
        public static string GenerarCodigoDisponible(string? anioex, string? tail, IEnumerable<string?>? codigosOcupados)
        {
            var anio = (anioex ?? string.Empty).Trim();
            var basePlacod = (anio.Length > 0 ? anio[^1].ToString() : string.Empty) + (tail ?? string.Empty).Trim();
            if (basePlacod.Length == 0)
            {
                throw new InvalidOperationException("No se pudo armar el código del plantel: faltan el año y la cola del código.");
            }

            var ocupados = (codigosOcupados ?? Enumerable.Empty<string?>())
                .Select(c => (c ?? string.Empty).Trim())
                .Where(c => c.Length > 0)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var candidato = basePlacod;
            var sufijo = 1;
            while (ocupados.Contains(candidato))
            {
                candidato = basePlacod + sufijo.ToString();
                sufijo++;
            }

            var error = ValidarCodigo(candidato);
            if (error != null)
            {
                throw new InvalidOperationException(error + " Revisá los códigos de plantel de la razón social.");
            }

            return candidato;
        }

        /// <summary>
        /// true si el registro (plantel, toro, certificado) puede participar en una operacion del
        /// establecimiento indicado: pertenece a ese establecimiento o no tiene ninguno asignado.
        /// Sin establecimiento de referencia (null) todo es compatible.
        /// </summary>
        public static bool EsCompatible(int? establecimientoDelRegistro, int? establecimientoOperacion)
            => !establecimientoOperacion.HasValue
               || !establecimientoDelRegistro.HasValue
               || establecimientoDelRegistro.Value == establecimientoOperacion.Value;

        /// <summary>
        /// true si el registro pertenece exactamente a ese establecimiento (no cuenta los "sin asignar").
        /// </summary>
        public static bool EsDelEstablecimiento(int? establecimientoDelRegistro, int? establecimientoOperacion)
            => establecimientoOperacion.HasValue
               && establecimientoDelRegistro.HasValue
               && establecimientoDelRegistro.Value == establecimientoOperacion.Value;

        public static int? AnioDe(PlantelDTO? plantel)
            => plantel != null && int.TryParse((plantel.Anioex ?? string.Empty).Trim(), out var anio) ? anio : null;

        /// <summary>
        /// Planteles de la razon social que pueden usarse en una operacion del establecimiento dado,
        /// ordenados: primero los del establecimiento, despues los sin asignar; dentro de cada grupo
        /// por año descendente.
        /// </summary>
        public static List<PlantelDTO> PlantelesCompatibles(
            IEnumerable<PlantelDTO>? planteles,
            string? scod,
            int? establecimientoId)
        {
            if (planteles == null || string.IsNullOrWhiteSpace(scod))
            {
                return new List<PlantelDTO>();
            }

            return planteles
                .Where(p => MismoCodigo(p.Nrocri, scod))
                .Where(p => EsCompatible(p.EstablecimientoId, establecimientoId))
                .OrderByDescending(p => EsDelEstablecimiento(p.EstablecimientoId, establecimientoId))
                .ThenByDescending(p => AnioDe(p).HasValue)
                .ThenByDescending(p => AnioDe(p))
                .ThenByDescending(p => p.Id)
                .ToList();
        }

        /// <summary>
        /// Plantel que aporta la existencia anterior de una inspeccion (o el plantel por defecto de
        /// cualquier formulario) para la razon social <paramref name="scod"/> en el establecimiento
        /// <paramref name="establecimientoId"/> y el año <paramref name="anio"/>.
        ///
        /// Regla:
        ///  1. Si hay planteles DEL establecimiento: el de ese año, si no el mas reciente.
        ///  2. Si no hay y la razon social tiene a lo sumo un establecimiento: los planteles sin
        ///     asignar valen como propios (van a quedar asignados de todos modos).
        ///  3. Si la razon social tiene varios establecimientos y solo hay planteles sin asignar:
        ///     NO se elige ninguno. Tomar el stock de la razon social entera como existencia anterior
        ///     de un campo seria mezclar establecimientos; lo decide el usuario.
        ///  4. Sin establecimiento de referencia: si la razon social tiene varios establecimientos
        ///     NO se elige ninguno (primero hay que decir en cual se inspecciona; mostrar la
        ///     existencia de un campo cualquiera seria mezclar establecimientos). Si tiene a lo sumo
        ///     uno (o ninguno cargado): comportamiento historico, por socio y año.
        /// </summary>
        public static PlantelDTO? ElegirPlantelPorDefecto(
            IEnumerable<PlantelDTO>? planteles,
            string? scod,
            int? establecimientoId,
            int cantidadEstablecimientosSocio,
            int anio)
        {
            if (planteles == null || string.IsNullOrWhiteSpace(scod))
            {
                return null;
            }

            var delSocio = planteles.Where(p => MismoCodigo(p.Nrocri, scod)).ToList();
            if (delSocio.Count == 0)
            {
                return null;
            }

            if (!establecimientoId.HasValue)
            {
                return cantidadEstablecimientosSocio > 1 ? null : PreferirAnio(delSocio, anio);
            }

            var propios = delSocio
                .Where(p => EsDelEstablecimiento(p.EstablecimientoId, establecimientoId))
                .ToList();
            if (propios.Count > 0)
            {
                return PreferirAnio(propios, anio);
            }

            if (cantidadEstablecimientosSocio <= 1)
            {
                var sinAsignar = delSocio.Where(p => !p.EstablecimientoId.HasValue).ToList();
                return PreferirAnio(sinAsignar, anio);
            }

            return null;
        }

        /// <summary>
        /// Plantel de destino de una inspeccion: el de ese socio, ese año y ese establecimiento.
        /// Devuelve null si no existe (hay que crearlo). Los planteles sin asignar del mismo año se
        /// devuelven aparte en <paramref name="legacySinAsignar"/> para que quien llama decida que
        /// hacer con ellos (asignarlos si el socio tiene un solo establecimiento, preguntar si tiene varios).
        /// </summary>
        public static PlantelDTO? BuscarPlantelDestino(
            IEnumerable<PlantelDTO>? planteles,
            string? scod,
            int? establecimientoId,
            string anio,
            out PlantelDTO? legacySinAsignar)
        {
            legacySinAsignar = null;
            if (planteles == null || string.IsNullOrWhiteSpace(scod))
            {
                return null;
            }

            var delAnio = planteles
                .Where(p => MismoCodigo(p.Nrocri, scod))
                .Where(p => string.Equals((p.Anioex ?? string.Empty).Trim(), (anio ?? string.Empty).Trim(), StringComparison.Ordinal))
                .OrderBy(p => p.Id)
                .ToList();

            if (!establecimientoId.HasValue)
            {
                return delAnio.FirstOrDefault();
            }

            legacySinAsignar = delAnio.FirstOrDefault(p => !p.EstablecimientoId.HasValue);
            return delAnio.FirstOrDefault(p => EsDelEstablecimiento(p.EstablecimientoId, establecimientoId));
        }

        private static PlantelDTO? PreferirAnio(List<PlantelDTO> candidatos, int anio)
        {
            if (candidatos.Count == 0)
            {
                return null;
            }

            var ordenados = candidatos
                .OrderByDescending(p => AnioDe(p).HasValue)
                .ThenByDescending(p => AnioDe(p))
                .ThenByDescending(p => p.Id)
                .ToList();

            return ordenados.FirstOrDefault(p => AnioDe(p) == anio) ?? ordenados.First();
        }

        /// <summary>
        /// Valor del filtro de establecimiento de los informes que significa "solo los planteles sin
        /// establecimiento asignado" (null en el filtro = todos los establecimientos). Es el mismo
        /// -1 que usa EstablecimientoSelect para la opción "Sin establecimiento".
        /// </summary>
        public const int FiltroSinEstablecimiento = -1;

        /// <summary>
        /// Aplica el filtro de establecimiento de los informes: null = todos; <see cref="FiltroSinEstablecimiento"/>
        /// = solo sin asignar; otro valor = solo ese establecimiento. Pensado para usarse igual en el
        /// cliente (tabla en pantalla) y en el servidor (Excel) para que ambos muestren lo mismo.
        /// </summary>
        public static IEnumerable<PlantelDTO> FiltrarPorEstablecimiento(IEnumerable<PlantelDTO>? planteles, int? establecimientoId)
        {
            if (planteles == null)
            {
                return Enumerable.Empty<PlantelDTO>();
            }

            if (!establecimientoId.HasValue)
            {
                return planteles;
            }

            if (establecimientoId.Value == FiltroSinEstablecimiento)
            {
                return planteles.Where(p => !p.EstablecimientoId.HasValue);
            }

            var id = establecimientoId.Value;
            return planteles.Where(p => p.EstablecimientoId == id);
        }

        /// <summary>Texto corto para mostrar el establecimiento de un registro en listas y typeaheads.</summary>
        public static string DescribirEstablecimiento(Estable? establecimiento, string sinAsignar = "Sin establecimiento")
        {
            if (establecimiento == null)
            {
                return sinAsignar;
            }

            var nombre = (establecimiento.Nombre ?? string.Empty).Trim();
            var ecod = (establecimiento.Ecod ?? string.Empty).Trim();
            if (nombre.Length == 0) return ecod.Length == 0 ? sinAsignar : ecod;
            return ecod.Length == 0 ? nombre : $"{ecod} - {nombre}";
        }

        public static string DescribirEstablecimiento(EstableDTO? establecimiento, string sinAsignar = "Sin establecimiento")
        {
            if (establecimiento == null)
            {
                return sinAsignar;
            }

            var nombre = (establecimiento.Nombre ?? string.Empty).Trim();
            var ecod = (establecimiento.Ecod ?? string.Empty).Trim();
            if (nombre.Length == 0) return ecod.Length == 0 ? sinAsignar : ecod;
            return ecod.Length == 0 ? nombre : $"{ecod} - {nombre}";
        }
    }
}
