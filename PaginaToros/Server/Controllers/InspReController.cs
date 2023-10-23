using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Context;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Resin1Controller : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Resin1> oRespuesta = new Respuesta<Resin1>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Resin1s
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
            Respuesta<List<Resin1>> oRespuesta = new Respuesta<List<Resin1>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    var lst = db.Resin1s.ToList();
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
        public IActionResult Add(Resin1 model)
        {
            Respuesta<List<Resin1>> oRespuesta = new Respuesta<List<Resin1>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Resin1 oResin1 = new Resin1();
                    oResin1.Nrores = model.Nrores;
                    oResin1.Nropla = model.Nropla;
                    oResin1.Observ = model.Observ;
                    oResin1.Ppajust = model.Ppajust;
                    oResin1.Epromedio = model.Epromedio;
                    oResin1.Emax = model.Emax;
                    oResin1.Emin = model.Emin;
                    oResin1.Tortot = model.Tortot;
                    oResin1.Torsb = model.Torsb;
                    oResin1.CodUsu = model.CodUsu;
                    oResin1.Editar = model.Editar;
                    oResin1.Icod = model.Icod;
                    oResin1.Scod = model.Scod;
                    oResin1.Estcod = model.Estcod;
                    oResin1.Freali = model.Freali;

                    db.Resin1s.Add(oResin1);
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
        public IActionResult Edit(Resin1 model)
        {
            Respuesta<List<Resin1>> oRespuesta = new Respuesta<List<Resin1>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Resin1 oResin1 = db.Resin1s.Find(model.Id);
                    oResin1.Nrores = model.Nrores;
                    oResin1.Nropla = model.Nropla;
                    oResin1.Observ = model.Observ;
                    oResin1.Ppajust = model.Ppajust;
                    oResin1.Epromedio = model.Epromedio;
                    oResin1.Emax = model.Emax;
                    oResin1.Emin = model.Emin;
                    oResin1.Tortot = model.Tortot;
                    oResin1.Torsb = model.Torsb;
                    oResin1.FchUsu = model.FchUsu;
                    oResin1.CodUsu = model.CodUsu;
                    oResin1.Editar = model.Editar;
                    oResin1.Icod = model.Icod;
                    oResin1.Scod = model.Scod;
                    oResin1.Estcod = model.Estcod;
                    oResin1.Freali = model.Freali;

                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oResin1).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Resin1>> oRespuesta = new Respuesta<List<Resin1>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Resin1 oResin1 = db.Resin1s.Find(Id);
                    db.Remove(oResin1);
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
