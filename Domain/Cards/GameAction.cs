using System.Collections.Generic;
using Vue.Domain;
using Vue.Domain.Cards;
using System.Linq;
using System;
using Vue.Domain.Champions;

namespace Vue.Domain
{
    public class GameAction
    {
        public GameAction(Character actor, List<Card> damagedCards, List<Card> healedCards)
        {
            Target = damagedCards?.FirstOrDefault()?.User;
            ActorUser = actor.User;
            DamagedCards = damagedCards;
            HealedCards = healedCards;
            Actor = actor;
            Damage = actor.Damage;
            Healing = actor.Healing;
        }

        public GameAction(string actionOverride)
        {
            ActionOverride = actionOverride;
        }
        
        public BasicUser Target { get; }
        public BasicUser ActorUser { get; }
        public List<Card> DamagedCards { get; }
        public List<Card> HealedCards { get; }
        public Character Actor { get; }
        public int Damage { get; }
        public int Healing { get; }
        public string ActionOverride { get; }

        public string ActionDescription()
        {
            if (!string.IsNullOrEmpty(ActionOverride))
            {
                return ActionOverride;
            }
            
            var actor = $"{ActorUser.Username}'s {Actor.Name}";
            var damage = string.Empty;
            if (DamagedCards != null && DamagedCards.Any())
            {
                damage = $"attacked {Target.Username}'s {string.Join(",", DamagedCards.Select(x => x.Name))} card(s) for {Actor.Damage} damage";
            }
            var healing = string.Empty;
            if (HealedCards != null && HealedCards.Any())
            {
                healing = $"healed the {string.Join(",", HealedCards.Select(x => x.Name))} card(s) for {Actor.Healing} health";
            }

            var hasDamage = !string.IsNullOrEmpty(damage);
            var hasHealing = !string.IsNullOrEmpty(healing);
            if (hasDamage || hasHealing)
            {
                return $"{actor} {(hasDamage ? damage : string.Empty)} {(hasDamage && hasHealing ? "and" : string.Empty)} {(hasHealing ? healing : string.Empty)} {Environment.NewLine}"; 
            }
            return string.Empty;
        }
    }
}