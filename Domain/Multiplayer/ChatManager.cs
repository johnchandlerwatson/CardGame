using System;
using System.Collections.Generic;
using System.Linq;

namespace Vue.Domain.Multiplayer
{
    public static class ChatManager
    {
        private static List<ChatMessage> _messages { get; set; } = new List<ChatMessage>();

        public static void AddMessage(string message)
        {
            var chatMessage = new ChatMessage(message);
            _messages.Add(chatMessage);
        }

        public static List<string> GetMessages()
        {
            DeleteOldMessages();
            return _messages.OrderByDescending(x => x.DateSent).Select(x => x.Message).ToList();
        }

        private static void DeleteOldMessages()
        {
            _messages.RemoveAll(x => x.DateSent < DateTime.Now.AddDays(-2));
        }
    }

    public class ChatMessage
    {
        public ChatMessage(string message)
        {
            Message = message;
            DateSent = DateTime.Now;
        }
        public string Message { get; set; }
        public DateTime DateSent { get; set; }
    }
}