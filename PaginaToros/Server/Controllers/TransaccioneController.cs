using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Request;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Server.Controllers
{
    [Route("api/TransaccioneData")]
    [ApiController]
    public class TransaccioneController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Transaccione> oRespuesta = new Respuesta<Transaccione>();

            try
            {
                using (BlazorCrudContext db = new()) { 

                    var lst = db.Transacciones
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
            Respuesta<List<Transaccione>> oRespuesta = new Respuesta<List<Transaccione>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.Transacciones.ToList();
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
        public IActionResult Add(Transaccione model)
        {
            Respuesta<List<Transaccione>> oRespuesta = new Respuesta<List<Transaccione>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Transaccione oTransaccione = new Transaccione();
                    oTransaccione.NombreVendedor = model.NombreVendedor;
                    oTransaccione.NombreComprador = model.NombreComprador;
                    oTransaccione.Fecha = model.Fecha;
                    oTransaccione.TotalToros = model.TotalToros;
                    oTransaccione.TotalVaquillonas = model.TotalVaquillonas;
                    oTransaccione.Toros = model.Toros;
                    db.Transacciones.Add(oTransaccione);
                    var ListaToros = model.Toros.Split(", ").ToList<string>();
                    foreach (string nombre in ListaToros){
                        Toro oToro = db.Toros.Where(x => x.Nombre == nombre && x.NombreEst==model.NombreVendedor).First();
                        oToro.NombreEst = model.NombreComprador;
                        var oEst = db.Establecimientos.Where(x => x.Nombre == model.NombreComprador).First();
                        oToro.IdEst = oEst.Id;
                        db.Entry(oToro).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
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
        public IActionResult Edit(Transaccione model)
        {
            Respuesta<List<Transaccione>> oRespuesta = new Respuesta<List<Transaccione>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Transaccione oTransaccione = db.Transacciones.Find(model.Id);
                    oTransaccione.NombreVendedor = model.NombreVendedor;
                    oTransaccione.NombreComprador = model.NombreComprador;
                    oTransaccione.Fecha = model.Fecha;
                    oTransaccione.TotalToros = model.TotalToros;
                    oTransaccione.TotalVaquillonas = model.TotalVaquillonas;
                    oTransaccione.Toros = model.Toros;
                    db.Entry(oTransaccione).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
            Respuesta<List<Transaccione>> oRespuesta = new Respuesta<List<Transaccione>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Transaccione oTransaccione = db.Transacciones.Find(Id);
                    db.Remove(oTransaccione);
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
