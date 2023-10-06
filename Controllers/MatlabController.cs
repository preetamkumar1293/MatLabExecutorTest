using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MatlabExecutor.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatlabController : ControllerBase
    {

        private readonly ILogger<MatlabController> _logger;
        private string MatLabBatchRunner = "run_matlab.bat";

        public MatlabController(ILogger<MatlabController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult RunMatlabExecutable(string matlabExePath, string arg)
        {
            // Specify the path to the batch script
            string BatchScriptPath = Path.Combine(Directory.GetCurrentDirectory(), MatLabBatchRunner);
            _logger.LogInformation("matlabExePath:"+ matlabExePath);
            _logger.LogInformation("arg:"+ arg);
            _logger.LogInformation("Command to Execute:"+ string.Format("{0} {1}", matlabExePath, arg));
            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Verb = "runas",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                    //FileName = matlabExePath,
                    //Arguments = string.Format(@"""{0}"" {1}", databasePath, string.Join(" ", arg)),
                    //RedirectStandardOutput = true,
                    //UseShellExecute = false,
                    //CreateNoWindow = true
                };

                using (var process = Process.Start(processInfo))
                {
                    if (process != null && process.Id != 0)
                    {

                        process.StandardInput.WriteLine(string.Format("{0} {1}", matlabExePath, arg));
                        _logger.LogInformation("Executed Command:"+ string.Format("{0} {1}", matlabExePath, arg));
                        process.StandardInput.Close();
                        string output = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();
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