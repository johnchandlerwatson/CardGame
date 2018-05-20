using System;
using System.Collections.Generic;

namespace Vue.Domain.Multiplayer
{
    public class Game
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ConnectionId { get; protected set; }
        public string Connection2Id { get; protected set; }
        public UserPair UserPair { get; set; }
        public List<GameAction> Actions { get; set; } = new List<GameAction>();
        public bool Full => User1 != null && User2 != null;
        public DateTime CreateTime { get; } = DateTime.UtcNow;    
        public int Turns { get; private set; }
        public DateTime LastTurnTime { get; private set; } = DateTime.UtcNow;
        public double SecondsSinceLastTurn => (DateTime.UtcNow - LastTurnTime).TotalSeconds;
        public bool User1HasPlayedThisTurn { get; set; }
        public bool User2HasPlayedThisTurn { get; set; }
        public bool TurnIsComplete => User1HasPlayedThisTurn && User2HasPlayedThisTurn;
        public User User1 => UserPair.User1;
        public User User2 => UserPair.User2;
        public bool IsOver => (User1?.Champion?.IsDead ?? false) || (User2?.Champion?.IsDead ?? false);
        
        public void EndTheTurn()
        {
            User1HasPlayedThisTurn = false;
            User2HasPlayedThisTurn = false;
            Turns++;
            LastTurnTime = DateTime.UtcNow;
        }
        
        public void ResetHandCards()
        {
            UserPair.User1.ResetHandCards();
            UserPair.User2.ResetHandCards();
        }

        public void SetConnectionId(string connectionId)
        {
            if (ConnectionId == null)
            {
                ConnectionId = connectionId;
            }
            else 
            {
                Connection2Id = connectionId;
            }
        }
    }
}