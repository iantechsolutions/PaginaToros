using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    /// <summary>
    /// Detecta y consolida los planteles duplicados que dejo el bucle de updatePlantel en
    /// AddReporte.razor: con el tilde "Crear nuevo plantel" prendido, cada vuelta del circuito
    /// derivaba un codigo nuevo del plantel que acababa de crear y creaba otro plantel.
    /// Ejemplo real: 5RJ01 -> 6RJ01 -> 6RJ011 -> 6J011 -> 6J0111 -> 60111.
    /// </summary>
    public class PlantelDiagnosticoRepositorio : IPlantelDiagnosticoRepositorio
    {
        private readonly hereford_prContext _db;
        private readonly ILogger<PlantelDiagnosticoRepositorio> _logger;

        public PlantelDiagnosticoRepositorio(hereford_prContext db, ILogger<PlantelDiagnosticoRepositorio> logger)
        {
            _db = db;
            _logger = logger;
        }

        private static string Norm(string? valor) => (valor ?? string.Empty).Trim().ToUpperInvariant();

        /// <summary>
        /// Replica el recorte de codigo de AddReporte.DerivarTailCodigo. Tiene que quedar igual
        /// que el del cliente: es lo que permite reconocer que un plantel nacio de otro.
        /// </summary>
        private static string DerivarTail(string? codigo)
        {
            var c = (codigo ?? string.Empty).Trim();
            if (c.Length == 0) return string.Empty;
            if (c.Length > 4 && char.IsDigit(c[0])) c = c.Substring(1);
            if (c.Length > 4) c = c.Substring(c.Length - 4);
            return c;
        }

        private static string DerivarCodigo(string? placodBase, string anioex)
        {
            var tail = DerivarTail(placodBase);
            var anio = (anioex ?? string.Empty).Trim();
            if (tail.Length == 0 || anio.Length == 0) return string.Empty;
            return $"{anio[anio.Length - 1]}{tail}";
        }

        /// <summary>
        /// true si <paramref name="hijo"/> pudo salir de <paramref name="padre"/>: o es exactamente
        /// el codigo derivado, o es ese codigo con el sufijo numerico que agregaba el bucle
        /// anticolision cuando el derivado ya existia.
        /// </summary>
        private static bool EsDerivado(string? hijo, string? padre, string anioex)
        {
            var esperado = DerivarCodigo(padre, anioex);
            if (esperado.Length == 0) return false;

            var h = Norm(hijo);
            var e = Norm(esperado);
            if (h.Length == 0 || h == Norm(padre)) return false;
            if (h == e) return true;

            return h.StartsWith(e, StringComparison.Ordinal)
                   && h.Length > e.Length
                   && h.Substring(e.Length).All(char.IsDigit);
        }

        public async Task<List<PlantelDuplicadoGrupoDTO>> ListarGruposDuplicadosAsync()
        {
            var planteles = await _db.Planteles.AsNoTracking().ToListAsync();

            var grupos = planteles
                .Where(p => !string.IsNullOrWhiteSpace(p.Nrocri) && !string.IsNullOrWhiteSpace(p.Anioex))
                .GroupBy(p => new { Nrocri = Norm(p.Nrocri), Anioex = Norm(p.Anioex) })
                .Where(g => g.Count() > 1)
                .ToList();

            if (grupos.Count == 0) return new List<PlantelDuplicadoGrupoDTO>();

            var codigos = grupos.SelectMany(g => g).Select(p => p.Placod ?? string.Empty).ToList();
            var codigosNorm = codigos.Select(Norm).ToHashSet();

            // Referencias en bloque: cinco tablas apuntan al plantel por texto y ninguna tiene FK.
            var resin1 = await _db.Resin1s.AsNoTracking()
                .Where(x => x.Nropla != null && codigos.Contains(x.Nropla))
                .Select(x => new { x.Id, Cod = x.Nropla, x.Nrores, x.Freali }).ToListAsync();
            var resin8 = await _db.Resin8s.AsNoTracking()
                .Where(x => x.Nropla != null && codigos.Contains(x.Nropla))
                .Select(x => new { x.Id, Cod = x.Nropla, x.Nrores }).ToListAsync();
            var desepla1 = await _db.Desepla1s.AsNoTracking()
                .Where(x => x.Nroplan != null && codigos.Contains(x.Nroplan))
                .Select(x => new { x.Id, Cod = x.Nroplan }).ToListAsync();
            var remansol = await _db.Remansols.AsNoTracking()
                .Where(x => x.Nroplan != null && codigos.Contains(x.Nroplan))
                .Select(x => new { x.Id, Cod = x.Nroplan }).ToListAsync();
            var retenidas = await _db.Retenidas.AsNoTracking()
                .Where(x => x.Nroplan != null && codigos.Contains(x.Nroplan))
                .Select(x => new { x.Id, Cod = x.Nroplan }).ToListAsync();

            var socios = await _db.Socios.AsNoTracking()
                .Select(s => new { s.Scod, s.Nombre }).ToListAsync();
            var nombrePorScod = socios
                .Where(s => !string.IsNullOrWhiteSpace(s.Scod))
                .GroupBy(s => Norm(s.Scod))
                .ToDictionary(g => g.Key, g => g.First().Nombre);

            // Insumos del recalculo, acotados a los reportes de estos planteles.
            var nroresDeGrupos = resin1.Where(r => !string.IsNullOrWhiteSpace(r.Nrores))
                                       .Select(r => r.Nrores!).Distinct().ToList();
            var historicos = nroresDeGrupos.Count == 0
                ? new List<Resin2>()
                : await _db.Resin2s.AsNoTracking()
                    .Where(h => h.Nrores != null && nroresDeGrupos.Contains(h.Nrores)).ToListAsync();
            var movimientos = nroresDeGrupos.Count == 0
                ? new List<Resin3>()
                : await _db.Resin3s.AsNoTracking()
                    .Where(m => m.Nrores != null && nroresDeGrupos.Contains(m.Nrores)).ToListAsync();

            var salida = new List<PlantelDuplicadoGrupoDTO>();

            foreach (var grupo in grupos)
            {
                var anioex = grupo.Key.Anioex;
                var items = grupo.OrderBy(p => p.Id).ToList();

                var dto = new PlantelDuplicadoGrupoDTO
                {
                    Nrocri = items.First().Nrocri?.Trim() ?? grupo.Key.Nrocri,
                    Anioex = items.First().Anioex?.Trim() ?? anioex,
                    SocioNombre = nombrePorScod.TryGetValue(grupo.Key.Nrocri, out var nombre) ? nombre : null
                };

                foreach (var p in items)
                {
                    var cod = p.Placod ?? string.Empty;
                    var refs = new PlantelReferenciasDTO
                    {
                        Resin1 = resin1.Count(x => Norm(x.Cod) == Norm(cod)),
                        Resin8 = resin8.Count(x => Norm(x.Cod) == Norm(cod)),
                        Desepla1 = desepla1.Count(x => Norm(x.Cod) == Norm(cod)),
                        Remansol = remansol.Count(x => Norm(x.Cod) == Norm(cod)),
                        Retenidas = retenidas.Count(x => Norm(x.Cod) == Norm(cod))
                    };

                    foreach (var r in resin1.Where(x => Norm(x.Cod) == Norm(cod)))
                        refs.Detalle.Add($"Reporte de inspección N° {r.Nrores} (RESIN1 #{r.Id})");
                    foreach (var r in resin8.Where(x => Norm(x.Cod) == Norm(cod)))
                        refs.Detalle.Add($"Causal de rechazo del reporte {r.Nrores} (RESIN8 #{r.Id})");
                    foreach (var r in desepla1.Where(x => Norm(x.Cod) == Norm(cod)))
                        refs.Detalle.Add($"Declaración de servicio (DESEPLA1 #{r.Id})");
                    foreach (var r in remansol.Where(x => Norm(x.Cod) == Norm(cod)))
                        refs.Detalle.Add($"Remanente de solicitud (REMANSOL #{r.Id})");
                    foreach (var r in retenidas.Where(x => Norm(x.Cod) == Norm(cod)))
                        refs.Detalle.Add($"Retenida (retenidas #{r.Id})");

                    var padre = items.FirstOrDefault(o => o.Id != p.Id && EsDerivado(cod, o.Placod, anioex));

                    dto.Planteles.Add(new PlantelDuplicadoDTO
                    {
                        Id = p.Id,
                        Placod = cod,
                        Anioex = p.Anioex,
                        Nrocri = p.Nrocri,
                        Fecing = p.Fecing,
                        Totales = new PlantelTotalesDTO
                        {
                            Varede = p.Varede ?? 0,
                            Vqcsrd = p.Vqcsrd ?? 0,
                            Vqssrd = p.Vqssrd ?? 0,
                            Varepr = p.Varepr ?? 0,
                            Vqcsrp = p.Vqcsrp ?? 0,
                            Vqssrp = p.Vqssrp ?? 0
                        },
                        Referencias = refs,
                        DerivadoDeOtroDelGrupo = padre != null,
                        DerivadoDe = padre?.Placod
                    });
                }

                // Criterio: el que sobrevive es la RAIZ de la cascada, o sea el unico cuyo codigo no
                // se deriva de otro del grupo. Su codigo es el que salio del plantel del año
                // anterior; los demas son los que fue inventando el bucle anticolision.
                var raices = dto.Planteles.Where(p => !p.DerivadoDeOtroDelGrupo).ToList();
                if (raices.Count == 1)
                {
                    dto.PlacodSugerido = raices[0].Placod;
                    dto.MotivoSugerencia = "Es el origen de la cascada: su código no deriva de ningún otro del grupo, " +
                                           "sino del plantel del año anterior.";
                }
                else
                {
                    var masViejo = dto.Planteles.OrderBy(p => p.Id).First();
                    dto.PlacodSugerido = masViejo.Placod;
                    dto.MotivoSugerencia = "No se detectó una cascada clara. Se sugiere el más antiguo (id menor), " +
                                           "pero conviene revisarlo a mano.";
                    dto.Advertencias.Add(raices.Count == 0
                        ? "Todos los planteles del grupo derivan de otro: la cadena es circular. Revisar a mano."
                        : $"Hay {raices.Count} planteles que no derivan de ningún otro; pueden ser duplicados legítimos y no una cascada.");
                }

                foreach (var p in dto.Planteles)
                    p.EsSugerido = Norm(p.Placod) == Norm(dto.PlacodSugerido);

                // Recalculo: existencia anterior (RESIN2) - salidas + entradas (RESIN3), tomando el
                // reporte mas reciente que apunte a cualquier plantel del grupo.
                var codigosGrupo = dto.Planteles.Select(p => Norm(p.Placod)).ToHashSet();
                var reportes = resin1.Where(r => codigosGrupo.Contains(Norm(r.Cod)))
                                     .OrderByDescending(r => r.Freali ?? DateTime.MinValue)
                                     .ThenByDescending(r => r.Id)
                                     .ToList();

                if (reportes.Count == 0)
                {
                    dto.Advertencias.Add("Ningún reporte de inspección apunta a este grupo, así que no se pueden recalcular los totales.");
                }
                else
                {
                    if (reportes.Count > 1)
                    {
                        dto.Advertencias.Add($"Hay {reportes.Count} reportes apuntando a este grupo. Se recalcula con el más reciente; " +
                                             "si las inspecciones son encadenadas, verificar el resultado.");
                    }

                    var reporte = reportes.First();
                    var hist = historicos.Where(h => Norm(h.Nrores) == Norm(reporte.Nrores))
                                         .OrderBy(h => h.Id).FirstOrDefault();

                    if (hist == null)
                    {
                        dto.Advertencias.Add($"El reporte {reporte.Nrores} no tiene existencia anterior (RESIN2) guardada: no se pueden recalcular los totales.");
                    }
                    else
                    {
                        if (historicos.Count(h => Norm(h.Nrores) == Norm(reporte.Nrores)) > 1)
                        {
                            dto.Advertencias.Add($"El reporte {reporte.Nrores} tiene más de una existencia anterior guardada. Se usa la original (id menor).");
                        }

                        var movs = movimientos.Where(m => Norm(m.Nrores) == Norm(reporte.Nrores)).ToList();
                        var entradas = movs.Where(m => Norm(m.Tipmov) != "S").ToList();
                        var salidas = movs.Where(m => Norm(m.Tipmov) == "S").ToList();

                        dto.TotalesRecalculados = new PlantelTotalesDTO
                        {
                            Varede = hist.Ea1 - salidas.Sum(m => m.Rdvac ?? 0) + entradas.Sum(m => m.Rdvac ?? 0),
                            Vqcsrd = (hist.Ea2 ?? 0) - salidas.Sum(m => m.Rdvaqcs ?? 0) + entradas.Sum(m => m.Rdvaqcs ?? 0),
                            Vqssrd = (hist.Ea3 ?? 0) - salidas.Sum(m => m.Rdvaqss ?? 0) + entradas.Sum(m => m.Rdvaqss ?? 0),
                            Varepr = (hist.Ea4 ?? 0) - salidas.Sum(m => m.Rpvac ?? 0) + entradas.Sum(m => m.Rpvac ?? 0),
                            Vqcsrp = (hist.Ea5 ?? 0) - salidas.Sum(m => m.Rpvaqcs ?? 0) + entradas.Sum(m => m.Rpvaqcs ?? 0),
                            Vqssrp = (hist.Ea6 ?? 0) - salidas.Sum(m => m.Rpvaqss ?? 0) + entradas.Sum(m => m.Rpvaqss ?? 0)
                        };

                        var fecha = reporte.Freali?.ToString("dd/MM/yyyy") ?? "sin fecha";
                        dto.OrigenTotalesRecalculados =
                            $"Reporte {reporte.Nrores} del {fecha}: existencia anterior + {entradas.Count} entrada(s) - {salidas.Count} salida(s).";

                        if (dto.TotalesRecalculados.TieneNegativos)
                        {
                            dto.Advertencias.Add("Los totales recalculados dan negativo. Eso indica que la existencia anterior guardada " +
                                                 "o los movimientos están mal cargados: revisar el reporte antes de grabar.");
                        }
                    }
                }

                salida.Add(dto);
            }

            return salida
                .OrderByDescending(g => g.Planteles.Count)
                .ThenBy(g => g.SocioNombre)
                .ToList();
        }

        public async Task<ConsolidarPlantelesResultado> ConsolidarAsync(ConsolidarPlantelesRequest request)
        {
            var res = new ConsolidarPlantelesResultado();

            var nrocri = Norm(request.Nrocri);
            var anioex = Norm(request.Anioex);
            var sobrevivienteCod = Norm(request.PlacodSobreviviente);

            var grupo = await _db.Planteles
                .Where(p => p.Nrocri != null && p.Anioex != null)
                .ToListAsync();

            grupo = grupo.Where(p => Norm(p.Nrocri) == nrocri && Norm(p.Anioex) == anioex).ToList();

            if (grupo.Count == 0)
            {
                res.Errores.Add($"No hay planteles para el socio {request.Nrocri} en {request.Anioex}.");
                return res;
            }

            if (grupo.Count == 1)
            {
                res.Errores.Add("El grupo ya tiene un solo plantel: no hay nada para consolidar.");
                return res;
            }

            var sobreviviente = grupo.FirstOrDefault(p => Norm(p.Placod) == sobrevivienteCod);
            if (sobreviviente == null)
            {
                res.Errores.Add($"El plantel elegido ({request.PlacodSobreviviente}) no pertenece a este grupo.");
                return res;
            }

            var aEliminarCods = request.PlacodsAEliminar.Select(Norm).Where(c => c.Length > 0).ToHashSet();

            if (aEliminarCods.Contains(sobrevivienteCod))
            {
                res.Errores.Add("El plantel elegido para sobrevivir no puede estar además en la lista de borrado.");
                return res;
            }

            var aEliminar = grupo.Where(p => aEliminarCods.Contains(Norm(p.Placod))).ToList();
            var desconocidos = aEliminarCods.Except(grupo.Select(p => Norm(p.Placod))).ToList();
            if (desconocidos.Count > 0)
            {
                res.Errores.Add($"Estos códigos no pertenecen al grupo: {string.Join(", ", desconocidos)}.");
                return res;
            }

            if (!request.RepuntarReferencias && !request.RecalcularTotales && !request.EliminarSobrantes)
            {
                res.Errores.Add("No se seleccionó ninguna acción.");
                return res;
            }

            if (request.EliminarSobrantes && aEliminar.Count == 0)
            {
                res.Advertencias.Add("No se marcó ningún plantel para eliminar.");
            }

            var codsAEliminar = aEliminar.Select(p => p.Placod ?? string.Empty).ToList();
            var destino = sobreviviente.Placod ?? string.Empty;

            // --- Referencias que hoy apuntan a los sobrantes ---
            var refResin1 = await _db.Resin1s.Where(x => x.Nropla != null && codsAEliminar.Contains(x.Nropla)).ToListAsync();
            var refResin8 = await _db.Resin8s.Where(x => x.Nropla != null && codsAEliminar.Contains(x.Nropla)).ToListAsync();
            var refDesepla1 = await _db.Desepla1s.Where(x => x.Nroplan != null && codsAEliminar.Contains(x.Nroplan)).ToListAsync();
            var refRemansol = await _db.Remansols.Where(x => x.Nroplan != null && codsAEliminar.Contains(x.Nroplan)).ToListAsync();
            var refRetenidas = await _db.Retenidas.Where(x => x.Nroplan != null && codsAEliminar.Contains(x.Nroplan)).ToListAsync();

            var totalRefs = refResin1.Count + refResin8.Count + refDesepla1.Count + refRemansol.Count + refRetenidas.Count;

            if (request.EliminarSobrantes && !request.RepuntarReferencias && totalRefs > 0)
            {
                res.Errores.Add($"Hay {totalRefs} registro(s) apuntando a los planteles marcados para borrar. " +
                                "Activá el repunte de referencias o no se puede eliminar sin dejar huérfanos.");
                return res;
            }

            if (request.RepuntarReferencias)
            {
                if (refResin1.Count > 0) res.Acciones.Add($"Repuntar {refResin1.Count} reporte(s) de inspección (RESIN1) a {destino}.");
                if (refResin8.Count > 0) res.Acciones.Add($"Repuntar {refResin8.Count} causal(es) de rechazo (RESIN8) a {destino}.");
                if (refDesepla1.Count > 0) res.Acciones.Add($"Repuntar {refDesepla1.Count} declaración(es) de servicio (DESEPLA1) a {destino}.");
                if (refRemansol.Count > 0) res.Acciones.Add($"Repuntar {refRemansol.Count} remanente(s) de solicitud (REMANSOL) a {destino}.");
                if (refRetenidas.Count > 0) res.Acciones.Add($"Repuntar {refRetenidas.Count} retenida(s) a {destino}.");
                if (totalRefs == 0) res.Acciones.Add("No hay referencias que repuntar.");
            }

            // --- Recalculo de totales ---
            PlantelTotalesDTO? nuevosTotales = null;
            if (request.RecalcularTotales)
            {
                var diagnostico = (await ListarGruposDuplicadosAsync())
                    .FirstOrDefault(g => Norm(g.Nrocri) == nrocri && Norm(g.Anioex) == anioex);

                nuevosTotales = diagnostico?.TotalesRecalculados;

                if (nuevosTotales == null)
                {
                    res.Errores.Add("No se pudieron recalcular los totales (falta el reporte o su existencia anterior). " +
                                    "Desmarcá el recálculo si igual querés consolidar.");
                    return res;
                }

                if (nuevosTotales.TieneNegativos && !request.AceptarTotalesNegativos)
                {
                    res.Errores.Add("Los totales recalculados dan negativo. No se graban salvo que lo aceptes explícitamente: " +
                                    "una categoría con cantidad negativa casi siempre significa que la existencia anterior " +
                                    "o los movimientos del reporte están mal.");
                    return res;
                }

                res.Acciones.Add($"Recalcular totales de {destino}: " +
                                 $"PR {nuevosTotales.Varede}/{nuevosTotales.Vqcsrd}/{nuevosTotales.Vqssrd} · " +
                                 $"VIP {nuevosTotales.Varepr}/{nuevosTotales.Vqcsrp}/{nuevosTotales.Vqssrp} " +
                                 $"(hoy: PR {sobreviviente.Varede ?? 0}/{sobreviviente.Vqcsrd ?? 0}/{sobreviviente.Vqssrd ?? 0} · " +
                                 $"VIP {sobreviviente.Varepr ?? 0}/{sobreviviente.Vqcsrp ?? 0}/{sobreviviente.Vqssrp ?? 0}).");
            }

            if (request.EliminarSobrantes && aEliminar.Count > 0)
            {
                res.Acciones.Add($"Eliminar {aEliminar.Count} plantel(es): {string.Join(", ", aEliminar.Select(p => p.Placod))}.");
            }

            if (!request.Aplicar)
            {
                res.Advertencias.Add("Simulación: no se modificó nada.");
                return res;
            }

            // --- Aplicar, todo o nada ---
            await using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                if (request.RepuntarReferencias)
                {
                    foreach (var x in refResin1) x.Nropla = destino;
                    foreach (var x in refResin8) x.Nropla = destino;
                    foreach (var x in refDesepla1) x.Nroplan = destino;
                    foreach (var x in refRemansol) x.Nroplan = destino;
                    foreach (var x in refRetenidas) x.Nroplan = destino;
                }

                if (request.RecalcularTotales && nuevosTotales != null)
                {
                    sobreviviente.Varede = nuevosTotales.Varede;
                    sobreviviente.Vqcsrd = nuevosTotales.Vqcsrd;
                    sobreviviente.Vqssrd = nuevosTotales.Vqssrd;
                    sobreviviente.Varepr = nuevosTotales.Varepr;
                    sobreviviente.Vqcsrp = nuevosTotales.Vqcsrp;
                    sobreviviente.Vqssrp = nuevosTotales.Vqssrp;
                }

                if (request.EliminarSobrantes && aEliminar.Count > 0)
                {
                    _db.Planteles.RemoveRange(aEliminar);
                }

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                res.Aplicado = true;
                _logger.LogWarning("Consolidacion de planteles aplicada. Socio {Nrocri} año {Anioex}. Sobrevive {Destino}. Acciones: {Acciones}",
                    request.Nrocri, request.Anioex, destino, string.Join(" | ", res.Acciones));
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                res.Aplicado = false;
                res.Errores.Add($"No se aplicó ningún cambio: {ex.Message}");
                _logger.LogError(ex, "Error consolidando planteles del socio {Nrocri} año {Anioex}", request.Nrocri, request.Anioex);
            }

            return res;
        }
    }
}
