using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ChatApp.API.Data;
using ChatApp.API.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API
{
    public class ChatHub : Hub
    {
        private readonly IChatRepository _repo;
        public ChatHub(IChatRepository repo)
        {
            _repo = repo;
        }

        public async Task SendMessage(string user, string message)
        {
            try
            {
                

                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
            catch { }
        }
    }
}