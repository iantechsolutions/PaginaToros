using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Services;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertifsemanController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICertifsemanRepositorio _certifsemanRepositorio;
        private readonly IUserSocioContextService _userSocioContextService;

        public CertifsemanController(
            ICertifsemanRepositorio certifsemanRepositorio,
            IMapper mapper,
            IUserSocioContextService userSocioContextService)
        {
            _mapper = mapper;
            _certifsemanRepositorio = certifsemanRepositorio;
            _userSocioContextService = userSocioContextService;
        }

        [Route("Lista")]
        public async Task<IActionResult> Lista(int skip, int take)
        {
            var response = new Respuesta<List<CertifsemanDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<CertifsemanDTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? BuildActiveSocioFilter(accessContext.ActiveSocioCode!)
                    : null;
                var items = await _certifsemanRepositorio.LimitadosFiltrados(skip, take, effectiveExpression);

                response = new Respuesta<List<CertifsemanDTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<CertifsemanDTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<CertifsemanDTO>>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = null
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route("Cantidad")]
        public async Task<IActionResult> CantidadTotal()
        {
            var response = new Respuesta<int>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<int>());
                }

                int cantidad;
                if (RequiresActiveSocioScope(accessContext))
                {
                    var query = await _certifsemanRepositorio.Consultar(x => x.Nrocri == accessContext.ActiveSocioCode);
                    cantidad = query.Count();
                }
                else
                {
                    cantidad = await _certifsemanRepositorio.CantidadTotal();
                }

                response = new Respuesta<int>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = cantidad
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<int>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = 0
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route("LimitadosFiltrados")]
        public async Task<IActionResult> LimitadosFiltrados(int skip, int take, string? expression = null)
        {
            var response = new Respuesta<List<CertifsemanDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<CertifsemanDTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? AppendFilter(expression, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : expression;
                var items = await _certifsemanRepositorio.LimitadosFiltrados(skip, take, effectiveExpression);

                response = new Respuesta<List<CertifsemanDTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<CertifsemanDTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<CertifsemanDTO>>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = null
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route("LimitadosFiltradosNoInclude")]
        public async Task<IActionResult> LimitadosFiltradosNoInclude(int skip, int take, string? expression = null)
        {
            var response = new Respuesta<List<CertifsemanDTO>>();

            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<List<CertifsemanDTO>>());
                }

                var effectiveExpression = RequiresActiveSocioScope(accessContext)
                    ? AppendFilter(expression, BuildActiveSocioFilter(accessContext.ActiveSocioCode!))
                    : expression;
                var items = await _certifsemanRepositorio.LimitadosFiltradosNoInclude(skip, take, effectiveExpression);

                response = new Respuesta<List<CertifsemanDTO>>
                {
                    Exito = 1,
                    Mensaje = "Exito",
                    List = _mapper.Map<List<CertifsemanDTO>>(items)
                };

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<List<CertifsemanDTO>>
                {
                    Exito = 0,
                    Mensaje = ex.Message,
                    List = null
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = new Respuesta<string>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                }

                var entity = await _certifsemanRepositorio.Obtener(u => u.Id == id);
                if (entity != null)
                {
                    if (RequiresActiveSocioScope(accessContext) &&
                        !string.Equals(entity.Nrocri, accessContext.ActiveSocioCode, StringComparison.OrdinalIgnoreCase))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<string>());
                    }

                    var ok = await _certifsemanRepositorio.Eliminar(entity);
                    response = ok
                        ? new Respuesta<string> { Exito = 1, Mensaje = "ok", List = string.Empty }
                        : new Respuesta<string> { Exito = 0, Mensaje = "No se pudo eliminar el identificador", List = string.Empty };
                }

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<string> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] CertifsemanDTO request)
        {
            if (request == null)
            {
                return BadRequest(new Respuesta<CertifsemanDTO> { Exito = 0, Mensaje = "Request vacío." });
            }

            var response = new Respuesta<CertifsemanDTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<CertifsemanDTO>());
                }

                var entity = _mapper.Map<Certifseman>(request);
                if (RequiresActiveSocioScope(accessContext))
                {
                    entity.Nrocri = accessContext.ActiveSocioCode!;
                }

                var created = await _certifsemanRepositorio.Crear(entity);

                response = created.Id != 0
                    ? new Respuesta<CertifsemanDTO>
                    {
                        Exito = 1,
                        Mensaje = "ok",
                        List = _mapper.Map<CertifsemanDTO>(created)
                    }
                    : new Respuesta<CertifsemanDTO>
                    {
                        Exito = 0,
                        Mensaje = "No se pudo crear el certificado."
                    };

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<CertifsemanDTO>
                {
                    Exito = 0,
                    Mensaje = ex.InnerException?.Message ?? ex.Message
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] CertifsemanDTO request)
        {
            var response = new Respuesta<CertifsemanDTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<CertifsemanDTO>());
                }

                var entity = _mapper.Map<Certifseman>(request);
                var entityToEdit = await _certifsemanRepositorio.Obtener(u => u.Id == entity.Id);

                if (entityToEdit != null)
                {
                    if (RequiresActiveSocioScope(accessContext) &&
                        !string.Equals(entityToEdit.Nrocri, accessContext.ActiveSocioCode, StringComparison.OrdinalIgnoreCase))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<CertifsemanDTO>());
                    }

                    entityToEdit.TipoCert = entity.TipoCert;
                    entityToEdit.NroConst = entity.NroConst;
                    entityToEdit.NroCert = entity.NroCert;
                    entityToEdit.Nrocen = entity.Nrocen;
                    entityToEdit.Fecvta = entity.Fecvta;
                    entityToEdit.FchConst = entity.FchConst;
                    entityToEdit.Nven = entity.Nven;
                    entityToEdit.Nrocri = RequiresActiveSocioScope(accessContext)
                        ? accessContext.ActiveSocioCode!
                        : entity.Nrocri;
                    entityToEdit.CategSc = entity.CategSc;
                    entityToEdit.Scod = entity.Scod;
                    entityToEdit.Tatpart = entity.Tatpart;
                    entityToEdit.Hba = entity.Hba;
                    entityToEdit.NomDad = entity.NomDad;
                    entityToEdit.NrInsc = entity.NrInsc;
                    entityToEdit.NrTsan = entity.NrTsan;
                    entityToEdit.NrInsd = entity.NrInsd;
                    entityToEdit.NrDosi = entity.NrDosi;
                    entityToEdit.NrDosiOr = entity.NrDosiOr;
                    entityToEdit.TipEnv = entity.TipEnv;
                    entityToEdit.Variedad = entity.Variedad;
                    entityToEdit.FchUsu = entity.FchUsu;
                    entityToEdit.CodUsu = entity.CodUsu;
                    entityToEdit.Id = entity.Id;
                    entityToEdit.Apodo = entity.Apodo;
                    entityToEdit.Apodacion = entity.Apodacion;

                    var ok = await _certifsemanRepositorio.Editar(entityToEdit);
                    response = ok
                        ? new Respuesta<CertifsemanDTO> { Exito = 1, Mensaje = "ok", List = _mapper.Map<CertifsemanDTO>(entityToEdit) }
                        : new Respuesta<CertifsemanDTO> { Exito = 0, Mensaje = "No se pudo editar el identificador" };
                }
                else
                {
                    response = new Respuesta<CertifsemanDTO> { Exito = 0, Mensaje = "No se encontró el identificador" };
                }

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response = new Respuesta<CertifsemanDTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut]
        [Route("{id}/nr-dosi")]
        public async Task<IActionResult> UpdateNrDosi(int id, [FromBody] int nrDosi)
        {
            var response = new Respuesta<CertifsemanDTO>();
            try
            {
                var accessContext = await _userSocioContextService.ResolveAsync(User);
                if (RequiresActiveSocioScope(accessContext) && string.IsNullOrWhiteSpace(accessContext.ActiveSocioCode))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<CertifsemanDTO>());
                }

                if (nrDosi < 0)
                {
                    return Conflict(new Respuesta<CertifsemanDTO> { Exito = 0, Mensaje = "NrDosi no puede ser negativo" });
                }

                var existente = await _certifsemanRepositorio.Obtener(x => x.Id == id);
                if (existente == null)
                {
                    response = new Respuesta<CertifsemanDTO> { Exito = 0, Mensaje = "No se encontró el certificado" };
                    return NotFound(response);
                }

                if (RequiresActiveSocioScope(accessContext) &&
                    !string.Equals(existente.Nrocri, accessContext.ActiveSocioCode, StringComparison.OrdinalIgnoreCase))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, BuildForbiddenResponse<CertifsemanDTO>());
                }

                var ok = await _certifsemanRepositorio.UpdateNrDosiAsync(id, nrDosi);
                if (!ok)
                {
                    response = new Respuesta<CertifsemanDTO> { Exito = 0, Mensaje = "No se pudo actualizar NrDosi" };
                    return StatusCode(StatusCodes.Status200OK, response);
                }

                existente.NrDosi = nrDosi;
                response = new Respuesta<CertifsemanDTO>
                {
                    Exito = 1,
                    Mensaje = "ok",
                    List = _mapper.Map<CertifsemanDTO>(existente)
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var err = new Respuesta<CertifsemanDTO> { Exito = 0, Mensaje = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }

        private static bool RequiresActiveSocioScope(UserSocioAccessContext accessContext)
            => accessContext.IsSocioUser && !accessContext.IsPrivilegedUser;

        private static string BuildActiveSocioFilter(string activeSocioCode)
            => $"Nrocri == \"{activeSocioCode}\"";

        private static string AppendFilter(string? expression, string requiredFilter)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return requiredFilter;
            }

            return $"({expression}) && ({requiredFilter})";
        }

        private static Respuesta<T> BuildForbiddenResponse<T>()
            => new Respuesta<T>
            {
                Exito = 0,
                Mensaje = "No tenes permisos para operar sobre otra razon social."
            };
    }
}
