using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Context;
using AutoMapper;
using PaginaToros.Server.Repositorio.Contrato;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentrosiumController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICentrosiumRepositorio _CentrosiumRepositorio;
        public CentrosiumController(ICentrosiumRepositorio CentrosiumRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _CentrosiumRepositorio = CentrosiumRepositorio;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<CentrosiumDTO>> _ResponseDTO = new Respuesta<List<CentrosiumDTO>>();

            try
            {
                List<CentrosiumDTO> listaPedido = new List<CentrosiumDTO>();
                var a = await _CentrosiumRepositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<CentrosiumDTO>>(a);

                _ResponseDTO = new Respuesta<List<CentrosiumDTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<CentrosiumDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                var a = await _CentrosiumRepositorio.CantidadTotal();

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

            Respuesta<List<CentrosiumDTO>> _ResponseDTO = new Respuesta<List<CentrosiumDTO>>();

            try
            {
                var a = await _CentrosiumRepositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<CentrosiumDTO>>(a);

                _ResponseDTO = new Respuesta<List<CentrosiumDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<CentrosiumDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Centrosium _CentrosiumEliminar = await _CentrosiumRepositorio.Obtener(u => u.Id == id);
                if (_CentrosiumEliminar != null)
                {

                    bool respuesta = await _CentrosiumRepositorio.Eliminar(_CentrosiumEliminar);

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
        public async Task<IActionResult> Guardar([FromBody] CentrosiumDTO request)
        {
            Respuesta<CentrosiumDTO> _Respuesta = new Respuesta<CentrosiumDTO>();
            try
            {
                Centrosium _Centrosium = _mapper.Map<Centrosium>(request);

                Centrosium _CentrosiumCreado = await _CentrosiumRepositorio.Crear(_Centrosium);

                if (_CentrosiumCreado.Id != 0)
                    _Respuesta = new Respuesta<CentrosiumDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<CentrosiumDTO>(_CentrosiumCreado) };
                else
                    _Respuesta = new Respuesta<CentrosiumDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<CentrosiumDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] CentrosiumDTO request)
        {
            Respuesta<CentrosiumDTO> _Respuesta = new Respuesta<CentrosiumDTO>();
            try
            {
                Centrosium _Centrosium = _mapper.Map<Centrosium>(request);
                Centrosium _CentrosiumParaEditar = await _CentrosiumRepositorio.Obtener(u => u.Id == _Centrosium.Id);

                if (_CentrosiumParaEditar != null)
                {
                    _CentrosiumParaEditar.Nrocen = _Centrosium.Nrocen;
                    _CentrosiumParaEditar.Nombre = _Centrosium.Nombre;
                    _CentrosiumParaEditar.NroCSayg = _Centrosium.NroCSayg;
                    _CentrosiumParaEditar.FchUsu = _Centrosium.FchUsu;
                    _CentrosiumParaEditar.CodUsu = _Centrosium.CodUsu;
                    _CentrosiumParaEditar.Id = _Centrosium.Id;

                    bool respuesta = await _CentrosiumRepositorio.Editar(_CentrosiumParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<CentrosiumDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<CentrosiumDTO>(_CentrosiumParaEditar) };
                    else
                        _Respuesta = new Respuesta<CentrosiumDTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<CentrosiumDTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<CentrosiumDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }
    }
}

