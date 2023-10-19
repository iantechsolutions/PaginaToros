using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Desepla1Controller : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Desepla1> oRespuesta = new Respuesta<Desepla1>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.Desepla1s
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
            Respuesta<List<Desepla1>> oRespuesta = new Respuesta<List<Desepla1>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.Desepla1s.ToList();
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
        public IActionResult Add(Desepla1 model)
        {
            Respuesta<List<Desepla1>> oRespuesta = new Respuesta<List<Desepla1>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Desepla1 oDesepla1 = new Desepla1();
                    oDesepla1.Nrodec = model.Nrodec;
                    oDesepla1.Nroplan = model.Nroplan;
                    oDesepla1.Tipse = model.Tipse;
                    oDesepla1.Semen = model.Semen;
                    oDesepla1.Cantv = model.Cantv;
                    oDesepla1.Cantb = model.Cantb;
                    oDesepla1.Remba = model.Remba;
                    oDesepla1.Remba = model.Remba;  
                    oDesepla1.Rempr = model.Rempr;  
                    oDesepla1.Ctrlu = model.Ctrlu;  
                    oDesepla1.Ctrlm = model.Ctrlm;  
                    oDesepla1.CoefAutoSn = model.CoefAutoSn;  
                    oDesepla1.CoefAutoIa = model.CoefAutoIa;  
                    oDesepla1.CoefAutoIar = model.CoefAutoIar;  
                    oDesepla1.IaSincro = model.IaSincro;  
                    oDesepla1.PastillasSincro = model.PastillasSincro;  
                    oDesepla1.Fecret = model.Fecret;  
                    oDesepla1.NrFolio = model.NrFolio;  
                    oDesepla1.FchUsu = model.FchUsu;  
                    oDesepla1.CodUsu = model.CodUsu;  
                    oDesepla1.Reten = model.Reten;  
                    oDesepla1.Edicion = model.Edicion;  
                    oDesepla1.Nrocri = model.Nrocri;  
                    oDesepla1.Desde = model.Desde;  
                    oDesepla1.Hasta = model.Hasta;  
                    oDesepla1.Fecdecl = model.Fecdecl;  
                    oDesepla1.Fchrecepcion = model.Fchrecepcion;  

                    db.Desepla1s.Add(oDesepla1);
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
        public IActionResult Edit(Desepla1 model)
        {
            Respuesta<List<Desepla1>> oRespuesta = new Respuesta<List<Desepla1>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Desepla1 oDesepla1 = db.Desepla1s.Find(model.Id);
                    oDesepla1.Nrodec = model.Nrodec;
                    oDesepla1.Nroplan = model.Nroplan;
                    oDesepla1.Tipse = model.Tipse;
                    oDesepla1.Semen = model.Semen;
                    oDesepla1.Cantv = model.Cantv;
                    oDesepla1.Cantb = model.Cantb;
                    oDesepla1.Remba = model.Remba;
                    oDesepla1.Remba = model.Remba;
                    oDesepla1.Rempr = model.Rempr;
                    oDesepla1.Ctrlu = model.Ctrlu;
                    oDesepla1.Ctrlm = model.Ctrlm;
                    oDesepla1.CoefAutoSn = model.CoefAutoSn;
                    oDesepla1.CoefAutoIa = model.CoefAutoIa;
                    oDesepla1.CoefAutoIar = model.CoefAutoIar;
                    oDesepla1.IaSincro = model.IaSincro;
                    oDesepla1.PastillasSincro = model.PastillasSincro;
                    oDesepla1.Fecret = model.Fecret;
                    oDesepla1.NrFolio = model.NrFolio;
                    oDesepla1.FchUsu = model.FchUsu;
                    oDesepla1.CodUsu = model.CodUsu;
                    oDesepla1.Reten = model.Reten;
                    oDesepla1.Edicion = model.Edicion;
                    oDesepla1.Nrocri = model.Nrocri;
                    oDesepla1.Desde = model.Desde;
                    oDesepla1.Hasta = model.Hasta;
                    oDesepla1.Fecdecl = model.Fecdecl;
                    oDesepla1.Fchrecepcion = model.Fchrecepcion;
                    db.Entry(oDesepla1).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Desepla1>> oRespuesta = new Respuesta<List<Desepla1>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Desepla1 oDesepla1 = db.Desepla1s.Find(Id);
                    db.Remove(oDesepla1);
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
