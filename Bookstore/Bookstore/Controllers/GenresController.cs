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

namespace Bookstore.Controllers
{
    public class GenresController : Controller
    {
        private BookstoreContext db = new BookstoreContext();

        // GET: Genres
        public async Task<ActionResult> Index(string search, string order)
        {
            var genreQuery = db.Genres.AsQueryable();

            if (!String.IsNullOrEmpty(search))
                genreQuery = genreQuery.Where(g => g.Name.Contains(search));

            switch (order)
            {
                case "AtoZ":
                    genreQuery = genreQuery.OrderBy(g => g.Name);
                    break;
                case "ZtoA":
                    genreQuery = genreQuery.OrderByDescending(g => g.Name);
                    break;
            }

            return View(await genreQuery.ToListAsync());
        }
    }
}
