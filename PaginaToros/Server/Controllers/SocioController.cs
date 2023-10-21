using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Client.Pages.Socios;
using PaginaToros.Server.Context;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocioController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Socio> oRespuesta = new Respuesta<Socio>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.Socios
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
            Respuesta<List<Socio>> oRespuesta = new Respuesta<List<Socio>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.Socios.ToList();
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
            Respuesta<Socio> oRespuesta = new Respuesta<Socio>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.Socios
                    .Where(x => x.NroSocio == nro).First();
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
        public IActionResult Add(Socio model)
        {
            Respuesta<List<Socio>> oRespuesta = new Respuesta<List<Socio>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Socio oSocio = new Socio();
                    var socioViejo = db.Socios.OrderByDescending(x => x.Id).First();
                    oSocio.NroSocio = socioViejo.NroSocio+1;
                    oSocio.Activo = model.Activo;
                    oSocio.NombreCompleto = model.NombreCompleto;
                    oSocio.Domicilio = model.Domicilio;
                    oSocio.Telefono = model.Telefono;
                    oSocio.Localidad = model.Localidad;
                    oSocio.CodPostal = model.CodPostal;
                    oSocio.Provincia = model.Provincia;
                    oSocio.Telefono2 = model.Telefono2;
                    oSocio.Mail = model.Mail;
                    oSocio.FechaExistencia = model.FechaExistencia;
                    oSocio.UltimoPlantel = model.UltimoPlantel;
                    db.Socios.Add(oSocio);
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
        public IActionResult Edit(Socio model)
        {
            Respuesta<List<Socio>> oRespuesta = new Respuesta<List<Socio>>();
            IQueryable<Toro> TorosPorId; ;
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Socio oSocio = db.Socios.Find(model.Id);
                    oSocio.NroSocio = model.NroSocio;
                    oSocio.Activo = model.Activo;
                    oSocio.NombreCompleto = model.NombreCompleto;
                    oSocio.Domicilio = model.Domicilio;
                    oSocio.Telefono = model.Telefono;
                    oSocio.Localidad = model.Localidad;
                    oSocio.CodPostal = model.CodPostal;
                    oSocio.Provincia = model.Provincia;
                    oSocio.Telefono2 = model.Telefono2;
                    oSocio.Mail = model.Mail;
                    oSocio.FechaExistencia = model.FechaExistencia;
                    oSocio.UltimoPlantel = model.UltimoPlantel;
                    db.Entry(oSocio).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Socio>> oRespuesta = new Respuesta<List<Socio>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Socio oSocio = db.Socios.Find(Id);
                    db.Remove(oSocio);
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
