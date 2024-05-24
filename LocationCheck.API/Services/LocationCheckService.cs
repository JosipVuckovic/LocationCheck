using CSharpFunctionalExtensions;
using LocationCheck.API.Helpers;
using LocationCheck.API.Models;
using LocationCheck.Data;
using LocationCheck.Data.Entities;
using LocationCheck.External.GoogleMapsPlatform;
using Microsoft.EntityFrameworkCore;

namespace LocationCheck.API.Services;

public interface ILocationCheckService
{
    Task<Result<Place[], ErrorModel>> CheckLocationAsync(LocationCheckRequest request,
        CancellationToken cancellationToken);
    Task<Result<BasicPlacesDTO[], ErrorModel>> ReturnAllLocationsAsync(FilterObject? filterObject,
        CancellationToken cancellationToken);
    
    Task<Result<bool, ErrorModel>> SetLocationAsFavoriteAsync(string externalIdentifier, int userId, CancellationToken cancellationToken);
    Task<Result<BasicPlacesDTO[], ErrorModel>>  CheckMyFavoritesAsync(FilterObject? filterObject, int userId, CancellationToken cancellationToken);

    Task<Result<Place, ErrorModel>> PlaceDetailsAsync(string externalIdentifier, CancellationToken cancellationToken);
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


    public async Task<Result<Place[], ErrorModel>> CheckLocationAsync(LocationCheckRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _googleMapsPlatformClient.NearbySearchAsync(request.FilterObject?.SearchFilter,
                request.GetLocationCoords, null, null,
                null, null, null, null, request.InRadius, request.FilterObject?.PlaceTypeFilter.ToString() ?? "", null,
                cancellationToken);

            if (result.Status is not PlacesSearchStatus.OK &&
                result.Status is not PlacesSearchStatus.ZERO_RESULTS)
            {
                return new ErrorModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = result.Status.ToString(),
                    ErrorMessage = result.Error_message
                };
            }

