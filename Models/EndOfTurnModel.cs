using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Vue.Domain.Cards;

namespace Vue.Models
{
    public class EndOfTurnModel
    {
        public string Selection { get; set; }
        public List<Card> EnemyPlayedCards { get; set; }
        public List<Card> UserPlayedCards { get; set; }
    }
}