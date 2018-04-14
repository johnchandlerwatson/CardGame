using System;
using System.Collections.Generic;
using System.Linq;

namespace Vue.Domain.Cards
{
    public class ElfDave : Card

    {
        public override string Name => "Elf Dave";

        public override Row Row => Row.Back;

        public override Row Targets => Row.Back;

        public override Rarity Rarity => Rarity.Common;

        public override int Speed => 3;

        public override int Damage {get;set;} = 1;
        public override int Healing => 0;

        public override string Description => "Elf Dave with the long range";

        public override int MaxHealth => 4;

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, List<GameAction> actions)
        {
            var cardsToAttack = TargetedCards(enemyCards);

            if (cardsToAttack.Any())
            {
                var rand = new Random();
                var index = rand.Next(cardsToAttack.Count);                
                var first = cardsToAttack[index];
                first.Health = first.Health - Damage;            
                actions.Add(new GameAction(this, new List<Card> { first }, null));
            }
        }
    }
}