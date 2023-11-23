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
    public class Resin6Controller : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IResin6Repositorio _Resin6Repositorio;
        public Resin6Controller(IResin6Repositorio Resin6Repositorio, IMapper mapper)
        {
            _mapper = mapper;
            _Resin6Repositorio = Resin6Repositorio;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<Resin6DTO>> _ResponseDTO = new Respuesta<List<Resin6DTO>>();

            try
            {
                List<Resin6DTO> listaPedido = new List<Resin6DTO>();
                var a = await _Resin6Repositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<Resin6DTO>>(a);

                _ResponseDTO = new Respuesta<List<Resin6DTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Resin6DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                var a = await _Resin6Repositorio.CantidadTotal();

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

            Respuesta<List<Resin6DTO>> _ResponseDTO = new Respuesta<List<Resin6DTO>>();

            try
            {
                var a = await _Resin6Repositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<Resin6DTO>>(a);

                _ResponseDTO = new Respuesta<List<Resin6DTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Resin6DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Resin6 _Resin6Eliminar = await _Resin6Repositorio.Obtener(u => u.Id == id);
                if (_Resin6Eliminar != null)
                {

                    bool respuesta = await _Resin6Repositorio.Eliminar(_Resin6Eliminar);

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
        public async Task<IActionResult> Guardar([FromBody] Resin6DTO request)
        {
            Respuesta<Resin6DTO> _Respuesta = new Respuesta<Resin6DTO>();
            try
            {
                Resin6 _Resin6 = _mapper.Map<Resin6>(request);

                Resin6 _Resin6Creado = await _Resin6Repositorio.Crear(_Resin6);

                if (_Resin6Creado.Id != 0)
                    _Respuesta = new Respuesta<Resin6DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin6DTO>(_Resin6Creado) };
                else
                    _Respuesta = new Respuesta<Resin6DTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Resin6DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Resin6DTO request)
        {
            Respuesta<Resin6DTO> _Respuesta = new Respuesta<Resin6DTO>();
            try
            {
                Resin6 _Resin6 = _mapper.Map<Resin6>(request);
                Resin6 _Resin6ParaEditar = await _Resin6Repositorio.Obtener(u => u.Id == _Resin6.Id);

                if (_Resin6ParaEditar != null)
                {
                    _Resin6ParaEditar.Hdp = _Resin6.Hdp;
                    _Resin6ParaEditar.HdpM = _Resin6.HdpM;
                    _Resin6ParaEditar.HdpAs = _Resin6.HdpAs;
                    _Resin6ParaEditar.Hdt = _Resin6.Hdt;
                    _Resin6ParaEditar.Hdb = _Resin6.Hdb;
                    _Resin6ParaEditar.Hpp = _Resin6.Hpp;
                    _Resin6ParaEditar.HppM = _Resin6.HppM;
                    _Resin6ParaEditar.HppAs = _Resin6.HppAs;
                    _Resin6ParaEditar.Hpt = _Resin6.Hpt;
                    _Resin6ParaEditar.Hpb = _Resin6.Hpb;
                    _Resin6ParaEditar.Hgvp = _Resin6.Hgvp;
                    _Resin6ParaEditar.Hgvb = _Resin6.Hgvb;
                    _Resin6ParaEditar.Hgqp = _Resin6.Hgqp;
                    _Resin6ParaEditar.Hgqb = _Resin6.Hgqb;
                    _Resin6ParaEditar.Mcp = _Resin6.Mcp;
                    _Resin6ParaEditar.McpM = _Resin6.McpM;
                    _Resin6ParaEditar.McpAs = _Resin6.McpAs;
                    _Resin6ParaEditar.Mct = _Resin6.Mct;
                    _Resin6ParaEditar.Msp = _Resin6.Msp;
                    _Resin6ParaEditar.MspM = _Resin6.MspM;
                    _Resin6ParaEditar.MspAs = _Resin6.MspAs;
                    _Resin6ParaEditar.Mst = _Resin6.Mst;
                    _Resin6ParaEditar.Mspsb = _Resin6.Mspsb;
                    _Resin6ParaEditar.Nrores = _Resin6.Nrores;

                    bool respuesta = await _Resin6Repositorio.Editar(_Resin6ParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<Resin6DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Resin6DTO>(_Resin6ParaEditar) };
                    else
                        _Respuesta = new Respuesta<Resin6DTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<Resin6DTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Resin6DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }
    }
}
