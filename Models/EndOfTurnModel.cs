using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Vue.Domain;
using Vue.Domain.Cards;

namespace Vue.Models
{
    public class EndOfTurnModel
    {
        public string Selection { get; set; }
        public User Enemy { get; set; }
        public User User { get; set; }
    }
}