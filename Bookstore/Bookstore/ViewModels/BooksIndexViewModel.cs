using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bookstore.ViewModels
{
    public class BooksIndexViewModel
    {
        public string ISBN { get; set; }

        private string _Title;
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (value.Length > 10) //If the string being assigned is longer than 10 characters...
                {
                    _Title = $"{value.Substring(0, 10)}..."; //...assign only the first 10 characters followed by "...";
                }
                else //If not...
                {
                    _Title = value;  //...assign the whole string.
                }
            }
        }

        private string _AuthorName;
        public string AuthorName
        {
            get
            {
                return _AuthorName;
            }
            set
            {
                if (value.Length > 9) //If the string being assigned is longer than 12 characters...
                {
                    _AuthorName = $"{value.Substring(0, 9)}..."; //...assign only the first 10 characters followed by "...";
                }
                else //If not...
                {
                    _AuthorName = value; //...assign the whole string.
                }
            }
        }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }


        public static List<BooksIndexViewModel> CardsList(DbSet<Book> bookSet) //Take a 'DBSet<Book>' Object and Turn It Into 'List<BookCardViewModel>'s
        {
            var bookCards = new List<BooksIndexViewModel>(); //Instantiate a new List of BookIndexViewModels and,

            foreach (var book in bookSet) //For each book in the bookSet parameter...
            {
                bookCards.Add(new BooksIndexViewModel //...map the book into a new BooksIndexViewModel and,
                {
                    ISBN = book.ISBN,
                    Title = book.Title,
                    AuthorName = book.AuthorName,
                    Price = book.Price,
                    ImagePath = book.ImagePath
                }); //add it to bookCards;
            }

            return bookCards; //Return bookCards;
        }
        public static List<BooksIndexViewModel> CardsList(List<Book> bookList) //Map a List of Books into a List of BookIndexViewModels:
        {
            var bookCards = new List<BooksIndexViewModel>(); //Instantiate a new List of BookIndexViewModels named bookCards;

            foreach (var book in bookList) //For each book in the bookList parameter...
            {
                bookCards.Add(new BooksIndexViewModel //...map the book into a new BooksIndexViewModel object and, 
                {
                    ISBN = book.ISBN,
                    Title = book.Title,
                    AuthorName = book.AuthorName,
                    Price = book.Price,
                    ImagePath = book.ImagePath
                }); //add it to bookCards;
            }

            return bookCards; //return bookCards;
        }

        public static List<BooksIndexViewModel> SortCards(List<BooksIndexViewModel> cardsList, string sortBy) //Sort a List of BooksIndexViewModels:
        {
            var cardsQuery = cardsList.AsQueryable(); //Instantiate a new cardsQery from the cardsList parameter;

            switch (sortBy) //Check the sortBy parameter:
            {
                case ("AtoZ"): //If it's "AtoZ"...
                    cardsQuery = cardsQuery.OrderBy(b => b.Title);  //...order cardsQuery items alphabetically (ascending);
                    break;
                case ("ZtoA"): //If it is "ZtoA"...
                    cardsQuery = cardsQuery.OrderByDescending(b => b.Title); //...order cardsQuery items alphabetically (descending);
                    break;
                case ("lowToHigh"): //If it is "lowToHigh"...
                    cardsQuery = cardsQuery.OrderBy(b => b.Price);  //...order cardsQuery items by price (ascending);
                    break;
                case ("highToLow"): //If it is "highToLow"...
                    cardsQuery = cardsQuery.OrderByDescending(b => b.Price); //...order cardsQuery items by price (descending);
                    break;
            }

            return cardsQuery.ToList(); //Return the cardsQuery as a List of BooksIndexViewModel.
        }
    }
}