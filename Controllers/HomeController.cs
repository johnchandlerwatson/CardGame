using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Vue.Domain.Cards;
using Vue.Models;

namespace Vue.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]
        public JsonResult Index()
        {
            var json = "{\"Selection\":\"Assassin\",\"EnemyPlayedCards\":[{\"$type\":\"Vue.Domain.Cards.AssassinCard, Vue\",\"Name\":\"Assassin\",\"Row\":0,\"Targets\":0,\"Rarity\":2,\"Speed\":5,\"Damage\":7,\"Healing\":0,\"MaxHealth\":5,\"Id\":\"3cc36bac-34ba-4ad8-a26d-871b05dd608b\",\"Health\":3,\"IsDead\":false}],\"UserPlayedCards\":[{\"$type\":\"Vue.Domain.Cards.AssassinCard, Vue\",\"Name\":\"Assassin\",\"Row\":0,\"Targets\":0,\"Rarity\":2,\"Speed\":5,\"Damage\":7,\"Healing\":0,\"MaxHealth\":5,\"Id\":\"3cc36bac-34ba-4ad8-a26d-871b05dd608b\",\"Health\":3,\"IsDead\":false}]}";
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            var model = JsonConvert.DeserializeObject<EndOfTurnModel>(json, settings);
            return Json(new HomeModel { decks = new List<string> { "Undead", "Human", "Elf" }});
        }
    }
}