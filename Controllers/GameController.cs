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
            user.ResetHandCards();

            var user2 = username == game.User1.Username ? game.User2 : game.User1;
            user2.ResetHandCards();

            return new MoveModel
            {
                User = user,
                Enemy = user2
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
                User = user,
                Enemy = bot
            };
        }
        
        [HttpPost]
        public ContentResult Move([FromBody] JObject jObject)
        {
            var json = jObject.ToString(Formatting.None);
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            var model = JsonConvert.DeserializeObject<EndOfTurnModel>(json, settings);
            var user = model.User;

            var playedCard = user.Username == "grim" ? new GrimReaper() : Dealer.DeckCards(user.CurrentDeck).First(x => x.Name == model.Selection);
            user.AddPlayedCard(playedCard);
            user.ResetHandCards();

            var bot = model.Enemy;
            bot.AddPlayedCard(Dealer.GetRandomCard(bot));
            bot.ResetHandCards();

            var gameEngine = new GameEngine();
            var results = gameEngine.ExecuteMove(user, bot);
            return Content(results.ToJson(), "application/json");
        }
    }
}
