using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Context;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FutcontrolController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Futcontrol> oRespuesta = new Respuesta<Futcontrol>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Futcontrols
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
            Respuesta<List<Futcontrol>> oRespuesta = new Respuesta<List<Futcontrol>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    var lst = db.Futcontrols.ToList();
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
        public IActionResult Add(Futcontrol model)
        {
            Respuesta<List<Futcontrol>> oRespuesta = new Respuesta<List<Futcontrol>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Futcontrol oFutcontrol = new Futcontrol();
                    oFutcontrol.NroTrans = model.NroTrans;
                    oFutcontrol.Fectrans = model.Fectrans;
                    oFutcontrol.Sven = model.Sven;
                    oFutcontrol.CategSv = model.CategSv;
                    oFutcontrol.Vnom = model.Vnom;
                    oFutcontrol.Scom = model.Scom;
                    oFutcontrol.CategSc = model.CategSc;
                    oFutcontrol.Cnom = model.Cnom;
                    oFutcontrol.Plantel = model.Plantel;
                    oFutcontrol.EdadCrias = model.EdadCrias;
                    oFutcontrol.CantHem = model.CantHem;
                    oFutcontrol.CantMach = model.CantMach;
                    oFutcontrol.PlantDest = model.PlantDest;
                    oFutcontrol.Incorp = model.Incorp;
                    oFutcontrol.Hemsta = model.Hemsta;
                    oFutcontrol.FchUsu = model.FchUsu;
                    oFutcontrol.CodUsu = model.CodUsu;

                    db.Futcontrols.Add(oFutcontrol);
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
        public IActionResult Edit(Futcontrol model)
        {
            Respuesta<List<Futcontrol>> oRespuesta = new Respuesta<List<Futcontrol>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Futcontrol oFutcontrol = db.Futcontrols.Find(model.Id);
                    oFutcontrol.NroTrans = model.NroTrans;
                    oFutcontrol.Fectrans = model.Fectrans;
                    oFutcontrol.Sven = model.Sven;
                    oFutcontrol.CategSv = model.CategSv;
                    oFutcontrol.Vnom = model.Vnom;
                    oFutcontrol.Scom = model.Scom;
                    oFutcontrol.CategSc = model.CategSc;
                    oFutcontrol.Cnom = model.Cnom;
                    oFutcontrol.Plantel = model.Plantel;
                    oFutcontrol.EdadCrias = model.EdadCrias;
                    oFutcontrol.CantHem = model.CantHem;
                    oFutcontrol.CantMach = model.CantMach;
                    oFutcontrol.PlantDest = model.PlantDest;
                    oFutcontrol.Incorp = model.Incorp;
                    oFutcontrol.Hemsta = model.Hemsta;
                    oFutcontrol.FchUsu = model.FchUsu;
                    oFutcontrol.CodUsu = model.CodUsu;


                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oFutcontrol).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Futcontrol>> oRespuesta = new Respuesta<List<Futcontrol>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Futcontrol oFutcontrol = db.Futcontrols.Find(Id);
                    db.Remove(oFutcontrol);
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

