using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Server.Context;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
namespace PaginaToros.Server.Cont{
    [Route("api/[controller]")]
    [ApiController]
     
    public class PlantelController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Plantel> oRespuesta = new Respuesta<Plantel>();

            try
            {
                using (hereford_prContext db = new())
                {
                        
                    var lst = db.Planteles
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
            Respuesta<List<Plantel>> oRespuesta = new Respuesta<List<Plantel>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    var lst = db.Planteles.ToList();
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
        public IActionResult Add(Plantel model)
        {
            Respuesta<List<Plantel>> oRespuesta = new Respuesta<List<Plantel>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Plantel oPlantel = new Plantel();
                    oPlantel.Placod = model.Placod;
                    oPlantel.Anioex = model.Anioex;
                    oPlantel.Varede = model.Varede;
                    oPlantel.Vqcsrd = model.Vqcsrd;
                    oPlantel.Vqssrd = model.Vqssrd;
                    oPlantel.Varepr = model.Varepr;
                    oPlantel.Vqcsrp = model.Vqcsrp;
                    oPlantel.Vqssrp = model.Vqssrp;
                    oPlantel.Feulti = model.Feulti;
                    oPlantel.Nroins = model.Nroins;
                    oPlantel.Nrocri = model.Nrocri;
                    oPlantel.Catego = model.Catego;
                    oPlantel.Aniopa = model.Aniopa;
                    oPlantel.Urein = model.Urein;
                    oPlantel.FchUsu = model.FchUsu;
                    oPlantel.CodUsu = model.CodUsu;
                    oPlantel.Comment = model.Comment;
                    oPlantel.Estado = model.Estado;
                    oPlantel.Fecing = model.Fecing;
                    db.Planteles.Add(oPlantel);
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
        public IActionResult Edit(Plantel model)
        {
            Respuesta<List<Plantel>> oRespuesta = new Respuesta<List<Plantel>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Plantel oPlantel = db.Planteles.Find(model.Id);
                    oPlantel.Placod = model.Placod;
                    oPlantel.Anioex = model.Anioex;
                    oPlantel.Varede = model.Varede;
                    oPlantel.Vqcsrd = model.Vqcsrd;
                    oPlantel.Vqssrd = model.Vqssrd;
                    oPlantel.Varepr = model.Varepr;
                    oPlantel.Vqcsrp = model.Vqcsrp;
                    oPlantel.Vqssrp = model.Vqssrp;
                    oPlantel.Feulti = model.Feulti;
                    oPlantel.Nroins = model.Nroins;
                    oPlantel.Nrocri = model.Nrocri;
                    oPlantel.Catego = model.Catego;
                    oPlantel.Aniopa = model.Aniopa;
                    oPlantel.Urein = model.Urein;
                    oPlantel.FchUsu = model.FchUsu;
                    oPlantel.CodUsu = model.CodUsu;
                    oPlantel.Comment = model.Comment;
                    oPlantel.Estado = model.Estado;
                    oPlantel.Fecing = model.Fecing;

                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}

                    db.Entry(oPlantel).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Plantel>> oRespuesta = new Respuesta<List<Plantel>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Plantel oPlantel = db.Planteles.Find(Id);
                    db.Remove(oPlantel);
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
