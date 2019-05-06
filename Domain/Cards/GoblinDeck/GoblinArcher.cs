using System.Collections.Generic;
using System.Linq;
using Vue.Domain.Champions;

namespace Vue.Domain.Cards
{
    public class GoblinArcherCard : Card
    {
        public override string Name => "Goblin Archer";
        public override Row Row => Row.Back;
        public override Row Targets => Row.Back;
        public override Rarity Rarity => Rarity.Common;
        public override int Speed => 2;
        public override int Damage { get; set; } = 4;
        public override int Healing => 0;
        public override int MaxHealth => 4;
        public override string Description => $"Attacks random card in the {Targets.ToString().ToLower()} row";

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, Champion enemyChamp, List<GameAction> actions)
        {
            var attackedCard = Attack(TargetedCards(enemyCards).FirstOrDefault());
            if (attackedCard != null)
            {
                actions.Add(new GameAction(this, new List<Character>{attackedCard}));
            }
            else
            {
                Attack(enemyChamp);
                actions.Add(new GameAction(this, new List<Character>{enemyChamp}));
            }
        }
    }
}