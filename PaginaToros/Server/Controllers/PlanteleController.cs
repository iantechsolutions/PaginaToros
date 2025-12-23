using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;
using System.Text.Json;

namespace PaginaPlantels.Server.Cont{
    [Route("api/[controller]")]
    [ApiController]
     
    public class PlantelController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPlantelRepositorio _plantelRepositorio;
        public PlantelController(IPlantelRepositorio plantelRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _plantelRepositorio = plantelRepositorio;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<PlantelDTO>> _ResponseDTO = new Respuesta<List<PlantelDTO>>();

            try
            {
                List<PlantelDTO> listaPedido = new List<PlantelDTO>();
                var a = await _plantelRepositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<PlantelDTO>>(a);
                // Diagnostics: log duplicate Id groups if any
                var dupGroups = listaPedido.GroupBy(p => p.Id).Where(g => g.Count() > 1).ToList();
                if (dupGroups.Any())
                {
                    Console.WriteLine($"[PlanteController][Lista] Duplicate groups count: {dupGroups.Count}");
                    foreach (var g in dupGroups)
                    {
                        Console.WriteLine($"[PlanteController][Lista] Id={g.Key} Count={g.Count()} Placods=[{string.Join(',', g.Select(x=>x.Placod))}]");
                    }
                }

                var deduped = Deduplicate(listaPedido);

                _ResponseDTO = new Respuesta<List<PlantelDTO>>() { Exito = 1, Mensaje = "Exito", List = deduped };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<PlantelDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                var a = await _plantelRepositorio.CantidadTotal();

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

            Respuesta<List<PlantelDTO>> _ResponseDTO = new Respuesta<List<PlantelDTO>>();

            try
            {
                var a = await _plantelRepositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<PlantelDTO>>(a);
                var dupGroups2 = listaFiltrada.GroupBy(p => p.Id).Where(g => g.Count() > 1).ToList();
                if (dupGroups2.Any())
                {
                    Console.WriteLine($"[PlanteController][LimitadosFiltrados] Duplicate groups count: {dupGroups2.Count}");
                    foreach (var g in dupGroups2)
                    {
                        Console.WriteLine($"[PlanteController][LimitadosFiltrados] Id={g.Key} Count={g.Count()} Placods=[{string.Join(',', g.Select(x=>x.Placod))}]");
                    }
                }
                var deduped = Deduplicate(listaFiltrada);

                _ResponseDTO = new Respuesta<List<PlantelDTO>>() { Exito = 1, Mensaje = "Exito", List = deduped };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<PlantelDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }
        [HttpGet]
        [Route("LimitadosFiltradosNoInclude")]
        public async Task<IActionResult> LimitadosFiltradosNoInclude(int skip, int take, string? expression = null)
        {

            Respuesta<List<PlantelDTO>> _ResponseDTO = new Respuesta<List<PlantelDTO>>();

            try
            {
                var a = await _plantelRepositorio.LimitadosFiltradosNoInclude(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<PlantelDTO>>(a);
                var dupGroups3 = listaFiltrada.GroupBy(p => p.Id).Where(g => g.Count() > 1).ToList();
                if (dupGroups3.Any())
                {
                    Console.WriteLine($"[PlanteController][LimitadosFiltradosNoInclude] Duplicate groups count: {dupGroups3.Count}");
                    foreach (var g in dupGroups3)
                    {
                        Console.WriteLine($"[PlanteController][LimitadosFiltradosNoInclude] Id={g.Key} Count={g.Count()} Placods=[{string.Join(',', g.Select(x=>x.Placod))}]");
                    }
                }
                var deduped = Deduplicate(listaFiltrada);

                _ResponseDTO = new Respuesta<List<PlantelDTO>>() { Exito = 1, Mensaje = "Exito", List = deduped };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<PlantelDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }

        [HttpGet]
        [Route("ObtenerPorAnios")]
        public async Task<IActionResult> ObtenerPorAnios(int anio1, int anio2)
        {
            var _ResponseDTO = new Respuesta<List<PlantelDTO>>();

            if (anio1 > anio2)
            {
                _ResponseDTO = new Respuesta<List<PlantelDTO>>()
                {
                    Exito = 0,
                    Mensaje = "El primer año debe ser menor o igual al segundo.",
                    List = null
                };
                return BadRequest(_ResponseDTO);
            }

            try
            {
                var a = await _plantelRepositorio.ObtenerPorAnios(anio1, anio2);
                var listaFiltrada = _mapper.Map<List<PlantelDTO>>(a);
                var dupGroups4 = listaFiltrada.GroupBy(p => p.Id).Where(g => g.Count() > 1).ToList();
                if (dupGroups4.Any())
                {
                    Console.WriteLine($"[PlanteController][ObtenerPorAnios] Duplicate groups count: {dupGroups4.Count}");
                    foreach (var g in dupGroups4)
                    {
                        Console.WriteLine($"[PlanteController][ObtenerPorAnios] Id={g.Key} Count={g.Count()} Placods=[{string.Join(',', g.Select(x=>x.Placod))}]");
                    }
                }
                var deduped = Deduplicate(listaFiltrada);

                _ResponseDTO = new Respuesta<List<PlantelDTO>>() { Exito = 1, Mensaje = "Éxito", List = deduped };
                return Ok(_ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<PlantelDTO>>() { Exito = 0, Mensaje = ex.Message, List = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }
        [HttpGet]
        [Route("ObtenerPorRangoFechas")]
        public async Task<IActionResult> ObtenerPorRangoFechas(DateTime desde, DateTime hasta)
        {
            var _ResponseDTO = new Respuesta<List<PlantelDTO>>();

            if (desde > hasta)
            {
                _ResponseDTO = new Respuesta<List<PlantelDTO>>()
                {
                    Exito = 0,
                    Mensaje = "La fecha inicial no puede ser posterior a la fecha final.",
                    List = null
                };
                return BadRequest(_ResponseDTO);
            }

            try
            {
                var lista = await _plantelRepositorio.ObtenerPorRangoFechas(desde, hasta); 
                var listaDTO = _mapper.Map<List<PlantelDTO>>(lista);
                var dupGroups5 = listaDTO.GroupBy(p => p.Id).Where(g => g.Count() > 1).ToList();
                if (dupGroups5.Any())
                {
                    Console.WriteLine($"[PlanteController][ObtenerPorRangoFechas] Duplicate groups count: {dupGroups5.Count}");
                    foreach (var g in dupGroups5)
                    {
                        Console.WriteLine($"[PlanteController][ObtenerPorRangoFechas] Id={g.Key} Count={g.Count()} Placods=[{string.Join(',', g.Select(x=>x.Placod))}]");
                    }
                }
                var deduped = Deduplicate(listaDTO);

                _ResponseDTO = new Respuesta<List<PlantelDTO>>()
                {
                    Exito = 1,
                    Mensaje = "Éxito",
                    List = deduped
                };

                return Ok(_ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<PlantelDTO>>()
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = null
                };

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
                Plantel _PlantelEliminar = await _plantelRepositorio.Obtener(u => u.Id == id);
                if (_PlantelEliminar != null)
                {

                    bool respuesta = await _plantelRepositorio.Eliminar(_PlantelEliminar);

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
        public async Task<IActionResult> Guardar([FromBody] PlantelDTO request)
        {
            var _Respuesta = new Respuesta<PlantelDTO>();
            try
            {
                var entidad = _mapper.Map<Plantel>(request);

                var creado = await _plantelRepositorio.Crear(entidad);

                if (creado?.Id > 0)
                {
                    _Respuesta = new Respuesta<PlantelDTO>
                    {
                        Exito = 1,
                        Mensaje = "ok",
                        List = _mapper.Map<PlantelDTO>(creado)
                    };
                    return StatusCode(StatusCodes.Status201Created, _Respuesta);
                }

                _Respuesta = new Respuesta<PlantelDTO>
                {
                    Exito = 0,
                    Mensaje = "No se pudo crear el identificador"
                };
                return BadRequest(_Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<PlantelDTO>
                {
                    Exito = 0,
                    Mensaje = ex.Message
                };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }


        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] PlantelDTO request)
        {
            Respuesta<PlantelDTO> _Respuesta = new Respuesta<PlantelDTO>();
            try
            {
                Console.WriteLine(JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true }));

                Plantel _Plantel = _mapper.Map<Plantel>(request);
                Plantel _PlantelParaEditar = await _plantelRepositorio.Obtener(u => u.Id == _Plantel.Id);

                if (_PlantelParaEditar != null)
                {
                    _PlantelParaEditar.Placod = _Plantel.Placod;
                    _PlantelParaEditar.Anioex = _Plantel.Anioex;
                    _PlantelParaEditar.Varede = _Plantel.Varede;
                    _PlantelParaEditar.Vqcsrd = _Plantel.Vqcsrd;
                    _PlantelParaEditar.Vqssrd = _Plantel.Vqssrd;
                    _PlantelParaEditar.Varepr = _Plantel.Varepr;
                    _PlantelParaEditar.Vqcsrp = _Plantel.Vqcsrp;
                    _PlantelParaEditar.Vqssrp = _Plantel.Vqssrp;
                    _PlantelParaEditar.Feulti = _Plantel.Feulti;
                    _PlantelParaEditar.Nroins = _Plantel.Nroins;
                    _PlantelParaEditar.Nrocri = _Plantel.Nrocri;
                    _PlantelParaEditar.Catego = _Plantel.Catego;
                    _PlantelParaEditar.Aniopa = _Plantel.Aniopa;
                    _PlantelParaEditar.Urein = _Plantel.Urein;
                    _PlantelParaEditar.FchUsu = _Plantel.FchUsu;
                    _PlantelParaEditar.CodUsu = _Plantel.CodUsu;
                    _PlantelParaEditar.Comment = _Plantel.Comment;
                    _PlantelParaEditar.Estado = _Plantel.Estado;
                    _PlantelParaEditar.Fecing = _Plantel.Fecing;

                    bool respuesta = await _plantelRepositorio.Editar(_PlantelParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<PlantelDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<PlantelDTO>(_PlantelParaEditar) };
                    else
                        _Respuesta = new Respuesta<PlantelDTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<PlantelDTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<PlantelDTO>() { Exito = 0, Mensaje = ex.ToString() };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPost]
        [Route("GetOrCreate")]
        public async Task<IActionResult> GetOrCreate([FromBody] PlantelGetOrCreateRequest request)
        {
            var _Respuesta = new Respuesta<PlantelDTO>();
            try
            {
                // Try to find by Placod
                Plantel? found = null;
                if (!string.IsNullOrWhiteSpace(request.Placod))
                {
                    found = await _plantelRepositorio.Obtener(p => p.Placod == request.Placod);
                }

                // If not found, try by Anioex + Nrocri
                if (found == null)
                {
                    found = (await _plantelRepositorio.LimitadosFiltradosNoInclude(0, 0, $"Anioex == \"{request.Anioex}\" && Nrocri == {request.Nrocri}"))
                            .FirstOrDefault();
                }

                if (found != null)
                {
                    // Update counts and return
                    found.Varede = request.Varede;
                    found.Vqcsrd = request.Vqcsrd;
                    found.Vqssrd = request.Vqssrd;
                    found.Varepr = request.Varepr;
                    found.Vqcsrp = request.Vqcsrp;
                    found.Vqssrp = request.Vqssrp;

                    await _plantelRepositorio.Editar(found);

                    _Respuesta = new Respuesta<PlantelDTO>
                    {
                        Exito = 1,
                        Mensaje = "ok",
                        List = _mapper.Map<PlantelDTO>(found)
                    };
                    return Ok(_Respuesta);
                }

                // Not found: create new using base tail
                var tail = request.BasePlacod ?? string.Empty;
                var nuevoPlacod = (!string.IsNullOrEmpty(request.Anioex) ? request.Anioex[^1].ToString() : "") + tail;

                var nuevo = new Plantel
                {
                    Anioex = request.Anioex,
                    Placod = nuevoPlacod,
                    Varede = request.Varede,
                    Vqcsrd = request.Vqcsrd,
                    Vqssrd = request.Vqssrd,
                    Varepr = request.Varepr,
                    Vqcsrp = request.Vqcsrp,
                    Vqssrp = request.Vqssrp,
                    Nrocri = request.Nrocri.ToString(),
                    Estado = "A",
                    Fecing = string.Empty
                };

                var creado = await _plantelRepositorio.Crear(nuevo);

                _Respuesta = new Respuesta<PlantelDTO>
                {
                    Exito = 1,
                    Mensaje = "ok",
                    List = _mapper.Map<PlantelDTO>(creado)
                };
                return StatusCode(StatusCodes.Status201Created, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<PlantelDTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        // Helper to remove duplicates using Placod+Anioex+Nrocri key, fallback to Id
        private List<PlantelDTO> Deduplicate(List<PlantelDTO> list)
        {
            if (list == null) return null;
            var grouped = list
                .GroupBy(p =>
                {
                    var placod = p.Placod ?? string.Empty;
                    var anio = p.Anioex ?? string.Empty;
                    var nrocri = p.Nrocri ?? string.Empty;
                    var key = (placod + "|" + anio + "|" + nrocri).Trim();
                    return string.IsNullOrEmpty(key) ? ("ID|" + p.Id.ToString()) : ("P|" + key);
                })
                .Select(g => g.First())
                .ToList();
            return grouped;
        }
    }
}
