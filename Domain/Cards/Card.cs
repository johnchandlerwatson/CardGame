using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Vue.Domain.Cards
{
    public abstract class Card
    {
        public Card()
        {
            Health = MaxHealth;
        }
        [JsonIgnore]
        public User User { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public abstract string Name { get; }
        public abstract Row Row { get; }
        public abstract Row Targets { get; }
        [JsonConverter(typeof(StringEnumConverter))]
        public abstract Rarity Rarity { get; }
        public abstract int Speed { get; }
        public abstract int Damage { get; }
        public abstract int Healing { get; }
        public abstract string Description { get; }
        public abstract int MaxHealth { get; }
        public int Health { get; set; }
        public int RoundsPlayed { get; set; } = 0;
        public bool IsDead => Health <= 0;

        public abstract void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, List<CardAction> actions);

        
        public List<Card> TargetedCards(List<Card> enemyCards)
        {
            if (Targets == Row.Both) return enemyCards;
            var cardsToAttack = enemyCards.Where(x => x.Row == Targets).ToList();
            if (cardsToAttack.Any()) return cardsToAttack;
            return enemyCards.Where(x => x.Row != Targets).ToList();
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