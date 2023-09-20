using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RechazoReController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<RechazoRe> oRespuesta = new Respuesta<RechazoRe>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.RechazoRes
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
        public IActionResult GetByRes(int nro)
        {
            Respuesta<List<RechazoRe>> oRespuesta = new Respuesta<List<RechazoRe>>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.RechazoRes
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
            Respuesta<List<RechazoRe>> oRespuesta = new Respuesta<List<RechazoRe>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.RechazoRes.ToList();
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
        public IActionResult Add(RechazoRe model)
        {
            Respuesta<List<RechazoRe>> oRespuesta = new Respuesta<List<RechazoRe>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    RechazoRe oRechazoRe = new RechazoRe();
                    oRechazoRe.FchRealizada = model.FchRealizada;
                    oRechazoRe.Nrores = model.Nrores;
                    oRechazoRe.Nropla = model.Nropla;
                    oRechazoRe.Hembras = model.Hembras;
                    oRechazoRe.Machos = model.Machos;
                    oRechazoRe.MotivoRechazo = model.MotivoRechazo;
                    db.RechazoRes.Add(oRechazoRe);
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
        public IActionResult Edit(RechazoRe model)
        {
            Respuesta<List<RechazoRe>> oRespuesta = new Respuesta<List<RechazoRe>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    RechazoRe oRechazoRe = db.RechazoRes.Find(model.Id);
                    oRechazoRe.FchRealizada = model.FchRealizada;
                    oRechazoRe.Nrores = model.Nrores;
                    oRechazoRe.Nropla = model.Nropla;
                    oRechazoRe.Hembras = model.Hembras;
                    oRechazoRe.Machos = model.Machos;
                    oRechazoRe.MotivoRechazo = model.MotivoRechazo;

                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oRechazoRe).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<RechazoRe>> oRespuesta = new Respuesta<List<RechazoRe>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    RechazoRe oRechazoRe = db.RechazoRes.Find(Id);
                    db.Remove(oRechazoRe);
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
