using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;

namespace LocationCheck.RequestNotifyConsole
{
    public class SignalRConnection
    {
        public async Task Start()
        {
            var url = "http://localhost:5100/requestlogs";

            var connection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();          

            // receive a message from the hub
            connection.On<string>("broadcastLog", OnReceiveMessage);           

            await connection.StartAsync();
        }

        private void OnReceiveMessage(string message)
        {
            Console.WriteLine($"{message}");
        }

    }
}
