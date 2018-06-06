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

        public override async Task OnConnectedAsync()
        {
            var messages = await _repo.GetChatHistory(); 
            await Clients.Caller.SendAsync("ChatHistory", messages);
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            try
            {
                var msgToSave = new ChatMessage
                {
                    User = user,
                    Message = message,
                    Sent = DateTime.Now
                };

                _repo.Add(msgToSave);           
                await _repo.SaveAllAsync();     

                await Clients.All.SendAsync("ReceiveMessage", msgToSave);
            }
            catch { }
        }

        public async Task GetHistory()
        {
            var messages = await _repo.GetChatHistory();
            await Clients.Caller.SendAsync("ChatHistory", messages);
        }
    }
}