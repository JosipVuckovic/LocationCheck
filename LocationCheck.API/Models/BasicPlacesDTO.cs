using LocationCheck.External.GoogleMapsPlatform;

namespace LocationCheck.API.Models;

public class BasicPlacesDTO
{
    public required string ExternalIdentifier { get; set; }
    public string PlaceName { get; set; }
    public ICollection<string> PlaceTypes { get; set; }
}