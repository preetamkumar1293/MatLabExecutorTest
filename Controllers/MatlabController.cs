using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MatlabExecutor.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatlabController : ControllerBase
    {

        private readonly ILogger<MatlabController> _logger;

        public MatlabController(ILogger<MatlabController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult RunMatlabExecutable(string matlabExePath, string databasePath, string arg)
        {
            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = matlabExePath,
                    Arguments = string.Format(@"""{0}"" {1}", databasePath, string.Join(" ", arg)),
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processInfo))
                {

                    if (process != null && process.Id != 0)
                    {
                        return Ok("Matlab executable started successfully");
                    }
                    else
                    {
                        return BadRequest("Matlab executable failed");
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while running the Matlab executable.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}