using System.Collections.Generic;
using System.Linq;
using Vue.Domain.Champions;

namespace Vue.Domain.Cards
{
    public class TrollCard : Card
    {
        public override string Name => "Troll";
        public override Row Row => Row.Front;
        public override Row Targets => Row.Front;
        public override Rarity Rarity => Rarity.Rare;
        public override int Damage { get; set; } = 4;
        public override int Healing => 0;
        public override int Speed => 1;
        public override int MaxHealth => 15;
        public override string Description => "Attacks two enemies in the front row";

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, Champion enemyChamp, List<GameAction> actions)
        {
            var targetedCards = TargetedCards(enemyCards);

            if (targetedCards.Any())
            {
                var twoCards = targetedCards.Take(2).ToList();
                var attackedCards = Attack(twoCards);
                actions.Add(new GameAction(this, attackedCards, null));
            }
            else 
            {
                Attack(enemyChamp);
                actions.Add(new GameAction(this, new List<Character>{enemyChamp}, null));
            }
        }
    }
}