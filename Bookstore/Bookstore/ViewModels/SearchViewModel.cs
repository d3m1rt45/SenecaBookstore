using Bookstore.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bookstore.ViewModels
{
    public class SearchViewModel
    {
        public string Keyword { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public List<BooksIndexViewModel> Books { get; set; }
        public IPagedList<BooksIndexViewModel> BooksPaged { get; set; }

        public SearchViewModel()
        {
            this.Books = new List<BooksIndexViewModel>();
        }

        public static SearchViewModel SearchTitles(BookstoreContext db, string author, string genre, string keyword) //Search Titles
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

            result.Books = Book.ToVMList(books);

            return result; //Return the object.
        }

        public bool MustBePaged()
        {
            if (this.Books.Count > 24)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}