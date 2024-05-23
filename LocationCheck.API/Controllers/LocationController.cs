using LocationCheck.API.Models;
using LocationCheck.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LocationCheck.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {   
        private readonly ILocationCheckService _locationCheckService;

        public LocationController(ILocationCheckService locationCheckService)
        {
            _locationCheckService = locationCheckService;
        }
        
        /// <summary>
        /// Sends location check to Google to lookup places around coordinate
        /// <br/> Only lat lng params are required, the rest is nullable
        /// <br/> can be filtered by place type and keyword
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("check")]        
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> Get([FromBody]LocationCheckRequest request ,CancellationToken cancellationToken)
        {
            var result = await _locationCheckService.CheckLocationAsync(request, new CancellationToken());
            return Ok(result);
        }
        
    }
}
