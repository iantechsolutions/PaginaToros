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
    public class Solici1Controller : ControllerBase
    {


        private readonly IMapper _mapper;
        private readonly ISolici1Repositorio _solicitudRepositorio;
        private readonly ISocioRepositorio _socioRepositorio;
        public Solici1Controller(ISolici1Repositorio solicitudRepositorio, ISocioRepositorio socioRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _solicitudRepositorio = solicitudRepositorio;
            _socioRepositorio = socioRepositorio;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<Solici1DTO>> _ResponseDTO = new Respuesta<List<Solici1DTO>>();

            try
            {
                List<Solici1DTO> listaPedido = new List<Solici1DTO>();
                var a = await _solicitudRepositorio.Lista(skip, take);
                

                listaPedido = _mapper.Map<List<Solici1DTO>>(a);

                _ResponseDTO = new Respuesta<List<Solici1DTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Solici1DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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

            Respuesta<List<Solici1DTO>> _ResponseDTO = new Respuesta<List<Solici1DTO>>();

            try
            {
                var a = await _solicitudRepositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<Solici1DTO>>(a);

                _ResponseDTO = new Respuesta<List<Solici1DTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Solici1DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }
        [HttpGet]
        [Route("LimitadosFiltradosNoInclude")]
        public async Task<IActionResult> LimitadosFiltradosNoInclude(int skip, int take, string? expression = null)
        {

            Respuesta<List<Solici1DTO>> _ResponseDTO = new Respuesta<List<Solici1DTO>>();

            try
            {
                var a = await _solicitudRepositorio.LimitadosFiltradosNoInclude(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<Solici1DTO>>(a);

                _ResponseDTO = new Respuesta<List<Solici1DTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Solici1DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Solici1 _SolicitudEliminar = await _solicitudRepositorio.Obtener(u => u.Id == id);
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
        public async Task<IActionResult> Guardar([FromBody] Solici1DTO request)
        {
            Respuesta<Solici1DTO> _Respuesta = new Respuesta<Solici1DTO>();
            try
            {
                Solici1 _Solicitud = _mapper.Map<Solici1>(request);
                var SolL = await _solicitudRepositorio.Lista(0, 1);
                Solici1 _SolicitudVieja = SolL.FirstOrDefault();
                _Solicitud.Nrosol = (Int32.Parse(_SolicitudVieja.Nrosol) + 1).ToString("D6");
                Solici1 _SolicitudCreado = await _solicitudRepositorio.Crear(_Solicitud);

                if (_SolicitudCreado.Id != 0)
                    _Respuesta = new Respuesta<Solici1DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Solici1DTO>(_SolicitudCreado) };
                else
                    _Respuesta = new Respuesta<Solici1DTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Solici1DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Solici1DTO request)
        {
            Respuesta<Solici1DTO> _Respuesta = new Respuesta<Solici1DTO>();
            try
            {
                Solici1 _Solicitud = _mapper.Map<Solici1>(request);
                Solici1 _SolicitudParaEditar = await _solicitudRepositorio.Obtener(u => u.Id == _Solicitud.Id);

                if (_SolicitudParaEditar != null)
                {
                    _SolicitudParaEditar.Nrosol = _Solicitud.Nrosol;
                    _SolicitudParaEditar.Codest = _Solicitud.Codest;
                    _SolicitudParaEditar.Fecsol = _Solicitud.Fecsol;
                    _SolicitudParaEditar.Lugar = _Solicitud.Lugar;
                    _SolicitudParaEditar.Cantor = _Solicitud.Cantor;
                    _SolicitudParaEditar.Cantvq = _Solicitud.Cantvq;
                    _SolicitudParaEditar.Produc = _Solicitud.Produc;
                    _SolicitudParaEditar.Reinsp = _Solicitud.Reinsp;
                    _SolicitudParaEditar.Canvac = _Solicitud.Canvac;
                    _SolicitudParaEditar.Canvaq = _Solicitud.Canvaq;
                    _SolicitudParaEditar.EdadMinMac = _Solicitud.EdadMinMac;
                    _SolicitudParaEditar.EdadMaxHem = _Solicitud.EdadMaxHem;
                    _SolicitudParaEditar.EdadMinHem = _Solicitud.EdadMinHem;
                    _SolicitudParaEditar.EdadMaxMac = _Solicitud.EdadMaxMac;
                    _SolicitudParaEditar.Tyncte = _Solicitud.Tyncte;
                    _SolicitudParaEditar.Banco = _Solicitud.Banco;
                    _SolicitudParaEditar.Import = _Solicitud.Import;
                    _SolicitudParaEditar.Fecins = _Solicitud.Fecins;
                    _SolicitudParaEditar.Ctrl1 = _Solicitud.Ctrl1;
                    _SolicitudParaEditar.Ctrl2 = _Solicitud.Ctrl2;
                    _SolicitudParaEditar.Fecret = _Solicitud.Fecret;
                    _SolicitudParaEditar.FchUsu = _Solicitud.FchUsu;
                    _SolicitudParaEditar.CodUsu = _Solicitud.CodUsu;

                    bool respuesta = await _solicitudRepositorio.Editar(_SolicitudParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<Solici1DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Solici1DTO>(_SolicitudParaEditar) };
                    else
                        _Respuesta = new Respuesta<Solici1DTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<Solici1DTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Solici1DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }




        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Respuesta<Solici1> oRespuesta = new Respuesta<Solici1>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Solici1s
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
            Respuesta<List<Solici1>> oRespuesta = new Respuesta<List<Solici1>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    var lst = db.Solici1s.ToList();
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
            Respuesta<Solici1> oRespuesta = new Respuesta<Solici1>();

            try
            {
                using (hereford_prContext db = new())
                {

                    var lst = db.Solici1s
                    .Where(x => x.Nrosol == nro).First();
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

        [HttpGet("pendientes")]
        public async Task <IActionResult> GetPendientes()
        {
            Respuesta<List<Solici1>> oRespuesta = new Respuesta<List<Solici1>>();

            try
            {
                using (hereford_prContext db = new())
                {
                    var lst = await db.Solici1s
                    .Where(t1 => !db.Resin1s.Any(t2 => t2.Nrores == t1.Nrosol))
                    .ToListAsync();
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
        public IActionResult Add(Solici1 _Solicitud)
        {
            Respuesta<List<Solici1>> oRespuesta = new Respuesta<List<Solici1>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    
                    Solici1 _SolicitudParaEditar = new Solici1();
                    
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
        public IActionResult Edit(Solici1 _Solicitud)
        {
            Respuesta<List<Solici1>> oRespuesta = new Respuesta<List<Solici1>>();
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Solici1 _SolicitudParaEditar = db.Solici1s.Find(_Solicitud.Id);
                    _SolicitudParaEditar.Codest = _Solicitud.Codest;
                    _SolicitudParaEditar.Nrosol = _Solicitud.Nrosol;
                    _SolicitudParaEditar.Fecsol = _Solicitud.Fecsol;
                    _SolicitudParaEditar.Lugar = _Solicitud.Lugar;
                    _SolicitudParaEditar.Cantor = _Solicitud.Cantor;
                    _SolicitudParaEditar.Cantvq = _Solicitud.Cantvq;
                    _SolicitudParaEditar.Produc = _Solicitud.Produc;
                    _SolicitudParaEditar.Reinsp = _Solicitud.Reinsp;
                    _SolicitudParaEditar.Canvac = _Solicitud.Canvac;
                    _SolicitudParaEditar.Canvaq = _Solicitud.Canvaq;
                    _SolicitudParaEditar.EdadMinMac = _Solicitud.EdadMinMac;
                    _SolicitudParaEditar.EdadMaxHem = _Solicitud.EdadMaxHem;
                    _SolicitudParaEditar.EdadMinHem = _Solicitud.EdadMinHem;
                    _SolicitudParaEditar.EdadMaxMac = _Solicitud.EdadMaxMac;
                    _SolicitudParaEditar.Tyncte = _Solicitud.Tyncte;
                    _SolicitudParaEditar.Banco = _Solicitud.Banco;
                    _SolicitudParaEditar.Import = _Solicitud.Import;
                    _SolicitudParaEditar.Fecins = _Solicitud.Fecins;
                    _SolicitudParaEditar.Ctrl1 = _Solicitud.Ctrl1;
                    _SolicitudParaEditar.Ctrl2 = _Solicitud.Ctrl2;
                    _SolicitudParaEditar.Fecret = _Solicitud.Fecret;
                    _SolicitudParaEditar.FchUsu = _Solicitud.FchUsu;
                    _SolicitudParaEditar.CodUsu = _Solicitud.CodUsu;
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
            Respuesta<List<Solici1>> oRespuesta = new Respuesta<List<Solici1>>();
            //IQueryable<Toro> TorosPorId;
            try
            {
                using (hereford_prContext db = new hereford_prContext())
                {
                    Solici1 _SolicitudParaEditar = db.Solici1s.Find(Id);
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
                    smtp.Credentials = new System.Net.NetworkCredential("puroregistrado@hotmail.com", "puro2024", "hotmail.com");
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
