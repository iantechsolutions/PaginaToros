using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Context;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudInspeccionController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<SolicitudInspeccion> oRespuesta = new Respuesta<SolicitudInspeccion>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.SolicitudInspeccions
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
            Respuesta<List<SolicitudInspeccion>> oRespuesta = new Respuesta<List<SolicitudInspeccion>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.SolicitudInspeccions.ToList();
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

        [HttpGet("pendientes")]
        public IActionResult GetByCompleted()
        {
            Respuesta<List<SolicitudInspeccion>> oRespuesta = new();

            try
            {
                using BlazorCrudContext db = new();

                var lst = db.SolicitudInspeccions
                    .Where(x => x.Completada == false).ToList();
                oRespuesta.Exito = 1;
                oRespuesta.List = lst;
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }

        [HttpGet("Nrores/{nro}")]
        public IActionResult GetByRes(int nro)
        {
            Respuesta<SolicitudInspeccion> oRespuesta = new Respuesta<SolicitudInspeccion>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.SolicitudInspeccions
                    .Where(x => x.NroSolicitud == nro).First();
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
        public IActionResult Add(SolicitudInspeccion model)
        {
            Respuesta<List<SolicitudInspeccion>> oRespuesta = new Respuesta<List<SolicitudInspeccion>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    SolicitudInspeccion oSolicitudInspeccion = new SolicitudInspeccion();
                    oSolicitudInspeccion.NroSolicitud = model.NroSolicitud;
                    oSolicitudInspeccion.NroSocio = model.NroSocio;
                    oSolicitudInspeccion.Activo = model.Activo;
                    oSolicitudInspeccion.NombreSocio = model.NombreSocio;
                    oSolicitudInspeccion.Establecimiento = model.Establecimiento;
                    oSolicitudInspeccion.FechaSolicitud = model.FechaSolicitud;
                    oSolicitudInspeccion.FechaAutorizacion = model.FechaAutorizacion;
                    oSolicitudInspeccion.Reinspeccion = model.Reinspeccion;
                    oSolicitudInspeccion.ControlProduccion = model.ControlProduccion;
                    oSolicitudInspeccion.Completada = model.Completada;
                    oSolicitudInspeccion.Min = model.Min;
                    oSolicitudInspeccion.Max = model.Max;
                    oSolicitudInspeccion.Ano = model.Ano;
                    oSolicitudInspeccion.ToroPr = model.ToroPr;
                    oSolicitudInspeccion.VcPr = model.VcPr;
                    oSolicitudInspeccion.VcVip = model.VcVip;
                    oSolicitudInspeccion.VqVip = model.VqVip;
                    db.SolicitudInspeccions.Add(oSolicitudInspeccion);
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
        public IActionResult Edit(SolicitudInspeccion model)
        {
            Respuesta<List<SolicitudInspeccion>> oRespuesta = new Respuesta<List<SolicitudInspeccion>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    SolicitudInspeccion oSolicitudInspeccion = db.SolicitudInspeccions.Find(model.Id);
                    oSolicitudInspeccion.NroSolicitud = model.NroSolicitud;
                    oSolicitudInspeccion.NroSocio = model.NroSocio;
                    oSolicitudInspeccion.Activo = model.Activo;
                    oSolicitudInspeccion.NombreSocio = model.NombreSocio;
                    oSolicitudInspeccion.Establecimiento = model.Establecimiento;
                    oSolicitudInspeccion.FechaSolicitud = model.FechaSolicitud;
                    oSolicitudInspeccion.FechaAutorizacion = model.FechaAutorizacion;
                    oSolicitudInspeccion.Reinspeccion = model.Reinspeccion;
                    oSolicitudInspeccion.ControlProduccion = model.ControlProduccion;
                    oSolicitudInspeccion.Completada = model.Completada;
                    oSolicitudInspeccion.Min = model.Min;
                    oSolicitudInspeccion.Max = model.Max;
                    oSolicitudInspeccion.Ano = model.Ano;
                    oSolicitudInspeccion.ToroPr = model.ToroPr;
                    oSolicitudInspeccion.VcPr = model.VcPr;
                    oSolicitudInspeccion.VcVip = model.VcVip;
                    oSolicitudInspeccion.VqVip = model.VqVip;
                    db.Entry(oSolicitudInspeccion).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<SolicitudInspeccion>> oRespuesta = new Respuesta<List<SolicitudInspeccion>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    SolicitudInspeccion oSolicitudInspeccion = db.SolicitudInspeccions.Find(Id);
                    db.Remove(oSolicitudInspeccion);
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
