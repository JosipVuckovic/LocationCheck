// See https://aka.ms/new-console-template for more information

using LocationCheck.RequestNotifyConsole;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

#region JsonAndConfig


var configuration = new ConfigurationBuilder()
     .SetBasePath(AppContext.BaseDirectory)     
     .AddJsonFile("appsettings.json")
     .AddEnvironmentVariables()     
     .Build();

var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(configuration)
    .BuildServiceProvider();

var configurationInstance = serviceProvider.GetService<IConfiguration>();

#endregion

var hubUrl = configurationInstance?.GetRequiredSection("HubUrl").Value;

if (hubUrl == null)
{
    Console.WriteLine("Missing Hub url in appsettings.json");
    throw new MissingFieldException("Missing hub url");
}

Console.WriteLine("Connecting to hub");


var signalRConnection = new SignalRConnection(hubUrl);
await signalRConnection.Start();

Console.WriteLine("Connected");

Console.Read();


