using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Client.Pages.Inspectores;
using PaginaToros.Server.Context;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Inspect> oRespuesta = new Respuesta<Inspect>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Inspects
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
            Respuesta<List<Inspect>> oRespuesta = new Respuesta<List<Inspect>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    var lst = db.Inspects.ToList();
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
        public IActionResult Add(Inspect model)
        {
            Respuesta<List<Inspect>> oRespuesta = new Respuesta<List<Inspect>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Inspect oInspector = new Inspect();
                    oInspector.Icod = model.Icod;
                    oInspector.Nombre = model.Nombre;
                    oInspector.Direcc = model.Direcc;
                    oInspector.Locali = model.Locali;
                    oInspector.Codpos = model.Codpos;
                    oInspector.Codpro = model.Codpro;
                    oInspector.Telefo = model.Telefo;
                    oInspector.Mail = model.Mail;
                    db.Inspects.Add(oInspector);
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
        public IActionResult Edit(Inspect model)
        {
            Respuesta<List<Inspect>> oRespuesta = new Respuesta<List<Inspect>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Inspect oInspector = db.Inspects.Find(model.Id);
                    oInspector.Icod = model.Icod;
                    oInspector.Nombre = model.Nombre;
                    oInspector.Direcc = model.Direcc;
                    oInspector.Locali = model.Locali;
                    oInspector.Codpos = model.Codpos;
                    oInspector.Codpro = model.Codpro;
                    oInspector.Telefo = model.Telefo;
                    oInspector.Mail = model.Mail;
                    db.Entry(oInspector).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Inspect>> oRespuesta = new Respuesta<List<Inspect>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Inspect oInspector = db.Inspects.Find(Id);
                    db.Remove(oInspector);
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
