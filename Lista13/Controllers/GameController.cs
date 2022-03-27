using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Lista13.Controllers
{
    [Route("")]
    public class GameController : Controller
    {
        static string N_TAG = "n";
        static string VALUE_TAG = "value";
        static string COUNTER_TAG = "counter";
        static string TRIES_TAG = "tries";
        public GameController(){}

        private void GenerateValue()
        {
            Random rnd = new Random();
            int? n = HttpContext.Session.GetInt32(N_TAG);
            if(n == null)
            {
                n = 50;
            }
            HttpContext.Session.SetInt32(VALUE_TAG, rnd.Next((int)n));
        }

        [HttpGet("Set,{new_n}")]
        public IActionResult Set(int new_n)
        {
            HttpContext.Session.SetInt32(N_TAG, new_n);
            return View();
        }

        [HttpGet("Draw")]
        public IActionResult Draw()
        {
            HttpContext.Session.SetInt32(TRIES_TAG, 0);
            GenerateValue();
            return View();
        }

        [HttpGet("Guess,{p_n}")]
        public IActionResult Guess(int p_n)
        {
            int? tries = HttpContext.Session.GetInt32(TRIES_TAG);
            int? value = HttpContext.Session.GetInt32(VALUE_TAG);
            if(tries == null)
                tries = 0;
            if (value == null)
                value = 0;
            ViewData["tries"] = ++tries;
            HttpContext.Session.SetInt32(TRIES_TAG, (int)tries);
            if (value == p_n)
            {
                ViewData["message"] = "Udało ci się!";
                ViewData["class"] = "x-results";                
            }
            else if(value > p_n)
            {
                ViewData["message"] = "Za niska wartość :(";
                ViewData["class"] = "no-result";
            }
            else
            {
                ViewData["message"] = "Za duża wartość :(";
                ViewData["class"] = "no-result";
            }
            return View();
        }
    }
}
