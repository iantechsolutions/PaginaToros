using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Client.Pages.Establecimientos;
using PaginaToros.Client.Pages.Socios;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Repositorio.Implementacion;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Request;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstablecimientoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEstableRepositorio _EstableRepositorio;
        public EstablecimientoController(IEstableRepositorio EstableRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _EstableRepositorio = EstableRepositorio;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {
            var _ResponseDTO = new Respuesta<List<EstableDTO>>();

            try
            {
                var entities = await _EstableRepositorio.Lista(skip, take);

           

                var listaPedido = _mapper.Map<List<EstableDTO>>(entities);

                _ResponseDTO = new Respuesta<List<EstableDTO>> { Exito = 1, Mensaje = "Exito", List = listaPedido };
                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<EstableDTO>> { Exito = 0, Mensaje = ex.Message, List = null };
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
                var a = await _EstableRepositorio.CantidadTotal();

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

            Respuesta<List<EstableDTO>> _ResponseDTO = new Respuesta<List<EstableDTO>>();

            try
            {
                var a = await _EstableRepositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<EstableDTO>>(a);

                _ResponseDTO = new Respuesta<List<EstableDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<EstableDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }
        [HttpGet]
        [Route("LimitadosFiltradosNoInclude")]
        public async Task<IActionResult> LimitadosFiltradosNoInclude(int skip, int take, string? expression = null)
        {

            Respuesta<List<EstableDTO>> _ResponseDTO = new Respuesta<List<EstableDTO>>();

            try
            {
                var a = await _EstableRepositorio.LimitadosFiltradosNoInclude(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<EstableDTO>>(a);

                _ResponseDTO = new Respuesta<List<EstableDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<EstableDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Estable _EstableEliminar = await _EstableRepositorio.Obtener(u => u.Id == id);
                if (_EstableEliminar != null)
                {

                    bool respuesta = await _EstableRepositorio.Eliminar(_EstableEliminar);

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
        public async Task<IActionResult> Guardar([FromBody] EstableDTO request)
        {
            Respuesta<EstableDTO> _Respuesta = new Respuesta<EstableDTO>();
            try
            {
                Estable _Estable = _mapper.Map<Estable>(request);
                var EstL = await _EstableRepositorio.Lista(0, 1);
                Estable _EstableViejo = EstL.FirstOrDefault();
                _Estable.Ecod = (Int32.Parse(_EstableViejo.Ecod) + 1).ToString("D6");

                Estable _EstableCreado = await _EstableRepositorio.Crear(_Estable);

                if (_EstableCreado.Id != 0)
                    _Respuesta = new Respuesta<EstableDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<EstableDTO>(_EstableCreado) };
                else
                    _Respuesta = new Respuesta<EstableDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<EstableDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] EstableDTO request)
        {
            Respuesta<EstableDTO> _Respuesta = new Respuesta<EstableDTO>();
            try
            {
                Estable _Estable = _mapper.Map<Estable>(request);
                Estable _EstableParaEditar = await _EstableRepositorio.Obtener(u => u.Id == _Estable.Id);

                if (_EstableParaEditar != null)
                {

                    _EstableParaEditar.Ecod = _Estable.Ecod;
                    _EstableParaEditar.Codsoc = _Estable.Codsoc;
                    _EstableParaEditar.Activo = _Estable.Activo;
                    _EstableParaEditar.Nombre = _Estable.Nombre;
                    _EstableParaEditar.Encargado = _Estable.Encargado;
                    _EstableParaEditar.Direcc = _Estable.Direcc;
                    _EstableParaEditar.Locali = _Estable.Locali;
                    _EstableParaEditar.Tel = _Estable.Tel;
                    _EstableParaEditar.Codpos = _Estable.Codpos;
                    _EstableParaEditar.Codpro = _Estable.Codpro;
                    _EstableParaEditar.Plano = _Estable.Plano;
                    _EstableParaEditar.Catego = _Estable.Catego;
                    _EstableParaEditar.Codzon = _Estable.Codzon;
                    _EstableParaEditar.Fechacreacion = _Estable.Fechacreacion;
                    _EstableParaEditar.Fecing = _Estable.Fecing;

                    bool respuesta = await _EstableRepositorio.Editar(_EstableParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<EstableDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<EstableDTO>(_EstableParaEditar) };
                    else
                        _Respuesta = new Respuesta<EstableDTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<EstableDTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<EstableDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }
    }
}
