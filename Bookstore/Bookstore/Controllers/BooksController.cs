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
            var cardsList = Book.ToVMList(db.Books);

            //Search:
            if (!String.IsNullOrEmpty(search))
                return RedirectToAction("Search", new { keyword = search });

            //SortBy:
            if (!String.IsNullOrEmpty(sortBy))
                cardsList = Book.SortVMList(cardsList, sortBy);

            //Paging:
            if (cardsList.Count > 24)
                return View("IndexPaged", cardsList.ToPagedList(page, 24));
            else
                return View(cardsList);
        }

        // Return a single Book Page
        public ActionResult ByISBN(string isbn) { return View(db.Books.Find(isbn)); }

        // Return Books from a specific genre
        public ActionResult ByGenre(string genreName, string sortBy, string searchKeyword)
        {
            var bookCards = Book.ToVMList(db.Genres.Find(genreName).Books);

            //Search:
            if (!String.IsNullOrEmpty(searchKeyword))
                return RedirectToAction("Search", new { keyword = searchKeyword, genre = genreName });

            //SortBy:
            if (!String.IsNullOrEmpty(sortBy))
                bookCards = Book.SortVMList(bookCards, sortBy);

            var byGenreInstance = new GenreViewModel
            {
                ImageClass = genreName.ToLower().Substring(0, 5),
                Name = genreName,
                BookCards = bookCards
            };

            return View(byGenreInstance);
        }

        // Return Books from a specific Author
        public ActionResult ByAuthor(string authorName, string order, string search) 
        {
            var bookCards = Book.ToVMList( db.Authors.Find(authorName).Books);

            //Search:
            if (!String.IsNullOrEmpty(search))
                return RedirectToAction("Search", new { keyword = search, author = authorName });

            //SortBy
            if (!String.IsNullOrEmpty(order))
                bookCards = Book.SortVMList(bookCards, order);

            var byAuthorVM = new ByAuthorViewModel { BookCards = bookCards, Author = authorName };

            return View(byAuthorVM);
        }

        // Run the methods for search
        public ActionResult Search(string keyword, string author, string genre, int page = 1)
        {
            // Search:
            SearchViewModel result = Book.Search(db, author, genre, keyword);
            result.Author = author;
            result.Genre = genre;

            // Paging:
            if (result.Books.Count > 24)
                result.BooksPaged = result.Books.ToPagedList(page, 24);

            return View("Search", result);
        }
    }
}
