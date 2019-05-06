using System.Collections.Generic;
using System.Linq;
using Vue.Domain.Champions;

namespace Vue.Domain.Cards
{
    public class GoblinShamanCard : Card
    {
        public override string Name => "Goblin Shaman";
        public override Row Row => Row.Back;
        public override Row Targets => Row.Both;
        public override Rarity Rarity => Rarity.Rare;
        public override int Speed => 3;
        public override int Damage { get; set; } = 2;
        public override int Healing => 2;
        public override int MaxHealth => 5;
        public override string Description => $"Heals 2 allies and attacks 2 enemies";

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, Champion enemyChamp, List<GameAction> actions)
        {
            var cardsToHeal = friendlyCards.Where(x => x.Row == Targets).ToList();
            var healedCards = new List<Character>();
            foreach (var ally in cardsToHeal)
            {
                healedCards.Add(Heal(ally));
            }

            var attackedCards = Attack(TargetedCards(enemyCards));
            if (!attackedCards.Any())
            {
                Attack(enemyChamp);
                actions.Add(new GameAction(this, new List<Character>{enemyChamp}));
            }

            if (attackedCards.Any() || healedCards.Any())
            {
                actions.Add(new GameAction(this, attackedCards, healedCards));
            }
        }
    }
}