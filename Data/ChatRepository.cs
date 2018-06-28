using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.API.Data
{
    public class ChatRepository : IChatRepository
    {
        private readonly DataContext _context;
        public ChatRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public async Task<List<User>> AddOnlineUser(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return await _context.Users.ToListAsync();
        }
        public async Task<List<User>> RemoveOnlineUser(string connectionId)
        {
            var userToRemove = await _context.Users
                .Where(u => u.ConnectionId == connectionId)
                .FirstOrDefaultAsync();

            _context.Remove(userToRemove);
            await _context.SaveChangesAsync();
            return await _context.Users.ToListAsync();            
        }

        public void Delete<T>(T entity) where T : class
        {
           _context.Remove(entity);
        }

        public async Task<List<ChatMessage>> GetChatHistory(string roomName)
        {
            var messages = await _context.ChatMessages
                .Where(m => m.Room == roomName)
                .OrderByDescending(m => m.Sent)
                .Take(50).ToListAsync();
            return messages.OrderBy(m => m.Sent).ToList();
        }


        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void ClearOnlineUsers()
        {
            var users = _context.Users.ToList();
            _context.Users.RemoveRange(users);
            _context.SaveChanges();
        }
    }
}