using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Request;
using PaginaToros.Shared.Models.Response;
namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToroController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Toro> oRespuesta = new Respuesta<Toro>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.Toros
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
            Respuesta<List<Toro>> oRespuesta = new Respuesta<List<Toro>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.Toros.ToList();
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
        public IActionResult Add(Toro model)
        {
            
            Respuesta<List<Toro>> oRespuesta = new Respuesta<List<Toro>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Toro oToro = new Toro();
                    oToro.Nombre = model.Nombre;
                    oToro.Calidad = model.Calidad;
                    oToro.IdEst = model.IdEst;
                    oToro.NombreEst = model.NombreEst;
                    db.Toros.Add(oToro);
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
        public IActionResult Edit(Toro model)
        {
            Respuesta<List<Toro>> oRespuesta = new Respuesta<List<Toro>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Toro oToro = db.Toros.Find(model.Id);
                    oToro.Nombre = model.Nombre;
                    oToro.Calidad = model.Calidad;
                    oToro.IdEst = model.IdEst;
                    oToro.NombreEst = model.NombreEst;
                    db.Entry(oToro).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
            Respuesta<List<Toro>> oRespuesta = new Respuesta<List<Toro>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Toro oToro = db.Toros.Find(Id);
                    db.Remove(oToro);
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
