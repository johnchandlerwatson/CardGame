using System.Collections.Generic;
using System.Linq;
using Vue.Domain.Champions;

namespace Vue.Domain.Cards
{
    public class GoblinGruntCard : Card
    {
        public override string Name => "Goblin Grunt";
        public override Row Row => Row.Front;
        public override Row Targets => Row.Front;
        public override Rarity Rarity => Rarity.Common;
        public override int Speed => 5;
        public override int Damage { get; set; } = 3;
        public override int Healing => 0;
        public override int MaxHealth => 5;
        public override string Description => $"Attacks the card with the lowest health in the {Targets.ToString().ToLower()} row";

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, Champion enemyChamp, List<GameAction> actions)
        {
            var attackedCard = Attack(TargetedCards(enemyCards).OrderBy(x => x.Health).FirstOrDefault());
            if (attackedCard != null)
            {
                actions.Add(new GameAction(this, new List<Character> {attackedCard}));
            }
            else
            {
                Attack(enemyChamp);
                actions.Add(new GameAction(this, new List<Character>{enemyChamp}));
            }
        }
    }
}