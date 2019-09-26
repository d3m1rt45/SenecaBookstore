using Bookstore.Models;
using Bookstore.ViewModels;
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

        public ActionResult Index(string searchKeyword)
        {
            var homeIndexInstance = new HomeIndexViewModel();

            homeIndexInstance.SetFeatured();
            homeIndexInstance.AddSection("Philosophy");
            homeIndexInstance.AddSection("Business, Finance and Law");
            homeIndexInstance.AddSection("Crime and Mystery");
            homeIndexInstance.AddSection("Psychology");
            homeIndexInstance.SetOtherGenres();

            if (!String.IsNullOrEmpty(searchKeyword))
            {
                return RedirectToAction("Search", "Books", new { keyword = searchKeyword });
            }
            else
            {
                return View(homeIndexInstance);
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