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
        public ActionResult Index(string search, string sortBy, int page = 1) //All Titles:
        {
<<<<<<< HEAD
            var cardsList = BooksIndexViewModel.CardsList(db.Books); //Make a List<BooksIndexViewModel> Object Populated from db.Books
=======
            var cardsList = BooksIndexViewModel.CardsList(db.Books); //Map a List of Book objects to a List of BookIndexViewModel objects
>>>>>>> PartialViews

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

        public ActionResult ByISBN(string isbn) //A Single Title:
        {
            Book book = db.Books.Find(isbn); //Find Book object by the isbn parameter, and
            return View(book); //pass it to the View;
        }

        public ActionResult ByGenre(string genreName, string sortBy, string searchKeyword) //Titles of One Specific Genre:
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

            //Instantiate a ByGenreViewModel object with 'bookCards' and 'genreName';
            var byGenreObject = new ByGenreViewModel { BookCards = bookCards, Genre = genreName };

            return View(byGenreObject);
        }
        public ActionResult ByAuthor(string authorName, string order, string search)
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
                else //If not...
                {
                    return View("Found", result); //...return 'Found' view;
                }
            }
            else //If not...
            {
                return View("NotFound", result);  //...return 'NotFound' view;
            }
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

<<<<<<< HEAD
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

        //SEARCH ACTION
        public ActionResult Search(string keyword, string author, string genre)
        {
            var result = SearchViewModel.SearchTitles(author, genre, keyword);

            //ANY FOUND?
            if (result.Books.Any()) //IF YES:
            {
                return View("Found", result); //RETURN 'Found' VIEW
            }
            else //IF NO:
            {
                return View("NotFound", result);  //RETURN 'NotFound' VIEW
            }
        }

        // GET: Books/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Book book = db.Books.Find(id);
            Author author = book.Author;
            Genre genre = book.Genre;

            //string imageFileName = Path.Combine(Server.MapPath("~/Images"), book.ImagePath);
            //System.IO.File.Delete(imageFileName);

            db.Books.Remove(book);

            if (author.Books.Count() < 1)
                db.Authors.Remove(author);

            if (genre.Books.Count() < 1)
                db.Genres.Remove(genre);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

=======
>>>>>>> PartialViews
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