            //This should be offloaded to background task, but no time left
            await AddNewPlacesToDb(result.Results, cancellationToken);
            return result.Results.ToArray();
        }
        catch (ApiException ex)
        {
            return new ErrorModel
            {
                StatusCode = ex.StatusCode,
                Status = ex.Response,
                ErrorMessage = ex.Message
            };
        }
        catch (Exception e)
        {
            return new ErrorModel
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Status = "Seems there where problems :(",
                ErrorMessage = e.Message
            };
        }
    }

    //Would be better if some extension method would be used
    public async Task<Result<BasicPlacesDTO[], ErrorModel>> ReturnAllLocationsAsync(FilterObject? filterObject,
        CancellationToken cancellationToken)
    {
        try
        {
            var placesQuerye = _db.PlaceBasicData.AsQueryable();

            if (filterObject is null)
            {
                return await placesQuerye.Select(_ => _.MapToDto()).ToArrayAsync(cancellationToken);
            }
            
            //Extension would be nice, but, time
            if (filterObject.PlaceTypeFilter is not null)
            {
                placesQuerye = placesQuerye.Where(w => w.PlaceType.Contains(filterObject.PlaceTypeFilter.ToString()!));
            }

            if (filterObject.SearchFilter is not null)
            {
                placesQuerye = placesQuerye.Where(w => w.PlaceName.Contains(filterObject.SearchFilter!));
            }

            return await placesQuerye.Select(_ => _.MapToDto()).ToArrayAsync(cancellationToken);
        }
        catch (Exception e)
        {
            return new ErrorModel
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Status = "Seems there where problems :(",
                ErrorMessage = e.Message
            };
        }
    }

    public async Task<Result<bool, ErrorModel>> SetLocationAsFavoriteAsync(string externalIdentifier, int userId, CancellationToken cancellationToken)
    {
        try
        {
            //Would be better if cached
            var place = await _db.PlaceBasicData.Where(_ => _.ExternalIdentifier == externalIdentifier)
                .FirstOrDefaultAsync(cancellationToken);

            if (place is null)
            {
                return new ErrorModel
                {
                    StatusCode = StatusCodes.Status204NoContent,
                    Status = "Place missing",
                    ErrorMessage = "Seems we can't find the place"
                };
            }

            var alreadyFav = await _db.FavoritePlaces
                .Where(_ => _.PlaceId == place.Id && _.UserId == userId).FirstOrDefaultAsync(cancellationToken);
        
            if (alreadyFav is not null)
            {
                return new ErrorModel
                {
                    StatusCode = StatusCodes.Status302Found,
                    Status = "Already fav!",
                    ErrorMessage = "Seems you already liked the place"
                };
            }

            await _db.FavoritePlaces.AddAsync(new UserFavoritePlaceEntity
            {
                UserId = userId,
                PlaceId = place.Id,
            }, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            return new ErrorModel
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Status = "Seems there where problems :(",
                ErrorMessage = e.Message
            };
        }
        
    }

    public async Task<Result<BasicPlacesDTO[], ErrorModel>> CheckMyFavoritesAsync(FilterObject? filterObject, int userId, CancellationToken cancellationToken)
    {
        try
        {
            var favQuerye = _db.FavoritePlaces.AsQueryable();

            if (filterObject is null)
            {
                //Would be better if there would be a filter on
                return await favQuerye.Where(_ => _.UserId == userId).Include(i => i.Place).Select(_ => _.Place.MapToDto()).ToArrayAsync(cancellationToken);
            }
            
            //Extension would be nice, but, time
            if (filterObject.PlaceTypeFilter is not null)
            {
                favQuerye = favQuerye.Where(w => w.Place.PlaceType.Contains(filterObject.PlaceTypeFilter.ToString()!));
            }

            if (filterObject.SearchFilter is not null)
            {
                favQuerye = favQuerye.Where(w => w.Place.PlaceName.Contains(filterObject.SearchFilter!));
            }

            return await favQuerye.Where(_ => _.UserId == userId).Include(i => i.Place).Select(_ => _.Place.MapToDto()).ToArrayAsync(cancellationToken);
        }
        catch (Exception e)
        {
            return new ErrorModel
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Status = "Seems there where problems :(",
            };
        }
    }

    public async Task<Result<Place, ErrorModel>> PlaceDetailsAsync(string externalIdentifier, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _googleMapsPlatformClient.PlaceDetailsAsync(externalIdentifier, null, null, null, null, null,
                null, cancellationToken);

            if (result.Status is not PlacesDetailsStatus.OK)
            {
                return new ErrorModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = result.Status.ToString(),
                    ErrorMessage = String.Join(';', result.Info_messages)
                };
            }
            
            return result.Result;
        }
        catch (ApiException ex)
        {
            return new ErrorModel
            {
                StatusCode = ex.StatusCode,
                Status = ex.Response,
                ErrorMessage = ex.Message
            };
        }
        catch (Exception e)
        {
            return new ErrorModel
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Status = "Seems there where problems :(",
                ErrorMessage = e.Message
            };
        }
    }

    private async Task AddNewPlacesToDb(IEnumerable<Place> places, CancellationToken cancellationToken)
    {
            //Would be nice to have a cache of all existing external idents so we wouldn't need to query db
            var existingPlacesIds =
                await _db.PlaceBasicData.Select(_ => _.ExternalIdentifier).ToArrayAsync(cancellationToken);

            var newPlaces = places.Where(p => !existingPlacesIds.Contains(p.Place_id)).Select(s =>
                new PlaceBasicDataEntity
                {
                    ExternalIdentifier = s.Place_id,
                    PlaceName = s.Name,
                    PlaceType = String.Join("|", s.Types)
                });

            await _db.PlaceBasicData.AddRangeAsync(newPlaces, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
    }
}