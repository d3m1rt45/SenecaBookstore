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

namespace Bookstore.Controllers
{
    public class BooksController : Controller
    {
        private BookstoreContext db = new BookstoreContext();

        // GET: Books
        public ActionResult Index(string search, string sortBy)
        {
            var cardsList = AllTitlesViewModel.CardsList(db.Books); //Make Smaller 'BookCardViewModel' Objects from a List of Cards

            //SEARCH
            if (!String.IsNullOrEmpty(search))  //Only Executed if A Search String is Passed
            {
                return RedirectToAction("Search", new { keyword = search });  //...REDIRECT TO "Found" ACTION IF SO
            }

            //SORTBY
            if (!String.IsNullOrEmpty(sortBy)) //Only Executed if A SortBy String is Passed
            {
                cardsList = AllTitlesViewModel.SortCards(cardsList, sortBy); //...Call the 'SortCards' Static Method
            }

            return View(cardsList);
        }

        // GET: Books/Details/5
        public ActionResult Details(string id)
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

        // GET: Books/Edit/5
        public ActionResult Edit(string id)
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

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                Book duplicate = db.Books.Find(book.ISBN);

                Author prevAuthor = duplicate.Author;
                Genre prevGenre = duplicate.Genre;


                db.Books.Remove(duplicate);

                if (prevAuthor.Books.Count() < 1)
                    db.Authors.Remove(prevAuthor);

                if (prevGenre.Books.Count() < 1)
                    db.Genres.Remove(prevGenre);

                db.SaveChanges();

                if (book.ImageFile.FileName != null)
                {
                    string imageFileName = Path.Combine(Server.MapPath("~/Images"), duplicate.ImagePath);
                    System.IO.File.Delete(imageFileName);

                    string extension = Path.GetExtension(book.ImageFile.FileName);
                    imageFileName = book.ISBN + extension;
                    book.ImagePath = imageFileName;
                    imageFileName = Path.Combine(Server.MapPath("~/Images"), imageFileName);
                    book.ImageFile.SaveAs(imageFileName);
                }

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

                if (!author.Books.Contains(book))
                    author.Books.Add(book);

                if (!genre.Books.Contains(book))
                    genre.Books.Add(book);

                book.Genre = genre;
                book.Author = author;

                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        public ActionResult ByISBN(string role)
        {
            Book book = db.Books.Find(role);
            return View(book);
        }

        public ActionResult ByGenre(string genreName, string sortBy, string searchKeyword)
        {
            var thisGenreBookCards = AllTitlesViewModel.CardsList(db.Genres.Find(genreName).Books);

            //Search by book title...
            if (!String.IsNullOrEmpty(searchKeyword))
            {
                return RedirectToAction("Search", new { keyword = searchKeyword, genre = genreName });
            }

            if (!String.IsNullOrEmpty(sortBy))
            {
                thisGenreBookCards = AllTitlesViewModel.SortCards(thisGenreBookCards, sortBy);
            }

            var byGenreInstance = new ByGenreViewModel { BookCards = thisGenreBookCards, Genre = genreName };

            return View(byGenreInstance);
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

        public ActionResult ByAuthor(string authorName, string order, string search)
        {
            var thisAuthorBookCards = AllTitlesViewModel.CardsList(db.Authors.Find(authorName).Books);

            //Search by book title...
            if (!String.IsNullOrEmpty(search))
            {
                return RedirectToAction("Search", new { keyword = search, author = authorName });
            }

            if (!String.IsNullOrEmpty(order))
            {
                thisAuthorBookCards = AllTitlesViewModel.SortCards(thisAuthorBookCards, order);
            }

            var byAuthorInstance = new ByAuthorViewModel { BookCards = thisAuthorBookCards, Author = authorName };

            return View(byAuthorInstance);
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
