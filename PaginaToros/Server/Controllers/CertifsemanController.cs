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
    public class CertifsemanController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICertifsemanRepositorio _CertifsemanRepositorio;
        public CertifsemanController(ICertifsemanRepositorio CertifsemanRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _CertifsemanRepositorio = CertifsemanRepositorio;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<CertifsemanDTO>> _ResponseDTO = new Respuesta<List<CertifsemanDTO>>();

            try
            {
                List<CertifsemanDTO> listaPedido = new List<CertifsemanDTO>();
                var a = await _CertifsemanRepositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<CertifsemanDTO>>(a);

                _ResponseDTO = new Respuesta<List<CertifsemanDTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<CertifsemanDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                var a = await _CertifsemanRepositorio.CantidadTotal();

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

            Respuesta<List<CertifsemanDTO>> _ResponseDTO = new Respuesta<List<CertifsemanDTO>>();

            try
            {
                var a = await _CertifsemanRepositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<CertifsemanDTO>>(a);

                _ResponseDTO = new Respuesta<List<CertifsemanDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<CertifsemanDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Certifseman _CertifsemanEliminar = await _CertifsemanRepositorio.Obtener(u => u.Id == id);
                if (_CertifsemanEliminar != null)
                {

                    bool respuesta = await _CertifsemanRepositorio.Eliminar(_CertifsemanEliminar);

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
        public async Task<IActionResult> Guardar([FromBody] CertifsemanDTO request)
        {
            Respuesta<CertifsemanDTO> _Respuesta = new Respuesta<CertifsemanDTO>();
            try
            {
                Certifseman _Certifseman = _mapper.Map<Certifseman>(request);

                Certifseman _CertifsemanCreado = await _CertifsemanRepositorio.Crear(_Certifseman);

                if (_CertifsemanCreado.Id != 0)
                    _Respuesta = new Respuesta<CertifsemanDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<CertifsemanDTO>(_CertifsemanCreado) };
                else
                    _Respuesta = new Respuesta<CertifsemanDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<CertifsemanDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] CertifsemanDTO request)
        {
            Respuesta<CertifsemanDTO> _Respuesta = new Respuesta<CertifsemanDTO>();
            try
            {
                Certifseman _Certifseman = _mapper.Map<Certifseman>(request);
                Certifseman _CertifsemanParaEditar = await _CertifsemanRepositorio.Obtener(u => u.Id == _Certifseman.Id);

                if (_CertifsemanParaEditar != null)
                {
                    _CertifsemanParaEditar.TipoCert = _Certifseman.TipoCert;
                    _CertifsemanParaEditar.NroConst = _Certifseman.NroConst;
                    _CertifsemanParaEditar.NroCert = _Certifseman.NroCert;
                    _CertifsemanParaEditar.Nrocen = _Certifseman.Nrocen;
                    _CertifsemanParaEditar.Fecvta = _Certifseman.Fecvta;
                    _CertifsemanParaEditar.FchConst = _Certifseman.FchConst;
                    _CertifsemanParaEditar.Nven = _Certifseman.Nven;
                    _CertifsemanParaEditar.Nrocri = _Certifseman.Nrocri;
                    _CertifsemanParaEditar.CategSc = _Certifseman.CategSc;
                    _CertifsemanParaEditar.Scod = _Certifseman.Scod;
                    _CertifsemanParaEditar.Tatpart = _Certifseman.Tatpart;
                    _CertifsemanParaEditar.Hba = _Certifseman.Hba;
                    _CertifsemanParaEditar.NomDad = _Certifseman.NomDad;
                    _CertifsemanParaEditar.NrInsc = _Certifseman.NrInsc;
                    _CertifsemanParaEditar.NrTsan = _Certifseman.NrTsan;
                    _CertifsemanParaEditar.NrInsd = _Certifseman.NrInsd;
                    _CertifsemanParaEditar.NrDosi = _Certifseman.NrDosi;
                    _CertifsemanParaEditar.NrDosiOr = _Certifseman.NrDosiOr;
                    _CertifsemanParaEditar.TipEnv = _Certifseman.TipEnv;
                    _CertifsemanParaEditar.Variedad = _Certifseman.Variedad;
                    _CertifsemanParaEditar.FchUsu = _Certifseman.FchUsu;
                    _CertifsemanParaEditar.CodUsu = _Certifseman.CodUsu;
                    _CertifsemanParaEditar.Id = _Certifseman.Id;
                    _CertifsemanParaEditar.Apodo = _Certifseman.Apodo;
                    bool respuesta = await _CertifsemanRepositorio.Editar(_CertifsemanParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<CertifsemanDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<CertifsemanDTO>(_CertifsemanParaEditar) };
                    else
                        _Respuesta = new Respuesta<CertifsemanDTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<CertifsemanDTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<CertifsemanDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }
    }
}

