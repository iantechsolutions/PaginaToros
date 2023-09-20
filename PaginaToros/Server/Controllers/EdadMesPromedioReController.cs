using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EdadMesPromedioReController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<EdadMesPromedioRe> oRespuesta = new Respuesta<EdadMesPromedioRe>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.EdadMesPromedioRes
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
            
            Respuesta<List<EdadMesPromedioRe>> oRespuesta = new Respuesta<List<EdadMesPromedioRe>>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.EdadMesPromedioRes
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
            Respuesta<List<EdadMesPromedioRe>> oRespuesta = new Respuesta<List<EdadMesPromedioRe>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.EdadMesPromedioRes.ToList();
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
        public IActionResult Add(EdadMesPromedioRe model)
        {
            Respuesta<List<EdadMesPromedioRe>> oRespuesta = new Respuesta<List<EdadMesPromedioRe>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    EdadMesPromedioRe oEdadMesPromedioRe = new EdadMesPromedioRe();
                    oEdadMesPromedioRe.Pedad = model.Pedad;
                    oEdadMesPromedioRe.Ppeso = model.Ppeso;
                    oEdadMesPromedioRe.Medad = model.Medad;
                    oEdadMesPromedioRe.Mpeso = model.Mpeso;
                    oEdadMesPromedioRe.Iedad = model.Iedad;
                    oEdadMesPromedioRe.Ipeso = model.Ipeso;
                    oEdadMesPromedioRe.Pdl = model.Pdl;
                    oEdadMesPromedioRe.P2d = model.P2d;
                    oEdadMesPromedioRe.P4d = model.P4d;
                    oEdadMesPromedioRe.Sexo = model.Sexo;
                    oEdadMesPromedioRe.Nrores = model.Nrores;

                    db.EdadMesPromedioRes.Add(oEdadMesPromedioRe);
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
        public IActionResult Edit(EdadMesPromedioRe model)
        {
            Respuesta<List<EdadMesPromedioRe>> oRespuesta = new Respuesta<List<EdadMesPromedioRe>>();
            IQueryable<Toro> TorosPorId; ;
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    EdadMesPromedioRe oEdadMesPromedioRe = db.EdadMesPromedioRes.Find(model.Id);
                    oEdadMesPromedioRe.Pedad = model.Pedad;
                    oEdadMesPromedioRe.Ppeso = model.Ppeso;
                    oEdadMesPromedioRe.Medad = model.Medad;
                    oEdadMesPromedioRe.Mpeso = model.Mpeso;
                    oEdadMesPromedioRe.Iedad = model.Iedad;
                    oEdadMesPromedioRe.Ipeso = model.Ipeso;
                    oEdadMesPromedioRe.Pdl = model.Pdl;
                    oEdadMesPromedioRe.P2d = model.P2d;
                    oEdadMesPromedioRe.P4d = model.P4d;
                    oEdadMesPromedioRe.Sexo = model.Sexo;
                    oEdadMesPromedioRe.Nrores = model.Nrores;

                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oEdadMesPromedioRe).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<EdadMesPromedioRe>> oRespuesta = new Respuesta<List<EdadMesPromedioRe>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    EdadMesPromedioRe oEdadMesPromedioRe = db.EdadMesPromedioRes.Find(Id);
                    db.Remove(oEdadMesPromedioRe);
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
