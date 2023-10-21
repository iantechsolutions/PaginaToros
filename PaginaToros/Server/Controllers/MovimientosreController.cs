using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Context;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientosreController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<MovimientosRe> oRespuesta = new Respuesta<MovimientosRe>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.MovimientosRes
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
            Respuesta<List<MovimientosRe>> oRespuesta = new Respuesta<List<MovimientosRe>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.MovimientosRes.ToList();
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
            Respuesta<List<MovimientosRe>> oRespuesta = new Respuesta<List<MovimientosRe>>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.MovimientosRes
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
        public IActionResult Add(MovimientosRe model)
        {
            Respuesta<List<MovimientosRe>> oRespuesta = new Respuesta<List<MovimientosRe>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    MovimientosRe oMovimientosRe = new MovimientosRe();
                    oMovimientosRe.Rdvac = model.Rdvac;
                    oMovimientosRe.Rdvaqcs = model.Rdvaqcs;
                    oMovimientosRe.Rdvaqss = model.Rdvaqss;
                    oMovimientosRe.Rpvac = model.Rpvac;
                    oMovimientosRe.Rpvaqcs = model.Rpvaqcs;
                    oMovimientosRe.Rpvaqss = model.Rpvaqss;
                    oMovimientosRe.Ctomov = model.Ctomov;
                    oMovimientosRe.Tipmov = model.Tipmov;
                    oMovimientosRe.Nrores = model.Nrores;

                    db.MovimientosRes.Add(oMovimientosRe);
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
        public IActionResult Edit(MovimientosRe model)
        {
            Respuesta<List<MovimientosRe>> oRespuesta = new Respuesta<List<MovimientosRe>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    MovimientosRe oMovimientosRe = db.MovimientosRes.Find(model.Id);
                    oMovimientosRe.Rdvac = model.Rdvac;
                    oMovimientosRe.Rdvaqcs = model.Rdvaqcs;
                    oMovimientosRe.Rdvaqss = model.Rdvaqss;
                    oMovimientosRe.Rpvac = model.Rpvac;
                    oMovimientosRe.Rpvaqcs = model.Rpvaqcs;
                    oMovimientosRe.Rpvaqss = model.Rpvaqss;
                    oMovimientosRe.Ctomov = model.Ctomov;
                    oMovimientosRe.Tipmov = model.Tipmov;
                    oMovimientosRe.Nrores = model.Nrores;

                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oMovimientosRe).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<MovimientosRe>> oRespuesta = new Respuesta<List<MovimientosRe>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    MovimientosRe oMovimientosRe = db.MovimientosRes.Find(Id);
                    db.Remove(oMovimientosRe);
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
