using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Server.Context;

namespace SGCLv3.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AspnetusersController : ControllerBase
    {
        private readonly hereford_prContext _db;

        public AspnetusersController(hereford_prContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            Respuesta<List<AspNetUser>> oRespuesta = new Respuesta<List<AspNetUser>>();

            try
            {
                var lst = _db.AspNetUsers.ToList();
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
        public IActionResult Add(AspNetUser model)
        {
            Respuesta<object> oRespuesta = new Respuesta<object>();

            try
            {
                AspNetUser oAspnetuserss = new AspNetUser();
                oAspnetuserss.AspNetUserRole = model.AspNetUserRole;
                oAspnetuserss.PhoneNumber = model.PhoneNumber;
                oAspnetuserss.AccessFailedCount = model.AccessFailedCount;
                oAspnetuserss.AspNetUserLogins = model.AspNetUserLogins;
                oAspnetuserss.AspNetUserTokens = model.AspNetUserTokens;
                oAspnetuserss.Id = model.Id;
                oAspnetuserss.UserName = model.UserName;
                oAspnetuserss.TwoFactorEnabled = model.TwoFactorEnabled;
                oAspnetuserss.SecurityStamp = model.SecurityStamp;
                oAspnetuserss.Email = model.Email;
                oAspnetuserss.ConcurrencyStamp = model.ConcurrencyStamp;
                oAspnetuserss.NormalizedEmail = model.NormalizedEmail;
                oAspnetuserss.NormalizedUserName = model.NormalizedUserName;
                oAspnetuserss.PasswordHash = model.PasswordHash;
                oAspnetuserss.PhoneNumberConfirmed = model.PhoneNumberConfirmed;

                _db.AspNetUsers.Add(oAspnetuserss);
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
        public IActionResult Edit(AspNetUser model)
        {
            Respuesta<object> oRespuesta = new Respuesta<object>();

            try
            {
                AspNetUser oAspnetuserss = _db.AspNetUsers.Find(model.Id);
                oAspnetuserss.AspNetUserRole = model.AspNetUserRole;
                oAspnetuserss.PhoneNumber = model.PhoneNumber;
                oAspnetuserss.AccessFailedCount = model.AccessFailedCount;
                oAspnetuserss.AspNetUserLogins = model.AspNetUserLogins;
                oAspnetuserss.AspNetUserTokens = model.AspNetUserTokens;
                oAspnetuserss.Id = model.Id;
                oAspnetuserss.UserName = model.UserName;
                oAspnetuserss.TwoFactorEnabled = model.TwoFactorEnabled;
                oAspnetuserss.SecurityStamp = model.SecurityStamp;
                oAspnetuserss.Email = model.Email;
                oAspnetuserss.ConcurrencyStamp = model.ConcurrencyStamp;
                oAspnetuserss.NormalizedEmail = model.NormalizedEmail;
                oAspnetuserss.NormalizedUserName = model.NormalizedUserName;
                oAspnetuserss.PasswordHash = model.PasswordHash;
                oAspnetuserss.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                _db.Entry(oAspnetuserss).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
                AspNetUser oAspnetuserss = _db.AspNetUsers.Find(Id);
                _db.Remove(oAspnetuserss);
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
                var lst = _db.AspNetUsers.Find(Id);
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

