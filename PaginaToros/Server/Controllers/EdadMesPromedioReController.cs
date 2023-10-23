using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Context;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Resin4Controller : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Resin4> oRespuesta = new Respuesta<Resin4>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Resin4s
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
        [HttpGet("Nrores/{nro}")]
        public IActionResult GetByRes(string nro)
        {
            
            Respuesta<List<Resin4>> oRespuesta = new Respuesta<List<Resin4>>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Resin4s
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

        [HttpGet]
        public IActionResult Get()
        {
            Respuesta<List<Resin4>> oRespuesta = new Respuesta<List<Resin4>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    var lst = db.Resin4s.ToList();
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
        public IActionResult Add(Resin4 model)
        {
            Respuesta<List<Resin4>> oRespuesta = new Respuesta<List<Resin4>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Resin4 oResin4 = new Resin4();
                    oResin4.Pedad = model.Pedad;
                    oResin4.Ppeso = model.Ppeso;
                    oResin4.Medad = model.Medad;
                    oResin4.Mpeso = model.Mpeso;
                    oResin4.Iedad = model.Iedad;
                    oResin4.Ipeso = model.Ipeso;
                    oResin4.Pdl = model.Pdl;
                    oResin4.P2d = model.P2d;
                    oResin4.P4d = model.P4d;
                    oResin4.Sexo = model.Sexo;
                    oResin4.Nrores = model.Nrores;

                    db.Resin4s.Add(oResin4);
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
        public IActionResult Edit(Resin4 model)
        {
            Respuesta<List<Resin4>> oRespuesta = new Respuesta<List<Resin4>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Resin4 oResin4 = db.Resin4s.Find(model.Id);
                    oResin4.Pedad = model.Pedad;
                    oResin4.Ppeso = model.Ppeso;
                    oResin4.Medad = model.Medad;
                    oResin4.Mpeso = model.Mpeso;
                    oResin4.Iedad = model.Iedad;
                    oResin4.Ipeso = model.Ipeso;
                    oResin4.Pdl = model.Pdl;
                    oResin4.P2d = model.P2d;
                    oResin4.P4d = model.P4d;
                    oResin4.Sexo = model.Sexo;
                    oResin4.Nrores = model.Nrores;

                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oResin4).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Resin4>> oRespuesta = new Respuesta<List<Resin4>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Resin4 oResin4 = db.Resin4s.Find(Id);
                    db.Remove(oResin4);
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
