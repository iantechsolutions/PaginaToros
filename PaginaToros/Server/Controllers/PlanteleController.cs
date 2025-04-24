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

                _ResponseDTO = new Respuesta<List<PlantelDTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

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

                _ResponseDTO = new Respuesta<List<PlantelDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

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

                _ResponseDTO = new Respuesta<List<PlantelDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

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

            Respuesta<List<PlantelDTO>> _ResponseDTO = new Respuesta<List<PlantelDTO>>();

            try
            {
                var a = await _plantelRepositorio.ObtenerPorAnios(anio1, anio2);

                var listaFiltrada = _mapper.Map<List<PlantelDTO>>(a);

                _ResponseDTO = new Respuesta<List<PlantelDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<PlantelDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
            Respuesta<PlantelDTO> _Respuesta = new Respuesta<PlantelDTO>();
            try
            {
                Plantel _Plantel = _mapper.Map<Plantel>(request);

                Plantel _PlantelCreado = await _plantelRepositorio.Crear(_Plantel);

                if (_PlantelCreado.Id != 0)
                    _Respuesta = new Respuesta<PlantelDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<PlantelDTO>(_PlantelCreado) };
                else
                    _Respuesta = new Respuesta<PlantelDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<PlantelDTO>() { Exito = 1, Mensaje = ex.Message };
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
    }
}
