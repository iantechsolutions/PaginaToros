using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanteleController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Plantele> oRespuesta = new Respuesta<Plantele>();

            try
            {
                using (BlazorCrudContext db = new())
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
            Respuesta<List<Plantele>> oRespuesta = new Respuesta<List<Plantele>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
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
        public IActionResult Add(Plantele model)
        {
            Respuesta<List<Plantele>> oRespuesta = new Respuesta<List<Plantele>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Plantele oPlantel = new Plantele();
                    oPlantel.NroPlantel = model.NroPlantel;
                    oPlantel.Activo = model.Activo;
                    oPlantel.FechaExistencia = model.FechaExistencia;
                    oPlantel.NroUltInspeccion = model.NroUltInspeccion;
                    oPlantel.UltimaInspeccion = model.UltimaInspeccion;
                    oPlantel.UltimaReinspeccion = model.UltimaReinspeccion;
                    oPlantel.Socio = model.Socio;
                    oPlantel.Vacas = model.Vacas;
                    oPlantel.VaquillServicio = model.VaquillServicio;
                    oPlantel.VaquillNoServicio = model.VaquillNoServicio;
                    oPlantel.VacasVip = model.VacasVip;
                    oPlantel.PrenadasVip = model.PrenadasVip;
                    oPlantel.VaquillNoServicioVip = model.VaquillNoServicioVip;
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
        public IActionResult Edit(Plantele model)
        {
            Respuesta<List<Plantele>> oRespuesta = new Respuesta<List<Plantele>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Plantele oPlantel = db.Planteles.Find(model.Id);
                    oPlantel.NroPlantel = model.NroPlantel;
                    oPlantel.Activo = model.Activo;
                    oPlantel.FechaExistencia = model.FechaExistencia;
                    oPlantel.NroUltInspeccion = model.NroUltInspeccion;
                    oPlantel.UltimaInspeccion = model.UltimaInspeccion;
                    oPlantel.UltimaReinspeccion = model.UltimaReinspeccion;
                    oPlantel.Socio = model.Socio;
                    oPlantel.Vacas = model.Vacas;
                    oPlantel.VaquillServicio = model.VaquillServicio;
                    oPlantel.VaquillNoServicio = model.VaquillNoServicio;
                    oPlantel.VacasVip = model.VacasVip;
                    oPlantel.PrenadasVip = model.PrenadasVip;
                    oPlantel.VaquillNoServicioVip = model.VaquillNoServicioVip;

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
            Respuesta<List<Plantele>> oRespuesta = new Respuesta<List<Plantele>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Plantele oPlantele = db.Planteles.Find(Id);
                    db.Remove(oPlantele);
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
