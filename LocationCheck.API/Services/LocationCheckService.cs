using CSharpFunctionalExtensions;
using LocationCheck.API.Models;
using LocationCheck.Data;
using LocationCheck.External.GoogleMapsPlatform;

namespace LocationCheck.API.Services;

public interface ILocationCheckService
{
    Task<Result<PlacesNearbySearchResponse ,ErrorModel>> CheckLocationAsync(LocationCheckRequest request, CancellationToken cancellationToken);
    Result<object, object> SetLocationAsFavoriteAsync();
    Result<object, object> ReturnAllLocationsAsync();
}

public class LocationCheckService : ILocationCheckService
{
    private readonly IGoogleMapsPlatformClient _googleMapsPlatformClient;
    private readonly LocationCheckDb _db;
    
    public LocationCheckService(IGoogleMapsPlatformClient googleMapsPlatformClient, LocationCheckDb db)
    {
        _googleMapsPlatformClient = googleMapsPlatformClient;
        _db = db;
    }
    
    
    public async Task<Result<PlacesNearbySearchResponse ,ErrorModel>> CheckLocationAsync(LocationCheckRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _googleMapsPlatformClient.NearbySearchAsync(request.FilterObject?.SearchFilter, request.GetLocationCoords, null, null,
                null, null, null, null, request.InRadius, request.FilterObject?.PlaceTypeFilter.ToString(), null ,cancellationToken);

            if (result.Status is not PlacesSearchStatus.OK &&
                result.Status is not PlacesSearchStatus.ZERO_RESULTS)
            {
                return new ErrorModel
                {
                    Status = result.Status.ToString(),
                    ErrorMessage = result.Error_message 
                };
            }
            
            return result;
        }
        catch (ApiException ex)
        {
            return new ErrorModel
            {
                Status = ex.StatusCode.ToString(),
                ErrorMessage = ex.Response 
            };
        }
        catch (Exception e)
        {
            return new ErrorModel
            {
                Status = 500.ToString(),
                ErrorMessage = "Our api ran into problems, we are sorry"
            };
        }
        
    }

    public Result<object, object> ReturnAllLocationsAsync()
    {
        throw new NotImplementedException();
    }

    public Result<object, object> SetLocationAsFavoriteAsync()
    {
        throw new NotImplementedException();
    }
}