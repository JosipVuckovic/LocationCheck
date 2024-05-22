using LocationCheck.Data.Models;

namespace LocationCheck.Data.Entities
{
    public class RequestResponseLogEntity
    {
        public Guid RequestId { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public required RequestLog Request { get; set; }
        public ResponseLog? Response { get; set; }
        
        public int ApiUserEntityId { get; set; }
        public ApiUserEntity ApiUserEntity { get; set; }
    }
}
