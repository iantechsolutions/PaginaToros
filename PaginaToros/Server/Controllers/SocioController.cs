using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Client.Pages.Socios;
using PaginaToros.Server.Context;
using AutoMapper;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Repositorio.Implementacion;
using System.Linq.Expressions;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocioController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISocioRepositorio _SocioRepositorio;
        private readonly hereford_prContext _db;
        public SocioController(ISocioRepositorio SocioRepositorio, IMapper mapper, hereford_prContext db)
        {
            _mapper = mapper;
            _SocioRepositorio = SocioRepositorio;
            _db = db;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<SocioDTO>> _ResponseDTO = new Respuesta<List<SocioDTO>>();

            try
            {
                List<SocioDTO> listaPedido = new List<SocioDTO>();
                var a = await _SocioRepositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<SocioDTO>>(a);

                _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                var a = await _SocioRepositorio.CantidadTotal();

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

            Respuesta<List<SocioDTO>> _ResponseDTO = new Respuesta<List<SocioDTO>>();

            try
            {
                var a = await _SocioRepositorio.LimitadosFiltrados(skip, take, expression);

                var listaOrdenada = a.OrderByDescending(s => s.Criador == "S").ToList();

                var listaFiltrada = _mapper.Map<List<SocioDTO>>(listaOrdenada);
                //var listaFiltrada = _mapper.Map<List<SocioDTO>>(a);

                _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }


        [Route("LimitadosFiltradoTodos")]
        public async Task<IActionResult> LimitadosFiltradosTodos(int skip, int take, string? expression = null)
        {
            Respuesta<List<SocioDTO>> _ResponseDTO = new Respuesta<List<SocioDTO>>();

            try
            {
                var a = await _SocioRepositorio.LimitadosFiltradosTodos(skip, take, expression);

                //var listaOrdenada = a.OrderByDescending(s => s.Criador == "S").ToList();

                var listaFiltrada = _mapper.Map<List<SocioDTO>>(a);

                _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = "Éxito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<SocioDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Socio _SocioEliminar = await _SocioRepositorio.Obtener(u => u.Id == id);
                if (_SocioEliminar != null)
                {

                    bool respuesta = await _SocioRepositorio.Eliminar(_SocioEliminar);

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
        public async Task<IActionResult> Guardar([FromBody] SocioDTO request)
        {
            Respuesta<SocioDTO> _Respuesta = new Respuesta<SocioDTO>();
            try
            {
                // Validate Codpos2 uniqueness if provided
                if (!string.IsNullOrWhiteSpace(request.Codpos2))
                {
                    var dup = _db.Socios.FirstOrDefault(s => s.Codpos2 == request.Codpos2);
                    if (dup != null)
                    {
                        _Respuesta = new Respuesta<SocioDTO>() { Exito = 0, Mensaje = "Codpos2 duplicado", List = _mapper.Map<SocioDTO>(dup) };
                        return StatusCode(StatusCodes.Status409Conflict, _Respuesta);
                    }
                }

                // Generate next Scod server-side (numeric sequence using numeric Scod values)
                var allScods = _db.Socios.Select(s => s.Scod).ToList();
                int max = 0;
                foreach (var sc in allScods)
                {
                    if (int.TryParse(sc, out var v)) if (v > max) max = v;
                }
                var nextScod = (max + 1).ToString("D4");

                var nuevoMap = _mapper.Map<Socio>(request);
                nuevoMap.Scod = nextScod;
                // ensure Id is 0 so repository will create
                nuevoMap.Id = 0;

                Socio _SocioCreado = await _SocioRepositorio.Crear(nuevoMap);

                if (_SocioCreado.Id != 0)
                    _Respuesta = new Respuesta<SocioDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<SocioDTO>(_SocioCreado) };
                else
                    _Respuesta = new Respuesta<SocioDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                    errorMessage += " | Inner: " + ex.InnerException.Message;

                _Respuesta = new Respuesta<SocioDTO>()
                {
                    Exito = 1,
                    Mensaje = errorMessage
                };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }

        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] SocioDTO request)
        {
            var resp = new Respuesta<SocioDTO>();
            try
            {
                var socioReq = _mapper.Map<Socio>(request);
                var socioDb = await _SocioRepositorio.Obtener(u => u.Id == socioReq.Id);

                if (socioDb == null)
                    return NotFound(new Respuesta<SocioDTO> { Exito = 0, Mensaje = "No se encontró el identificador" });

                // Do not allow changing Scod from client; keep existing

                // Validate Codpos2 uniqueness if provided and changed
                if (!string.IsNullOrWhiteSpace(socioReq.Codpos2) && !string.Equals(socioDb.Codpos2, socioReq.Codpos2, StringComparison.Ordinal))
                {
                    var conflictCod = _db.Socios.FirstOrDefault(s => s.Codpos2 == socioReq.Codpos2 && s.Id != socioReq.Id);
                    if (conflictCod != null)
                    {
                        return Conflict(new Respuesta<SocioDTO> { Exito = 0, Mensaje = "Codpos2 duplicado", List = _mapper.Map<SocioDTO>(conflictCod) });
                    }
                }

                socioDb.Nombre = socioReq.Nombre;
                socioDb.Direcc1 = socioReq.Direcc1;
                socioDb.Telefo1 = socioReq.Telefo1;
                socioDb.Telefo2 = socioReq.Telefo2;
                socioDb.Locali1 = socioReq.Locali1;
                socioDb.Codpos1 = socioReq.Codpos1;
                socioDb.Codpro1 = socioReq.Codpro1;
                socioDb.Criador = socioReq.Criador;
                socioDb.Mail = socioReq.Mail;
                socioDb.Fecing = socioReq.Fecing;
                socioDb.Placod = socioReq.Placod;
                socioDb.Codpos2 = socioReq.Codpos2;

                var ok = await _SocioRepositorio.Editar(socioDb);

                if (!ok)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new Respuesta<SocioDTO> { Exito = 0, Mensaje = "No se pudo editar el socio" });

                resp = new Respuesta<SocioDTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<SocioDTO>(socioDb) };
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Respuesta<SocioDTO> { Exito = 0, Mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("SyncCodpos2")]
        public async Task<IActionResult> SyncCodpos2()
        {
            try
            {
                var all = _db.Socios.ToList();
                foreach (var s in all)
                {
                    if (string.IsNullOrEmpty(s.Scod)) { s.Codpos2 = s.Scod; continue; }
                    // Codpos2 column has max length 4 in DB. Use rightmost 4 chars to avoid overflow.
                    s.Codpos2 = s.Scod.Length <= 4 ? s.Scod : s.Scod.Substring(s.Scod.Length - 4);
                }
                await _db.SaveChangesAsync();
                return Ok(new Respuesta<string> { Exito = 1, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<string> { Exito = 0, Mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("Reserve")]
        public async Task<IActionResult> Reserve()
        {
            try
            {
                // Compute next numeric Scod (consider only fully numeric Scod values)
                var allScods = _db.Socios.Select(s => s.Scod).ToList();
                int max = 0;
                foreach (var sc in allScods)
                {
                    if (int.TryParse(sc, out var v))
                    {
                        if (v > max) max = v;
                    }
                }
                var next = (max + 1).ToString("D4");

                var nuevo = new Socio
                {
                    Scod = next,
                    Criador = "N",
                    Nombre = string.Empty
                };

                _db.Socios.Add(nuevo);
                await _db.SaveChangesAsync();

                var dto = _mapper.Map<SocioDTO>(nuevo);
                return Ok(new Respuesta<SocioDTO> { Exito = 1, Mensaje = "Reservado", List = dto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<string> { Exito = 0, Mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("ExistsCodpos2")]
        public async Task<IActionResult> ExistsCodpos2(string codpos2, int? excludeId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codpos2))
                    return Ok(new Respuesta<bool> { Exito = 1, Mensaje = "OK", List = false });

                var query = _db.Socios.AsQueryable();
                if (excludeId.HasValue)
                    query = query.Where(s => s.Id != excludeId.Value);

                var exists = query.Any(s => s.Codpos2 == codpos2);
                return Ok(new Respuesta<bool> { Exito = 1, Mensaje = "OK", List = exists });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<string> { Exito = 0, Mensaje = ex.Message });
            }
        }
    }
}
