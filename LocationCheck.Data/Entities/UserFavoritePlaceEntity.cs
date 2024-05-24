namespace LocationCheck.Data.Entities;

public class UserFavoritePlaceEntity
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public virtual ApiUserEntity User { get; set; }
    public int PlaceId { get; set; }
    public virtual PlaceBasicDataEntity Place { get; set; }
}