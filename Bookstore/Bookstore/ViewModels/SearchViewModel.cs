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

        public static SearchViewModel SearchTitles(string author, string genre, string keyword) //Search Titles
        {
            var db = new BookstoreContext(); //Instantiate BookstoreContext for data access;

            var result = new SearchViewModel();  //Instantiate this ViewModel...
            result.Keyword = keyword;  //...and set the 'Keyword' property as the 'keyword' parameter;

            if (!String.IsNullOrEmpty(genre)) //If a genre name is passed...
            {
                result.Books = BooksIndexViewModel.CardsList( //Search all the books of said genre for the 'keyword' parameter;
                    db.Genres.Find(genre).Books.Where(b => b.Title.ToUpper().Contains(keyword.ToUpper())).ToList()); //set them as the Books property of the object;

                result.Genre = genre; //Set the 'Genre' property by the 'genre' parameter;
            }
            else if (!String.IsNullOrEmpty(author)) //If not, and if an author name is passed...
            {
                result.Books = BooksIndexViewModel.CardsList( //Search all the books of said author for the 'keyword' parameter;
                    db.Authors.Find(author).Books.Where(b => b.Title.ToUpper().Contains(keyword.ToUpper())).ToList()); //set them as the Books property of the object; 

                result.Author = author; //Set the 'Author' property by the 'author' parameter;
            }
            else //If neither is true...
            {
                result.Books = BooksIndexViewModel.CardsList( //Search all the books for the 'keyword' parameter;
                    db.Books.Where(b => b.Title.ToUpper().Contains(keyword.ToUpper())).ToList()); //set them as the Books property of the object; 
            }

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