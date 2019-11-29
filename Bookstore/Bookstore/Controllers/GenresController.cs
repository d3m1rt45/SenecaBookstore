using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bookstore.Models;
using Bookstore.ViewModels;

namespace Bookstore.Controllers
{
    public class GenresController : Controller
    {
        // EF Database Access Property
        private readonly BookstoreContext db = new BookstoreContext();

        // Get all genres.
        // If there is search or sorting selected, apply them.
        // Return result.
        public ActionResult Index(string search, string order)
        {
            var genreQuery = db.Genres.AsQueryable();
            var genreVMList = new List<GenreViewModel>();

            // Search:
            if (!String.IsNullOrEmpty(search))
                genreQuery = genreQuery.Where(g => g.Name.Contains(search));

            // Sorting:
            if (order == "AtoZ")
                genreQuery = genreQuery.OrderBy(g => g.Name);
            else if (order == "ZtoA")
                genreQuery = genreQuery.OrderByDescending(g => g.Name);

            //For each remaining item in the allGenres object..
            foreach (var gen in genreQuery)
                genreVMList.Add(new GenreViewModel { Name = gen.Name, ImageClass = gen.Name.ToLower().Substring(0, 5) });

            return View(genreVMList);
        }
    }
}
