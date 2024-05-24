using LocationCheck.API.Models;
using LocationCheck.Data.Entities;

namespace LocationCheck.API.Helpers;

public static class Mappers
{
    public static BasicPlacesDTO MapToDto(this PlaceBasicDataEntity entity)
    {
        return new BasicPlacesDTO
        {
            ExternalIdentifier = entity.ExternalIdentifier,
            PlaceName = entity.PlaceName,
            PlaceTypes = entity.PlaceType.Split('|')
        };
    }
}