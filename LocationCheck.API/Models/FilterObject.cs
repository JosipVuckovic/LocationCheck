using LocationCheck.External.GoogleMapsPlatform;

namespace LocationCheck.API.Models;

public class FilterObject
{
    public PlaceTypeEnum? PlaceTypeFilter { get; set; }
    public string? SearchFilter { get; set; }
}