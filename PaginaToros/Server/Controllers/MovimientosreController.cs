using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Context;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Resin3Controller : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Resin3> oRespuesta = new Respuesta<Resin3>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Resin3s
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
            Respuesta<List<Resin3>> oRespuesta = new Respuesta<List<Resin3>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    var lst = db.Resin3s.ToList();
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
            Respuesta<List<Resin3>> oRespuesta = new Respuesta<List<Resin3>>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Resin3s
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
        public IActionResult Add(Resin3 model)
        {
            Respuesta<List<Resin3>> oRespuesta = new Respuesta<List<Resin3>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Resin3 oResin3 = new Resin3();
                    oResin3.Rdvac = model.Rdvac;
                    oResin3.Rdvaqcs = model.Rdvaqcs;
                    oResin3.Rdvaqss = model.Rdvaqss;
                    oResin3.Rpvac = model.Rpvac;
                    oResin3.Rpvaqcs = model.Rpvaqcs;
                    oResin3.Rpvaqss = model.Rpvaqss;
                    oResin3.Ctomov = model.Ctomov;
                    oResin3.Tipmov = model.Tipmov;
                    oResin3.Nrores = model.Nrores;

                    db.Resin3s.Add(oResin3);
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
        public IActionResult Edit(Resin3 model)
        {
            Respuesta<List<Resin3>> oRespuesta = new Respuesta<List<Resin3>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Resin3 oResin3 = db.Resin3s.Find(model.Id);
                    oResin3.Rdvac = model.Rdvac;
                    oResin3.Rdvaqcs = model.Rdvaqcs;
                    oResin3.Rdvaqss = model.Rdvaqss;
                    oResin3.Rpvac = model.Rpvac;
                    oResin3.Rpvaqcs = model.Rpvaqcs;
                    oResin3.Rpvaqss = model.Rpvaqss;
                    oResin3.Ctomov = model.Ctomov;
                    oResin3.Tipmov = model.Tipmov;
                    oResin3.Nrores = model.Nrores;

                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oResin3).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Resin3>> oRespuesta = new Respuesta<List<Resin3>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Resin3 oResin3 = db.Resin3s.Find(Id);
                    db.Remove(oResin3);
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
