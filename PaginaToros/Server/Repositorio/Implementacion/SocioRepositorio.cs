using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using PaginaToros.Client.Pages.Auth;
using PaginaToros.Client.Pages.Socios;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PaginaToros.Server.Repositorio.Implementacion
{
    public class SocioRepositorio : ISocioRepositorio
    {
        private readonly hereford_prContext _dbContext;

        public SocioRepositorio(hereford_prContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Socio>> Lista(int skip, int take)
        {

            try
            {
                IQueryable<Socio> query = ApplyCreationOrder(
                    _dbContext.Socios
                        .Where(x => x.Criador == "S")
                        .Include(x => x.Provincia));

                if (skip > 0)
                {
                    query = query.Skip(skip);
                }

                if (take > 0)
                {
                    query = query.Take(take);
                }

                return await query.ToListAsync();
            }
            catch
            {
                throw;
            }
        }


        public async Task<Socio> ObtenerPorId(int id)
        {
            try
            {
                return await _dbContext.Socios.FirstOrDefaultAsync(s => s.Id == id);
            }
            catch
            {
                throw;
            }
        }

        public async Task<Socio> Obtener(Expression<Func<Socio, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Socios
                    .Where(filtro)
                    .FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Socio>> LimitadosFiltrados(int skip, int take, string filtro = null)
        {
            try
            {
                IQueryable<Socio> query = _dbContext.Socios
                    .Where(x => x.Criador == "S")
                    .Include(x => x.Provincia);

                if (filtro is not null)
                {
                    query = query.Where(filtro);
                }

                query = ApplyCreationOrder(query);

                if (skip > 0)
                {
                    query = query.Skip(skip);
                }

                if (take > 0)
                {
                    query = query.Take(take);
                }

                return await query.ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(Socio entidad)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            var socioId = entidad.Id;

            return await strategy.ExecuteAsync(async () =>
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                await using var tx = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    Console.WriteLine($"[EliminarSocio] INICIO socioId={socioId}");

                    var socio = await _dbContext.Socios.FirstOrDefaultAsync(s => s.Id == socioId);
                    if (socio == null)
                    {
                        Console.WriteLine($"[EliminarSocio] Socio no encontrado: {socioId}");
                        return false;
                    }

                    var scod = (socio.Scod ?? string.Empty).Trim();
                    Console.WriteLine($"[EliminarSocio] socioId={socioId} scod='{scod}' nombre='{socio.Nombre}'");

                    // === 1) ESTABLECIMIENTOS DEL SOCIO ===
                    var estabList = await _dbContext.Estables
                        .Where(e => (e.Socio != null && e.Socio.Id == socioId) || e.Codsoc == scod)
                        .ToListAsync();
                    var estabIds = estabList.Select(e => e.Id).ToList();
                    var estabEcods = estabList.Where(e => e.Ecod != null).Select(e => e.Ecod!).ToList();

                    // Hijos de Estable
                    var solici1 = await _dbContext.Solici1s
                        .Where(s => s.Establecimiento != null && estabIds.Contains(s.Establecimiento.Id))
                        .ToListAsync();
                    var transsb = await _dbContext.Transsbs
                        .Where(t => t.Ecod != null && estabEcods.Contains(t.Ecod))
                        .ToListAsync();

                    // === 2) PLANTELES DEL SOCIO ===
                    var planteles = await _dbContext.Planteles
                        .Where(p => (p.Socio != null && p.Socio.Id == socioId) || p.Nrocri == scod)
                        .ToListAsync();
                    var placods = planteles.Where(p => p.Placod != null).Select(p => p.Placod!).ToList();

                    // === 3) DECLARACIONES ===
                    var desepla1 = await _dbContext.Desepla1s.Where(d => d.Nrocri == scod).ToListAsync();
                    var nrodecs = desepla1.Select(d => d.Nrodec).ToList();
                    var desepla2 = await _dbContext.Desepla2s.Where(d => nrodecs.Contains(d.Nrodec)).ToListAsync();
                    var desepla3 = await _dbContext.Desepla3s.Where(d => nrodecs.Contains(d.Nrodec)).ToListAsync();

                    // === 4) RESULTADOS DE INSPECCIÓN ===
                    var resin1 = await _dbContext.Resin1s
                        .Where(r => r.Scod == scod || (r.Establecimiento != null && estabIds.Contains(r.Establecimiento.Id)))
                        .ToListAsync();
                    var nroresList = resin1.Where(r => r.Nrores != null).Select(r => r.Nrores!).ToList();

                    var resin2 = await _dbContext.Resin2s.Where(r => nroresList.Contains(r.Nrores)).ToListAsync();
                    var resin3 = await _dbContext.Resin3s.Where(r => nroresList.Contains(r.Nrores)).ToListAsync();
                    var resin4 = await _dbContext.Resin4s.Where(r => nroresList.Contains(r.Nrores)).ToListAsync();
                    var resin6 = await _dbContext.Resin6s.Where(r => nroresList.Contains(r.Nrores)).ToListAsync();
                    var resin8 = await _dbContext.Resin8s
                        .Where(r => (r.Nrores != null && nroresList.Contains(r.Nrores))
                                 || (r.Nropla != null && placods.Contains(r.Nropla)))
                        .ToListAsync();

                    // === 5) TOROS ===
                    var toros = await _dbContext.Torosunis
                        .Where(t => t.Socio != null && t.Socio.Id == socioId /* || t.Criador == scod */)
                        .ToListAsync();

                    // === 6) TRANSFERENCIAS / CERTIFICADOS ===
                    var transan = await _dbContext.Transans
                        .Where(t => t.Sven == scod || t.Scom == scod)
                        .ToListAsync();
                    var transem = await _dbContext.Transems
                        .Where(t => t.Scod == scod || t.Nrocri == scod)
                        .ToListAsync();
                    var certifseman = await _dbContext.Certifsemen
                        .Where(c => c.Scod == scod || c.Nrocri == scod)
                        .ToListAsync();

                    // === LOG de PREVIEW (cuánto se va a borrar) ===
                    LogPreview("Estables", estabList.Select(x => x.Id));
                    LogPreview("Solici1", solici1.Select(x => x.Id));
                    LogPreview("Transsb", transsb.Select(x => x.Id));
                    LogPreview("Planteles", planteles.Select(x => x.Id));
                    LogPreview("Desepla1", desepla1.Select(x => x.Id));
                    LogPreview("Desepla2", desepla2.Select(x => x.Id));
                    LogPreview("Desepla3", desepla3.Select(x => x.Id));
                    LogPreview("Resin1", resin1.Select(x => x.Id));
                    LogPreview("Resin2", resin2.Select(x => x.Id));
                    LogPreview("Resin3", resin3.Select(x => x.Id));
                    LogPreview("Resin4", resin4.Select(x => x.Id));
                    LogPreview("Resin6", resin6.Select(x => x.Id));
                    LogPreview("Resin8", resin8.Select(x => x.Id));
                    LogPreview("Torosuni", toros.Select(x => x.Id));
                    LogPreview("Transan", transan.Select(x => x.Id));
                    LogPreview("Certifseman", certifseman.Select(x => x.Id));
                    Console.WriteLine($"[EliminarSocio] Socio a borrar: socioId={socio.Id} scod='{scod}'");

                    // ===== ORDEN DE BORRADO =====
                    _dbContext.Resin6s.RemoveRange(resin6);
                    _dbContext.Resin4s.RemoveRange(resin4);
                    _dbContext.Resin3s.RemoveRange(resin3);
                    _dbContext.Resin2s.RemoveRange(resin2);
                    _dbContext.Resin8s.RemoveRange(resin8);

                    _dbContext.Desepla3s.RemoveRange(desepla3);
                    _dbContext.Desepla2s.RemoveRange(desepla2);

                    _dbContext.Solici1s.RemoveRange(solici1);
                    _dbContext.Transsbs.RemoveRange(transsb);

                    _dbContext.Resin1s.RemoveRange(resin1);
                    _dbContext.Desepla1s.RemoveRange(desepla1);
                    _dbContext.Torosunis.RemoveRange(toros);
                    _dbContext.Transans.RemoveRange(transan);
                    _dbContext.Transems.RemoveRange(transem);
                    _dbContext.Certifsemen.RemoveRange(certifseman);

                    _dbContext.Planteles.RemoveRange(planteles);
                    _dbContext.Estables.RemoveRange(estabList);

                    _dbContext.Socios.Remove(socio);

                    var affected = await _dbContext.SaveChangesAsync();
                    await tx.CommitAsync();

                    sw.Stop();
                    Console.WriteLine($"[EliminarSocio] FIN socioId={socioId} filas_afectadas={affected} tiempo={sw.ElapsedMilliseconds}ms");

                    return true;
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    Console.WriteLine($"[EliminarSocio][ERROR] socioId={socioId} - {ex.Message}");
                    throw;
                }
            });

            // ——— Helpers locales ———
            void LogPreview(string label, IEnumerable<int> ids)
            {
                var list = ids?.ToList() ?? new List<int>();
                Console.WriteLine($"[EliminarSocio][Preview] {label}: {list.Count}");
                if (list.Count > 0)
                {
                    var head = string.Join(",", list.Take(20));
                    var suffix = list.Count > 20 ? "..." : "";
                    Console.WriteLine($"[EliminarSocio][Preview] {label} IDs: {head}{suffix}");
                }
            }
        }


        public async Task<Socio> Crear(Socio entidad)
        {
            try
            {
                _dbContext.Set<Socio>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(Socio entidad)
        {
            if (entidad == null) throw new ArgumentNullException(nameof(entidad));

            var strategy = _dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var trx = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var socioViejo = await _dbContext.Socios
                        .FirstOrDefaultAsync(x => x.Id == entidad.Id);

                    if (socioViejo == null)
                        throw new InvalidOperationException("Socio no encontrado");

                    var scodViejo = socioViejo.Scod?.Trim() ?? "";
                    var scodNuevo = entidad.Scod?.Trim() ?? "";

                    // Si cambió el Scod, actualizo referencias
                    if (!string.Equals(scodViejo, scodNuevo, StringComparison.Ordinal))
                    {
                        var certificados = await _dbContext.Certifsemen
                            .Where(x => x.Nrocri == scodViejo).ToListAsync();
                        foreach (var c in certificados) c.Nrocri = scodNuevo;

                        var deseplas = await _dbContext.Desepla1s
                            .Where(x => x.Nrocri == scodViejo).ToListAsync();
                        foreach (var d in deseplas) d.Nrocri = scodNuevo;

                        var planteles = await _dbContext.Planteles
                            .Where(x => x.Nrocri == scodViejo).ToListAsync();
                        foreach (var p in planteles) p.Nrocri = scodNuevo;

                        var toros = await _dbContext.Torosunis
                            .Where(x => x.Criador == scodViejo).ToListAsync();
                        foreach (var t in toros) t.Criador = scodNuevo;

                        var establecimientos = await _dbContext.Estables
                            .Where(x => x.Codsoc == scodViejo).ToListAsync();
                        foreach (var e in establecimientos) e.Codsoc = scodNuevo;

                        var resultados = await _dbContext.Resin1s
                            .Where(x => x.Scod == scodViejo).ToListAsync();
                        foreach (var r in resultados) r.Scod = scodNuevo;
                    }

                    // Copio campos a la instancia trackeada (evita doble tracking)
                    _dbContext.Entry(socioViejo).CurrentValues.SetValues(entidad);

                    await _dbContext.SaveChangesAsync();
                    await trx.CommitAsync();
                    return true;
                }
                catch
                {
                    await trx.RollbackAsync();
                    throw;
                }
            });
        }

        public async Task<IQueryable<Socio>> Consultar(Expression<Func<Socio, bool>> filtro = null)
        {
            IQueryable<Socio> queryEntidad = filtro == null
                    ? _dbContext.Socios.Take(30)  // Apply Take(30) before filtering
                    : _dbContext.Socios.Where(filtro);

            return queryEntidad;
        }

        public async Task<int> CantidadTotal()
        {
            try
            {
                return _dbContext.Socios.Count();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Socio>> LimitadosFiltradosTodos(int skip, int take, string expression = null)
        {
            try
            {
                IQueryable<Socio> query = _dbContext.Socios
                    .AsNoTracking()
                    .Include(x => x.Provincia);

                if (expression is not null)
                {
                    query = query.Where(expression);
                }

                query = ApplyCreationOrder(query);

                if (skip > 0)
                {
                    query = query.Skip(skip);
                }

                if (take > 0)
                {
                    query = query.Take(take);
                }

                return await query.ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<(List<Socio> Items, int TotalCount)> SearchPagedAsync(
            int skip,
            int take,
            string? searchText = null,
            IReadOnlyCollection<int>? allowedSocioIds = null)
        {
            try
            {
                if (take <= 0)
                {
                    take = 15;
                }

                IQueryable<Socio> query = _dbContext.Socios
                    .AsNoTracking()
                    .Where(x => x.Criador == "S")
                    .Include(x => x.Provincia);

                if (allowedSocioIds != null && allowedSocioIds.Count > 0)
                {
                    query = query.Where(x => allowedSocioIds.Contains(x.Id));
                }

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    query = ApplySearchFilter(query, searchText);
                }

                query = ApplyCreationOrder(query);

                var totalCount = await query.CountAsync();
                var items = await query
                    .Skip(Math.Max(0, skip))
                    .Take(take)
                    .ToListAsync();

                return (items, totalCount);
            }
            catch
            {
                throw;
            }
        }

        private static IOrderedQueryable<Socio> ApplyCreationOrder(IQueryable<Socio> query)
            => query
                .OrderByDescending(x => x.Fecing.HasValue)
                .ThenByDescending(x => x.Fecing)
                .ThenByDescending(x => x.FchUsu.HasValue)
                .ThenByDescending(x => x.FchUsu)
                .ThenByDescending(x => x.Id);

        private static IQueryable<Socio> ApplySearchFilter(IQueryable<Socio> query, string searchText)
        {
            var terms = searchText
                .Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            foreach (var term in terms)
            {
                var currentTerm = term;
                query = query.Where(s =>
                    (s.Prenom != null && EF.Functions.Like(s.Prenom, $"%{currentTerm}%")) ||
                    (s.Nombre != null && EF.Functions.Like(s.Nombre, $"%{currentTerm}%")) ||
                    (s.Posnom != null && EF.Functions.Like(s.Posnom, $"%{currentTerm}%")) ||
                    (s.Mail != null && EF.Functions.Like(s.Mail, $"%{currentTerm}%")) ||
                    (s.Scod != null && EF.Functions.Like(s.Scod, $"%{currentTerm}%")) ||
                    (s.Codpos2 != null && EF.Functions.Like(s.Codpos2, $"%{currentTerm}%")) ||
                    (s.Users != null && s.Users.Any(u =>
                        (u.Names != null && EF.Functions.Like(u.Names, $"%{currentTerm}%")) ||
                        (u.LastNames != null && EF.Functions.Like(u.LastNames, $"%{currentTerm}%")))));
            }

            return query;
        }


    }
}
