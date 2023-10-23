using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Context;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Solici1Controller : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Solici1> oRespuesta = new Respuesta<Solici1>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Solici1s
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
            Respuesta<List<Solici1>> oRespuesta = new Respuesta<List<Solici1>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    var lst = db.Solici1s.ToList();
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
        public IActionResult GetByRes(string nro)
        {
            Respuesta<Solici1> oRespuesta = new Respuesta<Solici1>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Solici1s
                    .Where(x => x.Nrosol == nro).First();
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
        public IActionResult Add(Solici1 model)
        {
            Respuesta<List<Solici1>> oRespuesta = new Respuesta<List<Solici1>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Solici1 oSolici1 = new Solici1();
                    var solvieja = db.Solici1s.OrderByDescending(x => x.Id).First();
                    oSolici1.Codest = model.Codest;
                    oSolici1.Nrosol = model.Nrosol;
                    oSolici1.Fecsol = model.Fecsol;
                    oSolici1.Lugar = model.Lugar;
                    oSolici1.Cantor = model.Cantor;
                    oSolici1.Cantvq = model.Cantvq;
                    oSolici1.Produc = model.Produc;
                    oSolici1.Reinsp = model.Reinsp;
                    oSolici1.Canvac = model.Canvac;
                    oSolici1.Canvaq = model.Canvaq;
                    oSolici1.EdadMinMac = model.EdadMinMac;
                    oSolici1.EdadMaxHem = model.EdadMaxHem;
                    oSolici1.EdadMinHem = model.EdadMinHem;
                    oSolici1.EdadMaxMac = model.EdadMaxMac;
                    oSolici1.Tyncte = model.Tyncte;
                    oSolici1.Banco = model.Banco;
                    oSolici1.Import = model.Import;
                    oSolici1.Fecins = model.Fecins;
                    oSolici1.Ctrl1 = model.Ctrl1;
                    oSolici1.Ctrl2 = model.Ctrl2;
                    oSolici1.Fecret = model.Fecret;
                    oSolici1.FchUsu = model.FchUsu;
                    oSolici1.CodUsu = model.CodUsu;
                    db.Solici1s.Add(oSolici1);
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
        public IActionResult Edit(Solici1 model)
        {
            Respuesta<List<Solici1>> oRespuesta = new Respuesta<List<Solici1>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Solici1 oSolici1 = db.Solici1s.Find(model.Id);
                    oSolici1.Codest = model.Codest;
                    oSolici1.Nrosol = model.Nrosol;
                    oSolici1.Fecsol = model.Fecsol;
                    oSolici1.Lugar = model.Lugar;
                    oSolici1.Cantor = model.Cantor;
                    oSolici1.Cantvq = model.Cantvq;
                    oSolici1.Produc = model.Produc;
                    oSolici1.Reinsp = model.Reinsp;
                    oSolici1.Canvac = model.Canvac;
                    oSolici1.Canvaq = model.Canvaq;
                    oSolici1.EdadMinMac = model.EdadMinMac;
                    oSolici1.EdadMaxHem = model.EdadMaxHem;
                    oSolici1.EdadMinHem = model.EdadMinHem;
                    oSolici1.EdadMaxMac = model.EdadMaxMac;
                    oSolici1.Tyncte = model.Tyncte;
                    oSolici1.Banco = model.Banco;
                    oSolici1.Import = model.Import;
                    oSolici1.Fecins = model.Fecins;
                    oSolici1.Ctrl1 = model.Ctrl1;
                    oSolici1.Ctrl2 = model.Ctrl2;
                    oSolici1.Fecret = model.Fecret;
                    oSolici1.FchUsu = model.FchUsu;
                    oSolici1.CodUsu = model.CodUsu;
                    db.Entry(oSolici1).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Solici1>> oRespuesta = new Respuesta<List<Solici1>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Solici1 oSolici1 = db.Solici1s.Find(Id);
                    db.Remove(oSolici1);
                    //var dbToros = db.Toros.Where(x => x.IdEst == Id);
                    //foreach (Toro oElement in dbToros)
                    //{
                    //    db.Remove(oElement);
                    //}
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
