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
        private BookstoreContext db = new BookstoreContext(); //DataAccess

        // GET: Books
        public ActionResult Index(string search, string sortBy, int page = 1) //All titles:
        {
            var cardsList = BooksIndexViewModel.CardsList(db.Books); //Map a List of Book objects to a List of BookIndexViewModel objects

            //Search:
            if (!String.IsNullOrEmpty(search)) //If a search string is passed...
            {
                return RedirectToAction("Search", new { keyword = search });  //...pass the search string to the Search action;
            }

            //SortBy:
            if (!String.IsNullOrEmpty(sortBy)) //If a sortBy string is passed...
            {
                cardsList = BooksIndexViewModel.SortCards(cardsList, sortBy); //...sort cards accordingly;
            }

            //Paging:
            if (cardsList.Count > 24) //If cardsList have more than 24 members...
            {
                return View("IndexPaged", cardsList.ToPagedList(page, 24)); //...return 'IndexPaged' view with a PagedList<HomeIndexViewModel>;
            }
            else //If not...
            {
                return View(cardsList); //...return 'Index' view with a List<HomeIndexViewModel>;
            }
        }


        public ActionResult ByISBN(string isbn) //A single title:
        {
            var book = db.Books.Find(isbn); //Find Book object by the isbn parameter, and

            return View(book); //pass it to the View;
        }


        public ActionResult ByGenre(string genreName, string sortBy, string searchKeyword) //Titles of one specific genre:
        {
            var bookCards = BooksIndexViewModel.CardsList( //Find the Titles of a genre by the genreName parameter, and
                db.Genres.Find(genreName).Books); //map them into a List of BooksIndexViewModel objects named bookCards; 

            //Search:
            if (!String.IsNullOrEmpty(searchKeyword)) //If a search string is passed...
            {
                return RedirectToAction("Search", new { keyword = searchKeyword, genre = genreName }); //...pass the search string and genreName to the Search action;
            }

            //SortBy:
            if (!String.IsNullOrEmpty(sortBy)) //If a sortBy string is passed...
            {
                bookCards = BooksIndexViewModel.SortCards(bookCards, sortBy); //...sort bookCards accordingly;
            }

            var byGenreInstance = new GenreViewModel
            {
                ImageClass = genreName.ToLower().Substring(0, 5),
                Name = genreName,
                BookCards = bookCards
            };

            return View(byGenreInstance);
        }


        public ActionResult ByAuthor(string authorName, string order, string search) //Titles of one specific author:
        {
            var bookCards = BooksIndexViewModel.CardsList( //Find the Titles of an author by the authorName parameter, and
                db.Authors.Find(authorName).Books); //map them into a List of BooksIndexViewModel objects named bookCards; 

            //Search:
            if (!String.IsNullOrEmpty(search))
            {
                return RedirectToAction("Search", new { keyword = search, author = authorName });
            }

            //SortBy
            if (!String.IsNullOrEmpty(order)) //If an sortBy string is passed...
            {
                bookCards = BooksIndexViewModel.SortCards(bookCards, order); //...sort bookCards accordingly;
            }

            //Instantiate a ByGenreViewModel object with 'bookCards' and 'authorName';
            var byAuthorObject = new ByAuthorViewModel { BookCards = bookCards, Author = authorName };

            return View(byAuthorObject);
        }


        public ActionResult Search(string keyword, string author, string genre, int page = 1) //Titles as search results:
        {
            var result = SearchViewModel.SearchTitles(author, genre, keyword); //Instantiate a SearchViewModel object;
            result.Author = author; //Set its 'Author' property by the 'author' parameter;
            result.Genre = genre; //Set its 'Genre' property by the 'genre' parameter;

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
