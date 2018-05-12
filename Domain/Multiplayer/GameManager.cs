using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Vue.Domain.Multiplayer
{
    public static class GameManager
    {
        private static ConcurrentDictionary<Guid, Game> _games = new ConcurrentDictionary<Guid, Game>();

        public static Game JoinGame(User user)
        {
            var openGame = _games.Where(x => !x.Value.Full).OrderBy(x => x.Value.CreateTime).FirstOrDefault().Value;
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
            return _games.Where(x => !x.Value.IsOver).Count();
        }

        public static Game GetGame(Guid gameId)
        {
            return _games[gameId];
        }

        private static Game CreateGame(User user1)
        {
            CleanseGames();
            var gameToAdd = new Game
            {
                UserPair = new UserPair(user1, null)
            };
            _games.TryAdd(gameToAdd.Id, gameToAdd);
            return gameToAdd;
        }

        private static void CleanseGames()
        {
            var completedGames = _games.Where(x => x.Value.IsOver && x.Value.SecondsSinceLastTurn >= 30).ToList();           
            var inCompleteGames = _games.Where(x => x.Value.SecondsSinceLastTurn >= 300).ToList();
            completedGames.AddRange(inCompleteGames);

            foreach (var completedGame in completedGames)
            {
                _games.TryRemove(completedGame.Key, out var game);
            }
        }

        private static Game JoinExistingGame(Game game, User user)
        {
            game.UserPair.User2 = user;
            return game;
        }

        public static List<Game> GetGames()
        {
            return _games.Values.ToList();
        }

        public static bool AddMove(Guid gameId, User user)
        {
            var game = _games[gameId];

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