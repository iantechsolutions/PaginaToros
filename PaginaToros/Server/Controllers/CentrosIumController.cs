using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Context;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentrosIumController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<CentrosIum> oRespuesta = new Respuesta<CentrosIum>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.CentrosIa
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
            Respuesta<List<CentrosIum>> oRespuesta = new Respuesta<List<CentrosIum>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.CentrosIa.ToList();
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
        public IActionResult Add(CentrosIum model)
        {
            Respuesta<List<CentrosIum>> oRespuesta = new Respuesta<List<CentrosIum>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    CentrosIum oCentrosIum = new CentrosIum();
                    oCentrosIum.Nrocen = model.Nrocen;
                    oCentrosIum.Nombre = model.Nombre;
                    oCentrosIum.NroCSayg = model.NroCSayg;
                    oCentrosIum.FchUsu = model.FchUsu;
                    oCentrosIum.CodUsu = model.CodUsu;
                    oCentrosIum.Id = model.Id;

                    db.CentrosIa.Add(oCentrosIum);
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
        public IActionResult Edit(CentrosIum model)
        {
            Respuesta<List<CentrosIum>> oRespuesta = new Respuesta<List<CentrosIum>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    CentrosIum oCentrosIum = db.CentrosIa.Find(model.Id);
                    oCentrosIum.Nrocen = model.Nrocen;
                    oCentrosIum.Nombre = model.Nombre;
                    oCentrosIum.NroCSayg = model.NroCSayg;
                    oCentrosIum.FchUsu = model.FchUsu;
                    oCentrosIum.CodUsu = model.CodUsu;
                    oCentrosIum.Id = model.Id;

                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oCentrosIum).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<CentrosIum>> oRespuesta = new Respuesta<List<CentrosIum>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    CentrosIum oCentrosIum = db.CentrosIa.Find(Id);
                    db.Remove(oCentrosIum);
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

