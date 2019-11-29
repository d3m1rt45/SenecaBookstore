using Bookstore.Models;
using Bookstore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookstore.ExtensionMethods
{
    public static class BookstoreContextExtensionMethods
    {
        // Search Books
        public static SearchViewModel SearchForBooks(this BookstoreContext db, string author, string genre, string keyword)
        {
            var result = new SearchViewModel { Keyword = keyword };

            IEnumerable<Book> books;

            if (!String.IsNullOrEmpty(genre))
            {
                books = db.Genres.Find(genre).Books.Where(b => b.Title.ToUpper().Contains(keyword.ToUpper()));
                result.Genre = genre;
            }
            else if (!String.IsNullOrEmpty(author))
            {
                books = db.Authors.Find(author).Books.Where(b => b.Title.ToUpper().Contains(keyword.ToUpper()));
                result.Author = author;
            }
            else
                books = db.Books.Where(b => b.Title.ToUpper().Contains(keyword.ToUpper()));

            result.Books = books.ToVMList();

            return result;
        }
    }
    
    public static class HomeIndexViewModelExtensionMethods
    {
        // Fully sets up the HomeIndexViewModel by calling its extension methods defined below
        public static void FullSetUp(this HomeIndexViewModel homeIndexVM, BookstoreContext db)
        {
            homeIndexVM.SetFeaturedBooks(db);
            homeIndexVM.SetSections(db);
            homeIndexVM.SetOtherGenres(db);
        }

        // Sets 4 random books as the featured properth of the HomeIndexViewModel object
        public static void SetFeaturedBooks(this HomeIndexViewModel homeIndexVM, BookstoreContext db)
        {
            foreach (var book in db.Books.Take(4).ToList())
                homeIndexVM.Featured.Add(new FeaturedViewModel { ISBN = book.ISBN, ImagePath = book.ImagePath });
        }

        // Populate HomeIndexViewModel's Sections with up to 6 sections from the database that has 6 or more books
        public static void SetSections(this HomeIndexViewModel homeIndexVM, BookstoreContext db)
        {
            foreach (var genre in db.Genres.Where(x => x.Books.Count >= 6).Take(6))
            {
                var bookVMList = genre.Books.ToVMList();

                SectionViewModel sectionVM = new SectionViewModel
                {
                    Title = genre.Name,
                    BookCards = bookVMList,
                    ImageClass = genre.Name.Substring(0, 5).ToLower()
                };

                homeIndexVM.Sections.Add(sectionVM);
            }
        }

        // Populate HomeIndexViewModel's OtherGenres property with genres that has 5 or less books
        public static void SetOtherGenres(this HomeIndexViewModel homeIndexVM, BookstoreContext db)
        {
            var filteredGenres = db.Genres.ToList();

            //Filtering
            if (homeIndexVM.Sections != null)
            {
                foreach (var sect in homeIndexVM.Sections)
                    filteredGenres.Remove(filteredGenres.Find(x => x.Name == sect.Title));
            }

            //For each remaining genre after filtering
            foreach (var genre in filteredGenres)
            {
                homeIndexVM.OtherGenres.Add(new GenreViewModel
                {
                    Name = genre.Name,
                    ImageClass = genre.Name.ToLower().Substring(0, 5)
                });
            }
        }
    }

    public static class BookViewModelExtensionMethods
    {
        // Sort a List of BooksIndexViewModels
        public static List<BookViewModel> Sort(this List<BookViewModel> cardsList, string sortBy) 
        {
            var cardsQuery = cardsList.AsQueryable();

            switch (sortBy)
            {
                case ("AtoZ"):
                    cardsQuery = cardsQuery.OrderBy(b => b.Title);
                    break;
                case ("ZtoA"):
                    cardsQuery = cardsQuery.OrderByDescending(b => b.Title);
                    break;
                case ("lowToHigh"):
                    cardsQuery = cardsQuery.OrderBy(b => b.Price);
                    break;
                case ("highToLow"):
                    cardsQuery = cardsQuery.OrderByDescending(b => b.Price);
                    break;
            }

            return cardsQuery.ToList();
        }

        // Take a 'DBSet<Book>' Object and Turn It Into 'List<BookCardViewModel>'s
        public static List<BookViewModel> ToVMList(this IEnumerable<Book> bookSet)
        {
            var bookCards = new List<BookViewModel>();

            foreach (var book in bookSet)
            {
                bookCards.Add(new BookViewModel
                {
                    ISBN = book.ISBN,
                    Title = book.Title,
                    AuthorName = book.AuthorName,
                    Price = book.Price,
                    ImagePath = book.ImagePath
                });
            }

            return bookCards;
        }
    }
}