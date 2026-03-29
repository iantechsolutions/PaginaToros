using PaginaToros.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models.Request;
using PaginaToros.Server.Context;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AspNetRoleController : ControllerBase
    {
        private readonly hereford_prContext _db;

        public AspNetRoleController(hereford_prContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            Respuesta<List<AspNetRole>> oRespuesta = new Respuesta<List<AspNetRole>>();

            try
            {
                var lst = _db.AspNetRoles.ToList();
                oRespuesta.Exito = 1;
                oRespuesta.List = lst;
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }
        [HttpPost]
        public IActionResult Add(AspNetRole model)
        {
            Respuesta<object> oRespuesta = new Respuesta<object>();

            try
            {
                AspNetRole oAspNetRoles = new AspNetRole();
                oAspNetRoles.AspNetUserRoles = model.AspNetUserRoles;
                oAspNetRoles.AspNetRoleClaims = model.AspNetRoleClaims;
                oAspNetRoles.Id = model.Id;
                oAspNetRoles.ConcurrencyStamp = model.ConcurrencyStamp;

                _db.AspNetRoles.Add(oAspNetRoles);
                _db.SaveChanges();
                oRespuesta.Exito = 1;
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }
        [HttpPut]
        public IActionResult Edit(AspnetroleRequest model)
        {
            Respuesta<object> oRespuesta = new Respuesta<object>();

            try
            {
                AspNetRole oAspNetRoles = _db.AspNetRoles.Find(model.Id);
                oAspNetRoles.AspNetUserRoles = model.Aspnetuserroles;
                oAspNetRoles.AspNetRoleClaims = model.Aspnetroleclaims;
                oAspNetRoles.ConcurrencyStamp = model.ConcurrencyStamp;
                _db.Entry(oAspNetRoles).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _db.SaveChanges();
                oRespuesta.Exito = 1;
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            Respuesta<object> oRespuesta = new Respuesta<object>();

            try
            {
                AspNetRole oAspNetRoles = _db.AspNetRoles.Find(Id);
                _db.Remove(oAspNetRoles);
                _db.SaveChanges();
                oRespuesta.Exito = 1;
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }
        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            Respuesta<object> oRespuesta = new Respuesta<object>();

            try
            {
                var lst = _db.AspNetRoles.Find(Id);
                oRespuesta.Exito = 1;
                oRespuesta.List = lst;
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }

    }

}

