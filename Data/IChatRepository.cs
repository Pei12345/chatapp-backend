using System.Collections.Generic;
using System.Threading.Tasks;
using ChatApp.API.Models;

namespace ChatApp.API.Data
{
    public interface IChatRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void ClearOnlineUsers();   
        Task<List<ChatMessage>> GetChatHistory();
        Task<List<User>> AddOnlineUser(User user);
        Task<List<User>> RemoveOnlineUser(string connnectionId);     
        Task<bool> SaveAllAsync();
    }
}