namespace TwelveFactor.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;
    using System;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Mvc;
    
    [ApiController]
    [Route("[controller]")]
    public class GuidController : ControllerBase
    {
        private readonly ILogger<GuidController> _logger;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public GuidController(ILogger<GuidController> logger, IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            appLifetime.ApplicationStopping.Register(_cts.Cancel);
        }

        [HttpGet("New")]
        public async Task<ActionResult<string>> GetNewGuid(CancellationToken ct = default)
        {
            try
            {
                this.LogInfo("GetNewGuid called.");

                var guid = Guid.NewGuid().ToString();

                if (CheckIfConvertToUpperCase())
                {
                    guid = guid.ToUpper();
                }
                
                // Configurable delay
                var linkedCt = this.CreateLinkedToken(ct);
                await this.DelayAsync(linkedCt);
            
                this.LogInfo($"Returning new GUID <{guid}>.");
                return guid;
            }
            catch (TaskCanceledException)
            {
                // HTTP Status 503: Service Unavailable
                this.LogWarn($"The app received a stop signal. Returning status code 503 'Service Unavailable' to client.");
                return StatusCode(503);
            }
        }
        
        [HttpGet("Compare/{firstGuid}/{secondGuid}")]
        public async Task<ActionResult<bool>> CompareGuids(string firstGuid, string secondGuid, CancellationToken ct = default)
        {
            this.LogInfo($"CompareGuids called for GUIDs <{firstGuid}> and <{secondGuid}>.");

            if (Guid.TryParse(firstGuid, out var firstGuidToCompare) == false)
            {
                var message = $"First GUID <{firstGuid}>> is not a valid GUID.";
                this.LogWarn(message);
                return BadRequest(message);
            }
            
            if (Guid.TryParse(secondGuid, out var secondGuidToCompare) == false)
            {
                var message = $"Second GUID <{secondGuid}>> is not a valid GUID.";
                this.LogWarn(message);
                return BadRequest(message);
            }

            var guidsAreEqual = firstGuidToCompare == secondGuidToCompare;
            this.LogInfo($"GUIDs are {(guidsAreEqual ? string.Empty : "not ")}equal.");
            return guidsAreEqual;
        }
        
        [HttpGet("Validate/{guid}")]
        public async Task<ActionResult<bool>> ValidateGuid(string guid, CancellationToken ct = default)
        {
            this.LogInfo($"ValidateGuid called with argument <{guid}>.");
            var guidIsValid = Guid.TryParse(guid, out var validatedGuid);
            this.LogInfo($"Argument <{guid}> is {(guidIsValid ? string.Empty : "not ")}a valid GUID.");
            return guidIsValid;
        }

        // Creates a token that is cancelled when either the passed token 'ct' or the
        // global cancellation token source (which receives the SIGTERM signal when docker stop ist called) is cancelled
        private CancellationToken CreateLinkedToken(CancellationToken ct)
        {
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            _cts.Token.Register (() => linkedCts.Cancel());
            return linkedCts.Token;
        }
        
        private static bool CheckIfConvertToUpperCase()
        {
            try
            {
                return Convert.ToBoolean(Environment.GetEnvironmentVariable("UPPERCASEGUIDS"));
            }
            catch
            {
                return false;
            }
        }

        private async Task DelayAsync(CancellationToken ct)
        {
            var delayMs = 0;
            
            try
            {
                delayMs = Convert.ToInt32(Environment.GetEnvironmentVariable("GETNEWGUIDDELAYMS"));
            }
            catch
            {
                this.LogWarn("Failed to read environment variable GETNEWGUIDDELAYMS.");
                return;
            }

            this.LogInfo($"Delay GetNewGuid response for <{delayMs}> milliseconds.");
            await Task.Delay(delayMs, ct);
        }

        private void LogInfo(string msg)
        {
            _logger.LogInformation($"{DateTime.UtcNow}: {msg}");
        }
        
        private void LogWarn(string msg)
        {
            _logger.LogWarning($"{DateTime.UtcNow}: {msg}");
        }
    }
}