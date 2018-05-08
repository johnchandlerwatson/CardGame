using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Vue.Domain.Multiplayer
{
    public static class GameManager
    {
        private static ConcurrentBag<Game> _games = new ConcurrentBag<Game>();

        public static Game JoinGame(User user)
        {
            var openGame = _games.Where(x => !x.Full).OrderBy(x => x.CreateTime).FirstOrDefault();
            if (openGame != null)
            {
                return JoinExistingGame(openGame, user);
            }
            else 
            {
                return CreateGame(user);
            }
        }

        public static int GameCount()
        {
            return _games.Count;
        }

        public static Game GetGame(Guid gameId)
        {
            return _games.First(x => x.Id == gameId);
        }

        private static Game CreateGame(User user1)
        {
            var gameToAdd = new Game
            {
                User1 = user1
            };
            _games.Add(gameToAdd);
            return gameToAdd;
        }

        private static Game JoinExistingGame(Game game, User user)
        {
            game.User2 = user;
            return game;
        }

        public static bool AddMove(User user)
        {
            var game = _games.First(x => x.User1.Id == user.Id || x.User2.Id == user.Id);

            if (game.User1.Id == user.Id)
            {
                game.User1 = user;
                game.User1HasPlayedThisTurn = true;
            }
            else
            {
                game.User2 = user;
                game.User2HasPlayedThisTurn = true;
            }
            var turnIsComplete = game.TurnIsComplete;
            if (turnIsComplete)
            {
                game.EndTheTurn();
            } 
            return turnIsComplete;
        }
    }
}