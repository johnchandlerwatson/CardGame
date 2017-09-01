using System;
using Microsoft.AspNetCore.Mvc;

namespace Vue.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}