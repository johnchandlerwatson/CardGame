using System.Collections.Generic;
using System.Linq;
using Vue.Domain.Champions;

namespace Vue.Domain.Cards
{
    public class CannonCard : Card
    {
        public override string Name => "Cannon";

        public override Row Row => Row.Front;

        public override Row Targets => Row.Both;

        public override Rarity Rarity => Rarity.Rare;

        public override int Speed => 4;

        public override int Damage { get; set; } = 6;

        public override int Healing => 0;

        public override string Description => "Deals damage to 1 front row and 1 back row enemy every other turn";

        public override int MaxHealth => 15;

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, Champion enemyChamp, List<GameAction> actions)
        {
            var targets = TargetedCards(enemyCards);

            var front = targets.FirstOrDefault(x => x.Row == Row.Front);
            if (front != null)
            {
                Attack(front);
            }

            var back = targets.FirstOrDefault(x => x.Row == Row.Back);
            if (back != null)
            {
                Attack(back);
            }

            var damaged = new List<Character>{ front, back }.Where(x => x != null).ToList();
            if (damaged.Any())
            {
                actions.Add(new GameAction(this, damaged, null));
            }
            else 
            {
                Attack(enemyChamp);
                actions.Add(new GameAction(this, new List<Character>{ enemyChamp }, null));
            }
        }
    }
}
