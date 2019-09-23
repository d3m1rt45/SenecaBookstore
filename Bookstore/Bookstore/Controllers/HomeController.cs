using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bookstore.Controllers
{
    public class HomeController : Controller
    {
        private BookstoreContext db = new BookstoreContext();

        public ActionResult Index(string searching)
        {
            if (!String.IsNullOrEmpty(searching))
            {
                return RedirectToAction("Index", "Books", new { order = "", search = searching });
            }
            else
            {
                return View(db.Books);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}