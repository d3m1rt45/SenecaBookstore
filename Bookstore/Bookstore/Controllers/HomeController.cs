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
        private BookstoreContext db = new BookstoreContext(); //Data Access

        public ActionResult Index(string searchKeyword)
        {
            var homeIndexInstance = new HomeIndexViewModel();

            homeIndexInstance.SetFeatured(); //Set four featured books;

            var genres = db.Genres.Where(x => x.Books.Count > 5); //Make a List of every genre that has 6 or more books;


            foreach (var g in genres) //For each genre in the genres List...
            {
                homeIndexInstance.AddSection(g.Name); //...add a new section to the 'Sections' property;
            }

            if (homeIndexInstance.Sections.Count > 6) //If there are more than 6 genres in the Sections List...
            {
                homeIndexInstance.Sections = homeIndexInstance.Sections.Take(6).ToList(); //...take only the first 6;
            }

            homeIndexInstance.SetOtherGenres(); //Set the 'OtherGenres' property as genres that are not included in the 'Sections' property;

            //Search:
            if (!String.IsNullOrEmpty(searchKeyword)) //If a search string is passed...
            {
                return RedirectToAction("Search", "Books", new { keyword = searchKeyword }); //...pass it to the 'Search' View;
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