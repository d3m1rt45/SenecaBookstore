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

        public async Task<ActionResult> Index(string order, string search)
        {
            var authorQuery = db.Authors.AsQueryable();

            if (!String.IsNullOrEmpty(search))
                authorQuery = authorQuery.Where(a => a.Name.Contains(search));

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
    }
}
