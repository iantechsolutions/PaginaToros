using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Context;
using AutoMapper;
using PaginaToros.Server.Repositorio.Contrato;
using Newtonsoft.Json;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Desepla1Controller : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDesepla1Repositorio _Desepla1Repositorio;
        public Desepla1Controller(IDesepla1Repositorio Desepla1Repositorio, IMapper mapper)
        {
            _mapper = mapper;
            _Desepla1Repositorio = Desepla1Repositorio;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {
            Respuesta<List<Desepla1DTO>> _ResponseDTO = new Respuesta<List<Desepla1DTO>>();

            try
            {
                var a = await _Desepla1Repositorio.Lista(skip, take);

                // Map
                var listaPedido = _mapper.Map<List<Desepla1DTO>>(a);

                listaPedido = listaPedido
                    .GroupBy(x => x.Nrodec)
                    .Select(g => g.OrderByDescending(x => x.Id).First())
                    .OrderByDescending(x => x.Nrodec)
                    .ToList();

                _ResponseDTO = new Respuesta<List<Desepla1DTO>>()
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = listaPedido
                };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Desepla1DTO>>()
                {
                    Exito = 1,
                    Mensaje = ex.Message,
                    List = null
                };
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
                var a = await _Desepla1Repositorio.CantidadTotal();

                _ResponseDTO = new Respuesta<int>() { Exito = 1, Mensaje = "Exito", List = a };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<int>() { Exito = 1, Mensaje = ex.Message, List = 0 };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }

        [HttpGet("LimitadosFiltrados")]
        public async Task<IActionResult> LimitadosFiltrados(int skip, int take, string? expression = null)
        {
            var resp = new Respuesta<List<Desepla1DTO>>();
            try
            {
                var lista = await _Desepla1Repositorio.LimitadosFiltrados(skip, take, expression);
                resp.Exito = 1;
                resp.Mensaje = "Éxito";
                resp.List = _mapper.Map<List<Desepla1DTO>>(lista);
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




        [HttpGet]
        [Route("LimitadosFiltradosNoInclude")]
        public async Task<IActionResult> LimitadosFiltradosNoInclude(int skip, int take, string? expression = null)
        {

            Respuesta<List<Desepla1DTO>> _ResponseDTO = new Respuesta<List<Desepla1DTO>>();

            try
            {
                var a = await _Desepla1Repositorio.LimitadosFiltradosNoInclude(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<Desepla1DTO>>(a);

                _ResponseDTO = new Respuesta<List<Desepla1DTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<Desepla1DTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Desepla1 _Desepla1Eliminar = await _Desepla1Repositorio.Obtener(u => u.Id == id);
                if (_Desepla1Eliminar != null)
                {

                    bool respuesta = await _Desepla1Repositorio.Eliminar(_Desepla1Eliminar);

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
        public async Task<IActionResult> Guardar([FromBody] Desepla1DTO request)
        {
            Respuesta<Desepla1DTO> _Respuesta = new Respuesta<Desepla1DTO>();
            try
            {
                Console.WriteLine("Request recibido:");
                Console.WriteLine(JsonConvert.SerializeObject(request));
                Desepla1 _Desepla1 = _mapper.Map<Desepla1>(request);


                Console.WriteLine("Entro aca?");

                Desepla1 _Desepla1Creado = await _Desepla1Repositorio.Crear(_Desepla1);

                if (_Desepla1Creado.Id != 0)
                    _Respuesta = new Respuesta<Desepla1DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Desepla1DTO>(_Desepla1Creado) };
                else
                    _Respuesta = new Respuesta<Desepla1DTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.ToString());
                _Respuesta = new Respuesta<Desepla1DTO>() { Exito = 0, Mensaje = ex.ToString() };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Desepla1DTO request)
        {
            Respuesta<Desepla1DTO> _Respuesta = new Respuesta<Desepla1DTO>();
            try
            {
                Desepla1 _Desepla1 = _mapper.Map<Desepla1>(request);
                Desepla1 _Desepla1ParaEditar = await _Desepla1Repositorio.Obtener(u => u.Id == _Desepla1.Id);

                if (_Desepla1ParaEditar != null)
                {
                    _Desepla1ParaEditar.Nrodec = _Desepla1.Nrodec;
                    _Desepla1ParaEditar.Nroplan = _Desepla1.Nroplan;
                    _Desepla1ParaEditar.Tipse = _Desepla1.Tipse;
                    _Desepla1ParaEditar.Semen = _Desepla1.Semen;
                    _Desepla1ParaEditar.Cantv = _Desepla1.Cantv;
                    _Desepla1ParaEditar.Cantb = _Desepla1.Cantb;
                    _Desepla1ParaEditar.Remba = _Desepla1.Remba;
                    _Desepla1ParaEditar.Remba = _Desepla1.Remba;
                    _Desepla1ParaEditar.Rempr = _Desepla1.Rempr;
                    _Desepla1ParaEditar.Ctrlu = _Desepla1.Ctrlu;
                    _Desepla1ParaEditar.Ctrlm = _Desepla1.Ctrlm;
                    _Desepla1ParaEditar.CoefAutoSn = _Desepla1.CoefAutoSn;
                    _Desepla1ParaEditar.CoefAutoIa = _Desepla1.CoefAutoIa;
                    _Desepla1ParaEditar.CoefAutoIar = _Desepla1.CoefAutoIar;
                    _Desepla1ParaEditar.IaSincro = _Desepla1.IaSincro;
                    _Desepla1ParaEditar.PastillasSincro = _Desepla1.PastillasSincro;
                    _Desepla1ParaEditar.Fecret = _Desepla1.Fecret;
                    _Desepla1ParaEditar.NrFolio = _Desepla1.NrFolio;
                    _Desepla1ParaEditar.FchUsu = _Desepla1.FchUsu;
                    _Desepla1ParaEditar.CodUsu = _Desepla1.CodUsu;
                    _Desepla1ParaEditar.Reten = _Desepla1.Reten;
                    _Desepla1ParaEditar.Edicion = _Desepla1.Edicion;
                    _Desepla1ParaEditar.Nrocri = _Desepla1.Nrocri;
                    _Desepla1ParaEditar.Desde = _Desepla1.Desde;
                    _Desepla1ParaEditar.Hasta = _Desepla1.Hasta;
                    _Desepla1ParaEditar.Fecdecl = _Desepla1.Fecdecl;
                    _Desepla1ParaEditar.Fchrecepcion = _Desepla1.Fchrecepcion;

                    bool respuesta = await _Desepla1Repositorio.Editar(_Desepla1ParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<Desepla1DTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<Desepla1DTO>(_Desepla1ParaEditar) };
                    else
                        _Respuesta = new Respuesta<Desepla1DTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<Desepla1DTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<Desepla1DTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }
    }
}
