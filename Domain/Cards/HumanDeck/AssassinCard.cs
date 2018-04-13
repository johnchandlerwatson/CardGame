using System.Collections.Generic;
using System.Linq;

namespace Vue.Domain.Cards
{
    public class AssassinCard : Card
    {
        public override string Name => "Assassin";
        public override Row Row => Row.Front;
        public override Row Targets => Row.Front;
        public override Rarity Rarity => Rarity.Uncommon;
        public override int Speed => 5;
        public override int Damage { get; set; } = 5;
        public override int Healing => 0;
        public override int MaxHealth => 10;
        public override string Description => "Attacks lowest health card in front row";

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, List<CardAction> actions)
        {
            var cardsToAttack = TargetedCards(enemyCards);
            if (cardsToAttack.Any())
            {
                var lowestHealthCard = cardsToAttack.OrderBy(x => x.Health).First();
                lowestHealthCard.Health = lowestHealthCard.Health - Damage;
                actions.Add(new CardAction(this, new List<Card> {lowestHealthCard}, null));
            } 
        }
    }
}