using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Server.Context;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
namespace PaginaToros.Server.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class Resin2Controller : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Resin2> oRespuesta = new Respuesta<Resin2>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Resin2s
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
            Respuesta<List<Resin2>> oRespuesta = new Respuesta<List<Resin2>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    var lst = db.Resin2s.ToList();
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

        [HttpGet("Nrores/{nro}")]
        public IActionResult GetByRes(string nro)
        {
            Respuesta<List<Resin2>> oRespuesta = new Respuesta<List<Resin2>>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Resin2s
                    .Where(x => x.Nrores == nro).ToList();
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
        public IActionResult Add(Resin2 model)
        {
            Respuesta<List<Resin2>> oRespuesta = new Respuesta<List<Resin2>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Resin2 oResin2 = new Resin2();
                    oResin2.Ea1 = model.Ea1;
                    oResin2.Ea2 = model.Ea2;
                    oResin2.Ea3 = model.Ea3;
                    oResin2.Ea4 = model.Ea4;
                    oResin2.Ea5 = model.Ea5;
                    oResin2.Ea6 = model.Ea6;
                    oResin2.Nrores = model.Nrores;

                    db.Resin2s.Add(oResin2);
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
        public IActionResult Edit(Resin2 model)
        {
            Respuesta<List<Resin2>> oRespuesta = new Respuesta<List<Resin2>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Resin2 oResin2 = db.Resin2s.Find(model.Id);
                    oResin2.Ea1 = model.Ea1;
                    oResin2.Ea2 = model.Ea2;
                    oResin2.Ea3 = model.Ea3;
                    oResin2.Ea4 = model.Ea4;
                    oResin2.Ea5 = model.Ea5;
                    oResin2.Ea6 = model.Ea6;
                    oResin2.Nrores = model.Nrores;

                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oResin2).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Resin2>> oRespuesta = new Respuesta<List<Resin2>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Resin2 oResin2 = db.Resin2s.Find(Id);
                    db.Remove(oResin2);
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

