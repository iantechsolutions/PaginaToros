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
    public class TranssbController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITranssbRepositorio _TranssbRepositorio;
        public TranssbController(ITranssbRepositorio TranssbRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _TranssbRepositorio = TranssbRepositorio;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<TranssbDTO>> _ResponseDTO = new Respuesta<List<TranssbDTO>>();

            try
            {
                List<TranssbDTO> listaPedido = new List<TranssbDTO>();
                var a = await _TranssbRepositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<TranssbDTO>>(a);

                _ResponseDTO = new Respuesta<List<TranssbDTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<TranssbDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                var a = await _TranssbRepositorio.CantidadTotal();

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

            Respuesta<List<TranssbDTO>> _ResponseDTO = new Respuesta<List<TranssbDTO>>();

            try
            {
                var a = await _TranssbRepositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<TranssbDTO>>(a);

                _ResponseDTO = new Respuesta<List<TranssbDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<TranssbDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Transsb _TranssbEliminar = await _TranssbRepositorio.Obtener(u => u.Id == id);
                if (_TranssbEliminar != null)
                {

                    bool respuesta = await _TranssbRepositorio.Eliminar(_TranssbEliminar);

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
        public async Task<IActionResult> Guardar([FromBody] TranssbDTO request)
        {
            Respuesta<TranssbDTO> _Respuesta = new Respuesta<TranssbDTO>();
            try
            {
                Transsb _Transsb = _mapper.Map<Transsb>(request);

                Transsb _TranssbCreado = await _TranssbRepositorio.Crear(_Transsb);

                if (_TranssbCreado.Id != 0)
                    _Respuesta = new Respuesta<TranssbDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<TranssbDTO>(_TranssbCreado) };
                else
                    _Respuesta = new Respuesta<TranssbDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<TranssbDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] TranssbDTO request)
        {
            Respuesta<TranssbDTO> _Respuesta = new Respuesta<TranssbDTO>();
            try
            {
                Transsb _Transsb = _mapper.Map<Transsb>(request);
                Transsb _TranssbParaEditar = await _TranssbRepositorio.Obtener(u => u.Id == _Transsb.Id);

                if (_TranssbParaEditar != null)
                {
                    _TranssbParaEditar.NroTrans = _Transsb.NroTrans;
                    _TranssbParaEditar.Fectrans = _Transsb.Fectrans;
                    _TranssbParaEditar.NroOrden = _Transsb.NroOrden;
                    _TranssbParaEditar.Sven = _Transsb.Sven;
                    _TranssbParaEditar.CategSv = _Transsb.CategSv;
                    _TranssbParaEditar.Vnom = _Transsb.Vnom;
                    _TranssbParaEditar.Scom = _Transsb.Scom;
                    _TranssbParaEditar.CategSc = _Transsb.CategSc;
                    _TranssbParaEditar.Cnom = _Transsb.Cnom;
                    _TranssbParaEditar.Ecod = _Transsb.Ecod;
                    _TranssbParaEditar.FchUsu = _Transsb.FchUsu;
                    _TranssbParaEditar.CodUsu = _Transsb.CodUsu;
                    bool respuesta = await _TranssbRepositorio.Editar(_TranssbParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<TranssbDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<TranssbDTO>(_TranssbParaEditar) };
                    else
                        _Respuesta = new Respuesta<TranssbDTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<TranssbDTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<TranssbDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }
    }
}
