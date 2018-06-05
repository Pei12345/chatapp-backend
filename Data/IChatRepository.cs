using System.Collections.Generic;
using System.Threading.Tasks;
using ChatApp.API.Models;

namespace ChatApp.API.Data
{
    public interface IChatRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<List<ChatMessage>> GetChatHistory();
        Task<bool> SaveAllAsync();
    }
}