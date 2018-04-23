using System.Collections.Generic;
using System.Linq;
using Vue.Domain.Champions;

namespace Vue.Domain.Cards
{
    public class GrimReaper : Card
    {
        public override Row Row => Row.Front;

        public override Row Targets => Row.Both;

        public override Rarity Rarity => Rarity.Godly;

        public override int Speed => 100;

        public override string Name => "Grim Reaper";

        public override int MaxHealth => 100;

        public override int Damage { get; set;} = 100;

        public override int Healing => 0;

        public override string Description => "Death has descended";

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, Champion enemyChampion, List<GameAction> actions)
        {
            var targets = TargetedCards(enemyCards);        
            if (targets.Any())
            {
                Attack(targets);
            }
            else 
            {
                Attack(enemyChampion);
            }
            actions.Add(new GameAction(Description));
        }
    }
}