using System.Collections.Generic;
using System.Linq;
using Vue.Domain.Cards;
using Vue.Models;

namespace Vue.Domain
{
    public class GameEngine
    {
        public MoveModel ExecuteMove(User user, User enemy)
        {
            var allCards = user.Played.Union(enemy.Played).OrderByDescending(x => x.Speed).ToList();
            var actions = new List<CardAction>();
            foreach (var card in allCards)
            {
                var friendlyCards = allCards.Where(x => x.User == card.User).ToList();
                var enemyCards = allCards.Except(friendlyCards).ToList();
                card.ApplyMove(enemyCards, friendlyCards, actions);
            }
            return new MoveModel
            {
                User = user,
                Enemy = enemy,
                Actions = actions
            };
        }

        private List<Card> PlayedCards(List<Card> allCards, User user)
        {
            return allCards.Where(x => x.User == user && !x.IsDead).ToList();
        }
    }
}