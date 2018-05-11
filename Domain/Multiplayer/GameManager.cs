using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
                UserPair = new UserPair(user1, null)
            };
            _games.Add(gameToAdd);
            return gameToAdd;
        }

        private static Game JoinExistingGame(Game game, User user)
        {
            game.UserPair.User2 = user;
            return game;
        }

        public static List<Game> GetGames()
        {
            return _games.ToList();
        }

        public static bool AddMove(User user)
        {
            var game = _games.First(x => x.User1.Id == user.Id || x.User2.Id == user.Id);

            if (game.User1.Id == user.Id)
            {
                game.UserPair.User1 = user;
                game.User1HasPlayedThisTurn = true;
            }
            else
            {
                game.UserPair.User2 = user;
                game.User2HasPlayedThisTurn = true;
            }
            var turnIsComplete = game.TurnIsComplete;
            if (turnIsComplete)
            {
                game.EndTheTurn();

                var gameEngine = new GameEngine();
                var response = gameEngine.ExecuteMove(game.User1, game.User2);
                game.UserPair = response.UserPair;
                game.Actions = response.Actions;
            } 
            return turnIsComplete;
        }
    }
}