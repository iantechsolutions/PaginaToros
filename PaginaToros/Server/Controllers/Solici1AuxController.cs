using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Context;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Repositorio.Implementacion;
using System.Net.Mail;
using System.Net.Mime;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Solici1AuxController : ControllerBase
    {


        private readonly IMapper _mapper;
        private readonly ISolici1AuxRepositorio _solicitudRepositorio;
        private readonly ISocioRepositorio _socioRepositorio;
        public Solici1AuxController(ISolici1AuxRepositorio solicitudRepositorio, ISocioRepositorio socioRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _solicitudRepositorio = solicitudRepositorio;
            _socioRepositorio = socioRepositorio;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<Solici1AuxDTO>> _ResponseDTO = new Respuesta<List<Solici1AuxDTO>>();

            try
            {
                List<Solici1AuxDTO> listaPedido = new List<Solici1AuxDTO>();
                var a = await _solicitudRepositorio.Lista(skip, take);
                

                listaPedido = _mapper.Map<List<Solici1AuxDTO>>(a);

                _ResponseDTO = new Respuesta<List<Solici1AuxDTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Solici1AuxDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }

        [HttpGet]
        [Route("Cantidad")]
        public async Task<IActionResult> CantidadTotal()
        {

            Respuesta<int> _ResponseDTO = new Respuesta<int>();

            try
            {
                var a = await _solicitudRepositorio.CantidadTotal();

                _ResponseDTO = new Respuesta<int>() { Exito = 1, Mensaje = "Exito", List = a };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<int>() { Exito = 1, Mensaje = ex.Message, List = 0 };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }
        [HttpGet]
        [Route("LimitadosFiltrados")]
        public async Task<IActionResult> LimitadosFiltrados(int skip, int take, string? expression = null)
        {

            Respuesta<List<Solici1AuxDTO>> _ResponseDTO = new Respuesta<List<Solici1AuxDTO>>();

            try
            {
                var a = await _solicitudRepositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<Solici1AuxDTO>>(a);

                _ResponseDTO = new Respuesta<List<Solici1AuxDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Solici1AuxDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }
        [HttpGet]
        [Route("LimitadosFiltradosNoInclude")]
        public async Task<IActionResult> LimitadosFiltradosNoInclude(int skip, int take, string? expression = null)
        {

            Respuesta<List<Solici1AuxDTO>> _ResponseDTO = new Respuesta<List<Solici1AuxDTO>>();

            try
            {
                var a = await _solicitudRepositorio.LimitadosFiltradosNoInclude(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<Solici1AuxDTO>>(a);

                _ResponseDTO = new Respuesta<List<Solici1AuxDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Solici1AuxDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            Respuesta<string> _Respuesta = new Respuesta<string>();
            try
            {
                Solici1Aux _SolicitudEliminar = await _solicitudRepositorio.Obtener(u => u.Id == id);
                if (_SolicitudEliminar != null)
                {

                    bool respuesta = await _solicitudRepositorio.Eliminar(_SolicitudEliminar);

                    if (respuesta)
                        _Respuesta = new Respuesta<string>() { Exito = 1, Mensaje = "ok", List = "" };
                    else
                        _Respuesta = new Respuesta<string>() { Exito = 1, Mensaje = "No se pudo eliminar el identificador", List = "" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<string>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] Solici1AuxDTO request)
        {
            Respuesta<Solici1AuxDTO> _Respuesta = new Respuesta<Solici1AuxDTO>();
            try
            {
                Solici1Aux _Solicitud = _mapper.Map<Solici1Aux>(request);
                Solici1Aux _SolicitudCreado = await _solicitudRepositorio.Crear(_Solicitud);

                if (_SolicitudCreado.Id != 0)
                    _Respuesta = new Respuesta<Solici1AuxDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Solici1AuxDTO>(_SolicitudCreado) };
                else
                    _Respuesta = new Respuesta<Solici1AuxDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Solici1AuxDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Solici1AuxDTO request)
        {
            Respuesta<Solici1AuxDTO> _Respuesta = new Respuesta<Solici1AuxDTO>();
            try
            {
                Solici1Aux _Solicitud = _mapper.Map<Solici1Aux>(request);
                Solici1Aux _SolicitudParaEditar = await _solicitudRepositorio.Obtener(u => u.Id == _Solicitud.Id);
                Console.WriteLine(_Solicitud.Id);
                if (_SolicitudParaEditar != null)
                {
                    _SolicitudParaEditar.Cantor = _Solicitud.Cantor;
                    _SolicitudParaEditar.Cantvq = _Solicitud.Cantvq;
                    _SolicitudParaEditar.Canvac = _Solicitud.Canvac;
                    _SolicitudParaEditar.Canvaq = _Solicitud.Canvaq;
                    _SolicitudParaEditar.Anio = _Solicitud.Anio;
                    _SolicitudParaEditar.IdSoli = _Solicitud.IdSoli;
                    Console.WriteLine(_SolicitudParaEditar.Anio);
                    bool respuesta = await _solicitudRepositorio.Editar(_SolicitudParaEditar);
                    Console.WriteLine(respuesta);
                    if (respuesta)
                        _Respuesta = new Respuesta<Solici1AuxDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Solici1AuxDTO>(_SolicitudParaEditar) };
                    else
                        _Respuesta = new Respuesta<Solici1AuxDTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<Solici1AuxDTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Solici1AuxDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }




        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Solici1Aux> oRespuesta = new Respuesta<Solici1Aux>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Solici1Auxs
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
            Respuesta<List<Solici1Aux>> oRespuesta = new Respuesta<List<Solici1Aux>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    var lst = db.Solici1Auxs.ToList();
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
        public IActionResult Add(Solici1Aux _Solicitud)
        {
            Respuesta<List<Solici1Aux>> oRespuesta = new Respuesta<List<Solici1Aux>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    
                    Solici1Aux _SolicitudParaEditar = new Solici1Aux();
                    
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
        public IActionResult Edit(Solici1Aux _Solicitud)
        {
            Respuesta<List<Solici1Aux>> oRespuesta = new Respuesta<List<Solici1Aux>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Solici1Aux _SolicitudParaEditar = db.Solici1Auxs.Find(_Solicitud.Id);
                    _SolicitudParaEditar.Cantor = _Solicitud.Cantor;
                    _SolicitudParaEditar.Cantvq = _Solicitud.Cantvq;
                    _SolicitudParaEditar.Canvac = _Solicitud.Canvac;
                    _SolicitudParaEditar.Canvaq = _Solicitud.Canvaq;
                    _SolicitudParaEditar.Anio = _Solicitud.Anio;
                    _SolicitudParaEditar.IdSoli = _Solicitud.IdSoli;
                    db.Entry(_SolicitudParaEditar).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
            Respuesta<List<Solici1Aux>> oRespuesta = new Respuesta<List<Solici1Aux>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Solici1Aux _SolicitudParaEditar = db.Solici1Auxs.Find(Id);
                    db.Remove(_SolicitudParaEditar);
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


        [HttpPost("SendExcel/{socioId}")]
        public async Task<IActionResult> SendExcel(int socioId, [FromForm] IFormFile file)
        {
            try {
                var tempFilePath = Path.Combine(Path.GetTempPath(), $"Excel_Solicitud_{DateTime.Now.ToString("dd_MM_yyyy")}.xls");
                using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                string filtro = $"Id = {socioId}";
            var rta = await _socioRepositorio.LimitadosFiltrados(0, 1, filtro);
            Socio socio = rta.FirstOrDefault();
            using (MailMessage mail = new MailMessage())
            {
                MemoryStream memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                mail.From = new MailAddress("puroregistrado@hotmail.com");
                mail.To.Add("puroregistradohereford@gmail.com");
                mail.Subject = $"Solicitud de Inspeccion de: {socio.Nombre}";
                mail.Body = $"Nueva solicitud de inspeccion\nSocio: {socio.Nombre}";
                mail.Attachments.Add(new Attachment(tempFilePath, MediaTypeNames.Application.Octet));
                using (SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("puroregistrado@hotmail.com", "puro2025", "hotmail.com");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Ok(); // Return 200 OK if email sent successfully
        }


    }
}
