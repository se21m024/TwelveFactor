using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TwelveFactor.Models.Api;

namespace TwelveFactor.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<UserResponse>> GetUsers(CancellationToken ct = default)
        {
            return new List<UserResponse> {new UserResponse {Id = 2, Name = "Tom"}};
        }
    }
}