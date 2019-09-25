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
            //INSTANTIATE A QUERYABLE LIST OF BOOKS
            var booksQuery = db.Books.AsQueryable();  

            //"SearchBar" BUSINESS LOGIC
            if (!String.IsNullOrEmpty(search))  //ONLY EXECUTED IF A SEARCH STRING IS PASSED
            {
                return RedirectToAction("Search", new { keyword = search });  //...REDIRECT TO "Found" ACTION IF SO
            }

            //"SortBy" BUSINESS LOGIC
            switch (sortBy) 
            {
                case ("AtoZ"):  //IF THE STRING "AtoZ" IS PASSED...
                    booksQuery = booksQuery.OrderBy(b => b.Title);  //...ORDER ALPHABETICALLY (ASCENDING)
                    break;
                case ("ZtoA"):  //IF THE STRING "ZtoA" IS PASSED...
                    booksQuery = booksQuery.OrderByDescending(b => b.Title); //...ORDER ALPHABEDICALLY (DESCENDING)
                    break;
                case ("lowToHigh"):  //IF THE STRING "lowToHigh" IS PASSED...
                    booksQuery = booksQuery.OrderBy(b => b.Price);  //...ORDER BY PRICE (ASCENDING)
                    break;
                case ("highToLow"): //IF THE STRING "highToLow" IS PASSED...
                    booksQuery = booksQuery.OrderByDescending(b => b.Price); //...ORDER BY PRICE (DESCENDING)
                    break;
            }

            return View(booksQuery.ToList());
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

        public ActionResult ByGenre(string role, string order, string search)
        {
            var thisGenre = db.Genres.Find(role);
            var booksQuery = thisGenre.Books.AsQueryable();

            //Search by book title...
            if (!String.IsNullOrEmpty(search))
            { 
                return RedirectToAction("Search", new { keyword = search, genre = thisGenre.Name });
            }

            //Order by...
            switch (order)
            {
                case ("AtoZ"):
                    booksQuery = booksQuery.OrderBy(b => b.Title);
                    break;
                case ("ZtoA"):
                    booksQuery = booksQuery.OrderByDescending(b => b.Title);
                    break;
            }
            return View(booksQuery.ToList());
        }

        //SEARCH ACTION
        public ActionResult Search(string keyword, string author, string genre)
        {
            var result = new SearchViewModel();  //INSTANTIATE THE RELATED VIEWMODEL
            result.Keyword = keyword;  //SET ITS 'Keyword' PROPERTY ACCORDINGLY

            //SEACH TITLES
            if (!String.IsNullOrEmpty(genre)) //IF A GENRE NAME IS PASSED:
            {
                result.Books = db.Genres.Find(genre).Books.Where(b => b.Title.ToUpper().Contains(keyword.ToUpper())).ToList(); //SET 'Books' PROPERTY ACCORDINGLY
                result.Genre = genre; //SET THE "Genre" PROPERTY ACCORDINGLY
            }
            else if (!String.IsNullOrEmpty(author)) //IF AN AUTHOR NAME IS PASSED:
            {
                result.Books = db.Authors.Find(author).Books.Where(b => b.Title.ToUpper().Contains(keyword.ToUpper())).ToList(); //SET 'Books' PROPERTY ACCORDINGLY
                result.Author = author; //SET THE "Author" PROPERTY ACCORDINGLY
            }
            else //IF NEITHER:
            {
                result.Books = db.Books.Where(b => b.Title.ToUpper().Contains(keyword.ToUpper())).ToList(); //SET 'Books' PROPERTY ACCORDINGLY
            }

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

        public ActionResult ByAuthor(string role, string order, string search)
        {
            var thisAuthor = db.Authors.Find(role);
            var booksQuery = thisAuthor.Books.AsQueryable();

            //Search by book title...
            if (!String.IsNullOrEmpty(search))
            {
                return RedirectToAction("Search", new { keyword = search, author = role });
            }                

            //Order by...
            switch (order)
            {
                case ("AtoZ"):
                    booksQuery = booksQuery.OrderBy(b => b.Title);
                    break;
                case ("ZtoA"):
                    booksQuery = booksQuery.OrderByDescending(b => b.Title);
                    break;
                case ("lowToHigh"):
                    booksQuery = booksQuery.OrderBy(b => b.Price);
                    break;
                case ("highToLow"):
                    booksQuery = booksQuery.OrderByDescending(b => b.Price);
                    break;
            }

            return View(booksQuery.ToList());
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
