using System;
using System.Collections.Generic;
using Vue.Domain;
using Vue.Domain.Cards;
using System.Linq;
using Vue.Domain.Multiplayer;

namespace Vue.Models
{
    public class MoveModel
    {
        public string Selection { get; set; }
        public string Username { get; set; }
        public Game Game { get; set; }
        public string MoveSummary => string.Join(Environment.NewLine, Game.Actions.Select(x => x.ActionDescription()));
        public void ValidateModel(string username)
        {
            Console.WriteLine($"Sending model with User1 of {Game.User1.Username} to {username}");
            if (Game.User1.Username != username) throw new Exception($"User1 should be {username} however it is {Game.User2.Username}");
        }

        public void ArrangeUsers(string username)
        {
            var user  = username == Game.User1.Username ? Game.User1 : Game.User2;
            var enemy = username == Game.User1.Username ? Game.User2 : Game.User1;
            SetUserPair(user, enemy);
        }

        public void SetUserPair(User user1, User user2)
        {
            Game.UserPair.User1 = user1;
            Game.UserPair.User2 = user2;
        }
    }

    public class EndOfMoveModel
    {
        public string Selection { get; set; }
        public string Username { get; set; }
        public UserPair UserPair { get; set; }
        public Guid? GameId { get; set; }
        public bool IsBotGame => GameId == null;
    }
}