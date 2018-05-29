using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Vue.Domain;
using Vue.Domain.Multiplayer;
using Vue.Utility;

namespace Vue.Controllers
{
    [Route("api/[controller]/[action]")]
    public class GameLobbyController : Controller
    {
        private readonly IGameLobbyHub _hub;

        public GameLobbyController(IGameLobbyHub hub)
        {
            _hub = hub;
        }

        [HttpGet("{offset}")]
        public ContentResult Index()
        {
            var model = new GameLobbyModel
            {
                CurrentGameCount = GameManager.GameCount(),
                Messages = GetMessageModels()
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

        [HttpGet("{gameId}")]
        public ContentResult AlertClients(Guid gameId)
        {
            _hub.SendMessage(gameId);
            return Content(new {Success= true}.ToJson(), "application/json");
        }

        private List<ChatMessageModel> GetMessageModels()
        {
            var models = new List<ChatMessageModel>();
            foreach (var message in ChatManager.GetMessages())
            {
                var model = new ChatMessageModel(message);
                models.Add(model);
            }
            return models;
        }
    }

    public class GameLobbyModel
    {
        public int CurrentGameCount { get; set; }
        public List<ChatMessageModel> Messages { get; set; }
    }

    public class ChatMessageModel 
    {
        public ChatMessageModel(ChatMessage message)
        {
            this.message = message.Message;
            date = message.DateSent;
        }
        public string message { get; }
        public DateTime date { get; set; }
    }
}