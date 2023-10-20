using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Client.Pages.Inspectores;
using PaginaToros.Server.Context;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectoreController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Inspectore> oRespuesta = new Respuesta<Inspectore>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.Inspectores
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
            Respuesta<List<Inspectore>> oRespuesta = new Respuesta<List<Inspectore>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.Inspectores.ToList();
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
        public IActionResult Add(Inspectore model)
        {
            Respuesta<List<Inspectore>> oRespuesta = new Respuesta<List<Inspectore>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Inspectore oInspector = new Inspectore();
                    oInspector.Codigo = model.Codigo;
                    oInspector.Nombre = model.Nombre;
                    oInspector.Direccion = model.Direccion;
                    oInspector.Localidad = model.Localidad;
                    oInspector.CodPostal = model.CodPostal;
                    oInspector.Provincia = model.Provincia;
                    oInspector.Telefono = model.Telefono;
                    oInspector.Mail = model.Mail;
                    db.Inspectores.Add(oInspector);
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
        public IActionResult Edit(Inspectore model)
        {
            Respuesta<List<Inspectore>> oRespuesta = new Respuesta<List<Inspectore>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Inspectore oInspector = db.Inspectores.Find(model.Id);
                    oInspector.Codigo = model.Codigo;
                    oInspector.Nombre = model.Nombre;
                    oInspector.Direccion = model.Direccion;
                    oInspector.Localidad = model.Localidad;
                    oInspector.CodPostal = model.CodPostal;
                    oInspector.Provincia = model.Provincia;
                    oInspector.Telefono = model.Telefono;
                    oInspector.Mail = model.Mail;

                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}

                    db.Entry(oInspector).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Inspectore>> oRespuesta = new Respuesta<List<Inspectore>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Inspectore oInspector = db.Inspectores.Find(Id);
                    db.Remove(oInspector);
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
