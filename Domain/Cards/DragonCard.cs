using System.Collections.Generic;
using System.Linq;

namespace Vue.Domain.Cards
{
    public class DragonCard : Card
    {
        public override string Name => "Dragon";
        public override Row Row => Row.Front;
        public override Row Targets => Row.Both;
        public override Rarity Rarity => Rarity.Epic;
        public override int Speed => 1;
        public override int Damage => 4;
        public override int Healing => 0;
        public override int MaxHealth => 25;
        public override string Description => "Attacks all cards every 3 turns";

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, List<CardAction> actions)
        {
            var targetCards = TargetedCards(enemyCards);
            var shouldAttack = targetCards.Any() && ((RoundsPlayed + 2) % 3 == 0);
            if (shouldAttack)
            {
                foreach (var enemy in targetCards)
                {
                    enemy.Health = enemy.Health - Damage;
                }
                actions.Add(new CardAction(this, targetCards, null));
            }
        }
    }
}