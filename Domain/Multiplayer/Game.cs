using System;

namespace Vue.Domain.Multiplayer
{
    public class Game
    {
        public Game(User user1)
        {
            User1 = user1;
        }
        public Guid Id { get; } = Guid.NewGuid();
        public User User1 { get; }
        public User User2 { get; set; }
        public bool Full => User1 != null && User2 != null;  
        public int Turns { get; }
        public DateTime CreateTime { get; } = DateTime.Now;
    }
}