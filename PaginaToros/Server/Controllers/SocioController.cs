 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Client.Pages.Socios;
using PaginaToros.Server.Context;
using AutoMapper;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Repositorio.Implementacion;
using System.Linq.Expressions;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocioController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISocioRepositorio _SocioRepositorio;
        public SocioController(ISocioRepositorio SocioRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _SocioRepositorio = SocioRepositorio;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<SocioDTO>> _ResponseDTO = new Respuesta<List<SocioDTO>>();

            try
            {
                List<SocioDTO> listaPedido = new List<SocioDTO>();
                var a = await _SocioRepositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<SocioDTO>>(a);

                _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                var a = await _SocioRepositorio.CantidadTotal();

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

            Respuesta<List<SocioDTO>> _ResponseDTO = new Respuesta<List<SocioDTO>>();

            try
            {
                var a = await _SocioRepositorio.LimitadosFiltrados(skip, take, expression);

                var listaOrdenada = a.OrderByDescending(s => s.Criador == "S").ToList();

                var listaFiltrada = _mapper.Map<List<SocioDTO>>(listaOrdenada);
                //var listaFiltrada = _mapper.Map<List<SocioDTO>>(a);

                _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }


        [Route("LimitadosFiltradoTodos")]
        public async Task<IActionResult> LimitadosFiltradosTodos(int skip, int take, string? expression = null)
        {
            Respuesta<List<SocioDTO>> _ResponseDTO = new Respuesta<List<SocioDTO>>();

            try
            {
                var a = await _SocioRepositorio.LimitadosFiltradosTodos(skip, take, expression);

                //var listaOrdenada = a.OrderByDescending(s => s.Criador == "S").ToList();

                var listaFiltrada = _mapper.Map<List<SocioDTO>>(a);

                _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = "Éxito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Socio _SocioEliminar = await _SocioRepositorio.Obtener(u => u.Id == id);
                if (_SocioEliminar != null)
                {

                    bool respuesta = await _SocioRepositorio.Eliminar(_SocioEliminar);

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
        public async Task<IActionResult> Guardar([FromBody] SocioDTO request)
        {
            Respuesta<SocioDTO> _Respuesta = new Respuesta<SocioDTO>();
            try
            {
                Socio _Socio = _mapper.Map<Socio>(request);
                
                Socio _SocioCreado = await _SocioRepositorio.Crear(_Socio);

                if (_SocioCreado.Id != 0)
                    _Respuesta = new Respuesta<SocioDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<SocioDTO>(_SocioCreado) };
                else
                    _Respuesta = new Respuesta<SocioDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                    errorMessage += " | Inner: " + ex.InnerException.Message;

                _Respuesta = new Respuesta<SocioDTO>()
                {
                    Exito = 1,
                    Mensaje = errorMessage
                };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }

        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] SocioDTO request)
        {
            var resp = new Respuesta<SocioDTO>();
            try
            {
                var socioReq = _mapper.Map<Socio>(request);
                var socioDb = await _SocioRepositorio.Obtener(u => u.Id == socioReq.Id);

                if (socioDb == null)
                    return NotFound(new Respuesta<SocioDTO> { Exito = 0, Mensaje = "No se encontró el identificador" });

                socioDb.Scod = socioReq.Scod;
                socioDb.Nombre = socioReq.Nombre;
                socioDb.Direcc1 = socioReq.Direcc1;
                socioDb.Telefo1 = socioReq.Telefo1;
                socioDb.Telefo2 = socioReq.Telefo2;
                socioDb.Locali1 = socioReq.Locali1;
                socioDb.Codpos1 = socioReq.Codpos1;
                socioDb.Codpro1 = socioReq.Codpro1;
                socioDb.Criador = socioReq.Criador;
                socioDb.Mail = socioReq.Mail;
                socioDb.Fecing = socioReq.Fecing;
                socioDb.Placod = socioReq.Placod;

                var ok = await _SocioRepositorio.Editar(socioDb);

                if (!ok)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new Respuesta<SocioDTO> { Exito = 0, Mensaje = "No se pudo editar el socio" });

                resp = new Respuesta<SocioDTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<SocioDTO>(socioDb) };
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Respuesta<SocioDTO> { Exito = 0, Mensaje = ex.Message });
            }
        }
    }
}
