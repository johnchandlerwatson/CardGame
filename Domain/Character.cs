using System;
using System.Collections.Generic;
using Vue.Domain.Cards;
using Vue.Domain.Champions;

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
        public int RoundsPlayed { get; set; } = 0;
        public bool IsDead => Health <= 0;

        public abstract void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, Champion enemyChampion, List<GameAction> actions);

        public Character Attack(Character character)
        {
            if (character == null) return null;
            character.Health = character.Health - Damage;
            return character;
        }

        public List<Character> Attack(List<Card> characters)
        {
            var attackedCards = new List<Character>();
            if (characters == null) return attackedCards;
            foreach (var cardToAttack in characters)
            {
                attackedCards.Add(Attack(cardToAttack));
            }
            return attackedCards;
        }

        public Character Heal(Character character)
        {
            var healthAfter = character.Health + Healing;
            healthAfter = healthAfter > character.MaxHealth ? character.MaxHealth : healthAfter;
            character.Health = healthAfter;
            return character;
        }
    }
}