using LocationCheck.Data;
using LocationCheck.External.GoogleMapsPlatform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocationCheck.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {      

        private readonly ILogger<LocationController> _logger;
        private readonly IGoogleMapsPlatformClient _googleMapsPlatformClient;
        private readonly LocationCheckDb _locationCheckDb;

        public LocationController(ILogger<LocationController> logger, IGoogleMapsPlatformClient googleMapsPlatformClient, LocationCheckDb locationCheckDb)
        {
            _logger = logger;
            _googleMapsPlatformClient = googleMapsPlatformClient;
            _locationCheckDb = locationCheckDb;
        }

        
        [HttpGet("check")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var latlng = "40.714224,-73.961452";

            var test = await _googleMapsPlatformClient.GeocodeAsync(null, null, null, latlng, null, null, null, null, null, cancellationToken);

            return Ok(test);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var test = await _locationCheckDb.ApiUsers.ToListAsync(cancellationToken);

            return Ok(test);
        }
    }
}
