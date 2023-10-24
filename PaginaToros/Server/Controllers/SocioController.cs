using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Client.Pages.Socios;
using PaginaToros.Server.Context;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocioController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Socio> oRespuesta = new Respuesta<Socio>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Socios
                        .Where(x => x.Id == id)
                        .First();
                    oRespuesta.Exito = 1;
                    oRespuesta.List = lst;
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }

        [HttpGet]
        public IActionResult Get()
        {
            Respuesta<List<Socio>> oRespuesta = new Respuesta<List<Socio>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    var lst = db.Socios.ToList();
                    oRespuesta.Exito = 1;
                    oRespuesta.List = lst;
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }

        [HttpGet("Codigo/{nro}")]
        public IActionResult GetByCod(string nro)
        {
            Respuesta<Socio> oRespuesta = new Respuesta<Socio>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Socios
                    .Where(x => x.Scod == nro).First();
                    oRespuesta.Exito = 1;
                    oRespuesta.List = lst;
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }

        [HttpPost]
        public IActionResult Add(Socio model)
        {
            Respuesta<List<Socio>> oRespuesta = new Respuesta<List<Socio>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Socio oSocio = new Socio();
                    var socioViejo = db.Socios.OrderByDescending(x => x.Id).First();
                    oSocio.Scod = (Int32.Parse(socioViejo.Scod)+1).ToString("D5");
                    oSocio.Nombre = model.Nombre;
                    oSocio.Direcc1 = model.Direcc1;
                    oSocio.Telefo1 = model.Telefo1;
                    oSocio.Telefo2 = model.Telefo2;
                    oSocio.Locali1 = model.Locali1;
                    oSocio.Codpos1 = model.Codpos1;
                    oSocio.Codpro1 = model.Codpro1;
                    oSocio.Telefo2 = model.Telefo2;
                    oSocio.Criador = model.Criador;
                    oSocio.Mail = model.Mail;
                    oSocio.Fecing = model.Fecing;
                    oSocio.Placod = model.Placod;
                    db.Socios.Add(oSocio);
                    db.SaveChanges();
                    oRespuesta.Exito = 1;
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }

        [HttpPut]
        public IActionResult Edit(Socio model)
        {
            Respuesta<List<Socio>> oRespuesta = new Respuesta<List<Socio>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Socio oSocio = db.Socios.Find(model.Id,model.Scod);
                    oSocio.Nombre = model.Nombre;
                    oSocio.Direcc1 = model.Direcc1;
                    oSocio.Telefo1 = model.Telefo1;
                    oSocio.Telefo2 = model.Telefo2;
                    oSocio.Locali1 = model.Locali1;
                    oSocio.Codpos1 = model.Codpos1;
                    oSocio.Codpro1 = model.Codpro1;
                    oSocio.Telefo2 = model.Telefo2;
                    oSocio.Mail = model.Mail;
                    oSocio.Criador = model.Criador;
                    oSocio.Fecing = model.Fecing;
                    oSocio.Placod = model.Placod;
                    db.Entry(oSocio).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                    oRespuesta.Exito = 1;
                    
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }
        [HttpDelete("{id}/{cod}")]
        public IActionResult Delete(int Id,string cod)
        {
            Respuesta<List<Socio>> oRespuesta = new Respuesta<List<Socio>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Socio oSocio = db.Socios.Find(Id,cod);
                    db.Remove(oSocio);
                    //var dbToros = db.Toros.Where(x => x.IdEst == Id);
                    //foreach (Toro oElement in dbToros)
                    //{
                    //    db.Remove(oElement);
                    //}
                    db.SaveChanges();
                    oRespuesta.Exito = 1;
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }
    }
}
