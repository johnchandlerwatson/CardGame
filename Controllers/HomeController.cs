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
            return Json(new HomeModel { decks = new List<string> { "Undead", "Human", "Elf" }});
        }
    }
}