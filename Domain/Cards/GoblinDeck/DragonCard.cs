using System.Collections.Generic;
using System.Linq;
using Vue.Domain.Champions;

namespace Vue.Domain.Cards
{
    public class DragonCard : Card
    {
        public override string Name => "Dragon";
        public override Row Row => Row.Front;
        public override Row Targets => Row.Both;
        public override Rarity Rarity => Rarity.Epic;
        public override int Speed => 1;
        public override int Damage { get; set; } = 4;
        public override int Healing => 0;
        public override int MaxHealth => 15;
        public override string Description => "Attacks all cards every 3 turns";

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, Champion enemyChamp, List<GameAction> actions)
        {
            var targetCards = TargetedCards(enemyCards);
            var shouldAttack = targetCards.Any() && ((RoundsPlayed + 2) % 3 == 0);
            if (shouldAttack)
            {
                if (targetCards.Any())
                {
                    var attackedCards = Attack(targetCards);
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
}