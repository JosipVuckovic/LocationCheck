using CSharpFunctionalExtensions;
using LocationCheck.API.Models;
using LocationCheck.API.Services;
using LocationCheck.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        /// <br/> Zagreb data: "latitude": 45.815399, "longitude": 15.966568
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("check")]        
        [Produces("application/json")]
        [Consumes("application/json")]        
        public async Task<IActionResult> CheckLocationNearCoordsAsync([FromBody]LocationCheckRequest request ,CancellationToken cancellationToken)
        {
            var result = await _locationCheckService.CheckLocationAsync(request, new CancellationToken());
            return result.IsFailure ? StatusCode(result.Error.StatusCode, result.Error.ErrorMessage) : Ok(result);
        }
        
        [HttpGet("known-places")]        
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> ShowBasicPlacesDataAsync([FromQuery]FilterObject filter ,CancellationToken cancellationToken)
        {
            var result = await _locationCheckService.ReturnAllLocationsAsync(filter, cancellationToken);
            
            return result.IsFailure ? StatusCode(result.Error.StatusCode, result.Error.ErrorMessage) : Ok(result);
        }
        
        [HttpGet("place-details")]        
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PlaceDetailsAsync([FromQuery]string placeExternalIdentifier ,CancellationToken cancellationToken)
        {
            if (placeExternalIdentifier.IsNullOrEmpty())
            {
                return BadRequest("Place ExternalIdentifier can't be empty");
            }

            var result = await _locationCheckService.PlaceDetailsAsync(placeExternalIdentifier, cancellationToken);
            
            return result.IsFailure ? StatusCode(result.Error.StatusCode, result.Error.ErrorMessage) : Ok(result);
        }
        
        [HttpPost("mark-fav")]        
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> MarkPlaceAsFavorite([FromBody]MarkPlaceAsFavoriteRequest  request ,CancellationToken cancellationToken)
        {
            if (request.ExternalIdentifier.IsNullOrEmpty())
            {
                return BadRequest("Place ExternalIdentifier can't be empty");
            }

            request.UserId = ((ApiUserIdentity)User.Identity!).UserId;

            var result = await _locationCheckService.SetLocationAsFavoriteAsync(request.ExternalIdentifier, request.UserId,
                cancellationToken);

            if (result.IsFailure)
            {
                return StatusCode(result.Error.StatusCode, result.Error.ErrorMessage);
            }

            return Created();
        }
        
        [HttpGet("my-favs")]        
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> MyFavoritePlacesAsync([FromQuery]FilterObject filter  ,CancellationToken cancellationToken)
        {
            var userId = ((ApiUserIdentity)User.Identity!).UserId;
            var result = await _locationCheckService.CheckMyFavoritesAsync(filter, userId,cancellationToken);
            
            return result.IsFailure ? StatusCode(result.Error.StatusCode, result.Error.ErrorMessage) : Ok(result);
        }
        
    }
}
