using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Server.Context;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Resin6Controller : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Resin6> oRespuesta = new Respuesta<Resin6>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Resin6s
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
            Respuesta<List<Resin6>> oRespuesta = new Respuesta<List<Resin6>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    var lst = db.Resin6s.ToList();
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
        public IActionResult GetByRes(string nro)
        {
            Respuesta<List<Resin6>> oRespuesta = new Respuesta<List<Resin6>>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Resin6s
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
        public IActionResult Add(Resin6 model)
        {
            Respuesta<List<Resin6>> oRespuesta = new Respuesta<List<Resin6>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Resin6 oResin6 = new Resin6();
                    oResin6.Hdp = model.Hdp;
                    oResin6.HdpM = model.HdpM;
                    oResin6.HdpAs = model.HdpAs;
                    oResin6.Hdt = model.Hdt;
                    oResin6.Hdb = model.Hdb;
                    oResin6.Hpp = model.Hpp;
                    oResin6.HppM = model.HppM;
                    oResin6.HppAs = model.HppAs;
                    oResin6.Hpt = model.Hpt;
                    oResin6.Hpb = model.Hpb;
                    oResin6.Hgvp = model.Hgvp;
                    oResin6.Hgvb = model.Hgvb;
                    oResin6.Hgqp = model.Hgqp;
                    oResin6.Hgqb = model.Hgqb;
                    oResin6.Mcp = model.Mcp;
                    oResin6.McpM = model.McpM;
                    oResin6.McpAs = model.McpAs;
                    oResin6.Mct = model.Mct;
                    oResin6.Msp = model.Msp;
                    oResin6.MspM = model.MspM;
                    oResin6.MspAs = model.MspAs;
                    oResin6.Mst = model.Mst;
                    oResin6.Mspsb = model.Mspsb;
                    oResin6.Nrores = model.Nrores;

                    db.Resin6s.Add(oResin6);
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
        public IActionResult Edit(Resin6 model)
        {
            Respuesta<List<Resin6>> oRespuesta = new Respuesta<List<Resin6>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Resin6 oResin6 = db.Resin6s.Find(model.Id);
                    oResin6.Hdp = model.Hdp;
                    oResin6.HdpM = model.HdpM;
                    oResin6.HdpAs = model.HdpAs;
                    oResin6.Hdt = model.Hdt;
                    oResin6.Hdb = model.Hdb;
                    oResin6.Hpp = model.Hpp;
                    oResin6.HppM = model.HppM;
                    oResin6.HppAs = model.HppAs;
                    oResin6.Hpt = model.Hpt;
                    oResin6.Hpb = model.Hpb;
                    oResin6.Hgvp = model.Hgvp;
                    oResin6.Hgvb = model.Hgvb;
                    oResin6.Hgqp = model.Hgqp;
                    oResin6.Hgqb = model.Hgqb;
                    oResin6.Mcp = model.Mcp;
                    oResin6.McpM = model.McpM;
                    oResin6.McpAs = model.McpAs;
                    oResin6.Mct = model.Mct;
                    oResin6.Msp = model.Msp;
                    oResin6.MspM = model.MspM;
                    oResin6.MspAs = model.MspAs;
                    oResin6.Mst = model.Mst;
                    oResin6.Mspsb = model.Mspsb;
                    oResin6.Nrores = model.Nrores;

                    //TorosPorId = db.Toros.Where(row => row.IdEst == model.Id);
                    //foreach (var row in TorosPorId)
                    //{
                    //    row.NombreEst = model.Nombre;
                    //    db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //}
                    db.Entry(oResin6).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Resin6>> oRespuesta = new Respuesta<List<Resin6>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Resin6 oResin6 = db.Resin6s.Find(Id);
                    db.Remove(oResin6);
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
