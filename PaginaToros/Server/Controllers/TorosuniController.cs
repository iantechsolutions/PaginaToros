using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Context;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TorosuniController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Torosuni> oRespuesta = new Respuesta<Torosuni>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.Torosunis
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
            Respuesta<List<Torosuni>> oRespuesta = new Respuesta<List<Torosuni>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.Torosunis.ToList();
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
        public IActionResult Add(Torosuni model)
        {
            Respuesta<List<Torosuni>> oRespuesta = new Respuesta<List<Torosuni>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Torosuni oTorosuni = new Torosuni();
                    oTorosuni.Apodo = model.Apodo;
                    oTorosuni.Nombre = model.Nombre;
                    oTorosuni.NombreSocio = model.NombreSocio;
                    oTorosuni.EstDoc = model.EstDoc;
                    oTorosuni.ResInsp = model.ResInsp;
                    oTorosuni.SbcodOld = model.SbcodOld;
                    oTorosuni.Sbcod = model.Sbcod;
                    oTorosuni.TipToro = model.TipToro;
                    oTorosuni.Tatpart = model.Tatpart;
                    oTorosuni.NomDad = model.NomDad;
                    oTorosuni.NrInsc = model.NrInsc;
                    oTorosuni.NrTsan = model.NrTsan;
                    oTorosuni.NrInsd = model.NrInsd;
                    oTorosuni.Fecha = model.Fecha;
                    oTorosuni.Hba = model.Hba;
                    oTorosuni.Variedad = model.Variedad;
                    oTorosuni.Criador = model.Criador;
                    oTorosuni.Catego = model.Catego;
                    oTorosuni.Plantel = model.Plantel;
                    oTorosuni.Estcod = model.Estcod;
                    oTorosuni.FchBaja = model.FchBaja;
                    oTorosuni.Activo = model.Activo;
                    oTorosuni.MotivoBaj = model.MotivoBaj;
                    oTorosuni.Nacido = model.Nacido;
                    oTorosuni.Actualizado = model.Actualizado;
                    oTorosuni.CircEscrotal = model.CircEscrotal;
                    oTorosuni.CodEstado = model.CodEstado;
                    oTorosuni.IdTipo = model.IdTipo;
                    oTorosuni.FchUsu = model.FchUsu;
                    oTorosuni.CodUsu = model.CodUsu;
                    oTorosuni.Fecing = model.Fecing;
                    oTorosuni.Fechasba = model.Fechasba;
                    oTorosuni.Pnac = model.Pnac;
                    oTorosuni.Pajudest = model.Pajudest;
                    oTorosuni.Pajufinal = model.Pajufinal;
                    oTorosuni.Gdpostdest = model.Gdpostdest;
                    oTorosuni.Indicedest = model.Indicedest;
                    oTorosuni.Cescrot = model.Cescrot;
                    oTorosuni.Otros1 = model.Otros1;
                    oTorosuni.Promgrupo1 = model.Promgrupo1;
                    oTorosuni.Promgrupo2 = model.Promgrupo2;
                    oTorosuni.Gdvida = model.Gdvida;
                    oTorosuni.Indicefinal = model.Indicefinal;
                    oTorosuni.Frame = model.Frame;
                    oTorosuni.Otros2 = model.Otros2;
                    oTorosuni.Comentario = model.Comentario;

                    db.Torosunis.Add(oTorosuni);
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
        public IActionResult Edit(Torosuni model)
        {
            Respuesta<List<Torosuni>> oRespuesta = new Respuesta<List<Torosuni>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Torosuni oTorosuni = db.Torosunis.Find(model.Id);
                    oTorosuni.Apodo = model.Apodo;
                    oTorosuni.Nombre = model.Nombre;
                    oTorosuni.NombreSocio = model.NombreSocio;
                    oTorosuni.EstDoc = model.EstDoc;
                    oTorosuni.ResInsp = model.ResInsp;
                    oTorosuni.SbcodOld = model.SbcodOld;
                    oTorosuni.Sbcod = model.Sbcod;
                    oTorosuni.TipToro = model.TipToro;
                    oTorosuni.Tatpart = model.Tatpart;
                    oTorosuni.NomDad = model.NomDad;
                    oTorosuni.NrInsc = model.NrInsc;
                    oTorosuni.NrTsan = model.NrTsan;
                    oTorosuni.NrInsd = model.NrInsd;
                    oTorosuni.Fecha = model.Fecha;
                    oTorosuni.Hba = model.Hba;
                    oTorosuni.Variedad = model.Variedad;
                    oTorosuni.Criador = model.Criador;
                    oTorosuni.Catego = model.Catego;
                    oTorosuni.Plantel = model.Plantel;
                    oTorosuni.Estcod = model.Estcod;
                    oTorosuni.FchBaja = model.FchBaja;
                    oTorosuni.Activo = model.Activo;
                    oTorosuni.MotivoBaj = model.MotivoBaj;
                    oTorosuni.Nacido = model.Nacido;
                    oTorosuni.Actualizado = model.Actualizado;
                    oTorosuni.CircEscrotal = model.CircEscrotal;
                    oTorosuni.CodEstado = model.CodEstado;
                    oTorosuni.IdTipo = model.IdTipo;
                    oTorosuni.FchUsu = model.FchUsu;
                    oTorosuni.CodUsu = model.CodUsu;
                    oTorosuni.Fecing = model.Fecing;
                    oTorosuni.Fechasba = model.Fechasba;
                    oTorosuni.Pnac = model.Pnac;
                    oTorosuni.Pajudest = model.Pajudest;
                    oTorosuni.Pajufinal = model.Pajufinal;
                    oTorosuni.Gdpostdest = model.Gdpostdest;
                    oTorosuni.Indicedest = model.Indicedest;
                    oTorosuni.Cescrot = model.Cescrot;
                    oTorosuni.Otros1 = model.Otros1;
                    oTorosuni.Promgrupo1 = model.Promgrupo1;
                    oTorosuni.Promgrupo2 = model.Promgrupo2;
                    oTorosuni.Gdvida = model.Gdvida;
                    oTorosuni.Indicefinal = model.Indicefinal;
                    oTorosuni.Frame = model.Frame;
                    oTorosuni.Otros2 = model.Otros2;
                    oTorosuni.Comentario = model.Comentario;


                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oTorosuni).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Torosuni>> oRespuesta = new Respuesta<List<Torosuni>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    Torosuni oTorosuni = db.Torosunis.Find(Id);
                    db.Remove(oTorosuni);
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

