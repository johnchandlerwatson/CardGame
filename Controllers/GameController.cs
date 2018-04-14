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

namespace Vue.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        [HttpGet("{username}/{deck}")]
        public ContentResult Index(string username, string deck)
        {
            var user = new User(username, deck);
            user.AddHandCards(user);

            var bot = new User("bot", Dealer.RandomDeck().Name);
            bot.AddHandCards(user);

            var model = new MoveModel
            {
                User = user,
                Enemy = bot
            };
            return Content(Json(model), "application/json");
        }
        
        [HttpPost]
        public ContentResult Move([FromBody] JObject jObject)
        {
            var json = jObject.ToString(Formatting.None);
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            var model = JsonConvert.DeserializeObject<EndOfTurnModel>(json, settings);
            var user = new User("username", "Human"); //todo: fix this
            user.AddPlayedCards(model.UserPlayedCards);
            user.AddPlayedCard(Dealer.DeckCards(user.CurrentDeck).First(x => x.Name == model.Selection));
            user.AddHandCards(user);

            var bot = new User("bot", "Human"); //todo: fix this
            bot.AddPlayedCards(model.EnemyPlayedCards);
            bot.AddPlayedCard(Dealer.GetRandomCard(bot));
            bot.AddHandCards(bot);

            var gameEngine = new GameEngine();
            var results = gameEngine.ExecuteMove(user, bot);
            return Content(Json(results), "application/json");
        }

        private string Json(MoveModel model)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            };
            return JsonConvert.SerializeObject(model, settings);
        }
    }
}
