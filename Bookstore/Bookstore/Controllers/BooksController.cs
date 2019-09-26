using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bookstore.Models;
using System.Data.Entity.Migrations;
using System.IO;
using Bookstore.ViewModels;
using PagedList;

namespace Bookstore.Controllers
{
    public class BooksController : Controller
    {
        private BookstoreContext db = new BookstoreContext();

        // GET: Books
        public ActionResult Index(string search, string sortBy, int page = 1)
        {
            var cardsList = BooksIndexViewModel.CardsList(db.Books); //Make Smaller 'BookCardViewModel' Objects from a List of Cards

            //SEARCH
            if (!String.IsNullOrEmpty(search))  //Only Executed if A Search String is Passed
            {
                return RedirectToAction("Search", new { keyword = search });  //...REDIRECT TO "Found" ACTION IF SO
            }

            //SORTBY
            if (!String.IsNullOrEmpty(sortBy)) //Only Executed if A SortBy String is Passed
            {
                cardsList = BooksIndexViewModel.SortCards(cardsList, sortBy); //...Call the 'SortCards' Static Method
            }

            //PAGING
            if (cardsList.Count > 24) //If cardsList have more than 24 members...
            {
                return View("IndexPaged", cardsList.ToPagedList(page, 24)); //...return 'IndexPaged' view with a PagedList<HomeIndexViewModel>
            }
            else //If not...
            {
                return View(cardsList); //...return 'Index' view with a List<HomeIndexViewModel>
            }
        }

        public ActionResult ByISBN(string role)
        {
            Book book = db.Books.Find(role);
            return View(book);
        }

        public ActionResult ByGenre(string genreName, string sortBy, string searchKeyword)
        {
            var thisGenreBookCards = BooksIndexViewModel.CardsList(db.Genres.Find(genreName).Books);

            //Search by book title...
            if (!String.IsNullOrEmpty(searchKeyword))
            {
                return RedirectToAction("Search", new { keyword = searchKeyword, genre = genreName });
            }

            if (!String.IsNullOrEmpty(sortBy))
            {
                thisGenreBookCards = BooksIndexViewModel.SortCards(thisGenreBookCards, sortBy);
            }

            var byGenreInstance = new ByGenreViewModel { BookCards = thisGenreBookCards, Genre = genreName };

            return View(byGenreInstance);
        }

        public ActionResult Search(string keyword, string author, string genre, int page = 1)
        {
            var result = SearchViewModel.SearchTitles(author, genre, keyword);
            
            if (result.Books.Any()) //If any books found...
            {
                if (result.MustBePaged()) //...and if they must be paged (more than 24)...
                {
                    result.BooksPaged = result.Books.ToPagedList(page, 24); //...set the BooksPaged property as an IPagedList<Book>,
                    return View("FoundPaged", result); //return 'FoundPaged' view with a PagedList<Book>;
                }
                else //If less than 24 books found...
                {
                    return View("Found", result); //...return 'Found' view;
                }
            }
            else //If no books found...
            {
                return View("NotFound", result);  //...return 'NotFound' view;
            }
        }

        public ActionResult ByAuthor(string authorName, string order, string search)
        {
            var thisAuthorBookCards = BooksIndexViewModel.CardsList(db.Authors.Find(authorName).Books);

            //Search by book title...
            if (!String.IsNullOrEmpty(search))
            {
                return RedirectToAction("Search", new { keyword = search, author = authorName });
            }

            if (!String.IsNullOrEmpty(order))
            {
                thisAuthorBookCards = BooksIndexViewModel.SortCards(thisAuthorBookCards, order);
            }

            var byAuthorInstance = new ByAuthorViewModel { BookCards = thisAuthorBookCards, Author = authorName };

            return View(byAuthorInstance);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
