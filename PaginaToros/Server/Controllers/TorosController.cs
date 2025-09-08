using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Repositorio.Contrato;
using AutoMapper;
using PaginaToros.Server.Repositorio.Implementacion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Dynamic.Core;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TorosController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITorosRepositorio _torosRepositorio;
        public TorosController( ITorosRepositorio torosRepositorio,IMapper mapper)
        {
            _mapper = mapper;
            _torosRepositorio = torosRepositorio;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip,int take)
        {

            Respuesta<List<TorosuniDTO>> _ResponseDTO = new Respuesta<List<TorosuniDTO>>();

            try
            {
                List<TorosuniDTO> listaPedido = new List<TorosuniDTO>();
                var a = await _torosRepositorio.Lista(skip, take);

                
                listaPedido = _mapper.Map<List<TorosuniDTO>>(a);

                _ResponseDTO = new Respuesta<List<TorosuniDTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
             

            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<TorosuniDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                var a = await _torosRepositorio.CantidadTotal();

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
            var entidades = await _torosRepositorio.LimitadosFiltrados(skip, take, expression);

            var listaFiltrada = _mapper.Map<List<TorosuniDTO>>(entidades)
                .GroupBy(x => x.Id)
                .Select(g => g.First())
                .ToList();

            var resp = new Respuesta<List<TorosuniDTO>> { Exito = 1, Mensaje = "Exito", List = listaFiltrada };
            return StatusCode(StatusCodes.Status200OK, resp);
        }

        [HttpGet]
        [Route("LimitadosFiltradosNoInclude")]
        public async Task<IActionResult> LimitadosFiltradosNoInclude(int skip, int take, string? expression = null)
        {

            Respuesta<List<TorosuniDTO>> _ResponseDTO = new Respuesta<List<TorosuniDTO>>();

            try
            {
                var a = await _torosRepositorio.LimitadosFiltradosNoInclude(skip,take,expression);

                var listaFiltrada = _mapper.Map<List<TorosuniDTO>>(a);

                _ResponseDTO = new Respuesta<List<TorosuniDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<TorosuniDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Torosuni _ToroEliminar = await _torosRepositorio.Obtener(u => u.Id == id);
                if (_ToroEliminar != null)
                {

                    bool respuesta = await _torosRepositorio.Eliminar(_ToroEliminar);

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
        public async Task<IActionResult> Guardar([FromBody] TorosuniDTO request)
        {
            Respuesta<TorosuniDTO> _Respuesta = new Respuesta<TorosuniDTO>();
            try
            {
                Torosuni _Toro = _mapper.Map<Torosuni>(request);

                Torosuni _ToroCreado = await _torosRepositorio.Crear(_Toro);

                if (_ToroCreado.Id != 0)
                    _Respuesta = new Respuesta<TorosuniDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<TorosuniDTO>(_ToroCreado) };
                else
                    _Respuesta = new Respuesta<TorosuniDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<TorosuniDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] TorosuniDTO request)
        {
            Respuesta<TorosuniDTO> _Respuesta = new Respuesta<TorosuniDTO>();
            try
            {
                Torosuni _Toro = _mapper.Map<Torosuni>(request);
                Torosuni _ToroParaEditar = await _torosRepositorio.Obtener(u => u.Id == _Toro.Id);

                if (_ToroParaEditar != null)
                {

                    _ToroParaEditar.Apodo = _Toro.Apodo;
                    _ToroParaEditar.Nombre = _Toro.Nombre;
                    _ToroParaEditar.EstDoc = _Toro.EstDoc;
                    _ToroParaEditar.ResInsp = _Toro.ResInsp;
                    _ToroParaEditar.SbcodOld = _Toro.SbcodOld;
                    _ToroParaEditar.Sbcod = _Toro.Sbcod;
                    _ToroParaEditar.TipToro = _Toro.TipToro;
                    _ToroParaEditar.Tatpart = _Toro.Tatpart;
                    _ToroParaEditar.NomDad = _Toro.NomDad;
                    _ToroParaEditar.NrInsc = _Toro.NrInsc;
                    _ToroParaEditar.NrTsan = _Toro.NrTsan;
                    _ToroParaEditar.NrInsd = _Toro.NrInsd;
                    _ToroParaEditar.Fecha = _Toro.Fecha;
                    _ToroParaEditar.Hba = _Toro.Hba;
                    _ToroParaEditar.Variedad = _Toro.Variedad;
                    _ToroParaEditar.Criador = _Toro.Criador;
                    _ToroParaEditar.Catego = _Toro.Catego;
                    _ToroParaEditar.Plantel = _Toro.Plantel;
                    _ToroParaEditar.Estcod = _Toro.Estcod;
                    _ToroParaEditar.FchBaja = _Toro.FchBaja;
                    _ToroParaEditar.Activo = _Toro.Activo;
                    _ToroParaEditar.MotivoBaj = _Toro.MotivoBaj;
                    _ToroParaEditar.Nacido = _Toro.Nacido;
                    _ToroParaEditar.Actualizado = _Toro.Actualizado;
                    _ToroParaEditar.CircEscrotal = _Toro.CircEscrotal;
                    _ToroParaEditar.CodEstado = _Toro.CodEstado;
                    _ToroParaEditar.IdTipo = _Toro.IdTipo;
                    _ToroParaEditar.Fecing = _Toro.Fecing;
                    _ToroParaEditar.Fechasba = _Toro.Fechasba;
                    _ToroParaEditar.Pnac = _Toro.Pnac;
                    _ToroParaEditar.Pajudest = _Toro.Pajudest;
                    _ToroParaEditar.Pajufinal = _Toro.Pajufinal;
                    _ToroParaEditar.Gdpostdest = _Toro.Gdpostdest;
                    _ToroParaEditar.Indicedest = _Toro.Indicedest;
                    _ToroParaEditar.Cescrot = _Toro.Cescrot;
                    _ToroParaEditar.Otros1 = _Toro.Otros1;
                    _ToroParaEditar.Promgrupo1 = _Toro.Promgrupo1;
                    _ToroParaEditar.Promgrupo2 = _Toro.Promgrupo2;
                    _ToroParaEditar.Gdvida = _Toro.Gdvida;
                    _ToroParaEditar.Indicefinal = _Toro.Indicefinal;
                    _ToroParaEditar.Frame = _Toro.Frame;
                    _ToroParaEditar.Otros2 = _Toro.Otros2;
                    _ToroParaEditar.Comentario = _Toro.Comentario;

                    bool respuesta = await _torosRepositorio.Editar(_ToroParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<TorosuniDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<TorosuniDTO>(_ToroParaEditar) };
                    else
                        _Respuesta = new Respuesta<TorosuniDTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<TorosuniDTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<TorosuniDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }
    }
}

