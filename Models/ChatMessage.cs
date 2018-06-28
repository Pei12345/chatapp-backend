using System;

namespace ChatApp.API.Models
{
    public class ChatMessage
    {
        public int Id {get; set; }
        public DateTime Sent {get; set; }
        public string User {get; set; }
        public string Message { get; set; }
        public string Room { get; set; }
    }
}
