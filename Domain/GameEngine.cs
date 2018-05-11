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
        public GameEngineResponse ExecuteMove(User user1, User user2)
        {        
            var actions = new List<GameAction>();
            var userCardList = user1.Played.ToList();
            var enemyCardList = user2.Played.ToList();

            ExecuteCardMoves(enemyCardList, userCardList, user1, user2, actions);
            ExecuteChampionMove(enemyCardList, userCardList, user1.Champion, user2.Champion, actions);
            ExecuteChampionMove(userCardList, enemyCardList, user2.Champion, user1.Champion, actions);

            RemoveDeadCards(user1, user2);
            var userPair = new UserPair(user1, user2);
            return new GameEngineResponse { UserPair = userPair, Actions = actions };
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

    public class UserPair
    {
        public UserPair(User user1, User user2)
        {
            User1 = user1;
            User2 = user2;
        }
        public User User1 { get; set; }
        public User User2 { get; set; }
    }

    public class GameEngineResponse
    {
        public UserPair UserPair { get; set; }
        public List<GameAction> Actions { get; set; }
    }
}