using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Server.Context;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Resin8Controller : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Resin8> oRespuesta = new Respuesta<Resin8>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Resin8s
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
            Respuesta<List<Resin8>> oRespuesta = new Respuesta<List<Resin8>>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Resin8s
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
            Respuesta<List<Resin8>> oRespuesta = new Respuesta<List<Resin8>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    var lst = db.Resin8s.ToList();
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
        public IActionResult Add(Resin8 model)
        {
            Respuesta<List<Resin8>> oRespuesta = new Respuesta<List<Resin8>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Resin8 oResin8 = new Resin8();
                    oResin8.FchRealizada = model.FchRealizada;
                    oResin8.Nrores = model.Nrores;
                    oResin8.Nropla = model.Nropla;
                    oResin8.Hembras = model.Hembras;
                    oResin8.Machos = model.Machos;
                    oResin8.MotivoRechazo = model.MotivoRechazo;
                    db.Resin8s.Add(oResin8);
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
        public IActionResult Edit(Resin8 model)
        {
            Respuesta<List<Resin8>> oRespuesta = new Respuesta<List<Resin8>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Resin8 oResin8 = db.Resin8s.Find(model.Id);
                    oResin8.FchRealizada = model.FchRealizada;
                    oResin8.Nrores = model.Nrores;
                    oResin8.Nropla = model.Nropla;
                    oResin8.Hembras = model.Hembras;
                    oResin8.Machos = model.Machos;
                    oResin8.MotivoRechazo = model.MotivoRechazo;

                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oResin8).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Resin8>> oRespuesta = new Respuesta<List<Resin8>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Resin8 oResin8 = db.Resin8s.Find(Id);
                    db.Remove(oResin8);
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
