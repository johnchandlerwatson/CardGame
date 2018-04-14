using System;
using System.Collections.Generic;
using Vue.Domain.Cards;

namespace Vue.Domain
{
    public abstract class Character
    {
        public Character()
        {
            Health = MaxHealth;
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public BasicUser User { get; set; }
        public abstract string Name { get; }
        public abstract int MaxHealth { get; }
        public int Health { get; set; }
        public abstract int Damage { get; set; }
        public abstract int Healing { get; }
        public abstract string Description { get; }

        public abstract void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, List<GameAction> actions);
    }
}