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
        public GameAction(Character actor, List<Character> damagedCharacters, List<Character> healedCards)
        {
            Target = damagedCharacters?.FirstOrDefault()?.User;
            ActorUser = actor.User;
            DamagedCharacters = damagedCharacters;
            HealedCharacters = healedCards;
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
        public List<Character> DamagedCharacters { get; }
        public List<Character> HealedCharacters { get; }
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
            if (DamagedCharacters != null && DamagedCharacters.Any())
            {
                damage = $"attacked {Target.Username}'s {string.Join(",", DamagedCharacters.Select(x => x.Name))} for {Actor.Damage} damage";
            }
            var healing = string.Empty;
            if (HealedCharacters != null && HealedCharacters.Any())
            {
                healing = $"healed the {string.Join(",", HealedCharacters.Select(x => x.Name))} for {Actor.Healing} health";
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