using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Client.Pages.Socios;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Net.Mail;
using System.Net.Mime;
namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransanController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper _mapper;
        private readonly ITransanRepositorio _TransanRepositorio;
        public TransanController(ApplicationDbContext db, ITransanRepositorio TransanRepositorio, IMapper mapper)
        {
            this.db = db;
            _mapper = mapper;
            _TransanRepositorio = TransanRepositorio;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<TransanDTO>> _ResponseDTO = new Respuesta<List<TransanDTO>>();

            try
            {
                List<TransanDTO> listaPedido = new List<TransanDTO>();
                var a = await _TransanRepositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<TransanDTO>>(a);

                _ResponseDTO = new Respuesta<List<TransanDTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<TransanDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                var a = await _TransanRepositorio.CantidadTotal();

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

            Respuesta<List<TransanDTO>> _ResponseDTO = new Respuesta<List<TransanDTO>>();

            try
            {
                var a = await _TransanRepositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<TransanDTO>>(a);

                _ResponseDTO = new Respuesta<List<TransanDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<TransanDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }


        [HttpPost]
        [Route("SendMail")]
        public IActionResult ExampleMethod([FromBody] ContenidoMails request)
        {
            using (MailMessage mail = new MailMessage())
            {
                try
                {
                    // Obtener correo del socio vendedor
                    var socioVendedor = db.User.FirstOrDefault(x => x.SocioId == request.Vendedor);
                    if (socioVendedor != null && IsValidEmail(socioVendedor.Email))
                    {
                        //mail.To.Add(socioVendedor.Email);
                    }
                    else
                    {
                        Console.WriteLine("Socio vendedor sin cuenta o correo inválido");
                    }

                    // Obtener correo del socio comprador
                    if (string.IsNullOrEmpty(request.Direccion))
                    {
                        var socioComprador = db.User.FirstOrDefault(x => x.SocioId == request.Comprador);
                        if (socioComprador != null && IsValidEmail(socioComprador.Email))
                        {
                            //mail.To.Add(socioComprador.Email);
                        }
                        else
                        {
                            Console.WriteLine("Socio comprador sin cuenta o correo inválido");
                        }
                    }
                    else
                    {
                        if (IsValidEmail(request.Direccion))
                        {
                            mail.To.Add(request.Direccion);
                        }
                        else
                        {
                            Console.WriteLine("Dirección de correo inválida");
                        }
                    }

                    // Agregar correo de "puroregistrado"
                    if (IsValidEmail("puroregistrado@gmail.com"))
                    {
                        mail.To.Add("puroregistrado@gmail.com");
                    }
                    else
                    {
                        Console.WriteLine("Correo de puroregistrado no válido");
                    }

                    // Configurar el mensaje
                    mail.From = new MailAddress("puroregistrado@gmail.com");
                    mail.Subject = "Nueva transferencia animal";
                    mail.Body = request.Mail;

                    // Configurar SMTP y enviar
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("puroregistrado@gmail.com", "dmuu kdke jobp bhgo"); // Agrega tu contraseña aquí
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                    return BadRequest("Error al enviar el correo.");
                }

                return Ok("Correo enviado correctamente.");
            }
        }


        [HttpPost]
        [Route("SendMailChange")]
        public IActionResult ExampleMethodChange([FromBody] ContenidoMails2 request)
        {
            using (MailMessage mail = new MailMessage())
            {
                try
                {
                    // Obtener correo del socio vendedor

                   

                    // Agregar correo de "puroregistrado"
                    if (IsValidEmail("puroregistrado@gmail.com"))
                    {
                        mail.To.Add("puroregistrado@gmail.com");
                    }
                    else
                    {
                        Console.WriteLine("Correo de puroregistrado no válido");
                    }

                    // Configurar el mensaje
                    mail.From = new MailAddress("puroregistrado@gmail.com");
                    mail.Subject = "Cambio de socio";
                    mail.Body = request.Mail;

                    // Configurar SMTP y enviar
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("puroregistrado@gmail.com", "dmuu kdke jobp bhgo"); // Agrega tu contraseña aquí
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                    return BadRequest("Error al enviar el correo.");
                }

                return Ok("Correo enviado correctamente.");
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }


        //dmuu kdke jobp bhgo dmuu kdke jobp bhgo
        // puroregistradohereford@gmail.com

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            Respuesta<string> _Respuesta = new Respuesta<string>();
            try
            {
                Transan _TransanEliminar = await _TransanRepositorio.Obtener(u => u.Id == id);
                if (_TransanEliminar != null)
                {

                    bool respuesta = await _TransanRepositorio.Eliminar(_TransanEliminar);

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
        public async Task<IActionResult> Guardar([FromBody] TransanDTO request)
        {
            Respuesta<TransanDTO> _Respuesta = new Respuesta<TransanDTO>();
            try
            {
                Transan _Transan = _mapper.Map<Transan>(request);

                Transan _TransanCreado = await _TransanRepositorio.Crear(_Transan);

                if (_TransanCreado.Id != 0)
                    _Respuesta = new Respuesta<TransanDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<TransanDTO>(_TransanCreado) };
                else
                    _Respuesta = new Respuesta<TransanDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<TransanDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] TransanDTO request)
        {
            Respuesta<TransanDTO> _Respuesta = new Respuesta<TransanDTO>();
            try
            {
                Transan _Transan = _mapper.Map<Transan>(request);
                Transan _TransanParaEditar = await _TransanRepositorio.Obtener(u => u.Id == _Transan.Id);

                if (_TransanParaEditar != null)
                {

                    _TransanParaEditar.NroCert = _Transan.NroCert;
                    _TransanParaEditar.Fecvta = _Transan.Fecvta;
                    _TransanParaEditar.Sven = _Transan.Sven;
                    _TransanParaEditar.CategSv = _Transan.CategSv;
                    _TransanParaEditar.Vnom = _Transan.Vnom;
                    _TransanParaEditar.Scom = _Transan.Scom;
                    _TransanParaEditar.CategSc = _Transan.CategSc;
                    _TransanParaEditar.Cnom = _Transan.Cnom;
                    _TransanParaEditar.Plant = _Transan.Plant;
                    _TransanParaEditar.NvoPla = _Transan.NvoPla;
                    _TransanParaEditar.CantHem = _Transan.CantHem;
                    _TransanParaEditar.CantMach = _Transan.CantMach;
                    _TransanParaEditar.Tiphac = _Transan.Tiphac;
                    _TransanParaEditar.Hemsta = _Transan.Hemsta;
                    _TransanParaEditar.Tipani = _Transan.Tipani;
                    _TransanParaEditar.Incorp = _Transan.Incorp;
                    _TransanParaEditar.Tipohem = _Transan.Tipohem;
                    _TransanParaEditar.CantChem = _Transan.CantChem;
                    _TransanParaEditar.CantCmach = _Transan.CantCmach;
                    _TransanParaEditar.FchUsu = _Transan.FchUsu;
                    _TransanParaEditar.CodUsu = _Transan.CodUsu;

                    bool respuesta = await _TransanRepositorio.Editar(_TransanParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<TransanDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<TransanDTO>(_TransanParaEditar) };
                    else
                        _Respuesta = new Respuesta<TransanDTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<TransanDTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<TransanDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        public class ContenidoMails2
        {
            public int Tipo { get; set; }
            public int Clase { get; set; }
            public string Mail { get; set; }
            public string? Nombre { get; set; }

        }
        public class ContenidoMails
        {
            public int Vendedor { get; set; }
            public int Comprador { get; set; }
            public string Mail { get; set; }
            public string? Direccion { get; set; }

        }
    }
}
