namespace LocationCheck.Data.Entities
{
    public class RequestResponseLogEntity
    {
        public Guid RequestId { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        
        public int ApiUserEntityId { get; set; }
        public ApiUserEntity ApiUserEntity { get; set; }
    }
}
