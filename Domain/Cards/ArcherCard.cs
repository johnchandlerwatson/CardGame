using System.Collections.Generic;
using System.Linq;

namespace Vue.Domain.Cards
{
    public class ArcherCard : Card
    {
        public override string Name => "Archer";
        public override Row Row => Row.Back;
        public override Row Targets => Row.Back;
        public override Rarity Rarity => Rarity.Uncommon;
        public override int Damage => 2;
        public override int Healing => 0;
        public override int Speed => 4;
        public override int MaxHealth => 4;

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, List<CardAction> actions)
        {
            var cardsToAttack = TargetedCards(enemyCards);

            if (cardsToAttack.Any())
            {
                foreach (var enemy in cardsToAttack)
                {
                    enemy.Health = enemy.Health - Damage;
                }
                actions.Add(new CardAction(this, cardsToAttack, null));
            }
        }
    }
}