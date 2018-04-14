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
    [Route("api/[controller]/[action]")]
    public class SelectionController : Controller
    {
        [HttpGet]
        public JsonResult Decks()
        {
            var model = new { Decks = Dealer.AllDecks };
            return Json(model);
        }

        [HttpGet]
        public JsonResult Champs()
        {
            var model = new { Champs = Dealer.AllChamps };
            return Json(model);
        }
    }
}
