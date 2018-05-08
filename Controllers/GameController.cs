using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vue.Models;
using Vue.Domain;
using Vue.Domain.Cards;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vue.Utility;
using Vue.Domain.Multiplayer;

namespace Vue.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly IGameLobbyHub _hub;

        public GameController(IGameLobbyHub hub)
        {
            _hub = hub;
        }

        [HttpGet("{username}/{deck}/{champ}/{gameId}")]
        public ContentResult Index(string username, string deck, string champ, Guid? gameId)
        {
            var model = GetModel(username, deck, champ, gameId);
            return Content(model.ToJson(), "application/json");
        }

        private MoveModel GetModel(string username, string deck, string champ, Guid? gameId)
        {
            if (gameId.HasValue)
            {
                return PlayerGame(gameId.Value, username);
            }
            else 
            {
                return BotGame(username, deck, champ);
            }
        }

        private MoveModel PlayerGame(Guid gameId, string username)
        {
            var game = GameManager.GetGame(gameId);
            var user = username == game.User1.Username ? game.User1 : game.User2;
            var user2 = username == game.User1.Username ? game.User2 : game.User1;
            user.ResetHandCards();
            user2.ResetHandCards();
            game.User1 = user;
            game.User2 = user2;

            return new MoveModel
            {
                Game = game
            };
        }

        private MoveModel BotGame(string username, string deck, string champ)
        {
            var user = new User(username, deck, Dealer.AllChamps.First(x => x.Name == champ));
            user.ResetHandCards();

            var bot = new User("bot", Dealer.RandomDeck().Name, Dealer.RandomChamp());
            bot.ResetHandCards();

            return new MoveModel
            {
                Game = new Game
                {
                    User1 = user,
                    User2 = bot
                }
            };
        }
        
        [HttpPost]
        public ContentResult Move([FromBody] JObject jObject)
        {
            var json = jObject.ToString(Formatting.None);
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            var model = JsonConvert.DeserializeObject<MoveModel>(json, settings);
            var user = model.Username == model.Game.User1.Username ? model.Game.User1 : model.Game.User2;

            var playedCard = user.Username == "grim" ? new GrimReaper() : Dealer.DeckCards(user.CurrentDeck).First(x => x.Name == model.Selection);
            user.AddPlayedCard(playedCard);
            user.ResetHandCards();            

            if (model.Game.IsBotGame)
            {
                var enemy = model.Username == model.Game.User1.Username ? model.Game.User2 : model.Game.User1;
                enemy.AddPlayedCard(Dealer.GetRandomCard(enemy));
                enemy.ResetHandCards();       
                var gameEngine = new GameEngine();
                var results = gameEngine.ExecuteMove(user, enemy, new Game());
                return Content(results.ToJson(), "application/json");
            }
            else 
            {
                var turnIsComplete = GameManager.AddMove(user);
                if (turnIsComplete)
                {
                    _hub.EndTurn(model.Game.Id);
                }
                return Content(new { PlayerGame = true, User = user }.ToJson(), "application/json");
            }
        }

        [HttpGet("{gameId}/{username}")]
        public ContentResult GetResults(Guid gameId, string username)
        {
            var game = GameManager.GetGame(gameId);
            var gameEngine = new GameEngine();
            var user = username == game.User1.Username ? game.User1 : game.User2;
            var enemy = username == game.User1.Username ? game.User2 : game.User1;
            var model = gameEngine.ExecuteMove(user, enemy, game);
            model.Username = username;
            return Content(model.ToJson(), "application/json");
        }
    }
}
