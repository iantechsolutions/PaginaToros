using Microsoft.AspNetCore.Mvc;
using PaginaToros.Client.Pages.Socios;
using PaginaToros.Server.Context;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Request;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstablecimientoController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Estable> oRespuesta = new Respuesta<Estable>();
            
            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Estables
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
            Respuesta<List<Estable>> oRespuesta = new Respuesta<List<Estable>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    var lst = db.Estables.ToList();
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
            Respuesta<Estable> oRespuesta = new Respuesta<Estable>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Estables
                    .Where(x => x.Ecod == nro).First();
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
        public IActionResult Add(Estable model)
        {
            Respuesta<List<Estable>> oRespuesta = new Respuesta<List<Estable>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Estable oEstablecimiento = new Estable();
                    var estviejo = db.Estables.OrderByDescending(x => x.Id).First();
                    oEstablecimiento.Ecod = (Int32.Parse(estviejo.Ecod) + 1).ToString("D6");
                    oEstablecimiento.Codsoc = model.Codsoc;
                    oEstablecimiento.Activo = model.Activo;
                    oEstablecimiento.Nombre = model.Nombre;
                    oEstablecimiento.Encargado = model.Encargado;
                    oEstablecimiento.Direcc = model.Direcc;
                    oEstablecimiento.Tel = model.Tel;
                    oEstablecimiento.Locali = model.Locali;
                    oEstablecimiento.Codpos = model.Codpos;
                    oEstablecimiento.Codpro = model.Codpro;
                    oEstablecimiento.Plano = model.Plano;
                    oEstablecimiento.Codzon = model.Codzon;
                    oEstablecimiento.Fecing = model.Fecing;
                    oEstablecimiento.Fechacreacion = model.Fechacreacion;
                    db.Estables.Add(oEstablecimiento);
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
        public IActionResult Edit(Estable model)
        {
            Respuesta<List<Estable>> oRespuesta = new Respuesta<List<Estable>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Estable oEstablecimiento = db.Estables.Find(model.Id);
                    oEstablecimiento.Ecod = model.Ecod;
                    oEstablecimiento.Codsoc = model.Codsoc;
                    oEstablecimiento.Activo = model.Activo;
                    oEstablecimiento.Nombre = model.Nombre;
                    oEstablecimiento.Encargado = model.Encargado;
                    oEstablecimiento.Direcc = model.Direcc;
                    oEstablecimiento.Tel = model.Tel;
                    oEstablecimiento.Locali = model.Locali;
                    oEstablecimiento.Codpos = model.Codpos;
                    oEstablecimiento.Codpro = model.Codpro;
                    oEstablecimiento.Plano = model.Plano;
                    oEstablecimiento.Codzon = model.Codzon;
                    oEstablecimiento.Fecing = model.Fecing;
                    oEstablecimiento.Fechacreacion = model.Fechacreacion;
                    db.Entry(oEstablecimiento).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
        [HttpDelete("{id}")]
        public IActionResult Delete(int Id)
        {
            Respuesta<List<Estable>> oRespuesta = new Respuesta<List<Estable>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                Estable oEstablecimiento = db.Estables.Find(Id);
                db.Remove(oEstablecimiento);
                //var dbToros = db.Toros.Where(x => x.IdEst == Id);
                //foreach(Toro oElement in dbToros)
                //    {
                //        db.Remove(oElement);
                //    }
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
