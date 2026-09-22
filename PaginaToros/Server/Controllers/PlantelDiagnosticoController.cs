using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Server.Controllers
{
    /// <summary>
    /// Mantenimiento de planteles duplicados. Es una herramienta destructiva, asi que va con
    /// [Authorize] por rol en TODAS las acciones: el resto de los controllers de inspecciones
    /// quedaron sin proteger y no hay que copiar ese patron.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMINISTRADOR,USUARIOMAESTRO")]
    public class PlantelDiagnosticoController : ControllerBase
    {
        private readonly IPlantelDiagnosticoRepositorio _repositorio;

        public PlantelDiagnosticoController(IPlantelDiagnosticoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet("Duplicados")]
        public async Task<IActionResult> Duplicados()
        {
            var respuesta = new Respuesta<List<PlantelDuplicadoGrupoDTO>>();
            try
            {
                respuesta.List = await _repositorio.ListarGruposDuplicadosAsync();
                respuesta.Exito = 1;
                respuesta.Mensaje = respuesta.List.Count == 0
                    ? "No hay planteles duplicados."
                    : $"{respuesta.List.Count} grupo(s) con planteles duplicados.";
            }
            catch (Exception ex)
            {
                respuesta.Exito = 0;
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }

        /// <summary>
        /// Con Aplicar = false devuelve el plan sin tocar nada. Con Aplicar = true lo ejecuta
        /// dentro de una transaccion: o se hace todo o no se hace nada.
        /// </summary>
        [HttpPost("Consolidar")]
        public async Task<IActionResult> Consolidar([FromBody] ConsolidarPlantelesRequest request)
        {
            var respuesta = new Respuesta<ConsolidarPlantelesResultado>();

            if (request == null)
            {
                respuesta.Exito = 0;
                respuesta.Mensaje = "Falta el cuerpo del pedido.";
                return BadRequest(respuesta);
            }

            try
            {
                var resultado = await _repositorio.ConsolidarAsync(request);
                respuesta.List = resultado;
                respuesta.Exito = resultado.Errores.Count == 0 ? 1 : 0;
                respuesta.Mensaje = resultado.Aplicado
                    ? "Cambios aplicados."
                    : (resultado.Errores.Count == 0 ? "Simulación completada." : "No se aplicó ningún cambio.");
            }
            catch (Exception ex)
            {
                respuesta.Exito = 0;
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }
    }
}
