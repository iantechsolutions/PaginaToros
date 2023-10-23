using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Server.Context;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransanController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Transan> oRespuesta = new Respuesta<Transan>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Transans
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
            Respuesta<List<Transan>> oRespuesta = new Respuesta<List<Transan>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    var lst = db.Transans.ToList();
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
        public IActionResult Add(Transan model)
        {
            Respuesta<List<Transan>> oRespuesta = new Respuesta<List<Transan>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Transan oTransan = new Transan();
                    oTransan.NroCert = model.NroCert;
                    oTransan.Fecvta = model.Fecvta;
                    oTransan.Sven = model.Sven;
                    oTransan.CategSv = model.CategSv;
                    oTransan.Vnom = model.Vnom;
                    oTransan.Scom = model.Scom;
                    oTransan.CategSc = model.CategSc;
                    oTransan.Cnom = model.Cnom;
                    oTransan.Plant = model.Plant;
                    oTransan.NvoPla = model.NvoPla;
                    oTransan.CantHem = model.CantHem;
                    oTransan.CantMach = model.CantMach;
                    oTransan.Tiphac = model.Tiphac;
                    oTransan.Hemsta = model.Hemsta;
                    oTransan.Tipani = model.Tipani;
                    oTransan.Incorp = model.Incorp;
                    oTransan.Tipohem = model.Tipohem;
                    oTransan.CantChem = model.CantChem;
                    oTransan.CantCmach = model.CantCmach;
                    oTransan.FchUsu = model.FchUsu;
                    oTransan.CodUsu = model.CodUsu;

                    db.Transans.Add(oTransan);
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
        public IActionResult Edit(Transan model)
        {
            Respuesta<List<Transan>> oRespuesta = new Respuesta<List<Transan>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Transan oTransan = db.Transans.Find(model.Id);
                    oTransan.NroCert = model.NroCert;
                    oTransan.Fecvta = model.Fecvta;
                    oTransan.Sven = model.Sven;
                    oTransan.CategSv = model.CategSv;
                    oTransan.Vnom = model.Vnom;
                    oTransan.Scom = model.Scom;
                    oTransan.CategSc = model.CategSc;
                    oTransan.Cnom = model.Cnom;
                    oTransan.Plant = model.Plant;
                    oTransan.NvoPla = model.NvoPla;
                    oTransan.CantHem = model.CantHem;
                    oTransan.CantMach = model.CantMach;
                    oTransan.Tiphac = model.Tiphac;
                    oTransan.Hemsta = model.Hemsta;
                    oTransan.Tipani = model.Tipani;
                    oTransan.Incorp = model.Incorp;
                    oTransan.Tipohem = model.Tipohem;
                    oTransan.CantChem = model.CantChem;
                    oTransan.CantCmach = model.CantCmach;
                    oTransan.FchUsu = model.FchUsu;
                    oTransan.CodUsu = model.CodUsu;


                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oTransan).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Transan>> oRespuesta = new Respuesta<List<Transan>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Transan oTransan = db.Transans.Find(Id);
                    db.Remove(oTransan);
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
