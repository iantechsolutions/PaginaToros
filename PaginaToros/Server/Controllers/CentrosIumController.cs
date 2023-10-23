using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Context;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentrosiumController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Centrosium> oRespuesta = new Respuesta<Centrosium>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Centrosia
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
            Respuesta<List<Centrosium>> oRespuesta = new Respuesta<List<Centrosium>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    var lst = db.Centrosia.ToList();
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
        public IActionResult Add(Centrosium model)
        {
            Respuesta<List<Centrosium>> oRespuesta = new Respuesta<List<Centrosium>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Centrosium oCentrosium = new Centrosium();
                    oCentrosium.Nrocen = model.Nrocen;
                    oCentrosium.Nombre = model.Nombre;
                    oCentrosium.NroCSayg = model.NroCSayg;
                    oCentrosium.FchUsu = model.FchUsu;
                    oCentrosium.CodUsu = model.CodUsu;
                    oCentrosium.Id = model.Id;

                    db.Centrosia.Add(oCentrosium);
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
        public IActionResult Edit(Centrosium model)
        {
            Respuesta<List<Centrosium>> oRespuesta = new Respuesta<List<Centrosium>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Centrosium oCentrosium = db.Centrosia.Find(model.Id);
                    oCentrosium.Nrocen = model.Nrocen;
                    oCentrosium.Nombre = model.Nombre;
                    oCentrosium.NroCSayg = model.NroCSayg;
                    oCentrosium.FchUsu = model.FchUsu;
                    oCentrosium.CodUsu = model.CodUsu;
                    oCentrosium.Id = model.Id;

                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oCentrosium).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Centrosium>> oRespuesta = new Respuesta<List<Centrosium>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Centrosium oCentrosium = db.Centrosia.Find(Id);
                    db.Remove(oCentrosium);
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

