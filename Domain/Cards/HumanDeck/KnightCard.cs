using System.Collections.Generic;
using System.Linq;

namespace Vue.Domain.Cards
{
    public class KnightCard : Card
    {
        public override string Name => "Knight";
        public override Row Row => Row.Front;
        public override Row Targets => Row.Front;
        public override Rarity Rarity => Rarity.Common;
        public override int Damage { get; set; } = 4;
        public override int Healing => 0;
        public override int Speed => 2;
        public override int MaxHealth => 15;
        public override string Description => "Targets highest health in front row";

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, List<GameAction> actions)
        {
            var targetedCards = TargetedCards(enemyCards);

            if (targetedCards.Any())
            {
                var highestHealth = targetedCards.OrderByDescending(x => x.Health).First();
                highestHealth.Health = highestHealth.Health - Damage;
                actions.Add(new GameAction(this, new List<Card> { highestHealth }, null));
            }
        }
    }
}