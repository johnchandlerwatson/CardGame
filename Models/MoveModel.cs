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