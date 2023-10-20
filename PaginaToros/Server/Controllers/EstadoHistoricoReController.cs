using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Server.Context;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
namespace PaginaToros.Server.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoHistoricoReController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<EstadoHistoricoRe> oRespuesta = new Respuesta<EstadoHistoricoRe>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.EstadoHistoricoRes
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
            Respuesta<List<EstadoHistoricoRe>> oRespuesta = new Respuesta<List<EstadoHistoricoRe>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.EstadoHistoricoRes.ToList();
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
            Respuesta<List<EstadoHistoricoRe>> oRespuesta = new Respuesta<List<EstadoHistoricoRe>>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.EstadoHistoricoRes
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
        public IActionResult Add(EstadoHistoricoRe model)
        {
            Respuesta<List<EstadoHistoricoRe>> oRespuesta = new Respuesta<List<EstadoHistoricoRe>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    EstadoHistoricoRe oEstadoHistoricoRe = new EstadoHistoricoRe();
                    oEstadoHistoricoRe.Ea1 = model.Ea1;
                    oEstadoHistoricoRe.Ea2 = model.Ea2;
                    oEstadoHistoricoRe.Ea3 = model.Ea3;
                    oEstadoHistoricoRe.Ea4 = model.Ea4;
                    oEstadoHistoricoRe.Ea5 = model.Ea5;
                    oEstadoHistoricoRe.Ea6 = model.Ea6;
                    oEstadoHistoricoRe.Nrores = model.Nrores;

                    db.EstadoHistoricoRes.Add(oEstadoHistoricoRe);
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
        public IActionResult Edit(EstadoHistoricoRe model)
        {
            Respuesta<List<EstadoHistoricoRe>> oRespuesta = new Respuesta<List<EstadoHistoricoRe>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    EstadoHistoricoRe oEstadoHistoricoRe = db.EstadoHistoricoRes.Find(model.Id);
                    oEstadoHistoricoRe.Ea1 = model.Ea1;
                    oEstadoHistoricoRe.Ea2 = model.Ea2;
                    oEstadoHistoricoRe.Ea3 = model.Ea3;
                    oEstadoHistoricoRe.Ea4 = model.Ea4;
                    oEstadoHistoricoRe.Ea5 = model.Ea5;
                    oEstadoHistoricoRe.Ea6 = model.Ea6;
                    oEstadoHistoricoRe.Nrores = model.Nrores;

                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oEstadoHistoricoRe).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<EstadoHistoricoRe>> oRespuesta = new Respuesta<List<EstadoHistoricoRe>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    EstadoHistoricoRe oEstadoHistoricoRe = db.EstadoHistoricoRes.Find(Id);
                    db.Remove(oEstadoHistoricoRe);
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

