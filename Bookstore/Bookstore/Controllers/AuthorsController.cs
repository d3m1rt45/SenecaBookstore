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
    public class AuthorsController : Controller
    {
        private BookstoreContext db = new BookstoreContext();

        // GET: Authors
        public async Task<ActionResult> Index(string order, string searchKeyword)
        {
            var authorQuery = db.Authors.AsQueryable();

            if (!String.IsNullOrEmpty(searchKeyword))
            {
                return RedirectToAction("Search", new { search = searchKeyword });
            }

            switch (order)
            {
                case "AtoZ":
                    authorQuery = authorQuery.OrderBy(a => a.Name);
                    break;
                case "ZtoA":
                    authorQuery = authorQuery.OrderByDescending(a => a.Name);
                    break;
            }

            return View(await authorQuery.ToListAsync());
        }

        public ActionResult Search(string search)
        {
            var result = db.Authors.Where(x => x.Name.ToUpper().Contains(search.ToUpper()));
            return View(result.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
