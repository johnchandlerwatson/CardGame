using System.Collections.Generic;
using Vue.Domain.Cards;

namespace Vue.Domain.Champions
{
    public class JoshDestoyerOfWorlds : Champion
    {
        public override string ChampImage => "josh";

        public override string Name => "Josh Destroyer of Worlds";

        public override int MaxHealth => 50;

        public override int Damage { get; set;} = 5;

        public override int Healing => 0;

        public override string Description => "Eats worlds for breakfast";

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, Champion enemyChampion, List<GameAction> actions)
        {
            if (RoundsPlayed % 3 == 0)
            {
                var targets = TargetedCards(enemyCards);
                var attackedCards = Attack(targets);
                actions.Add(new GameAction(this, attackedCards, null));
            }
        }
    }
}