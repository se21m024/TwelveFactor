using System;
using Microsoft.Extensions.Logging;

namespace TwelveFactor.Controllers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using TwelveFactor.Models.Api;
    using Microsoft.AspNetCore.Mvc;
    
    [ApiController]
    [Route("[controller]")]
    public class GuidController : ControllerBase
    {
        private readonly ILogger<GuidController> _logger;

        public GuidController(ILogger<GuidController> logger)
        {
            _logger = logger;
        }

        [HttpGet("New")]
        public async Task<ActionResult<Guid>> GetNewGuid(CancellationToken ct = default)
        {
            _logger.LogInformation("GetNewGuid called.");
            var guid = Guid.NewGuid();
            _logger.LogInformation($"Returning new GUID <{guid}>.");
            return guid;
        }
        
        [HttpGet("Compare/{firstGuid}/{secondGuid}")]
        public async Task<ActionResult<bool>> CompareGuids(string firstGuid, string secondGuid, CancellationToken ct = default)
        {
            _logger.LogInformation($"CompareGuids called for GUIDs <{firstGuid}> and <{secondGuid}>.");

            if (Guid.TryParse(firstGuid, out var firstGuidToCompare) == false)
            {
                var message = $"First GUID <{firstGuid}>> is not a valid GUID.";
                _logger.LogWarning(message);
                return BadRequest(message);
            }
            
            if (Guid.TryParse(secondGuid, out var secondGuidToCompare) == false)
            {
                var message = $"Second GUID <{secondGuid}>> is not a valid GUID.";
                _logger.LogWarning(message);
                return BadRequest(message);
            }

            var guidsAreEqual = firstGuidToCompare == secondGuidToCompare;
            _logger.LogInformation($"GUIDs are {(guidsAreEqual ? string.Empty : "not ")}equal.");
            return guidsAreEqual;
        }
        
        [HttpGet("Validate/{guid}")]
        public async Task<ActionResult<bool>> ValidateGuid(string guid, CancellationToken ct = default)
        {
            _logger.LogInformation($"ValidateGuid called with argument <{guid}>.");
            var guidIsValid = Guid.TryParse(guid, out var validatedGuid);
            _logger.LogInformation($"Argument <{guid}> is {(guidIsValid ? string.Empty : "not ")}a valid GUID.");
            return guidIsValid;
        }
    }
}