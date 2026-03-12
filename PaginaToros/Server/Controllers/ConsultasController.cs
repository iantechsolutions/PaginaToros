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
            return Ok(new
            {
                frontendVersion = FrontendVersion,
                environment = environment.EnvironmentName,
                machineName = Environment.MachineName,
                serverUtcNow = DateTime.UtcNow,
                processStartTimeUtc = GetProcessStartTimeUtc(),
                assemblyLastWriteTimeUtc = GetAssemblyLastWriteTimeUtc(),
                appBaseDirectory = AppContext.BaseDirectory
            });
        }

        private static DateTime? GetAssemblyLastWriteTimeUtc()
        {
            try
            {
                var entryAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
                var assemblyLocation = entryAssembly.Location;

                if (string.IsNullOrWhiteSpace(assemblyLocation) || !System.IO.File.Exists(assemblyLocation))
                {
                    return null;
                }

                return System.IO.File.GetLastWriteTimeUtc(assemblyLocation);
            }
            catch
            {
                return null;
            }
        }

        private static DateTime? GetProcessStartTimeUtc()
        {
            try
            {
                var process = Process.GetCurrentProcess();
                return process.StartTime.Kind == DateTimeKind.Utc
                    ? process.StartTime
                    : process.StartTime.ToUniversalTime();
            }
            catch
            {
                return null;
            }
        }
    }
}
