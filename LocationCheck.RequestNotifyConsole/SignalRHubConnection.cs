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
        private readonly string _hubUrl;

        public SignalRConnection(string hubUrl)
        {
            _hubUrl = hubUrl;
        }

        public async Task Start()
        {           

            var connection = new HubConnectionBuilder()
                .WithUrl(_hubUrl)
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
