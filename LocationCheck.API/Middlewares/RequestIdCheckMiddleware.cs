﻿namespace LocationCheck.API.Middlewares;

public class RequestIdCheckMiddleware
{
    private readonly RequestDelegate _next;

    public RequestIdCheckMiddleware(RequestDelegate next)
    {
        _next = next;
    }


    public async Task Invoke(HttpContext context)
    {
        var requestId = context.Request.Headers["RequestId"];
        
        if (string.IsNullOrEmpty(requestId))
        {
            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Missing RequestsId Header");
            return;
        }

        await _next(context);
    }
}