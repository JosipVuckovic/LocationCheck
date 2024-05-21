// See https://aka.ms/new-console-template for more information

using LocationCheck.RequestNotifyConsole;

var signalRConnection = new SignalRConnection();
await signalRConnection.Start();

Console.Read();


