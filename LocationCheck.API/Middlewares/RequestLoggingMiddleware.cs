using LocationCheck.Data;
using LocationCheck.Data.Entities;
using Newtonsoft.Json;

namespace LocationCheck.API.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context, LocationCheckDb db)
        {
            db.RequestResponseLogs.Add(new RequestResponseLogEntity
            {
                RequestId = Guid.Parse(context.Request.Headers["RequestId"]!), //If we are here, we have the header, that's why warning is suppressed
                Request = JsonConvert.SerializeObject(context.Request),
                ApiUserEntityId = Convert.ToInt32(context.User.Identity?.Name)
            });
            
            //TODO: After signal is implemented, notify subs
            
            await db.SaveChangesAsync();
            await _next(context);
        }

        
    }
}