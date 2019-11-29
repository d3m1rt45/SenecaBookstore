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
            if (!String.IsNullOrEmpty(searchKeyword))
                return RedirectToAction("Search", "Books", new { keyword = searchKeyword });

            var homeIndexVM = new HomeIndexViewModel();
            Book.SetFeaturedForVM(db, homeIndexVM);

            // Add a maximum of 6 sections, one for each genre that contains 6 or more books
            foreach (var g in db.Genres.Where(x => x.Books.Count >= 6).Take(6))
                homeIndexVM.AddSection(g.Name);

            homeIndexVM.SetOtherGenres();
            return View(homeIndexVM);
        }
    }
}