using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HembrasYmachosReController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<HembrasYmachosRe> oRespuesta = new Respuesta<HembrasYmachosRe>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.HembrasYmachosRes
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
            Respuesta<List<HembrasYmachosRe>> oRespuesta = new Respuesta<List<HembrasYmachosRe>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.HembrasYmachosRes.ToList();
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
        public IActionResult GetByRes(int nro)
        {
            Respuesta<List<HembrasYmachosRe>> oRespuesta = new Respuesta<List<HembrasYmachosRe>>();

            try
            {
                using (BlazorCrudContext db = new())
                {

                    var lst = db.HembrasYmachosRes
                    .Where(x => x.Nrores == nro).ToList();
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
        public IActionResult Add(HembrasYmachosRe model)
        {
            Respuesta<List<HembrasYmachosRe>> oRespuesta = new Respuesta<List<HembrasYmachosRe>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    HembrasYmachosRe oHembrasYmachosRe = new HembrasYmachosRe();
                    oHembrasYmachosRe.Hdp = model.Hdp;
                    oHembrasYmachosRe.HdpM = model.HdpM;
                    oHembrasYmachosRe.HdpAs = model.HdpAs;
                    oHembrasYmachosRe.Hdt = model.Hdt;
                    oHembrasYmachosRe.Hdb = model.Hdb;
                    oHembrasYmachosRe.Hpp = model.Hpp;
                    oHembrasYmachosRe.HppM = model.HppM;
                    oHembrasYmachosRe.HppAs = model.HppAs;
                    oHembrasYmachosRe.Hpt = model.Hpt;
                    oHembrasYmachosRe.Hpb = model.Hpb;
                    oHembrasYmachosRe.Hgvp = model.Hgvp;
                    oHembrasYmachosRe.Hgvb = model.Hgvb;
                    oHembrasYmachosRe.Hgqp = model.Hgqp;
                    oHembrasYmachosRe.Hgqb = model.Hgqb;
                    oHembrasYmachosRe.Mcp = model.Mcp;
                    oHembrasYmachosRe.McpM = model.McpM;
                    oHembrasYmachosRe.McpAs = model.McpAs;
                    oHembrasYmachosRe.Mct = model.Mct;
                    oHembrasYmachosRe.Msp = model.Msp;
                    oHembrasYmachosRe.MspM = model.MspM;
                    oHembrasYmachosRe.MspAs = model.MspAs;
                    oHembrasYmachosRe.Mst = model.Mst;
                    oHembrasYmachosRe.Mspsb = model.Mspsb;
                    oHembrasYmachosRe.Nrores = model.Nrores;

                    db.HembrasYmachosRes.Add(oHembrasYmachosRe);
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
        public IActionResult Edit(HembrasYmachosRe model)
        {
            Respuesta<List<HembrasYmachosRe>> oRespuesta = new Respuesta<List<HembrasYmachosRe>>();
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    HembrasYmachosRe oHembrasYmachosRe = db.HembrasYmachosRes.Find(model.Id);
                    oHembrasYmachosRe.Hdp = model.Hdp;
                    oHembrasYmachosRe.HdpM = model.HdpM;
                    oHembrasYmachosRe.HdpAs = model.HdpAs;
                    oHembrasYmachosRe.Hdt = model.Hdt;
                    oHembrasYmachosRe.Hdb = model.Hdb;
                    oHembrasYmachosRe.Hpp = model.Hpp;
                    oHembrasYmachosRe.HppM = model.HppM;
                    oHembrasYmachosRe.HppAs = model.HppAs;
                    oHembrasYmachosRe.Hpt = model.Hpt;
                    oHembrasYmachosRe.Hpb = model.Hpb;
                    oHembrasYmachosRe.Hgvp = model.Hgvp;
                    oHembrasYmachosRe.Hgvb = model.Hgvb;
                    oHembrasYmachosRe.Hgqp = model.Hgqp;
                    oHembrasYmachosRe.Hgqb = model.Hgqb;
                    oHembrasYmachosRe.Mcp = model.Mcp;
                    oHembrasYmachosRe.McpM = model.McpM;
                    oHembrasYmachosRe.McpAs = model.McpAs;
                    oHembrasYmachosRe.Mct = model.Mct;
                    oHembrasYmachosRe.Msp = model.Msp;
                    oHembrasYmachosRe.MspM = model.MspM;
                    oHembrasYmachosRe.MspAs = model.MspAs;
                    oHembrasYmachosRe.Mst = model.Mst;
                    oHembrasYmachosRe.Mspsb = model.Mspsb;
                    oHembrasYmachosRe.Nrores = model.Nrores;

                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oHembrasYmachosRe).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<HembrasYmachosRe>> oRespuesta = new Respuesta<List<HembrasYmachosRe>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    HembrasYmachosRe oHembrasYmachosRe = db.HembrasYmachosRes.Find(Id);
                    db.Remove(oHembrasYmachosRe);
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
