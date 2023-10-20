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
        [HttpGet]
        public IActionResult Get()
        {
            Respuesta<List<AspNetRole>> oRespuesta = new Respuesta<List<AspNetRole>>();

            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.AspNetRoles.ToList();
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
        public IActionResult Add(AspNetRole model)
        {
            Respuesta<object> oRespuesta = new Respuesta<object>();

            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    AspNetRole oAspNetRoles = new AspNetRole();
                    oAspNetRoles.AspNetUserRoles = model.AspNetUserRoles;
                    oAspNetRoles.AspNetRoleClaims = model.AspNetRoleClaims;
                    oAspNetRoles.Id = model.Id;
                    oAspNetRoles.ConcurrencyStamp = model.ConcurrencyStamp;

                    db.AspNetRoles.Add(oAspNetRoles);
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
        public IActionResult Edit(AspnetroleRequest model)
        {
            Respuesta<object> oRespuesta = new Respuesta<object>();

            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    AspNetRole oAspNetRoles = db.AspNetRoles.Find(model.Id);
                    oAspNetRoles.AspNetUserRoles = model.Aspnetuserroles;
                    oAspNetRoles.AspNetRoleClaims = model.Aspnetroleclaims;
                    oAspNetRoles.ConcurrencyStamp = model.ConcurrencyStamp;
                    db.Entry(oAspNetRoles).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            Respuesta<object> oRespuesta = new Respuesta<object>();

            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    AspNetRole oAspNetRoles = db.AspNetRoles.Find(Id);
                    db.Remove(oAspNetRoles);
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
        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            Respuesta<object> oRespuesta = new Respuesta<object>();

            try
            {
                using (BlazorCrudContext db = new BlazorCrudContext())
                {
                    var lst = db.AspNetRoles.Find(Id);
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

    }

}


