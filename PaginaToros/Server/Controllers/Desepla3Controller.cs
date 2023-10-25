using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Server.Context;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Desepla3Controller : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Desepla3> oRespuesta = new Respuesta<Desepla3>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Desepla3s
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
            Respuesta<List<Desepla3>> oRespuesta = new Respuesta<List<Desepla3>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    var lst = db.Desepla3s.ToList();
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
        public IActionResult Add(Desepla3 model)
        {
            Respuesta<List<Desepla3>> oRespuesta = new Respuesta<List<Desepla3>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Desepla3 oDesepla3 = new Desepla3();
                    oDesepla3.Nrodec = model.Nrodec;
                    oDesepla3.Cantv = model.Cantv;
                    oDesepla3.Desde = model.Desde;
                    oDesepla3.Hasta = model.Hasta;
                    oDesepla3.Tatpart = model.Tatpart;
                    oDesepla3.Hardb = model.Hardb;

                    db.Desepla3s.Add(oDesepla3);
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

        [HttpPost]
        public IActionResult AddList(List<Desepla3> list)
        {
            Respuesta<List<Desepla3>> oRespuesta = new Respuesta<List<Desepla3>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    foreach(var model in list) { 
                    Desepla3 oDesepla3 = new Desepla3();
                    oDesepla3.Nrodec = model.Nrodec;
                    oDesepla3.Cantv = model.Cantv;
                    oDesepla3.Desde = model.Desde;
                    oDesepla3.Hasta = model.Hasta;
                    oDesepla3.Tatpart = model.Tatpart;
                    oDesepla3.Hardb = model.Hardb;
                    db.Desepla3s.Add(oDesepla3);
                    }
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
        public IActionResult Edit(Desepla3 model)
        {
            Respuesta<List<Desepla3>> oRespuesta = new Respuesta<List<Desepla3>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Desepla3 oDesepla3 = db.Desepla3s.Find(model.Id);
                    oDesepla3.Nrodec = model.Nrodec;
                    oDesepla3.Cantv = model.Cantv;
                    oDesepla3.Desde = model.Desde;
                    oDesepla3.Hasta = model.Hasta;
                    oDesepla3.Tatpart = model.Tatpart;
                    oDesepla3.Hardb = model.Hardb; 
                    db.Entry(oDesepla3).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Desepla3>> oRespuesta = new Respuesta<List<Desepla3>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Desepla3 oDesepla3 = db.Desepla3s.Find(Id);
                    db.Remove(oDesepla3);
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
