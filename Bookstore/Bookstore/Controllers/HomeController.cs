using Bookstore.ExtensionMethods;
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

        // Populate a new HomeIndexViewModel instance to Home/Index view
        public ActionResult Index(string searchKeyword)
        {
            // Search:
            if (!String.IsNullOrEmpty(searchKeyword))
                return RedirectToAction("Search", "Books", new { keyword = searchKeyword });

            var homeIndexVM = new HomeIndexViewModel();
            homeIndexVM.FullSetUp(db);

            return View(homeIndexVM);
        }
    }
}