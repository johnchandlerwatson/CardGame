using System.Collections.Generic;
using System.Linq;

namespace Vue.Domain.Cards
{
    public class HealerCard : Card
    {
        public override string Name => "Healer";
        public override Row Row => Row.Back;
        public override Row Targets => Row.Front;
        public override Rarity Rarity => Rarity.Rare;
        public override int MaxHealth => 6;
        public override int Damage { get; set; } = 1;
        public override int Healing => 3;
        public override int Speed => 3;
        public override string Description => "Heals front row allies and attacks all front row enemies";

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, List<GameAction> actions)
        {
            var cardsToHeal = friendlyCards.Where(x => x.Row == Targets).ToList();
            foreach (var ally in cardsToHeal)
            {
                var healthAfter = ally.Health + Healing;
                healthAfter = healthAfter > ally.MaxHealth ? ally.MaxHealth : healthAfter;
                ally.Health = healthAfter;
            }

            var cardsToDamage = TargetedCards(enemyCards);
            foreach (var enemy in cardsToDamage)
            {
                enemy.Health = enemy.Health - Damage;
            }

            if (cardsToHeal.Any() || cardsToDamage.Any())
            {
                actions.Add(new GameAction(this, cardsToDamage, cardsToHeal));
            }
        }
    }
}