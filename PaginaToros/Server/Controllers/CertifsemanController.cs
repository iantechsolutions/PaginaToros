using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Context;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertifsemanController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Certifseman> oRespuesta = new Respuesta<Certifseman>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.Certifsemen
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
            Respuesta<List<Certifseman>> oRespuesta = new Respuesta<List<Certifseman>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.Certifsemen.ToList();
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
        public IActionResult Add(Certifseman model)
        {
            Respuesta<List<Certifseman>> oRespuesta = new Respuesta<List<Certifseman>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Certifseman oCertifseman = new Certifseman();
                    oCertifseman.TipoCert = model.TipoCert;
                    oCertifseman.NroConst = model.NroConst;
                    oCertifseman.NroCert = model.NroCert;
                    oCertifseman.Nrocen = model.Nrocen;
                    oCertifseman.Fecvta = model.Fecvta;
                    oCertifseman.FchConst = model.FchConst;
                    oCertifseman.Nven = model.Nven;
                    oCertifseman.Nrocri = model.Nrocri;
                    oCertifseman.CategSc = model.CategSc;
                    oCertifseman.Scod = model.Scod;
                    oCertifseman.Tatpart = model.Tatpart;
                    oCertifseman.Hba = model.Hba;
                    oCertifseman.NomDad = model.NomDad;
                    oCertifseman.NrInsc = model.NrInsc;
                    oCertifseman.NrTsan = model.NrTsan;
                    oCertifseman.NrInsd = model.NrInsd;
                    oCertifseman.NrDosi = model.NrDosi;
                    oCertifseman.NrDosiOr = model.NrDosiOr;
                    oCertifseman.TipEnv = model.TipEnv;
                    oCertifseman.Variedad = model.Variedad;
                    oCertifseman.FchUsu = model.FchUsu;
                    oCertifseman.CodUsu = model.CodUsu;
                    oCertifseman.Id = model.Id;
                    oCertifseman.Apodo = model.Apodo;

                    db.Certifsemen.Add(oCertifseman);
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
        public IActionResult Edit(Certifseman model)
        {
            Respuesta<List<Certifseman>> oRespuesta = new Respuesta<List<Certifseman>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Certifseman oCertifseman = db.Certifsemen.Find(model.Id);
                    oCertifseman.TipoCert = model.TipoCert;
                    oCertifseman.NroConst = model.NroConst;
                    oCertifseman.NroCert = model.NroCert;
                    oCertifseman.Nrocen = model.Nrocen;
                    oCertifseman.Fecvta = model.Fecvta;
                    oCertifseman.FchConst = model.FchConst;
                    oCertifseman.Nven = model.Nven;
                    oCertifseman.Nrocri = model.Nrocri;
                    oCertifseman.CategSc = model.CategSc;
                    oCertifseman.Scod = model.Scod;
                    oCertifseman.Tatpart = model.Tatpart;
                    oCertifseman.Hba = model.Hba;
                    oCertifseman.NomDad = model.NomDad;
                    oCertifseman.NrInsc = model.NrInsc;
                    oCertifseman.NrTsan = model.NrTsan;
                    oCertifseman.NrInsd = model.NrInsd;
                    oCertifseman.NrDosi = model.NrDosi;
                    oCertifseman.NrDosiOr = model.NrDosiOr;
                    oCertifseman.TipEnv = model.TipEnv;
                    oCertifseman.Variedad = model.Variedad;
                    oCertifseman.FchUsu = model.FchUsu;
                    oCertifseman.CodUsu = model.CodUsu;
                    oCertifseman.Id = model.Id;
                    oCertifseman.Apodo = model.Apodo;

                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oCertifseman).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Certifseman>> oRespuesta = new Respuesta<List<Certifseman>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Certifseman oCertifseman = db.Certifsemen.Find(Id);
                    db.Remove(oCertifseman);
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

