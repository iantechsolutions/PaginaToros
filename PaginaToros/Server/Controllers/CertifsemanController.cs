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
                using (hereford_prContext db = new())
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
                using (hereford_prContext db = new hereford_prContext())
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
                using (hereford_prContext db = new hereford_prContext())
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
                using (hereford_prContext db = new hereford_prContext())
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
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Certifseman oCertifseman = db.Certifsemen.Find(Id);
                    db.Remove(oCertifseman);
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

