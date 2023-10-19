using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FutControlController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<FutControl> oRespuesta = new Respuesta<FutControl>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.FutControls
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
            Respuesta<List<FutControl>> oRespuesta = new Respuesta<List<FutControl>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.FutControls.ToList();
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
        public IActionResult Add(FutControl model)
        {
            Respuesta<List<FutControl>> oRespuesta = new Respuesta<List<FutControl>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    FutControl oFutControl = new FutControl();
                    oFutControl.NroTrans = model.NroTrans;
                    oFutControl.Fectrans = model.Fectrans;
                    oFutControl.Sven = model.Sven;
                    oFutControl.CategSv = model.CategSv;
                    oFutControl.Vnom = model.Vnom;
                    oFutControl.Scom = model.Scom;
                    oFutControl.CategSc = model.CategSc;
                    oFutControl.Cnom = model.Cnom;
                    oFutControl.Plantel = model.Plantel;
                    oFutControl.EdadCrias = model.EdadCrias;
                    oFutControl.CantHem = model.CantHem;
                    oFutControl.CantMach = model.CantMach;
                    oFutControl.PlantDest = model.PlantDest;
                    oFutControl.Incorp = model.Incorp;
                    oFutControl.Hemsta = model.Hemsta;
                    oFutControl.FchUsu = model.FchUsu;
                    oFutControl.CodUsu = model.CodUsu;

                    db.FutControls.Add(oFutControl);
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
        public IActionResult Edit(FutControl model)
        {
            Respuesta<List<FutControl>> oRespuesta = new Respuesta<List<FutControl>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    FutControl oFutControl = db.FutControls.Find(model.Id);
                    oFutControl.NroTrans = model.NroTrans;
                    oFutControl.Fectrans = model.Fectrans;
                    oFutControl.Sven = model.Sven;
                    oFutControl.CategSv = model.CategSv;
                    oFutControl.Vnom = model.Vnom;
                    oFutControl.Scom = model.Scom;
                    oFutControl.CategSc = model.CategSc;
                    oFutControl.Cnom = model.Cnom;
                    oFutControl.Plantel = model.Plantel;
                    oFutControl.EdadCrias = model.EdadCrias;
                    oFutControl.CantHem = model.CantHem;
                    oFutControl.CantMach = model.CantMach;
                    oFutControl.PlantDest = model.PlantDest;
                    oFutControl.Incorp = model.Incorp;
                    oFutControl.Hemsta = model.Hemsta;
                    oFutControl.FchUsu = model.FchUsu;
                    oFutControl.CodUsu = model.CodUsu;


                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oFutControl).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<FutControl>> oRespuesta = new Respuesta<List<FutControl>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    FutControl oFutControl = db.FutControls.Find(Id);
                    db.Remove(oFutControl);
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

