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
    public class Resin3Controller : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IResin3Repositorio _Resin3Repositorio;
        public Resin3Controller(IResin3Repositorio Resin3Repositorio, IMapper mapper)
        {
            _mapper = mapper;
            _Resin3Repositorio = Resin3Repositorio;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<Resin3DTO>> _ResponseDTO = new Respuesta<List<Resin3DTO>>();

            try
            {
                List<Resin3DTO> listaPedido = new List<Resin3DTO>();
                var a = await _Resin3Repositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<Resin3DTO>>(a);

                _ResponseDTO = new Respuesta<List<Resin3DTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Resin3DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                var a = await _Resin3Repositorio.CantidadTotal();

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

            Respuesta<List<Resin3DTO>> _ResponseDTO = new Respuesta<List<Resin3DTO>>();

            try
            {
                var a = await _Resin3Repositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<Resin3DTO>>(a);

                _ResponseDTO = new Respuesta<List<Resin3DTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Resin3DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Resin3 _Resin3Eliminar = await _Resin3Repositorio.Obtener(u => u.Id == id);
                if (_Resin3Eliminar != null)
                {

                    bool respuesta = await _Resin3Repositorio.Eliminar(_Resin3Eliminar);

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
        public async Task<IActionResult> Guardar([FromBody] Resin3DTO request)
        {
            Respuesta<Resin3DTO> _Respuesta = new Respuesta<Resin3DTO>();
            try
            {
                Resin3 _Resin3 = _mapper.Map<Resin3>(request);

                Resin3 _Resin3Creado = await _Resin3Repositorio.Crear(_Resin3);

                if (_Resin3Creado.Id != 0)
                    _Respuesta = new Respuesta<Resin3DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin3DTO>(_Resin3Creado) };
                else
                    _Respuesta = new Respuesta<Resin3DTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Resin3DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Resin3DTO request)
        {
            Respuesta<Resin3DTO> _Respuesta = new Respuesta<Resin3DTO>();
            try
            {
                Resin3 _Resin3 = _mapper.Map<Resin3>(request);
                Resin3 _Resin3ParaEditar = await _Resin3Repositorio.Obtener(u => u.Id == _Resin3.Id);

                if (_Resin3ParaEditar != null)
                {
                    _Resin3ParaEditar.Rdvac = _Resin3.Rdvac;
                    _Resin3ParaEditar.Rdvaqcs = _Resin3.Rdvaqcs;
                    _Resin3ParaEditar.Rdvaqss = _Resin3.Rdvaqss;
                    _Resin3ParaEditar.Rpvac = _Resin3.Rpvac;
                    _Resin3ParaEditar.Rpvaqcs = _Resin3.Rpvaqcs;
                    _Resin3ParaEditar.Rpvaqss = _Resin3.Rpvaqss;
                    _Resin3ParaEditar.Ctomov = _Resin3.Ctomov;
                    _Resin3ParaEditar.Tipmov = _Resin3.Tipmov;
                    _Resin3ParaEditar.Nrores = _Resin3.Nrores;

                    bool respuesta = await _Resin3Repositorio.Editar(_Resin3ParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<Resin3DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin3DTO>(_Resin3ParaEditar) };
                    else
                        _Respuesta = new Respuesta<Resin3DTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<Resin3DTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Resin3DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }
    }
}
