using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Shared.Models.Response;
using PaginaToros.Shared.Models;
using PaginaToros.Server.Context;
using AutoMapper;
using PaginaToros.Server.Repositorio.Contrato;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentrosiumController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICentrosiumRepositorio _CentrosiumRepositorio;
        private readonly hereford_prContext _db;
        public CentrosiumController(ICentrosiumRepositorio CentrosiumRepositorio, IMapper mapper, hereford_prContext db)
        {
            _mapper = mapper;
            _CentrosiumRepositorio = CentrosiumRepositorio;
            _db = db;
        }
        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {

            Respuesta<List<CentrosiumDTO>> _ResponseDTO = new Respuesta<List<CentrosiumDTO>>();

            try
            {
                List<CentrosiumDTO> listaPedido = new List<CentrosiumDTO>();
                var a = await _CentrosiumRepositorio.Lista(skip, take);


                listaPedido = _mapper.Map<List<CentrosiumDTO>>(a);

                _ResponseDTO = new Respuesta<List<CentrosiumDTO>>() { Exito = 1, Mensaje = "Exito", List = listaPedido };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<CentrosiumDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                var a = await _CentrosiumRepositorio.CantidadTotal();

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

            Respuesta<List<CentrosiumDTO>> _ResponseDTO = new Respuesta<List<CentrosiumDTO>>();

            try
            {
                var a = await _CentrosiumRepositorio.LimitadosFiltrados(skip, take, expression);

                var listaFiltrada = _mapper.Map<List<CentrosiumDTO>>(a);

                _ResponseDTO = new Respuesta<List<CentrosiumDTO>>() { Exito = 1, Mensaje = "Exito", List = listaFiltrada };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);


            }
            catch (Exception ex)
            {
                _ResponseDTO = new Respuesta<List<CentrosiumDTO>>() { Exito = 1, Mensaje = ex.Message, List = null };
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
                Centrosium _CentrosiumEliminar = await _CentrosiumRepositorio.Obtener(u => u.Id == id);
                if (_CentrosiumEliminar != null)
                {

                    bool respuesta = await _CentrosiumRepositorio.Eliminar(_CentrosiumEliminar);

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
        public async Task<IActionResult> Guardar([FromBody] CentrosiumDTO request)
        {
            Respuesta<CentrosiumDTO> _Respuesta = new Respuesta<CentrosiumDTO>();
            try
            {
                Centrosium _Centrosium = _mapper.Map<Centrosium>(request);

                Centrosium _CentrosiumCreado = await _CentrosiumRepositorio.Crear(_Centrosium);

                if (_CentrosiumCreado.Id != 0)
                    _Respuesta = new Respuesta<CentrosiumDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<CentrosiumDTO>(_CentrosiumCreado) };
                else
                    _Respuesta = new Respuesta<CentrosiumDTO>() { Exito = 1, Mensaje = "No se pudo crear el identificador" };

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<CentrosiumDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] CentrosiumDTO request)
        {
            Respuesta<CentrosiumDTO> _Respuesta = new Respuesta<CentrosiumDTO>();
            try
            {
                Centrosium _Centrosium = _mapper.Map<Centrosium>(request);
                Centrosium _CentrosiumParaEditar = await _CentrosiumRepositorio.Obtener(u => u.Id == _Centrosium.Id);

                if (_CentrosiumParaEditar != null)
                {
                    _CentrosiumParaEditar.Nrocen = _Centrosium.Nrocen;
                    _CentrosiumParaEditar.Nombre = _Centrosium.Nombre;
                    _CentrosiumParaEditar.NroCSayg = _Centrosium.NroCSayg;
                    _CentrosiumParaEditar.FchUsu = _Centrosium.FchUsu;
                    _CentrosiumParaEditar.CodUsu = _Centrosium.CodUsu;
                    _CentrosiumParaEditar.Id = _Centrosium.Id;

                    bool respuesta = await _CentrosiumRepositorio.Editar(_CentrosiumParaEditar);

                    if (respuesta)
                        _Respuesta = new Respuesta<CentrosiumDTO>() { Exito = 1, Mensaje = "ok", List = _mapper.Map<CentrosiumDTO>(_CentrosiumParaEditar) };
                    else
                        _Respuesta = new Respuesta<CentrosiumDTO>() { Exito = 1, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    _Respuesta = new Respuesta<CentrosiumDTO>() { Exito = 1, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, _Respuesta);
            }
            catch (Exception ex)
            {
                _Respuesta = new Respuesta<CentrosiumDTO>() { Exito = 1, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _Respuesta);
            }
        }

        /// <summary>
        /// Diagnostica (y opcionalmente repara) los centros que quedaron sin NROCEN.
        /// Un centro sin NROCEN se puede elegir en el alta de certificados porque el combo
        /// muestra el NOMBRE, pero el certificado se graba por NROCEN, así que el backend
        /// lo rechaza con "El centro es obligatorio.".
        /// Con aplicar=false devuelve sólo el plan, sin escribir nada.
        /// </summary>
        [HttpPost]
        [Route("RepararNrocen")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMINISTRADOR,USUARIOMAESTRO")]
        public async Task<IActionResult> RepararNrocen(bool aplicar = false)
        {
            var resultado = new CentroRepairResult { SoloDiagnostico = !aplicar };

            try
            {
                var centros = await _db.Centrosia
                    .AsNoTracking()
                    .OrderBy(c => c.Id)
                    .ToListAsync();

                resultado.TotalCentros = centros.Count;

                var sinCodigo = centros
                    .Where(c => string.IsNullOrWhiteSpace(c.Nrocen))
                    .ToList();

                resultado.CentrosSinNrocen = sinCodigo.Count;

                // NROCEN es principal key de la relación con CERTIFSEMEN: no puede repetirse.
                var codigosUsados = new HashSet<string>(
                    centros.Where(c => !string.IsNullOrWhiteSpace(c.Nrocen))
                           .Select(c => c.Nrocen.Trim()),
                    StringComparer.OrdinalIgnoreCase);

                resultado.CertificadosHuerfanos = await _db.Certifsemen
                    .AsNoTracking()
                    .CountAsync(c => c.Nrocen == null || c.Nrocen.Trim() == "");

                if (sinCodigo.Count == 0)
                {
                    resultado.Observaciones.Add("No hay centros sin NROCEN: no hay nada que reparar.");

                    if (resultado.CertificadosHuerfanos > 0)
                    {
                        resultado.Observaciones.Add(
                            $"Quedan {resultado.CertificadosHuerfanos} certificado(s) con NROCEN vacío. No hay ningún " +
                            "centro sin código al que atribuirlos, así que hay que corregirlos a mano indicando el centro.");
                    }

                    return Ok(new Respuesta<CentroRepairResult> { Exito = 1, Mensaje = "OK", List = resultado });
                }

                // Plan: primero se intenta reusar NRO_C_SAYG (el código real del centro según
                // la Secretaría); si no sirve, se genera el primer número libre.
                foreach (var centro in sinCodigo)
                {
                    var sayg = centro.NroCSayg?.Trim();
                    string nuevo;
                    string origen;

                    if (!string.IsNullOrWhiteSpace(sayg) && sayg.Length <= 6 && !codigosUsados.Contains(sayg))
                    {
                        nuevo = sayg;
                        origen = "NRO_C_SAYG";
                    }
                    else
                    {
                        nuevo = SiguienteNrocenLibre(codigosUsados);
                        origen = "generado";

                        if (!string.IsNullOrWhiteSpace(sayg))
                        {
                            resultado.Observaciones.Add(
                                $"Centro Id={centro.Id}: no se pudo usar NRO_C_SAYG '{sayg}' " +
                                "(supera los 6 caracteres o ya está en uso).");
                        }
                    }

                    codigosUsados.Add(nuevo);

                    resultado.Detalle.Add(new CentroRepairItem
                    {
                        Id = centro.Id,
                        Nombre = centro.Nombre,
                        NroCSayg = sayg,
                        NrocenNuevo = nuevo,
                        Origen = origen,
                        Estado = aplicar ? "reparado" : "pendiente"
                    });
                }

                // Los certificados con NROCEN vacío sólo se pueden reasignar sin ambigüedad
                // cuando hay exactamente un centro sin código.
                var revincularCertificados = sinCodigo.Count == 1 && resultado.CertificadosHuerfanos > 0;

                if (resultado.CertificadosHuerfanos > 0 && !revincularCertificados)
                {
                    resultado.Observaciones.Add(
                        $"Hay {resultado.CertificadosHuerfanos} certificado(s) con NROCEN vacío y {sinCodigo.Count} centros " +
                        "sin código: no se puede deducir a qué centro corresponde cada uno, así que quedan sin tocar.");
                }

                if (!aplicar)
                {
                    resultado.Observaciones.Insert(0,
                        revincularCertificados
                            ? $"Modo análisis: no se escribió nada. Al aplicar se asignan {resultado.Detalle.Count} código(s) " +
                              $"y se revinculan {resultado.CertificadosHuerfanos} certificado(s)."
                            : $"Modo análisis: no se escribió nada. Al aplicar se asignan {resultado.Detalle.Count} código(s).");

                    return Ok(new Respuesta<CentroRepairResult> { Exito = 1, Mensaje = "OK", List = resultado });
                }

                await using var transaction = await _db.Database.BeginTransactionAsync();

                // Se escribe con SQL directo porque NROCEN es principal key de la relación y EF
                // no permite modificar propiedades de clave en una entidad rastreada.
                foreach (var item in resultado.Detalle)
                {
                    var nuevo = item.NrocenNuevo!;
                    var id = item.Id;

                    await _db.Database.ExecuteSqlInterpolatedAsync(
                        $"UPDATE CENTROSIA SET NROCEN = {nuevo} WHERE id = {id}");

                    resultado.CentrosReparados++;
                }

                if (revincularCertificados)
                {
                    var nuevo = resultado.Detalle[0].NrocenNuevo!;

                    resultado.CertificadosRevinculados = await _db.Database.ExecuteSqlInterpolatedAsync(
                        $"UPDATE CERTIFSEMEN SET NROCEN = {nuevo} WHERE NROCEN IS NULL OR TRIM(NROCEN) = ''");

                    resultado.Observaciones.Add(
                        $"Se revincularon {resultado.CertificadosRevinculados} certificado(s) al centro " +
                        $"'{resultado.Detalle[0].Nombre}' (NROCEN '{nuevo}'), el único que estaba sin código.");
                }

                await transaction.CommitAsync();

                return Ok(new Respuesta<CentroRepairResult>
                {
                    Exito = 1,
                    Mensaje = $"Se repararon {resultado.CentrosReparados} centro(s).",
                    List = resultado
                });
            }
            catch (Exception ex)
            {
                resultado.Observaciones.Add("No se aplicó ningún cambio: la transacción se revirtió.");

                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta<CentroRepairResult>
                {
                    Exito = 0,
                    Mensaje = ex.InnerException?.Message ?? ex.Message,
                    List = resultado
                });
            }
        }

        /// <summary>Primer NROCEN numérico libre, respetando el largo máximo de 6 de la columna.</summary>
        private static string SiguienteNrocenLibre(HashSet<string> codigosUsados)
        {
            var maximo = codigosUsados
                .Select(codigo => int.TryParse(codigo, out var valor) ? valor : 0)
                .DefaultIfEmpty(0)
                .Max();

            var candidato = Math.Max(1, maximo + 1);

            while (candidato <= 999999)
            {
                var texto = candidato.ToString("D4");

                if (texto.Length <= 6 && !codigosUsados.Contains(texto))
                {
                    return texto;
                }

                candidato++;
            }

            throw new InvalidOperationException("No quedan valores libres de NROCEN de hasta 6 caracteres.");
        }
    }
}

