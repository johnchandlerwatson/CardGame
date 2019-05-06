using System;
using System.Collections.Generic;
using System.Linq;
using Vue.Domain.Champions;

namespace Vue.Domain.Cards
{
    public class GoblinKingCard : Card
    {
        public override string Name => "Goblin King";
        public override Row Row => Row.Back;
        public override Row Targets => Row.Both;
        public override Rarity Rarity => Rarity.Legendary;
        public override int Speed => 2;
        public override int Damage { get; set; } = 0;
        public override int Healing => 0;
        public override int MaxHealth => 20;
        public override string Description => $"Spawns a goblin card every other turn";

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, Champion enemyChamp, List<GameAction> actions)
        {
            if (RoundsPlayed % 2 == 0)
            {
                actions.Add(new GameAction(AddGoblin(friendlyCards))); 
            }
        }

        private string AddGoblin(List<Card> friendlyCards) 
        {
            var cardToAdd = new GoblinGruntCard();
            cardToAdd.User = this.User;
            friendlyCards.Add(cardToAdd);
            return $"{User.Username}'s {Name} card added a {cardToAdd.Name} card to the field!";
        }
    }
}