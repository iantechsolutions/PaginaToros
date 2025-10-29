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
        [Route("CantidadFiltrada")]
        public async Task<IActionResult> CantidadFiltrada(string? expression = null)
        {
            try
            {
                var count = await _torosRepositorio.CantidadFiltrada(expression);
                return Ok(new Respuesta<int> { Exito = 1, Mensaje = "Éxito", List = count });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Respuesta<int> { Exito = 0, Mensaje = ex.Message, List = 0 });
            }
        }
        [HttpGet]
        [Route("LimitadosFiltrados")]
        public async Task<IActionResult> LimitadosFiltrados(int skip, int take, string? expression = null)
        {
            try
            {
                var entidades = await _torosRepositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<TorosuniDTO>>(entidades)
                    .GroupBy(x => x.Id)
                    .Select(g => g.First())
                    .ToList();

                var resp = new Respuesta<List<TorosuniDTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = listaFiltrada
                };

                return Ok(resp);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TorosController] Error: {ex}");
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Respuesta<List<TorosuniDTO>>
                    {
                        Exito = 0,
                        Mensaje = $"Error en filtro: {ex.Message}",
                        List = null
                    });
            }
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

        [HttpGet("ById/{id:int}")]
        public async Task<IActionResult> ById(int id)
        {
            Console.WriteLine($"=== ENTRO Toros/ById/{id} ===");
            try
            {
                var repo = await _torosRepositorio.GetById(id);
                Console.WriteLine($"[Ctrl] Repo.Exito={repo.Exito}, HasObj={repo.List != null}");

                if (repo.Exito == 0 || repo.List == null)
                {
                    Console.WriteLine($"[Ctrl] NOT FOUND Id={id}. Msg={repo.Mensaje}");
                    return NotFound(new Respuesta<TorosuniDTO>
                    {
                        Exito = 0,
                        Mensaje = repo.Mensaje
                    });
                }

                var dto = _mapper.Map<TorosuniDTO>(repo.List);
                Console.WriteLine($"[Ctrl] Map OK -> DTO Id={dto?.Id}");

                var resp = new Respuesta<TorosuniDTO>
                {
                    Exito = 1,
                    Mensaje = "Éxito",
                    List = dto
                };

                Console.WriteLine("=== SALIO OK Toros/ById ===");
                return Ok(resp);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Ctrl] EX ById {id}: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Respuesta<TorosuniDTO> { Exito = 0, Mensaje = ex.Message });
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
            var resp = new Respuesta<TorosuniDTO>();

            try
            {
                Torosuni entidad;

                if (request.Id != 0)
                {
                    entidad = await _torosRepositorio.Obtener(t => t.Id == request.Id);

                    if (entidad == null)
                    {
                        resp = new Respuesta<TorosuniDTO> { Exito = 0, Mensaje = "No se encontró el identificador" };
                        return StatusCode(StatusCodes.Status404NotFound, resp);
                    }

                    if (!string.IsNullOrWhiteSpace(request.Criador))
                        request.Criador = request.Criador.Trim();

                    _mapper.Map(request, entidad);


                    var ok = await _torosRepositorio.Editar(entidad);
                    if (!ok)
                    {
                        resp = new Respuesta<TorosuniDTO> { Exito = 0, Mensaje = "No se pudo editar el identificador" };
                        return StatusCode(StatusCodes.Status200OK, resp);
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(request.Criador))
                        request.Criador = request.Criador.Trim();

                    entidad = _mapper.Map<Torosuni>(request);
                    entidad = await _torosRepositorio.Crear(entidad);

                    if (entidad.Id == 0)
                    {
                        resp = new Respuesta<TorosuniDTO> { Exito = 0, Mensaje = "No se pudo crear el identificador" };
                        return StatusCode(StatusCodes.Status200OK, resp);
                    }
                }

                resp = new Respuesta<TorosuniDTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<TorosuniDTO>(entidad) };
                return StatusCode(StatusCodes.Status200OK, resp);
            }
            catch (Exception ex)
            {
                resp = new Respuesta<TorosuniDTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, resp);
            }
        }
    }
}

