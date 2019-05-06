using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Vue.Utility;

namespace Vue.Domain.Cards
{
    public abstract class Card : Character
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public abstract Row Row { get; }
        public abstract Row Targets { get; }
        [JsonConverter(typeof(StringEnumConverter))]
        public abstract Rarity Rarity { get; }
        public abstract int Speed { get; }
        public bool CanHaveDuplicates => Rarity != Rarity.Legendary &&
            Rarity != Rarity.Epic &&
            Rarity != Rarity.Mythic &&
            Rarity != Rarity.Godly;
        
        public List<Card> TargetedCards(List<Card> enemyCards)
        {
            enemyCards = enemyCards.Where(x => !x.IsDead).ToList();
            if (Targets == Row.Both) return enemyCards;
            var cardsToAttack = enemyCards.Where(x => x.Row == Targets).ToList();
            if (cardsToAttack.Any()) return cardsToAttack;
            var correctCards =  enemyCards.Where(x => x.Row != Targets).ToList();
            return correctCards.Shuffle();
        }
    }

    public enum Row 
    {
        Front,
        Back,
        Both
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic,
        Godly
    }
}