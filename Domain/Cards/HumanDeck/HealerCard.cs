using System.Collections.Generic;
using System.Linq;
using Vue.Domain.Champions;

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

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, Champion enemyChamp, List<GameAction> actions)
        {
            var cardsToHeal = friendlyCards.Where(x => x.Row == Targets).ToList();
            var healedCards = new List<Character>();
            foreach (var ally in cardsToHeal)
            {
                healedCards.Add(Heal(ally));
            }

            var cardsToDamage = TargetedCards(enemyCards);
            var damagedCards = new List<Character>();
            if (cardsToDamage.Any())
            {
                damagedCards = Attack(cardsToDamage);
            }
            else 
            {
                Attack(enemyChamp);
                actions.Add(new GameAction(this, new List<Character>{enemyChamp}, null));
            }

            if (cardsToHeal.Any() || cardsToDamage.Any())
            {
                actions.Add(new GameAction(this, damagedCards, healedCards));
            }
        }
    }
}