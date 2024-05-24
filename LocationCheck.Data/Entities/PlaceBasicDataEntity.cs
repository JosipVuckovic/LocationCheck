namespace LocationCheck.Data.Entities;

public class PlaceBasicDataEntity
{
    public int Id { get; set; }
    public required string ExternalIdentifier { get; set; }
    public string PlaceName { get; set; }
    public string PlaceType { get; set; }
    
}