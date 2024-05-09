using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using PaginaToros.Client.Pages.Auth;
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
                return await _dbContext.Socios.Include(x => x.Provincia)
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



        public async Task<Socio> Obtener(Expression<Func<Socio, bool>> filtro = null)
        {
            try
            {
                return await _dbContext.Socios.Where(filtro).FirstOrDefaultAsync();
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
                    a = await _dbContext.Socios.Include(x=>x.Provincia).Where(filtro).Skip(skip).ToListAsync();
                }
                else
                {
                    a = await _dbContext.Socios.Include(x => x.Provincia).Skip(skip).ToListAsync();
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
            try
            {
                _dbContext.Socios.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
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
            try
            {
                var socioViejo = _dbContext.Find<Socio>(entidad.Id);
                try { 
                if (socioViejo.Scod != entidad.Scod)
                {
                    var certificados = _dbContext.Certifsemen.Where(x => x.Nrocri == socioViejo.Scod).ToList();
                    foreach (var certificado in certificados)
                    {
                        certificado.Nrocri = entidad.Scod;
                        _dbContext.Update(certificado);
                    }
                    var deseplas = _dbContext.Desepla1s.Where(x => x.Nrocri == socioViejo.Scod).ToList();
                    foreach (var desepla in deseplas)
                    {
                        desepla.Nrocri = entidad.Scod;
                        _dbContext.Update(desepla);
                    }
                    var planteles = _dbContext.Planteles.Where(x => x.Nrocri == socioViejo.Scod).ToList();
                    foreach(var plantel in planteles)
                        {
                        plantel.Nrocri = entidad.Scod;
                        _dbContext.Update(plantel);
                    }
                    var toros = _dbContext.Torosunis.Where(x => x.Criador == socioViejo.Scod).ToList();
                    foreach (var toro in toros)
                    {
                        toro.Criador = entidad.Scod;
                        _dbContext.Update(toro);
                    }
                    var establecimientos = _dbContext.Estables.Where(x => x.Codsoc == socioViejo.Scod).ToList();
                    foreach (var establecimiento in establecimientos)
                        {
                        establecimiento.Codsoc = entidad.Scod;
                        _dbContext.Update(establecimiento);
                    }
                    var resultados = _dbContext.Resin1s.Where(x => x.Scod == socioViejo.Scod).ToList();
                    foreach (var resultado in resultados)
                        {
                        resultado.Scod = entidad.Scod;
                        _dbContext.Update(resultado);
                    }

                }

                _dbContext.Update(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
                }
                catch {
                    return false;
                }
                
            }
            catch
            {
                throw;
            }
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
    }
}
