using System.Collections.Generic;
using Vue.Domain.Cards;
using System.Linq;

namespace Vue.Domain.Champions
{
    public class DaveThePowerful : Champion
    {
        public override string Name => "Dave the Unmerciful";
        public override int MaxHealth => 50;
        public override int Healing => 0;
        public override int Damage { get; set;} = 10;
        public override string Description => "The all powerful Dave!!!! All fear him. Bow down before him!";

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, List<GameAction> actions)
        {
            if (friendlyCards.Count == 0)
            {
                var targets = TargetedCards(enemyCards);
                
                foreach (var target in targets)
                {
                    target.Health = target.Health - Damage;
                }
                actions.Add(new GameAction(this, targets, null));
            }
        }
    }
}