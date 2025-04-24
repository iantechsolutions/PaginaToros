using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Client.Pages.Inspectores;
using PaginaToros.Server.Context;
using AutoMapper;
using PaginaToros.Server.Repositorio.Contrato;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IInspectRepositorio _InspectRepositorio;
        public InspectController(IInspectRepositorio InspectRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _InspectRepositorio = InspectRepositorio;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<InspectDTO>> _ResponseDTO = new Respuesta<List<InspectDTO>>();

            try
            {
                List<InspectDTO> listaPedido = new List<InspectDTO>();
                var a = await _InspectRepositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<InspectDTO>>(a);

                _ResponseDTO = new Respuesta<List<InspectDTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<InspectDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                var a = await _InspectRepositorio.CantidadTotal();

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

            Respuesta<List<InspectDTO>> _ResponseDTO = new Respuesta<List<InspectDTO>>();

            try
            {
                var a = await _InspectRepositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<InspectDTO>>(a);

                _ResponseDTO = new Respuesta<List<InspectDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<InspectDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }

        [HttpGet]
        [Route("LimitadosFiltradosIncludeZonas")]
        public async Task<IActionResult> LimitadosFiltradosIncludeZonas(int skip, int take, string? expression = null)
        {

            Respuesta<List<InspectDTO>> _ResponseDTO = new Respuesta<List<InspectDTO>>();

            try
            {
                var a = await _InspectRepositorio.LimitadosFiltradosIncludeZonas(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<InspectDTO>>(a);

                _ResponseDTO = new Respuesta<List<InspectDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<InspectDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Inspect _InspectEliminar = await _InspectRepositorio.Obtener(u => u.Id == id);
                if (_InspectEliminar != null)
                {

                    bool respuesta = await _InspectRepositorio.Eliminar(_InspectEliminar);

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
        public async Task<IActionResult> Guardar([FromBody] InspectDTO request)
        {
            Respuesta<InspectDTO> _Respuesta = new Respuesta<InspectDTO>();
            try
            {
                Inspect _Inspect = _mapper.Map<Inspect>(request);

                Inspect _InspectCreado = await _InspectRepositorio.Crear(_Inspect);

                if (_InspectCreado.Id != 0)
                    _Respuesta = new Respuesta<InspectDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<InspectDTO>(_InspectCreado) };
                else
                    _Respuesta = new Respuesta<InspectDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<InspectDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] InspectDTO request)
        {
            Respuesta<InspectDTO> _Respuesta = new Respuesta<InspectDTO>();
            try
            {
                Inspect _Inspect = _mapper.Map<Inspect>(request);
                Inspect _InspectParaEditar = await _InspectRepositorio.Obtener(u => u.Id == _Inspect.Id);

                Console.WriteLine("Entro");
                Console.WriteLine($"Request ID: {request.Id}, Nombre: {request.Nombre}");
                Console.WriteLine($"Inspect encontrado: {_InspectParaEditar != null}, ID: {_Inspect.Id}");

                if (_InspectParaEditar != null)
                {
                    _InspectParaEditar.Icod = _Inspect.Icod;
                    _InspectParaEditar.Nombre = _Inspect.Nombre;
                    _InspectParaEditar.Direcc = _Inspect.Direcc;
                    _InspectParaEditar.Locali = _Inspect.Locali;
                    _InspectParaEditar.Codpos = _Inspect.Codpos;
                    _InspectParaEditar.Codpro = _Inspect.Codpro;
                    _InspectParaEditar.Telefo = _Inspect.Telefo;
                    _InspectParaEditar.Mail = _Inspect.Mail;

                    Console.WriteLine("Antes de llamar a Editar");
                    bool respuesta = await _InspectRepositorio.Editar(_InspectParaEditar);
                    Console.WriteLine("Después de llamar a Editar");

                    Console.WriteLine($"Nombre actualizado: {_InspectParaEditar.Nombre}");
                    Console.WriteLine($"Respuesta de editar: {respuesta}");

                    if (respuesta)
                        _Respuesta = new Respuesta<InspectDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<InspectDTO>(_InspectParaEditar) };
                    else
                        _Respuesta = new Respuesta<InspectDTO>() { Exito = 0, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<InspectDTO>() { Exito = 0, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                _Respuesta = new Respuesta<InspectDTO>() { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }
    }
    

}
