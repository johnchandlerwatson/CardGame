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
        public List<GameAction> Actions { get; set; } = new List<GameAction>();
        public string MoveSummary => string.Join(Environment.NewLine, Actions.Select(x => x.ActionDescription()));
    }
}