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
using ClosedXML.Excel;
using System.Text;
using System.Globalization;
using System.Net;
using Org.BouncyCastle.Asn1.Ocsp;

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

                Console.WriteLine(_Solicitud.Nrosol);
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
                Console.WriteLine(_Solicitud.Id);
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
                    _SolicitudParaEditar.Anio = _Solicitud.Anio;
                    Console.WriteLine(_SolicitudParaEditar.Anio);
                    bool respuesta = await _solicitudRepositorio.Editar(_SolicitudParaEditar);
                    Console.WriteLine(respuesta);
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
                    var lst = db.Solici1s
                    .OrderByDescending(s => s.Nrosol)
                    .ToList();
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
                    _SolicitudParaEditar.Anio = _Solicitud.Anio;
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
            try
            {
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
                    mail.From = new MailAddress("planteles@hereford.org.ar");
                    mail.To.Add("puroregistradohereford@gmail.com");
                    mail.To.Add("planteles@hereford.org.ar");
                    mail.Subject = $"Solicitud de Inspeccion de: {socio.Nombre}";
                    mail.Body = $"Nueva solicitud de inspeccion\nSocio: {socio.Nombre}";
                    mail.Attachments.Add(new Attachment(tempFilePath, MediaTypeNames.Application.Octet));
                    using (SmtpClient smtp = new SmtpClient("mail.hereford.org.ar", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("planteles@hereford.org.ar", "Hereford.2033");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Ok(); // Return 200 OK if email sent successfully
        }

        //[HttpPost("SendExcel/{socioId}")]
        //public async Task<IActionResult> SendExcel(int socioId, [FromForm] IFormFile file)
        //{
        //    try
        //    {




        //        var tempFilePath = Path.Combine(Path.GetTempPath(), $"Excel_Solicitud_{DateTime.Now:dd_MM_yyyy_HH_mm_ss}.xlsx");
        //        using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
        //        {
        //            await file.CopyToAsync(fileStream);
        //        }

        //        using var workbook = new XLWorkbook(tempFilePath);
        //        var worksheet = workbook.Worksheet(1);

        //        var codestRaw = worksheet.Cell("E12").GetString();
        //        string codestNormalizado = codestRaw.All(char.IsDigit) ? codestRaw.PadLeft(6, '0') : codestRaw.Trim();

        //        var razonSocial = worksheet.Cell("E10").GetString();
        //        var localidad = worksheet.Cell("H12").GetString();
        //        var producto = worksheet.Cell("B29").GetString();
        //        var provincia = worksheet.Cell("K12").GetString();


        //        var fechaRaw = worksheet.Cell("J4").GetString();
        //        DateTime.TryParse(fechaRaw.Replace("FECHA: ", "").Trim(), out var fechaSolicitud);
        //        fechaSolicitud = fechaSolicitud == default ? DateTime.Now : fechaSolicitud;

        //        using var db = new hereford_prContext();
        //        var establecimientos = await db.Estables.ToListAsync();
        //        var est = establecimientos.FirstOrDefault(e =>
        //            e.Ecod == codestNormalizado || NormalizeString(e.Nombre) == NormalizeString(codestRaw));

        //        if (est == null)
        //            return BadRequest($"No se encontró ningún establecimiento con código o nombre '{codestRaw}'.");

        //        var solicitud = new Solici1
        //        {
        //            Fecsol = fechaSolicitud,
        //            Lugar = localidad,
        //            Produc = "N",
        //            Reinsp = "N",
        //            Anio = DateTime.Now.Year.ToString(),
        //            FchUsu = DateTime.Now,
        //            CodUsu = socioId,
        //            Codest = est.Ecod
        //        };

        //        var ultimaSolicitud = await db.Solici1s
        //        .Where(s => !string.IsNullOrEmpty(s.Nrosol))
        //        .OrderByDescending(s => s.Nrosol)
        //        .FirstOrDefaultAsync();

        //        string nuevoNumeroSolicitud = "000001";
        //        if (ultimaSolicitud != null && int.TryParse(ultimaSolicitud.Nrosol, out int nro))
        //        {
        //            nuevoNumeroSolicitud = (nro + 1).ToString("D6");
        //        }

        //        solicitud.Nrosol = nuevoNumeroSolicitud;
        //        Console.WriteLine($"Número de solicitud asignado: {solicitud.Nrosol}");

        //        db.Solici1s.Add(solicitud);
        //        await db.SaveChangesAsync();
        //        Console.WriteLine($"Se creó Solici1 ID={solicitud.Id}");

        //        string anio1 = worksheet.Cell("E17").GetString();
        //        double.TryParse(worksheet.Cell("E19").GetString(), out var vacasPR1);
        //        double.TryParse(worksheet.Cell("E20").GetString(), out var machosPR1);

        //        int solicitudesAgregadas = 0;


        //        if ((vacasPR1 + machosPR1) > 0)
        //        {
        //            db.Solici1Auxs.Add(new Solici1Aux
        //            {
        //                IdSoli = solicitud.Id,
        //                Anio = anio1,
        //                Cantor = machosPR1,
        //                Cantvq = vacasPR1
        //            });
        //            solicitudesAgregadas++;
        //        }

        //        // 2do bloque PR extra
        //        string anio2 = worksheet.Cell("E23").GetString();
        //        double.TryParse(worksheet.Cell("E25").GetString(), out var vacasPR2);
        //        double.TryParse(worksheet.Cell("E26").GetString(), out var machosPR2);

        //        if ((vacasPR2 + machosPR2) > 0)
        //        {
        //            db.Solici1Auxs.Add(new Solici1Aux
        //            {
        //                IdSoli = solicitud.Id,
        //                Anio = anio2,
        //                Cantor = machosPR2,
        //                Cantvq = vacasPR2
        //            });
        //            solicitudesAgregadas++;
        //        }

        //        // Bloque VIP (todos mismo año actual)
        //        string anioVIP = DateTime.Now.Year.ToString();
        //        double.TryParse(worksheet.Cell("E30").GetString(), out var vacasVIP);
        //        double.TryParse(worksheet.Cell("E31").GetString(), out var machosVIP);

        //        if ((vacasVIP + machosVIP) > 0)
        //        {
        //            db.Solici1Auxs.Add(new Solici1Aux
        //            {
        //                IdSoli = solicitud.Id,
        //                Anio = anioVIP,
        //                Canvac = vacasVIP,
        //                Cantor = 0
        //            });
        //            solicitudesAgregadas++;
        //        }

        //        if (solicitudesAgregadas == 0)
        //        {
        //            return BadRequest("El Excel no contiene animales registrados en ningún año.");
        //        }

        //        await db.SaveChangesAsync();
        //        Console.WriteLine($"Se cargaron {solicitudesAgregadas} Solici1Aux para Solici1 ID={solicitud.Id}");

        //        // Enviar el correo
        //        var socio = (await _socioRepositorio.LimitadosFiltrados(0, 1, $"Id = {socioId}")).FirstOrDefault();
        //        if (socio == null)
        //            return BadRequest("No se encontró el socio.");

        //        using var mail = new MailMessage
        //        {
        //            From = new MailAddress("planteles@hereford.org.ar"),
        //            Subject = $"Solicitud de Inspección de: {socio.Nombre}",
        //            Body = $"Nueva solicitud de inspección\nSocio: {socio.Nombre}"
        //        };

        //        mail.To.Add("puroregistradohereford@gmail.com");
        //        mail.To.Add("planteles@hereford.org.ar");
        //        mail.Attachments.Add(new Attachment(tempFilePath, MediaTypeNames.Application.Octet));

        //        using var smtp = new SmtpClient("mail.hereford.org.ar", 587)
        //        {
        //            UseDefaultCredentials = false,
        //            Credentials = new NetworkCredential("planteles@hereford.org.ar", "Hereford.2033"),
        //            EnableSsl = true
        //        };
        //        smtp.Send(mail);

        //        Console.WriteLine("Correo enviado con solicitud adjunta.");

        //        System.IO.File.Delete(tempFilePath);
        //        return Ok("Solicitud registrada y correo enviado.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"ERROR general en SendExcel: {ex.Message}");
        //        return BadRequest($"Error: {ex.Message}");
        //    }
        //}


        ///// <summary>
        ///// Normaliza cadenas eliminando tildes, convirtiendo a minúsculas y quitando espacios extras.
        ///// </summary>
        //private static string NormalizeString(string input)
        //{
        //    if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        //    var normalized = input.Normalize(NormalizationForm.FormD);
        //    var builder = new System.Text.StringBuilder();

        //    foreach (var c in normalized)
        //    {
        //        var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
        //        if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
        //            builder.Append(c);
        //    }

        //    return builder.ToString().ToLowerInvariant().Trim();
        //}




    }
}
