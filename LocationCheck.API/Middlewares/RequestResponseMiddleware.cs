using System.Text;
using LocationCheck.API.Hubs;
using LocationCheck.Data;
using LocationCheck.Data.Entities;
using LocationCheck.Data.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace LocationCheck.API.Middlewares
{
    public class RequestResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, LocationCheckDb db, IHubContext<RequestNotifyHub> notifyHub)
        {
            //If we are here, we have the header, that's why warning is suppressed
            var reqId = Guid.Parse(context.Request.Headers["RequestId"]!); 
            var requestLog = db.RequestResponseLogs
                .FirstOrDefault(x => x.RequestId == reqId);           
            

            if (requestLog?.Response is not null)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(requestLog.Response.Body);
                await context.Response.CompleteAsync();
                return;
            }
        
            requestLog = new RequestResponseLogEntity
            {
                RequestId = reqId,
                TimeStamp = DateTimeOffset.UtcNow,
                Request = await ReadRequestToRequestLog(context),
                Response = null,
                ApiUserEntityId = ((ApiUserIdentity)context.User.Identity!).UserId
            };

            db.RequestResponseLogs.Add(requestLog);
            await notifyHub.Clients.All.SendAsync("broadcastLog", JsonConvert.SerializeObject(requestLog));

            var originalBodyStream = context.Response.Body;
            
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);
                
                if (context.Response.StatusCode is 200)
                {
                    requestLog.Response = await ReadResponseToResponseLog(context.Response);
                }
                
                await db.SaveChangesAsync();
               
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        
        private async Task<RequestLog> ReadRequestToRequestLog(HttpContext context)
        {
            context.Request.EnableBuffering();
            string requestBody = await new StreamReader(context.Request.Body, Encoding.UTF8).ReadToEndAsync();
            context.Request.Body.Position = 0;

            return new RequestLog
            {
                TimeStamp = DateTimeOffset.UtcNow,
                Headers = context.Request.Headers.ToDictionary(a => a.Key, a => a.Value.ToString()),
                Method = context.Request.Method,
                Host = context.Request.Host.ToString(),
                Path = context.Request.Path,
                QueryParams = context.Request.QueryString.ToString(),
                Body = requestBody
            };
        }
        
        private async Task<ResponseLog> ReadResponseToResponseLog(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            
            var bodyAsString = await new StreamReader(response.Body).ReadToEndAsync();
            
            response.Body.Seek(0, SeekOrigin.Begin);
            
            return new ResponseLog
            {
                TimeStamp = DateTimeOffset.UtcNow,
                Body = bodyAsString
            };
        }
    }
}