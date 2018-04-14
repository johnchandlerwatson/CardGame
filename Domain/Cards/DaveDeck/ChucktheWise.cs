using System;
using System.Collections.Generic;
using System.Linq;

namespace Vue.Domain.Cards
{
    public class ChucktheWise : Card

    {
        public override string Name => "Chuck the Wise";

        public override Row Row => Row.Back;

        public override Row Targets => Row.Both;

        public override Rarity Rarity => Rarity.Uncommon;

        public override int Speed => 3;

        public override int Damage {get;set;} = 2;
        public override int Healing => 0;

        public override string Description => "Chuck the wise is said to know how Dave came into being";

        public override int MaxHealth => 7;

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