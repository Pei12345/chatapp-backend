using System;
using System.Collections.Generic;
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
            // add user to users online on connect
            var httpContext = Context.GetHttpContext();
            var username = httpContext.Request.Query["name"];
            var connectionId = Context.ConnectionId;
            var userToAdd = new User
            {
                ConnectionId = connectionId,
                Username = username
            };

            var roomName = httpContext.Request.Query["roomName"];
            await JoinRoom(roomName);

            var onlineUsers = await _repo.AddOnlineUser(userToAdd);
            await Clients.All.SendAsync("GetOnlineUsers", onlineUsers);

            await GetRoomChatHistory(roomName);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // remove user from users online on disconnect
            var onlineUsers = await _repo.RemoveOnlineUser(Context.ConnectionId);            
            await Clients.All.SendAsync("GetOnlineUsers", onlineUsers);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string room, string message)
        {
            try
            {
                var msgToSave = new ChatMessage
                {
                    User = user,
                    Message = message,
                    Sent = DateTime.UtcNow,
                    Room = room
                };
                _repo.Add(msgToSave);
                await _repo.SaveAllAsync();

                await Clients.Group(room).SendAsync("ReceiveMessage", msgToSave);
            }
            catch { }
        }

        public async Task ChangeRoom(string fromRoom, string toRoom)
        {
            try 
            {
                await LeaveRoom(fromRoom);
                await JoinRoom(toRoom);
                await GetRoomChatHistory(toRoom);
            }
            catch {}
        }

        public async Task GetRoomChatHistory(string roomName)
        {
            var messages = await _repo.GetChatHistory(roomName);
            await Clients.Caller.SendAsync("ChatHistory", messages);
        }

        public async Task JoinRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
    }
}
