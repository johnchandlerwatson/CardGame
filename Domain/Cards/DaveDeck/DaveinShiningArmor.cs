using System;
using System.Collections.Generic;
using System.Linq;
using Vue.Domain.Champions;

namespace Vue.Domain.Cards
{
    public class DaveinShiningArmor : Card

    {
        public override string Name => "Dave in Shining Armor";

        public override Row Row => Row.Front;

        public override Row Targets => Row.Front;

        public override Rarity Rarity => Rarity.Rare;

        public override int Speed => 4;

        public override int Damage {get;set;} = 3;
        public override int Healing => 0;

        public override string Description => "Everyone is looking for their Dave in Shining Armor";

        public override int MaxHealth => 9;

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, Champion enemyChamp, List<GameAction> actions)
        {
            var cardsToAttack = TargetedCards(enemyCards);

            if (cardsToAttack.Any())
            {
                var rand = new Random();
                var index = rand.Next(cardsToAttack.Count);                
                var first = cardsToAttack[index];
                first.Health = first.Health - Damage;            
                actions.Add(new GameAction(this, new List<Character> { first }, null));
            }
            else
            {
                Attack(enemyChamp);
                actions.Add(new GameAction(this, new List<Character> { enemyChamp }, null));
            }
        }
    }
}