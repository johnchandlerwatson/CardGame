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

            var userCardList = user.Played.ToList();
            var enemyCardList = enemy.Played.ToList();

            foreach (var card in sortedCards)
            {
                if (card.RoundsPlayed > 0)
                {
                    if (card.User.Username == user.Username)
                    {
                        card.ApplyMove(enemyCardList, userCardList, actions);
                    }
                    else
                    {
                        card.ApplyMove(userCardList, enemyCardList, actions);
                    }
                    
                    user.Played = userCardList;
                    enemy.Played = enemyCardList;
                }
                card.RoundsPlayed++;
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