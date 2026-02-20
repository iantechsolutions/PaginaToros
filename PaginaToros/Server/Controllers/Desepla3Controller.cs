using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Server.Context;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using AutoMapper;
using PaginaToros.Server.Repositorio.Contrato;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Desepla3Controller : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDesepla3Repositorio _Desepla3Repositorio;
        public Desepla3Controller(IDesepla3Repositorio Desepla3Repositorio, IMapper mapper)
        {
            _mapper = mapper;
            _Desepla3Repositorio = Desepla3Repositorio;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<Desepla3DTO>> _ResponseDTO = new Respuesta<List<Desepla3DTO>>();

            try
            {
                List<Desepla3DTO> listaPedido = new List<Desepla3DTO>();
                var a = await _Desepla3Repositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<Desepla3DTO>>(a);

                _ResponseDTO = new Respuesta<List<Desepla3DTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Desepla3DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                var a = await _Desepla3Repositorio.CantidadTotal();

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

            Respuesta<List<Desepla3DTO>> _ResponseDTO = new Respuesta<List<Desepla3DTO>>();

            try
            {
                var a = await _Desepla3Repositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<Desepla3DTO>>(a);

                _ResponseDTO = new Respuesta<List<Desepla3DTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Desepla3DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }

        // New: Get lines by exact Nrodec parameter (avoids dynamic LINQ in querystring)
        [HttpGet]
        [Route("GetByNrodec")]
        public async Task<IActionResult> GetByNrodec(string nrodec)
        {
            var resp = new Respuesta<List<Desepla3DTO>>();
            try
            {
                if (string.IsNullOrWhiteSpace(nrodec))
                {
                    resp.Exito = 1;
                    resp.Mensaje = "Nrodec vacío";
                    resp.List = new List<Desepla3DTO>();
                    return Ok(resp);
                }

                // Validate length against DB column max length (6)
                if (nrodec.Length > 6)
                {
                    return BadRequest(new Respuesta<List<Desepla3DTO>> { Exito = 0, Mensaje = "Nrodec demasiado largo" });
                }

                var lista = await _Desepla3Repositorio.GetByNrodec(nrodec.Trim());
                resp.Exito = 1;
                resp.Mensaje = "Éxito";
                resp.List = _mapper.Map<List<Desepla3DTO>>(lista);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = ex.Message;
                resp.List = null;
                return StatusCode(StatusCodes.Status500InternalServerError, resp);
            }
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            Respuesta<string> _Respuesta = new Respuesta<string>();
            try
            {
                Desepla3 _Desepla3Eliminar = await _Desepla3Repositorio.Obtener(u => u.Id == id);
                if (_Desepla3Eliminar != null)
                {

                    bool respuesta = await _Desepla3Repositorio.Eliminar(_Desepla3Eliminar);

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
        public async Task<IActionResult> Guardar([FromBody] Desepla3DTO request)
        {
            Respuesta<Desepla3DTO> _Respuesta = new Respuesta<Desepla3DTO>();
            try
            {
                Desepla3 _Desepla3 = _mapper.Map<Desepla3>(request);

                Console.WriteLine("O entro aca??");

                Desepla3 _Desepla3Creado = await _Desepla3Repositorio.Crear(_Desepla3);

                if (_Desepla3Creado.Id != 0)
                    _Respuesta = new Respuesta<Desepla3DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Desepla3DTO>(_Desepla3Creado) };
                else
                    _Respuesta = new Respuesta<Desepla3DTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Desepla3DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Desepla3DTO request)
        {
            Respuesta<Desepla3DTO> _Respuesta = new Respuesta<Desepla3DTO>();
            try
            {
                Desepla3 _Desepla3 = _mapper.Map<Desepla3>(request);
                Desepla3 _Desepla3ParaEditar = await _Desepla3Repositorio.Obtener(u => u.Id == _Desepla3.Id);

                if (_Desepla3ParaEditar != null)
                {
                    _Desepla3ParaEditar.Nrodec = _Desepla3.Nrodec;
                    _Desepla3ParaEditar.Cantv = _Desepla3.Cantv;
                    _Desepla3ParaEditar.Desde = _Desepla3.Desde;
                    _Desepla3ParaEditar.Hasta = _Desepla3.Hasta;
                    _Desepla3ParaEditar.Tatpart = _Desepla3.Tatpart;
                    _Desepla3ParaEditar.Hardb = _Desepla3.Hardb;

                    bool respuesta = await _Desepla3Repositorio.Editar(_Desepla3ParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<Desepla3DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Desepla3DTO>(_Desepla3ParaEditar) };
                    else
                        _Respuesta = new Respuesta<Desepla3DTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<Desepla3DTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Desepla3DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }
    }
}
