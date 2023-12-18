using AutoMapper;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Resin8Controller : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IResin8Repositorio _Resin8Repositorio;
        public Resin8Controller(IResin8Repositorio Resin8Repositorio, IMapper mapper)
        {
            _mapper = mapper;
            _Resin8Repositorio = Resin8Repositorio;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<Resin8DTO>> _ResponseDTO = new Respuesta<List<Resin8DTO>>();

            try
            {
                List<Resin8DTO> listaPedido = new List<Resin8DTO>();
                var a = await _Resin8Repositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<Resin8DTO>>(a);

                _ResponseDTO = new Respuesta<List<Resin8DTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Resin8DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                var a = await _Resin8Repositorio.CantidadTotal();

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

            Respuesta<List<Resin8DTO>> _ResponseDTO = new Respuesta<List<Resin8DTO>>();

            try
            {
                var a = await _Resin8Repositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<Resin8DTO>>(a);

                _ResponseDTO = new Respuesta<List<Resin8DTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Resin8DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Resin8 _Resin8Eliminar = await _Resin8Repositorio.Obtener(u => u.Id == id);
                if (_Resin8Eliminar != null)
                {

                    bool respuesta = await _Resin8Repositorio.Eliminar(_Resin8Eliminar);

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
        public async Task<IActionResult> Guardar([FromBody] Resin8DTO request)
        {
            Respuesta<Resin8DTO> _Respuesta = new Respuesta<Resin8DTO>();
            try
            {
                Resin8 _Resin8 = _mapper.Map<Resin8>(request);

                Resin8 _Resin8Creado = await _Resin8Repositorio.Crear(_Resin8);

                if (_Resin8Creado.Id != 0)
                    _Respuesta = new Respuesta<Resin8DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin8DTO>(_Resin8Creado) };
                else
                    _Respuesta = new Respuesta<Resin8DTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Resin8DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Resin8DTO request)
        {
            Respuesta<Resin8DTO> _Respuesta = new Respuesta<Resin8DTO>();
            try
            {
                Resin8 _Resin8 = _mapper.Map<Resin8>(request);
                Resin8 _Resin8ParaEditar = await _Resin8Repositorio.Obtener(u => u.Id == _Resin8.Id);

                if (_Resin8ParaEditar != null)
                {
                    _Resin8ParaEditar.FchRealizada = _Resin8.FchRealizada;
                    _Resin8ParaEditar.Nrores = _Resin8.Nrores;
                    _Resin8ParaEditar.Nropla = _Resin8.Nropla;
                    _Resin8ParaEditar.Hembras = _Resin8.Hembras;
                    _Resin8ParaEditar.Machos = _Resin8.Machos;
                    _Resin8ParaEditar.MotivoRechazo = _Resin8.MotivoRechazo;

                    bool respuesta = await _Resin8Repositorio.Editar(_Resin8ParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<Resin8DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin8DTO>(_Resin8ParaEditar) };
                    else
                        _Respuesta = new Respuesta<Resin8DTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<Resin8DTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Resin8DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }
    }
}
