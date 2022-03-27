using Lista13.Data;
using Lista13.Models;
using Lista13.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Lista13.Controllers
{
    public class CartController : Controller
    {
        Lab13Context _context;

        public CartController(Lab13Context context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            string role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role == "Admin")
            {
                return true;
            }
            return false;
        }

        public IActionResult Index()
        {
            if (IsAdmin())
            {
                return Unauthorized();
            }
            
            ViewBag.Price = 0;
            if (Request.Cookies.Count == 0)
            {
                return View(new List<(Article article, int count)>());
            }
            ViewBag.Categories = _context.Category.ToList();
            var ids = Request.Cookies.Select(c => c.Key).ToList();
            var articles = _context.Article.Where((ar) => ids.Contains(ar.Id.ToString())).ToList();
            var articlesTuple = articles
                .Select(ar => (
                    article: ar,
                    count: int.Parse(Request.Cookies.Where(c => c.Key == ar.Id.ToString()).First().Value)
                )).ToList();
            articlesTuple.ForEach(ar => ViewBag.Price += ar.article.Price * ar.count);
            return View(articlesTuple);
        }

        public IActionResult AddItem(int? id)
        {
            if (IsAdmin())
            {
                return Unauthorized();
            }
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(7);
            var cookie = Request.Cookies.Where((cookie) => cookie.Key == id.ToString()).FirstOrDefault();
            if (!cookie.Equals(default(KeyValuePair<string, string>)))
                Response.Cookies.Append(cookie.Key, (int.Parse(cookie.Value) + 1).ToString(), cookieOptions);
            else 
                Response.Cookies.Append(id.ToString(), "1", cookieOptions);
            TempData["AddItemMessage"] = "Added product";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult RemoveItem(int? id)
        {
            if (IsAdmin())
            {
                return Unauthorized();
            }
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(7);
            var cookie = Request.Cookies.Where((cookie) => cookie.Key == id.ToString()).FirstOrDefault();
            if (!cookie.Equals(default(KeyValuePair<string, string>)) && int.Parse(cookie.Value) - 1 > 0)
                Response.Cookies.Append(cookie.Key, (int.Parse(cookie.Value) - 1).ToString(), cookieOptions);
            else if(!cookie.Equals(default(KeyValuePair<string, string>)))
                Response.Cookies.Delete(cookie.Key);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [Authorize]
        public IActionResult Order()
        {
            ViewBag.Price = 0;
            if (Request.Cookies.Count == 0)
            {
                return View(new List<(Article article, int count)>());
            }
            ViewBag.Categories = _context.Category.ToList();
            var ids = Request.Cookies.Select(c => c.Key).ToList();
            var articles = _context.Article.Where((ar) => ids.Contains(ar.Id.ToString())).ToList();
            var articlesTuple = articles
                .Select(ar => (
                    article: ar,
                    count: int.Parse(Request.Cookies.Where(c => c.Key == ar.Id.ToString()).First().Value)
                )).ToList();
            articlesTuple.ForEach(ar => ViewBag.Price += ar.article.Price * ar.count);
            ViewBag.PaymentMethods = new List<string>() { "Blik", "Przelew", "Karta płatnicza" }
                .Select(elem => new SelectListItem(elem, elem));
            OrderViewModel viewModel = new OrderViewModel();
            viewModel.Articles = articlesTuple;
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmOrder([Bind("Name,Surname,Address,PaymentMethod")]OrderCredentials credentials)
        {
            TempData["CredentialsName"] = credentials.Name;
            TempData["CredentialsSurname"] = credentials.Surname;
            TempData["CredentialsAddress"] = credentials.Address;
            TempData["CredentialsPaymentMethod"] = credentials.PaymentMethod;
            Request.Cookies.Select(c => c.Key).ToList().ForEach(key => {
                if (int.TryParse(key, out _))
                    Response.Cookies.Delete(key);
                }
            );
            return RedirectToAction("OrderConfirmed");
        }

        [Authorize]
        public IActionResult OrderConfirmed()
        {
            OrderCredentials credentials = new OrderCredentials()
            {
                Name = TempData["CredentialsName"].ToString(),
                Surname = TempData["CredentialsSurname"].ToString(),
                Address = TempData["CredentialsAddress"].ToString(),
                PaymentMethod = TempData["CredentialsPaymentMethod"].ToString()
            };
            return View(credentials);
        }
    }
}
