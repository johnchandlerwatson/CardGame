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
            model.ValidateModel(username);
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
            game.ResetHandCards();
            var model = new MoveModel { Game = GetNewGame(game), Username = username };
            model.ArrangeUsers(username);
            return model;
        }

        private Game GetNewGame(Game game)
        {
            return new Game { UserPair = new UserPair(game.User1, game.User2), Id = game.Id, Actions = game.Actions};
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
                    UserPair = new UserPair(user, bot)
                }
            };
        }
        
        [HttpPost]
        public ContentResult Move([FromBody] JObject jObject)
        {
            var json = jObject.ToString(Formatting.None);
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            var model = JsonConvert.DeserializeObject<EndOfMoveModel>(json, settings);
            var user = model.Username == model.Username ? model.UserPair.User1 : model.UserPair.User2;

            var playedCard = user.Username == "grim" ? new GrimReaper() : Dealer.DeckCards(user.CurrentDeck).First(x => x.Name == model.Selection);
            user.AddPlayedCard(playedCard);
            user.ResetHandCards();            

            if (model.IsBotGame)
            {
                var enemy = model.Username == model.UserPair.User1.Username ? model.UserPair.User2 : model.UserPair.User1;
                enemy.AddPlayedCard(Dealer.GetRandomCard(enemy));
                enemy.ResetHandCards();       
                var gameEngine = new GameEngine();
                var results = gameEngine.ExecuteMove(user, enemy);
                var game = new Game { UserPair = results.UserPair, Actions = results.Actions};
                var resultModel = new MoveModel { Game = game, Selection = model.Selection, Username = model.Username };
                return Content(resultModel.ToJson(), "application/json"); 
            }
            else 
            {
                var turnIsComplete = GameManager.AddMove(user);
                if (turnIsComplete)
                {
                    _hub.EndTurn(model.GameId.Value);
                }
                return Content(new { PlayerGame = true, User = user }.ToJson(), "application/json");
            }
        }

        [HttpGet("{gameId}/{username}")]
        public ContentResult GetResults(Guid gameId, string username)
        {
            var game = GameManager.GetGame(gameId);        
            var model = new MoveModel { Game = GetNewGame(game), Username = username };
            model.ArrangeUsers(username);
            model.ValidateModel(username);
            return Content(model.ToJson(), "application/json");
        }
    }
}
