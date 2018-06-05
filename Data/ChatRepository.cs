using System.Collections.Generic;
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

        public void Delete<T>(T entity) where T : class
        {
           _context.Remove(entity);
        }

        public async Task<List<ChatMessage>> GetChatHistory()
        {
            // return await _context.ChatMessages.ToListAsync();
            var messages = await _context.ChatMessages.OrderByDescending(m => m.Sent).Take(50).ToListAsync();
            return messages.OrderBy(m => m.Sent).ToList();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}