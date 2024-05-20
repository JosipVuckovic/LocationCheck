using LocationCheck.Data;

namespace LocationCheck.API.Middlewares
{
    public class ResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context, LocationCheckDb db)
        {
            
            
            
            await _next(context);
            
            
        }

      
    }
}