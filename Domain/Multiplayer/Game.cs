using System;

namespace Vue.Domain.Multiplayer
{
    public class Game
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public User User1 { get; set; }
        public User User2 { get; set; }
        public bool Full => User1 != null && User2 != null;
        public DateTime CreateTime { get; } = DateTime.Now;
        public bool IsBotGame => User1.Username == "bot" || User2?.Username == "bot";
        public int Turns { get; private set; }
        public bool User1HasPlayedThisTurn { get; set; }
        public bool User2HasPlayedThisTurn { get; set; }
        public bool TurnIsComplete => User1HasPlayedThisTurn && User2HasPlayedThisTurn;
        public void EndTheTurn()
        {
            User1HasPlayedThisTurn = false;
            User2HasPlayedThisTurn = false;
            Turns++;
        }
    }
}