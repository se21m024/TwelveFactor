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
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<UserResponse>> GetUsers(CancellationToken ct = default)
        {
            _logger.LogInformation("GetUsers called.");

            try
            {
                // Throw random error
                if (new Random().Next(1, 2) % 2 == 0)
                {
                    throw new Exception("Service crashed.");
                }
                
                return new List<UserResponse> {new UserResponse(3, "Tom"),};
            }
            catch (Exception e)
            {
                _logger.LogError($"Unexpected error occurred when trying to get users: {e}");
                throw;
            }
        }
    }
}