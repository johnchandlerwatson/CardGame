using System.Collections.Generic;
using System.Linq;

namespace Vue.Domain.Cards
{
    public class TrollCard : Card
    {
        public override string Name => "Troll";
        public override Row Row => Row.Front;
        public override Row Targets => Row.Front;
        public override Rarity Rarity => Rarity.Rare;
        public override int Damage => 4;
        public override int Healing => 0;
        public override int Speed => 1;
        public override int MaxHealth => 20;

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, List<CardAction> actions)
        {
            var targetedCards = TargetedCards(enemyCards);

            if (targetedCards.Any())
            {
                var twoCards = targetedCards.Take(2).ToList();
                foreach (var enemy in twoCards)
                {
                    enemy.Health = enemy.Health - Damage;
                }
                actions.Add(new CardAction(this, twoCards, null));
            }
        }
    }
}