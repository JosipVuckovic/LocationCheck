using LocationCheck.External.GoogleMapsPlatform;
using Microsoft.AspNetCore.Mvc;

namespace LocationCheck.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {      

        private readonly ILogger<LocationController> _logger;
        private readonly IGoogleMapsPlatformClient _googleMapsPlatformClient;

        public LocationController(ILogger<LocationController> logger, IGoogleMapsPlatformClient googleMapsPlatformClient)
        {
            _logger = logger;
            _googleMapsPlatformClient = googleMapsPlatformClient;
        }

        [HttpGet(Name = "GetLocation")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var latlng = "40.714224,-73.961452";

            var test = await _googleMapsPlatformClient.GeocodeAsync(null, null, null, latlng, null, null, null, null, null, cancellationToken);

            return Ok(test);
        }
    }
}
