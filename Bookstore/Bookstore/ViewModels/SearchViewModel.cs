using Bookstore.Models;
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
        public List<Book> Books { get; set; }

        public static SearchViewModel SearchTitles(string author, string genre, string keyword)
        {
            var db = new BookstoreContext();

            var result = new SearchViewModel();  //INSTANTIATE THE RELATED VIEWMODEL
            result.Keyword = keyword;  //SET ITS 'Keyword' PROPERTY ACCORDINGLY

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

            return result;
        }
    }
}