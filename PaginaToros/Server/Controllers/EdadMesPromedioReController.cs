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
    public class Resin4Controller : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IResin4Repositorio _Resin4Repositorio;
        public Resin4Controller(IResin4Repositorio Resin4Repositorio, IMapper mapper)
        {
            _mapper = mapper;
            _Resin4Repositorio = Resin4Repositorio;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<Resin4DTO>> _ResponseDTO = new Respuesta<List<Resin4DTO>>();

            try
            {
                List<Resin4DTO> listaPedido = new List<Resin4DTO>();
                var a = await _Resin4Repositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<Resin4DTO>>(a);

                _ResponseDTO = new Respuesta<List<Resin4DTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Resin4DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                var a = await _Resin4Repositorio.CantidadTotal();

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

            Respuesta<List<Resin4DTO>> _ResponseDTO = new Respuesta<List<Resin4DTO>>();

            try
            {
                var a = await _Resin4Repositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<Resin4DTO>>(a);

                _ResponseDTO = new Respuesta<List<Resin4DTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Resin4DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Resin4 _Resin4Eliminar = await _Resin4Repositorio.Obtener(u => u.Id == id);
                if (_Resin4Eliminar != null)
                {

                    bool respuesta = await _Resin4Repositorio.Eliminar(_Resin4Eliminar);

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
        public async Task<IActionResult> Guardar([FromBody] Resin4DTO request)
        {
            Respuesta<Resin4DTO> _Respuesta = new Respuesta<Resin4DTO>();
            try
            {
                Resin4 _Resin4 = _mapper.Map<Resin4>(request);

                Resin4 _Resin4Creado = await _Resin4Repositorio.Crear(_Resin4);

                if (_Resin4Creado.Id != 0)
                    _Respuesta = new Respuesta<Resin4DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin4DTO>(_Resin4Creado) };
                else
                    _Respuesta = new Respuesta<Resin4DTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Resin4DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Resin4DTO request)
        {
            Respuesta<Resin4DTO> _Respuesta = new Respuesta<Resin4DTO>();
            try
            {
                Resin4 _Resin4 = _mapper.Map<Resin4>(request);
                Resin4 _Resin4ParaEditar = await _Resin4Repositorio.Obtener(u => u.Id == _Resin4.Id);

                if (_Resin4ParaEditar != null)
                {
                    _Resin4ParaEditar.Pedad = _Resin4.Pedad;
                    _Resin4ParaEditar.Ppeso = _Resin4.Ppeso;
                    _Resin4ParaEditar.Medad = _Resin4.Medad;
                    _Resin4ParaEditar.Mpeso = _Resin4.Mpeso;
                    _Resin4ParaEditar.Iedad = _Resin4.Iedad;
                    _Resin4ParaEditar.Ipeso = _Resin4.Ipeso;
                    _Resin4ParaEditar.Pdl = _Resin4.Pdl;
                    _Resin4ParaEditar.P2d = _Resin4.P2d;
                    _Resin4ParaEditar.P4d = _Resin4.P4d;
                    _Resin4ParaEditar.Sexo = _Resin4.Sexo;
                    _Resin4ParaEditar.Nrores = _Resin4.Nrores;

                    bool respuesta = await _Resin4Repositorio.Editar(_Resin4ParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<Resin4DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin4DTO>(_Resin4ParaEditar) };
                    else
                        _Respuesta = new Respuesta<Resin4DTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<Resin4DTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Resin4DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }
    }
}
