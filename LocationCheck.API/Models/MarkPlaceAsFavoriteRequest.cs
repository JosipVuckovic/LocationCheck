namespace LocationCheck.API.Models;

public class MarkPlaceAsFavoriteRequest
{
    public string ExternalIdentifier { get; set; }
    internal int UserId { get; set; }
}