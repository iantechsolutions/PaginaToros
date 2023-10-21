using Microsoft.AspNetCore.Mvc;
using PaginaToros.Server.Context;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Request;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstablecimientoController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Establecimiento> oRespuesta = new Respuesta<Establecimiento>();
            
            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.Establecimientos
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
            Respuesta<List<Establecimiento>> oRespuesta = new Respuesta<List<Establecimiento>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.Establecimientos.ToList();
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

        [HttpGet("Codigo/{nro}")]
        public IActionResult GetByCod(int nro)
        {
            Respuesta<Establecimiento> oRespuesta = new Respuesta<Establecimiento>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.Establecimientos
                    .Where(x => x.Codigo == nro).First();
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
        public IActionResult Add(Establecimiento model)
        {
            Respuesta<List<Establecimiento>> oRespuesta = new Respuesta<List<Establecimiento>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Establecimiento oEstablecimiento = new Establecimiento();
                    var estviejo = db.Establecimientos.OrderByDescending(x => x.Id).First();
                    oEstablecimiento.Codigo = estviejo.Codigo + 1;
                    oEstablecimiento.NombreSocio = model.NombreSocio;
                    oEstablecimiento.CodigoSocio = model.CodigoSocio;
                    oEstablecimiento.Activo = model.Activo;
                    oEstablecimiento.Nombre = model.Nombre;
                    oEstablecimiento.Encargado = model.Encargado;
                    oEstablecimiento.Domicilio = model.Domicilio;
                    oEstablecimiento.Telefono = model.Telefono;
                    oEstablecimiento.Localidad = model.Localidad;
                    oEstablecimiento.CodPostal = model.CodPostal;
                    oEstablecimiento.Provincia = model.Provincia;
                    oEstablecimiento.Informacion = model.Informacion;
                    oEstablecimiento.CodZona = model.CodZona;
                    oEstablecimiento.FechaExistencia = model.FechaExistencia;
                    
                    db.Establecimientos.Add(oEstablecimiento);
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
        public IActionResult Edit(Establecimiento model)
        {
            Respuesta<List<Establecimiento>> oRespuesta = new Respuesta<List<Establecimiento>>();
            IQueryable<Toro> TorosPorId; ;
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Establecimiento oEstablecimiento = db.Establecimientos.Find(model.Id);
                    oEstablecimiento.Codigo = model.Codigo;
                    oEstablecimiento.NombreSocio = model.NombreSocio;
                    oEstablecimiento.CodigoSocio = model.CodigoSocio;
                    oEstablecimiento.Activo = model.Activo;
                    oEstablecimiento.Nombre = model.Nombre;
                    oEstablecimiento.Encargado = model.Encargado;
                    oEstablecimiento.Domicilio = model.Domicilio;
                    oEstablecimiento.Telefono = model.Telefono;
                    oEstablecimiento.Localidad = model.Localidad;
                    oEstablecimiento.CodPostal = model.CodPostal;
                    oEstablecimiento.Provincia = model.Provincia;
                    oEstablecimiento.Informacion = model.Informacion;
                    oEstablecimiento.CodZona = model.CodZona;
                    oEstablecimiento.FechaExistencia = model.FechaExistencia;
                    
                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oEstablecimiento).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Establecimiento>> oRespuesta = new Respuesta<List<Establecimiento>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                Establecimiento oEstablecimiento = db.Establecimientos.Find(Id);
                db.Remove(oEstablecimiento);
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
