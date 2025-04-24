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
    public class Resin1Controller : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IResin1Repositorio _Resin1Repositorio;
        public Resin1Controller(IResin1Repositorio Resin1Repositorio, IMapper mapper)
        {
            _mapper = mapper;
            _Resin1Repositorio = Resin1Repositorio;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<Resin1DTO>> _ResponseDTO = new Respuesta<List<Resin1DTO>>();

            try
            {
                List<Resin1DTO> listaPedido = new List<Resin1DTO>();
                var a = await _Resin1Repositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<Resin1DTO>>(a);

                _ResponseDTO = new Respuesta<List<Resin1DTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Resin1DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                var a = await _Resin1Repositorio.CantidadTotal();

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

            Respuesta<List<Resin1DTO>> _ResponseDTO = new Respuesta<List<Resin1DTO>>();

            try
            {
                var a = await _Resin1Repositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<Resin1DTO>>(a);

                _ResponseDTO = new Respuesta<List<Resin1DTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Resin1DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }
        [HttpGet]
        [Route("LimitadosFiltradosNoInclude")]
        public async Task<IActionResult> LimitadosFiltradosNoInclude(int skip, int take, string? expression = null)
        {

            Respuesta<List<Resin1DTO>> _ResponseDTO = new Respuesta<List<Resin1DTO>>();

            try
            {
                var a = await _Resin1Repositorio.LimitadosFiltradosNoInclude(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<Resin1DTO>>(a);

                _ResponseDTO = new Respuesta<List<Resin1DTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Resin1DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Resin1 _Resin1Eliminar = await _Resin1Repositorio.Obtener(u => u.Id == id);
                if (_Resin1Eliminar != null)
                {

                    bool respuesta = await _Resin1Repositorio.Eliminar(_Resin1Eliminar);

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
        public async Task<IActionResult> Guardar([FromBody] Resin1DTO request)
        {
            Respuesta<Resin1DTO> _Respuesta = new Respuesta<Resin1DTO>();
            try
            {
                Resin1 _Resin1 = _mapper.Map<Resin1>(request);

                Resin1 _Resin1Creado = await _Resin1Repositorio.Crear(_Resin1);

                if (_Resin1Creado.Id != 0)
                    _Respuesta = new Respuesta<Resin1DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin1DTO>(_Resin1Creado) };
                else
                    _Respuesta = new Respuesta<Resin1DTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Resin1DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Resin1DTO request)
        {
            Respuesta<Resin1DTO> _Respuesta = new Respuesta<Resin1DTO>();

            Console.WriteLine("Colo colo");
            try
            {
                Resin1 _Resin1 = _mapper.Map<Resin1>(request);
                Resin1 _Resin1ParaEditar = await _Resin1Repositorio.Obtener(u => u.Id == _Resin1.Id);

                if (_Resin1ParaEditar != null)
                {
                    _Resin1ParaEditar.Nrores = _Resin1.Nrores;
                    _Resin1ParaEditar.Nropla = _Resin1.Nropla;
                    _Resin1ParaEditar.Observ = _Resin1.Observ;
                    _Resin1ParaEditar.Ppajust = _Resin1.Ppajust;
                    _Resin1ParaEditar.Epromedio = _Resin1.Epromedio;
                    _Resin1ParaEditar.Emax = _Resin1.Emax;
                    _Resin1ParaEditar.Emin = _Resin1.Emin;
                    _Resin1ParaEditar.Tortot = _Resin1.Tortot;
                    _Resin1ParaEditar.Torsb = _Resin1.Torsb;
                    _Resin1ParaEditar.CodUsu = _Resin1.CodUsu;
                    _Resin1ParaEditar.Editar = _Resin1.Editar;
                    _Resin1ParaEditar.Icod = _Resin1.Icod;
                    _Resin1ParaEditar.Scod = _Resin1.Scod;
                    _Resin1ParaEditar.Estcod = _Resin1.Estcod;
                    _Resin1ParaEditar.Freali = _Resin1.Freali;

                    bool respuesta = await _Resin1Repositorio.Editar(_Resin1ParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<Resin1DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin1DTO>(_Resin1ParaEditar) };
                    else
                        _Respuesta = new Respuesta<Resin1DTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<Resin1DTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Resin1DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }
    }
}
