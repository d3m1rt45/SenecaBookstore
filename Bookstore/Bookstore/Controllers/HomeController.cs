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
        private readonly BookstoreContext db = new BookstoreContext();

        // Pass a new HomeIndexViewModel instance to Home/Index view
        public ActionResult Index(string searchKeyword)
        {
            // Search:
            if (!String.IsNullOrEmpty(searchKeyword))
                return RedirectToAction("Search", "Books", new { keyword = searchKeyword });

            var homeIndexVM = new HomeIndexViewModel();
            Book.SetFeaturedBooksFor(homeIndexVM, db);
            Genre.SetSectionsFor(homeIndexVM, db);
            Genre.SetOtherGenresFor(homeIndexVM, db);

            return View(homeIndexVM);
        }
    }
}