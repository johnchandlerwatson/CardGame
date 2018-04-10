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
            var allCards = user.Played.Union(enemy.Played);
            var sortedCards = allCards.OrderByDescending(x => x.Speed).ToList();
            var actions = new List<CardAction>();
            foreach (var card in allCards)
            {
                if (card.RoundsSincePlayed > 0)
                {
                    var friendlyCards = allCards.Where(x => x.User == card.User).ToList();
                    var enemyCards = allCards.Except(friendlyCards).ToList();
                    card.ApplyMove(enemyCards, friendlyCards, actions);
                }
                card.RoundsSincePlayed++;
            }
            RemoveDeadCards(user);
            RemoveDeadCards(enemy);
            
            return new MoveModel
            {
                User = user,
                Enemy = enemy,
                Actions = actions
            };
        }

        private void RemoveDeadCards(User user)
        {
            user.Played.RemoveAll(x => x.IsDead);
        }
    }
}