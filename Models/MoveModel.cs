using System;
using System.Collections.Generic;
using Vue.Domain;
using Vue.Domain.Cards;
using System.Linq;

namespace Vue.Models
{
    public class MoveModel
    {
        public User User { get; set; }
        public User Enemy { get; set; }
        public List<GameAction> Actions { get; set; } = new List<GameAction>();
        public string MoveSummary => string.Join(Environment.NewLine, Actions.Select(x => x.ActionDescription()));
        public EndOfTurnModel EndOfTurnModel => new EndOfTurnModel 
        {
            User = User,
            Enemy = Enemy,
        };
    }
}