using System.Collections.Generic;
using Vue.Domain.Cards;
using System.Linq;

namespace Vue.Domain.Champions
{
    public abstract class Champion : Character
    {
        public abstract string ChampImage { get; }
        public List<Card> TargetedCards(List<Card> enemyCards)
        {
            var cardsToAttack = enemyCards.Where(x => x.Row == Row.Front).ToList();
            if (cardsToAttack.Any()) return cardsToAttack;
            return enemyCards.Where(x => x.Row != Row.Back).ToList();
        }
    }
}