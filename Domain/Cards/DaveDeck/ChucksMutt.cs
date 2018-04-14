using System;
using System.Collections.Generic;
using System.Linq;

namespace Vue.Domain.Cards
{
    public class ChucksMutt : Card

    {
        public override string Name => "Chuck's Mutt";

        public override Row Row => Row.Front;

        public override Row Targets => Row.Front;

        public override Rarity Rarity => Rarity.Uncommon;

        public override int Speed => 4;

        public override int Damage {get;set;} = 5;
        public override int Healing => 0;

        public override string Description => "Fast but not accurate, Wildy mutt ready to pounce when Dave is not";

        public override int MaxHealth => 2;

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, List<GameAction> actions)
        {
            var cardsToAttack = TargetedCards(enemyCards);

            if (cardsToAttack.Any())
            {
                var rand = new Random();
                var index = rand.Next(cardsToAttack.Count);                
                var first = cardsToAttack[index];
                first.Health = first.Health - Damage;            
                actions.Add(new GameAction(this, new List<Card> { first }, null));
            }
        }
    }
}