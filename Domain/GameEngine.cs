using System.Collections.Generic;
using System.Linq;
using Vue.Domain.Cards;
using Vue.Domain.Champions;
using Vue.Domain.Multiplayer;
using Vue.Models;

namespace Vue.Domain
{
    public class GameEngine
    {
        public MoveModel ExecuteMove(User user, User enemy, Game game)
        {        
            var actions = new List<GameAction>();
            var userCardList = user.Played.ToList();
            var enemyCardList = enemy.Played.ToList();

            ExecuteCardMoves(enemyCardList, userCardList, user, enemy, actions);
            ExecuteChampionMove(enemyCardList, userCardList, user.Champion, enemy.Champion, actions);
            ExecuteChampionMove(userCardList, enemyCardList, enemy.Champion, user.Champion, actions);

            RemoveDeadCards(user, enemy);
            
            game.User1 = user;
            game.User2 = enemy;
            return new MoveModel { Game = game, Actions = actions };
        }

        private void ExecuteChampionMove(List<Card> enemyCardList, List<Card> userCardList, Champion actorChampion, Champion enemyChampion, List<GameAction> actions)
        {
            actorChampion.ApplyMove(enemyCardList, userCardList, enemyChampion, actions);
            actorChampion.RoundsPlayed++;
        }

        private void ExecuteCardMoves(List<Card> enemyCardList, List<Card> userCardList, User user, User enemy, List<GameAction> actions)
        {
            var allCards = user.Played.Union(enemy.Played);
            var sortedCards = allCards.OrderByDescending(x => x.Speed).ToList();
            foreach (var card in sortedCards)
            {
                if (card.RoundsPlayed > 0)
                {
                    if (card.User.Username == user.Username)
                    {
                        card.ApplyMove(enemyCardList, userCardList, enemy.Champion, actions);
                    }
                    else
                    {
                        card.ApplyMove(userCardList, enemyCardList, user.Champion, actions);
                    }
                    
                    user.Played = userCardList;
                    enemy.Played = enemyCardList;
                }
                card.RoundsPlayed++;
            }
        }

        private void RemoveDeadCards(User user, User enemy)
        {
            RemoveDeadCards(user);
            RemoveDeadCards(enemy);
        }

        private void RemoveDeadCards(User user)
        {
            user.Played.RemoveAll(x => x.IsDead);
        }
    }
}