using System.Collections.Generic;
using Vue.Domain;
using Vue.Domain.Cards;
using System.Linq;
using System;

namespace Vue.Domain
{
    public class CardAction
    {
        public CardAction(Card actorCard, List<Card> damagedCards, List<Card> healedCards)
        {
            Target = damagedCards?.FirstOrDefault()?.User;
            Actor = actorCard.User;
            DamagedCards = damagedCards;
            HealedCards = healedCards;
            ActorCard = actorCard;
            Damage = actorCard.Damage;
            Healing = actorCard.Healing;
        }

        public CardAction(string actionOverride)
        {
            ActionOverride = actionOverride;
        }
        
        public User Target { get; }
        public User Actor { get; }
        public List<Card> DamagedCards { get; }
        public List<Card> HealedCards { get; }
        public Card ActorCard { get; }
        public int Damage { get; }
        public int Healing { get; }
        public string ActionOverride { get; }

        public string ActionDescription()
        {
            if (!string.IsNullOrEmpty(ActionOverride))
            {
                return ActionOverride;
            }
            
            var actor = $"{Actor.Username}'s {ActorCard.Name} card";
            var damage = string.Empty;
            if (DamagedCards != null && DamagedCards.Any())
            {
                damage = $"attacked {Target.Username}'s {string.Join(",", DamagedCards.Select(x => x.Name))} card(s) for {ActorCard.Damage} damage";
            }
            var healing = string.Empty;
            if (HealedCards != null && HealedCards.Any())
            {
                healing = $"healed the {string.Join(",", HealedCards.Select(x => x.Name))} card(s) for {ActorCard.Healing} health";
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