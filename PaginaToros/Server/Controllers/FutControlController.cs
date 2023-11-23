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
    public class FutcontrolController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFutcontrolRepositorio _FutcontrolRepositorio;
        public FutcontrolController(IFutcontrolRepositorio FutcontrolRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _FutcontrolRepositorio = FutcontrolRepositorio;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<FutcontrolDTO>> _ResponseDTO = new Respuesta<List<FutcontrolDTO>>();

            try
            {
                List<FutcontrolDTO> listaPedido = new List<FutcontrolDTO>();
                var a = await _FutcontrolRepositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<FutcontrolDTO>>(a);

                _ResponseDTO = new Respuesta<List<FutcontrolDTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<FutcontrolDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                var a = await _FutcontrolRepositorio.CantidadTotal();

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

            Respuesta<List<FutcontrolDTO>> _ResponseDTO = new Respuesta<List<FutcontrolDTO>>();

            try
            {
                var a = await _FutcontrolRepositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<FutcontrolDTO>>(a);

                _ResponseDTO = new Respuesta<List<FutcontrolDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<FutcontrolDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Futcontrol _FutcontrolEliminar = await _FutcontrolRepositorio.Obtener(u => u.Id == id);
                if (_FutcontrolEliminar != null)
                {

                    bool respuesta = await _FutcontrolRepositorio.Eliminar(_FutcontrolEliminar);

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
        public async Task<IActionResult> Guardar([FromBody] FutcontrolDTO request)
        {
            Respuesta<FutcontrolDTO> _Respuesta = new Respuesta<FutcontrolDTO>();
            try
            {
                Futcontrol _Futcontrol = _mapper.Map<Futcontrol>(request);

                Futcontrol _FutcontrolCreado = await _FutcontrolRepositorio.Crear(_Futcontrol);

                if (_FutcontrolCreado.Id != 0)
                    _Respuesta = new Respuesta<FutcontrolDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<FutcontrolDTO>(_FutcontrolCreado) };
                else
                    _Respuesta = new Respuesta<FutcontrolDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<FutcontrolDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] FutcontrolDTO request)
        {
            Respuesta<FutcontrolDTO> _Respuesta = new Respuesta<FutcontrolDTO>();
            try
            {
                Futcontrol _Futcontrol = _mapper.Map<Futcontrol>(request);
                Futcontrol _FutcontrolParaEditar = await _FutcontrolRepositorio.Obtener(u => u.Id == _Futcontrol.Id);

                if (_FutcontrolParaEditar != null)
                {
                    _FutcontrolParaEditar.NroTrans = _Futcontrol.NroTrans;
                    _FutcontrolParaEditar.Fectrans = _Futcontrol.Fectrans;
                    _FutcontrolParaEditar.Sven = _Futcontrol.Sven;
                    _FutcontrolParaEditar.CategSv = _Futcontrol.CategSv;
                    _FutcontrolParaEditar.Vnom = _Futcontrol.Vnom;
                    _FutcontrolParaEditar.Scom = _Futcontrol.Scom;
                    _FutcontrolParaEditar.CategSc = _Futcontrol.CategSc;
                    _FutcontrolParaEditar.Cnom = _Futcontrol.Cnom;
                    _FutcontrolParaEditar.Plantel = _Futcontrol.Plantel;
                    _FutcontrolParaEditar.EdadCrias = _Futcontrol.EdadCrias;
                    _FutcontrolParaEditar.CantHem = _Futcontrol.CantHem;
                    _FutcontrolParaEditar.CantMach = _Futcontrol.CantMach;
                    _FutcontrolParaEditar.PlantDest = _Futcontrol.PlantDest;
                    _FutcontrolParaEditar.Incorp = _Futcontrol.Incorp;
                    _FutcontrolParaEditar.Hemsta = _Futcontrol.Hemsta;
                    _FutcontrolParaEditar.FchUsu = _Futcontrol.FchUsu;
                    _FutcontrolParaEditar.CodUsu = _Futcontrol.CodUsu;
                    bool respuesta = await _FutcontrolRepositorio.Editar(_FutcontrolParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<FutcontrolDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<FutcontrolDTO>(_FutcontrolParaEditar) };
                    else
                        _Respuesta = new Respuesta<FutcontrolDTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<FutcontrolDTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<FutcontrolDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }
    }
}

