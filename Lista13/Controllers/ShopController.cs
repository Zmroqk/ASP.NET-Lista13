using Lista13.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lista13.Controllers
{
    public class ShopController : Controller
    {
        Lab13Context _context;

        public ShopController(Lab13Context context)
        {
            _context = context;
        }
        // GET: ShopController
        public ActionResult Index(int? id)
        {
            if(TempData["AddItemMessage"] != null)
                ViewBag.CartMessage = TempData["AddItemMessage"].ToString();
            ViewBag.Categories = _context.Category.ToList();
            ViewBag.CategoryId = id;
            if(id == null)
                return View(_context.Article.Take(5).ToList());
            else
            {
                return View(_context.Article.Where((ar) => ar.CategoryId == id).Take(5).ToList());
            }
        }

        public ActionResult Refresh()
        {
            return Redirect("Index");
        }
    }
}
