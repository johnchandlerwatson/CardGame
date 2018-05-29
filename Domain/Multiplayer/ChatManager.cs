using System;
using System.Collections.Generic;
using System.Linq;

namespace Vue.Domain.Multiplayer
{
    public static class ChatManager
    {
        private static List<ChatMessage> _messages { get; set; } = new List<ChatMessage>();

        public static void AddMessage(ChatMessage message)
        {
            _messages.Add(message);
        }

        public static List<ChatMessage> GetMessages()
        {
            DeleteOldMessages();
            return _messages.OrderByDescending(x => x.DateSent).ToList();        
        }

        private static void DeleteOldMessages()
        {
            _messages.RemoveAll(x => x.DateSent < DateTime.UtcNow.AddDays(-2));
        }
    }

    public class ChatMessage
    {
        public ChatMessage(string message)
        {
            Message = message;
            DateSent = DateTime.UtcNow;
        }
        public string Message { get; set; }
        public DateTime DateSent { get; set; }
    }
}