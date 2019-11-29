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
        private BookstoreContext db = new BookstoreContext(); //DataAccess

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

        // Return a Book Page
        public ActionResult ByISBN(string isbn) { return View(db.Books.Find(isbn)); }

        // Get Books from a specific genre
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

        // Get Books from a specific Author
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


        public ActionResult Search(string keyword, string author, string genre, int page = 1) //Titles as search results:
        {
            var result = SearchViewModel.SearchTitles(db, author, genre, keyword);
            result.Author = author;
            result.Genre = genre;

            if (result.MustBePaged()) //...and if they must be paged (more than 24)...
            {
                result.BooksPaged = result.Books.ToPagedList(page, 24); //...set the BooksPaged property as an IPagedList<Book>;
            }

            return View("Search", result); //Return 'FoundPaged' view with the result;
           
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book)
        {
            bool isDuplicate = db.Books.Any(x => x.ISBN == book.ISBN);
            if (isDuplicate)
            {
                ViewBag.Message = "ISBN already exists in the database.";
                return View(book);
            }

            if (ModelState.IsValid)
            {
                string extension = Path.GetExtension(book.ImageFile.FileName);
                string imageFileName = book.ISBN + extension;
                book.ImagePath = imageFileName;
                imageFileName = Path.Combine(Server.MapPath("~/Images"), imageFileName);
                book.ImageFile.SaveAs(imageFileName);

                Author author = db.Authors.Find(book.AuthorName);
                Genre genre = db.Genres.Find(book.GenreName);

                if (author == null)
                    author = new Author() { Name = book.AuthorName };

                if (genre == null)
                    genre = new Genre() { Name = book.GenreName };

                if (author.Books == null)
                    author.Books = new List<Book>();

                if (genre.Books == null)
                    genre.Books = new List<Book>();

                book.Genre = genre;
                book.Author = author;

                author.Books.Add(book);
                genre.Books.Add(book);

                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
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
