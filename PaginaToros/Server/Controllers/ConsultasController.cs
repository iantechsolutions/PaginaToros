using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

namespace PaginaToros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMINISTRADOR")]
    public class ConsultasController : ControllerBase
    {
        private const string FrontendVersion = "1.0.0";
        private readonly IWebHostEnvironment environment;

        public ConsultasController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        [HttpGet("info")]
        public ActionResult GetInfo()
        {
            var entryAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            var assemblyLocation = entryAssembly.Location;
            var assemblyLastWriteTimeUtc = string.IsNullOrWhiteSpace(assemblyLocation)
                ? (DateTime?)null
                : System.IO.File.GetLastWriteTimeUtc(assemblyLocation);

            var process = Process.GetCurrentProcess();
            var processStartTimeUtc = process.StartTime.Kind == DateTimeKind.Utc
                ? process.StartTime
                : process.StartTime.ToUniversalTime();

            return Ok(new
            {
                frontendVersion = FrontendVersion,
                environment = environment.EnvironmentName,
                machineName = Environment.MachineName,
                serverUtcNow = DateTime.UtcNow,
                processStartTimeUtc,
                assemblyLastWriteTimeUtc,
                appBaseDirectory = AppContext.BaseDirectory
            });
        }
    }
}
