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

        public static SearchViewModel SearchTitles(string author, string genre, string keyword)
        {
            var db = new BookstoreContext();

            var result = new SearchViewModel();  //Instantiate this ViewModel...
            result.Keyword = keyword;  //...and set the Keyword property accordingly;

            if (!String.IsNullOrEmpty(genre)) //If a genre name is passed...
            {
                result.Books = BooksIndexViewModel.CardsList(
                    db.Genres.Find(genre).Books.Where(b => b.Title.ToUpper().Contains(keyword.ToUpper())).ToList()); //...set the Books property accordingly,
                result.Genre = genre; //set the Genre property accordingly;
            }
            else if (!String.IsNullOrEmpty(author)) //If an author name is passed...
            {
                result.Books = BooksIndexViewModel.CardsList(
                    db.Authors.Find(author).Books.Where(b => b.Title.ToUpper().Contains(keyword.ToUpper())).ToList()); //...set the Books property accordingly, 

                result.Author = author; //SET THE "Author" PROPERTY ACCORDINGLY
            }
            else //If neither...
            {
                result.Books = BooksIndexViewModel.CardsList(
                    db.Books.Where(b => b.Title.ToUpper().Contains(keyword.ToUpper())).ToList()); //...set the Books property accordingly;
            }

            return result; //Return the final result.
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