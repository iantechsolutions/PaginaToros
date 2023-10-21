using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Context;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspReController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<InspRe> oRespuesta = new Respuesta<InspRe>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.InspRes
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
            Respuesta<List<InspRe>> oRespuesta = new Respuesta<List<InspRe>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.InspRes.ToList();
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
        public IActionResult Add(InspRe model)
        {
            Respuesta<List<InspRe>> oRespuesta = new Respuesta<List<InspRe>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    InspRe oInspRe = new InspRe();
                    oInspRe.Nrores = model.Nrores;
                    oInspRe.Nropla = model.Nropla;
                    oInspRe.Observ = model.Observ;
                    oInspRe.Ppajust = model.Ppajust;
                    oInspRe.Epromedio = model.Epromedio;
                    oInspRe.Emax = model.Emax;
                    oInspRe.Emin = model.Emin;
                    oInspRe.Tortot = model.Tortot;
                    oInspRe.Torsb = model.Torsb;
                    oInspRe.CodUsu = model.CodUsu;
                    oInspRe.Editar = model.Editar;
                    oInspRe.Icod = model.Icod;
                    oInspRe.Scod = model.Scod;
                    oInspRe.Estcod = model.Estcod;
                    oInspRe.FechaInspeccion = model.FechaInspeccion;
                    oInspRe.NombreSocio = model.NombreSocio;

                    db.InspRes.Add(oInspRe);
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
        public IActionResult Edit(InspRe model)
        {
            Respuesta<List<InspRe>> oRespuesta = new Respuesta<List<InspRe>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    InspRe oInspRe = db.InspRes.Find(model.Id);
                    oInspRe.Nrores = model.Nrores;
                    oInspRe.Nropla = model.Nropla;
                    oInspRe.Observ = model.Observ;
                    oInspRe.Ppajust = model.Ppajust;
                    oInspRe.Epromedio = model.Epromedio;
                    oInspRe.Emax = model.Emax;
                    oInspRe.Emin = model.Emin;
                    oInspRe.Tortot = model.Tortot;
                    oInspRe.Torsb = model.Torsb;
                    oInspRe.FchUsu = model.FchUsu;
                    oInspRe.CodUsu = model.CodUsu;
                    oInspRe.Editar = model.Editar;
                    oInspRe.Icod = model.Icod;
                    oInspRe.Scod = model.Scod;
                    oInspRe.Estcod = model.Estcod;
                    oInspRe.FechaInspeccion = model.FechaInspeccion;
                    oInspRe.NombreSocio = model.NombreSocio;

                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oInspRe).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<InspRe>> oRespuesta = new Respuesta<List<InspRe>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    InspRe oInspRe = db.InspRes.Find(Id);
                    db.Remove(oInspRe);
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
