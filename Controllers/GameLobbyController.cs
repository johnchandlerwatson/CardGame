using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Vue.Domain;
using Vue.Domain.Multiplayer;
using Vue.Utility;

namespace Vue.Controllers
{
    [Route("api/[controller]")]
    public class GameLobbyController : Controller
    {
        [HttpGet]
        public ContentResult Index()
        {
            var model = new GameLobbyModel
            {
                CurrentGameCount = GameManager.GameCount()
            };
            return Content(model.ToJson(), "application/json");
        }

        [HttpGet("{username}/{deck}/{champ}")]
        public ContentResult JoinGame(string username, string deck, string champ)
        {
            var user = new User(username, deck, Dealer.AllChamps.First(x => x.Name == champ));
            var game = GameManager.JoinGame(user);
            return Content(game.ToJson(), "application/json");
        }
    }

    public class GameLobbyModel
    {
        public int CurrentGameCount { get; set; }
    }

    public class JoinGameModel
    {
        public Game Game { get; set; }
    }
}