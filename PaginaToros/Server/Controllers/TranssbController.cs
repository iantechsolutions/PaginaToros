using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Server.Context;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranssbController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Transsb> oRespuesta = new Respuesta<Transsb>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Transsbs
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
            Respuesta<List<Transsb>> oRespuesta = new Respuesta<List<Transsb>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    var lst = db.Transsbs.ToList();
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
        public IActionResult Add(Transsb model)
        {
            Respuesta<List<Transsb>> oRespuesta = new Respuesta<List<Transsb>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Transsb oTranssb = new Transsb();
                    oTranssb.NroTrans = model.NroTrans;
                    oTranssb.Fectrans = model.Fectrans;
                    oTranssb.NroOrden = model.NroOrden;
                    oTranssb.Sven = model.Sven;
                    oTranssb.CategSv = model.CategSv;
                    oTranssb.Vnom = model.Vnom;
                    oTranssb.Scom = model.Scom;
                    oTranssb.CategSc = model.CategSc;
                    oTranssb.Cnom = model.Cnom;
                    oTranssb.Ecod = model.Ecod;
                    oTranssb.FchUsu = model.FchUsu;
                    oTranssb.CodUsu = model.CodUsu;
                    db.Transsbs.Add(oTranssb);
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
        public IActionResult Edit(Transsb model)
        {
            Respuesta<List<Transsb>> oRespuesta = new Respuesta<List<Transsb>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Transsb oTranssb = db.Transsbs.Find(model.Id);
                    oTranssb.NroTrans = model.NroTrans;
                    oTranssb.Fectrans = model.Fectrans;
                    oTranssb.NroOrden = model.NroOrden;
                    oTranssb.Sven = model.Sven;
                    oTranssb.CategSv = model.CategSv;
                    oTranssb.Vnom = model.Vnom;
                    oTranssb.Scom = model.Scom;
                    oTranssb.CategSc = model.CategSc;
                    oTranssb.Cnom = model.Cnom;
                    oTranssb.Ecod = model.Ecod;
                    oTranssb.FchUsu = model.FchUsu;
                    oTranssb.CodUsu = model.CodUsu;
                    db.Entry(oTranssb).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Transsb>> oRespuesta = new Respuesta<List<Transsb>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Transsb oTranssb = db.Transsbs.Find(Id);
                    db.Remove(oTranssb);
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
