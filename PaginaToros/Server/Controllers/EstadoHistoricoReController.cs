using AutoMapper;
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
    public class Resin2Controller : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IResin2Repositorio _Resin2Repositorio;
        public Resin2Controller(IResin2Repositorio Resin2Repositorio, IMapper mapper)
        {
            _mapper = mapper;
            _Resin2Repositorio = Resin2Repositorio;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<Resin2DTO>> _ResponseDTO = new Respuesta<List<Resin2DTO>>();

            try
            {
                List<Resin2DTO> listaPedido = new List<Resin2DTO>();
                var a = await _Resin2Repositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<Resin2DTO>>(a);

                _ResponseDTO = new Respuesta<List<Resin2DTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Resin2DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                var a = await _Resin2Repositorio.CantidadTotal();

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
        public async Task<IActionResult> LimitadosFiltrados(int skip, int take, string expression)
        {

            Respuesta<List<Resin2DTO>> _ResponseDTO = new Respuesta<List<Resin2DTO>>();

            try
            {
                var a = await _Resin2Repositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<Resin2DTO>>(a);

                _ResponseDTO = new Respuesta<List<Resin2DTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Resin2DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Resin2 _Resin2Eliminar = await _Resin2Repositorio.Obtener(u => u.Id == id);
                if (_Resin2Eliminar != null)
                {

                    bool respuesta = await _Resin2Repositorio.Eliminar(_Resin2Eliminar);

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
        public async Task<IActionResult> Guardar([FromBody] Resin2DTO request)
        {
            Respuesta<Resin2DTO> _Respuesta = new Respuesta<Resin2DTO>();
            try
            {
                Resin2 _Resin2 = _mapper.Map<Resin2>(request);

                Resin2 _Resin2Creado = await _Resin2Repositorio.Crear(_Resin2);

                if (_Resin2Creado.Id != 0)
                    _Respuesta = new Respuesta<Resin2DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin2DTO>(_Resin2Creado) };
                else
                    _Respuesta = new Respuesta<Resin2DTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Resin2DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Resin2DTO request)
        {
            Respuesta<Resin2DTO> _Respuesta = new Respuesta<Resin2DTO>();
            try
            {
                Resin2 _Resin2 = _mapper.Map<Resin2>(request);
                Resin2 _Resin2ParaEditar = await _Resin2Repositorio.Obtener(u => u.Id == _Resin2.Id);

                if (_Resin2ParaEditar != null)
                {
                    _Resin2ParaEditar.Ea1 = _Resin2.Ea1;
                    _Resin2ParaEditar.Ea2 = _Resin2.Ea2;
                    _Resin2ParaEditar.Ea3 = _Resin2.Ea3;
                    _Resin2ParaEditar.Ea4 = _Resin2.Ea4;
                    _Resin2ParaEditar.Ea5 = _Resin2.Ea5;
                    _Resin2ParaEditar.Ea6 = _Resin2.Ea6;
                    _Resin2ParaEditar.Nrores = _Resin2.Nrores;

                    bool respuesta = await _Resin2Repositorio.Editar(_Resin2ParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<Resin2DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin2DTO>(_Resin2ParaEditar) };
                    else
                        _Respuesta = new Respuesta<Resin2DTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<Resin2DTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Resin2DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }




    }
}

