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

                // Use Skip and Take for paging, and include Socio
                return await _dbContext.Socios.Where(x=>x.Criador=="S").Include(x => x.Provincia)
                                                 .OrderByDescending(t => t.Id)
                                                 .Skip(skip)
                                                 .Take(take)
                                                 .ToListAsync();
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
                return await _dbContext.Socios.Where(x => x.Criador == "S").Where(filtro).FirstOrDefaultAsync();
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
                List<Socio> a;
                if(filtro is not null) { 
                    a = await _dbContext.Socios
                        .Where(x => x.Criador == "S")
                        .Include(x=>x.Provincia).Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    a = await _dbContext.Socios
                        .Where(x => x.Criador == "S")
                        .Include(x => x.Provincia).Skip(skip).ToListAsync();
                }
                if (take == 0)
                {
                    return a.OrderByDescending(t => t.Id).ToList();
                }
                else
                {
                    return a.Take(take).OrderByDescending(t => t.Id).ToList();
                }
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
                await using var tx = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var socio = await _dbContext.Socios.FirstOrDefaultAsync(s => s.Id == socioId);
                    if (socio == null) return false;

                    var scod = socio.Scod ?? string.Empty;

                    // 1) RESIN1 (+ hijos RESIN2/3/4/6/8)
                    //    Busco todas las inspecciones del socio (por código y/o por CodUsu si existe)
                    var resin1s = await _dbContext.Resin1s
                        .Where(r => (r.Scod == scod) || (r.CodUsu == socioId)) // ajustá si tu Resin1 no tiene CodUsu
                        .ToListAsync();

                    foreach (var r1 in resin1s)
                    {
                        // Mismo patrón que ya tenés en tu Eliminar(Resin1)
                        _dbContext.Resin2s.RemoveRange(_dbContext.Resin2s.Where(x => x.Nrores == r1.Nrores));
                        _dbContext.Resin3s.RemoveRange(_dbContext.Resin3s.Where(x => x.Nrores == r1.Nrores));
                        _dbContext.Resin4s.RemoveRange(_dbContext.Resin4s.Where(x => x.Nrores == r1.Nrores));
                        _dbContext.Resin6s.RemoveRange(_dbContext.Resin6s.Where(x => x.Nrores == r1.Nrores));
                        _dbContext.Resin8s.RemoveRange(_dbContext.Resin8s.Where(x => x.Nrores == r1.Nrores));
                    }
                    _dbContext.Resin1s.RemoveRange(resin1s);

                    // 2) DECLARACIONES: Desepla1 (cabecera) + Desepla3 (líneas)
                    var nroDecsDelSocio = await _dbContext.Desepla1s
                        .Where(d => d.Nrocri == scod) // si tenés SocioId, podés sumar OR d.SocioId == socioId
                        .Select(d => d.Nrodec)
                        .ToListAsync();

                    if (nroDecsDelSocio.Count > 0)
                    {
                        _dbContext.Desepla3s.RemoveRange(_dbContext.Desepla3s.Where(l => nroDecsDelSocio.Contains(l.Nrodec)));
                        _dbContext.Desepla1s.RemoveRange(_dbContext.Desepla1s.Where(h => nroDecsDelSocio.Contains(h.Nrodec)));
                    }

                    _dbContext.Planteles.RemoveRange(
                        _dbContext.Planteles.Where(p => p.Nrocri == scod || p.CodUsu == socioId) // usa los campos que existan
                    );

                    // 4) TOROS (por CodUsu y también por código criador)
                    _dbContext.Torosunis.RemoveRange(
                        _dbContext.Torosunis.Where(t => t.CodUsu == socioId || t.Criador == scod)
                    );

                    // 5) CERTIFICADOS (por CodUsu y/o Nrocri)
                    _dbContext.Certifsemen.RemoveRange(
                        _dbContext.Certifsemen.Where(c => c.CodUsu == socioId || c.Nrocri == scod)
                    );

                    // 6) Finalmente, el socio
                    _dbContext.Socios.Remove(socio);

                    await _dbContext.SaveChangesAsync();
                    await tx.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    Console.Error.WriteLine($"Error en EliminarSocioProfundo: {ex}");
                    throw;
                }
            });
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
                List<Socio> a;

                if (expression is not null)
                {
                    a = await _dbContext.Socios
                        .Include(x => x.Provincia)
                        .Where(expression)
                        .OrderByDescending(s => s.Criador == "S") 
                        .ThenByDescending(t => t.Id)     
                        .Skip(skip)
                        .ToListAsync();
                }
                else
                {
                    a = await _dbContext.Socios
                        .Include(x => x.Provincia)
                        .OrderByDescending(s => s.Criador == "S")
                        .ThenByDescending(t => t.Id)
                        .Skip(skip)
                        .ToListAsync();
                }

                if (take == 0)
                {
                    return a; 
                }
                else
                {
                    return a.Take(take).ToList(); 
                }
            }
            catch
            {
                throw;
            }
        }


    }
}
