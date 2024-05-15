namespace LocationCheck.Data.Entities
{
    public class ApiUserEntity
    {     
        public required string Username { get; set; }
        public Guid ApiKey { get; set; }
        public bool Active { get; set; }
    }
}
