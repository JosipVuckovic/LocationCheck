using LocationCheck.API.Middlewares;
using LocationCheck.External;
using LocationCheck.Data;
using LocationCheck.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLocationCheckExternal(builder.Configuration);
builder.Services.AddLocationCheckData(builder.Configuration);
builder.Services.AddLocationCheckSecurity(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<RequestIdCheckMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ResponseLoggingMiddleware>();

app.Run();
