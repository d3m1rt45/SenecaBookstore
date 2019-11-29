using Bookstore.ExtensionMethods;
using Bookstore.Models;
using Bookstore.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Bookstore.Controllers
{
    public class BooksController : Controller
    {
        // EF Database Access Property
        private readonly BookstoreContext db = new BookstoreContext();

        // If there is a search, return results. If not, return all books
        public ActionResult Index(string search, string sortBy, int page = 1)
        {
            var bookVMList = db.Books.ToVMList();

            //Search:
            if (!String.IsNullOrEmpty(search))
                return RedirectToAction("Search", new { keyword = search });

            //SortBy:
            if (!String.IsNullOrEmpty(sortBy))
                bookVMList = bookVMList.Sort(sortBy);

            //Paging:
            if (bookVMList.Count > 24)
                return View("IndexPaged", bookVMList.ToPagedList(page, 24));
            else
                return View(bookVMList);
        }

        // Return a single Book Page
        public ActionResult ByISBN(string isbn) { return View(db.Books.Find(isbn)); }

        // Return Books from a specific genre
        public ActionResult ByGenre(string genreName, string sortBy, string searchKeyword)
        {
            var bookVMList = db.Genres.Find(genreName).Books.ToVMList();

            //Search:
            if (!String.IsNullOrEmpty(searchKeyword))
                return RedirectToAction("Search", new { keyword = searchKeyword, genre = genreName });

            //SortBy:
            if (!String.IsNullOrEmpty(sortBy))
                bookVMList = bookVMList.Sort(sortBy);

            var genreVM = new GenreViewModel
            {
                ImageClass = genreName.ToLower().Substring(0, 5),
                Name = genreName,
                BookCards = bookVMList
            };

            return View(genreVM);
        }

        // Return Books from a specific Author
        public ActionResult ByAuthor(string authorName, string order, string search) 
        {
            var bookVMList = db.Authors.Find(authorName).Books.ToVMList();

            //Search:
            if (!String.IsNullOrEmpty(search))
                return RedirectToAction("Search", new { keyword = search, author = authorName });

            //SortBy
            if (!String.IsNullOrEmpty(order))
                bookVMList = bookVMList.Sort(order);

            var byAuthorVM = new ByAuthorViewModel { BookCards = bookVMList, Author = authorName };

            return View(byAuthorVM);
        }

        // Run the methods for search
        public ActionResult Search(string keyword, string author, string genre, int page = 1)
        {
            // Search:
            SearchViewModel result = db.SearchForBooks(author, genre, keyword);
            result.Author = author;
            result.Genre = genre;

            // Paging:
            if (result.Books.Count > 24)
                result.BooksPaged = result.Books.ToPagedList(page, 24);

            return View("Search", result);
        }
    }
}
