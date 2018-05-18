using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Vue.Domain;
using Vue.Domain.Cards;
using Vue.Domain.Multiplayer;
using Vue.Models;

namespace Vue.Controllers
{
    [Route("api/[controller]")]
    public class InfoController : Controller
    {
        [HttpGet]
        public string Index()
        {
            var games = GameManager.GetGames();
            return JsonConvert.SerializeObject(GetModel(games), Formatting.Indented);
        }

        private GameInfoModel GetModel(List<Game> games)
        {
            return new GameInfoModel
            {
                Games = GameModels(games)
            };
        }

        private List<GameInfo> GameModels(List<Game> games)
        {
            var gameInfos = new List<GameInfo>();
            
            foreach (var game in games)
            {
                var user1 = UserModel(game.User1);
                var user2 = UserModel(game.User2);
                var gameInfo = new GameInfo 
                { 
                    Id = game.Id, 
                    User1 = user1, 
                    User2 = user2,
                    IsOver = game.IsOver,
                    LastTurnTime = game.LastTurnTime
                };
                gameInfos.Add(gameInfo);
            }
            return gameInfos; 
        }

        private UserInfo UserModel(User user)
        {
            if (user == null) return null;
            var cards = new List<CardInfo>();
            foreach (var card in user.Played)
            {
                cards.Add(CardModel(card));
            }
            return new UserInfo { Username = user.Username, Cards = cards };
        }

        private CardInfo CardModel(Card card)
        {
            return new CardInfo { Type = card.Name };
        }
    }

    public class GameInfoModel
    {
        public List<GameInfo> Games { get; set; }
    }

    public class GameInfo
    {
        public Guid Id { get; set; }
        public bool IsOver { get; set; }
        public DateTime LastTurnTime { get; set; }
        public UserInfo User1 { get; set; }
        public UserInfo User2 { get; set; }
    }

    public class UserInfo
    {
        public string Username { get; set; }
        public List<CardInfo> Cards { get; set; }
    }

    public class CardInfo
    {
        public string Type { get; set; }
    }
}