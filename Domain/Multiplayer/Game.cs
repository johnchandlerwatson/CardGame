using System;
using System.Collections.Generic;

namespace Vue.Domain.Multiplayer
{
    public class Game
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public UserPair UserPair { get; set; }
        public List<GameAction> Actions { get; set; } = new List<GameAction>();
        public bool Full => User1 != null && User2 != null;
        public DateTime CreateTime { get; } = DateTime.Now;
        public int Turns { get; private set; }
        public bool User1HasPlayedThisTurn { get; set; }
        public bool User2HasPlayedThisTurn { get; set; }
        public bool TurnIsComplete => User1HasPlayedThisTurn && User2HasPlayedThisTurn;
        public User User1 => UserPair.User1;
        public User User2 => UserPair.User2;
        public void EndTheTurn()
        {
            User1HasPlayedThisTurn = false;
            User2HasPlayedThisTurn = false;
            Turns++;
        }
        
        public void ResetHandCards()
        {
            UserPair.User1.ResetHandCards();
            UserPair.User2.ResetHandCards();
        }
    }
}