using LocationCheck.API.Helpers.Swagger;
using LocationCheck.API.Middlewares;
using LocationCheck.External;
using LocationCheck.Data;
using LocationCheck.Security;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using LocationCheck.API.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLocationCheckExternal(builder.Configuration);
builder.Services.AddLocationCheckData(builder.Configuration);
builder.Services.AddLocationCheckSecurity(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
    {
        Description = "Basic auth added to authorization header \n Enter Basic {ApiKey}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "basic",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Basic" }
            },
            new List<string>()
        }
    });
    c.OperationFilter<SwaggerHeaderParameterFilter>();
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
        options.SerializerSettings.MaxDepth = 64;
    });

builder.Services.AddSignalR();
builder.Services.AddSingleton<RequestNotifyHub>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    await new DbInitializer(scope).InitAsync();
}

app.UseHttpsRedirection();
app.MapControllers();



app.MapHub<RequestNotifyHub>("requestlogs");

//So we leave the signalR from auth and middlewares
app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
{
    appBuilder.UseMiddleware<RequestIdCheckMiddleware>();
    appBuilder.UseMiddleware<RequestResponseMiddleware>();
    appBuilder.UseAuthorization();
});

app.Run();
