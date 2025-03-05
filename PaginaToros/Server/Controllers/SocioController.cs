using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Client.Pages.Socios;
using PaginaToros.Server.Context;
using AutoMapper;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Repositorio.Implementacion;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocioController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISocioRepositorio _SocioRepositorio;
        public SocioController(ISocioRepositorio SocioRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _SocioRepositorio = SocioRepositorio;
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

                var listaOrdenada = a.OrderByDescending(s => s.Criador == "S").ToList();

                var listaFiltrada = _mapper.Map<List<SocioDTO>>(listaOrdenada);

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
                Socio _Socio = _mapper.Map<Socio>(request);
                //var SocL = await _SocioRepositorio.Lista(0, 1);
                //Socio _SocioViejo = SocL.FirstOrDefault();
                //_Socio.Scod = (Int32.Parse(_SocioViejo.Scod) + 1).ToString("D4");
                Socio _SocioCreado = await _SocioRepositorio.Crear(_Socio);

                if (_SocioCreado.Id != 0)
                    _Respuesta = new Respuesta<SocioDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<SocioDTO>(_SocioCreado) };
                else
                    _Respuesta = new Respuesta<SocioDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<SocioDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] SocioDTO request)
        {
            Respuesta<SocioDTO> _Respuesta = new Respuesta<SocioDTO>();
            try
            {
                Socio _Socio = _mapper.Map<Socio>(request);
                Socio _SocioParaEditar = await _SocioRepositorio.Obtener(u => u.Id == _Socio.Id);

                if (_SocioParaEditar != null)
                {
                    _SocioParaEditar.Scod = _Socio.Scod;
                    _SocioParaEditar.Nombre = _Socio.Nombre;
                    _SocioParaEditar.Direcc1 = _Socio.Direcc1;
                    _SocioParaEditar.Telefo1 = _Socio.Telefo1;
                    _SocioParaEditar.Telefo2 = _Socio.Telefo2;
                    _SocioParaEditar.Locali1 = _Socio.Locali1;
                    _SocioParaEditar.Codpos1 = _Socio.Codpos1;
                    _SocioParaEditar.Codpro1 = _Socio.Codpro1;
                    _SocioParaEditar.Telefo2 = _Socio.Telefo2;
                    _SocioParaEditar.Criador = _Socio.Criador;
                    _SocioParaEditar.Mail = _Socio.Mail;
                    _SocioParaEditar.Fecing = _Socio.Fecing;
                    _SocioParaEditar.Placod = _Socio.Placod;

                    bool respuesta = await _SocioRepositorio.Editar(_SocioParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<SocioDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<SocioDTO>(_SocioParaEditar) };
                    else
                        _Respuesta = new Respuesta<SocioDTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<SocioDTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<SocioDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }
    }
}
