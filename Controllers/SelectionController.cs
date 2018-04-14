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
    public class SelectionController : Controller
    {
        [HttpGet]
        public JsonResult Decks()
        {
            var model = new { Decks = Dealer.AllDecks };
            return Json(model);
        }
    }
}
