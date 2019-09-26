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
                if (value.Length > 10)
                {
                    _Title = $"{value.Substring(0, 10)}...";
                }
                else
                {
                    _Title = value;
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
                if (value.Length > 12)
                {
                    _AuthorName = $"{value.Substring(0, 12)}...";
                }
                else
                {
                    _AuthorName = value;
                }
            }
        }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }


        public static List<BooksIndexViewModel> CardsList(DbSet<Book> bookSet) //Take a 'DBSet<Book>' Object and Turn It Into 'List<BookCardViewModel>'s
        {
            var bookCards = new List<BooksIndexViewModel>();

            foreach (var book in bookSet)
            {
                bookCards.Add(new BooksIndexViewModel
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
        public static List<BooksIndexViewModel> CardsList(List<Book> bookSet) //Take a 'List<Book>' Object and Turn It Into 'List<BookCardViewModel>'s
        {
            var bookCards = new List<BooksIndexViewModel>();

            foreach (var book in bookSet)
            {
                bookCards.Add(new BooksIndexViewModel
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

        public static List<BooksIndexViewModel> SortCards(List<BooksIndexViewModel> cardsList, string sortBy)
        {
            var cardsQuery = cardsList.AsQueryable();

            switch (sortBy)
            {
                case ("AtoZ"):  //IF THE STRING "AtoZ" IS PASSED...
                    cardsQuery = cardsQuery.OrderBy(b => b.Title);  //...ORDER ALPHABETICALLY (ASCENDING)
                    break;
                case ("ZtoA"):  //IF THE STRING "ZtoA" IS PASSED...
                    cardsQuery = cardsQuery.OrderByDescending(b => b.Title); //...ORDER ALPHABEDICALLY (DESCENDING)
                    break;
                case ("lowToHigh"):  //IF THE STRING "lowToHigh" IS PASSED...
                    cardsQuery = cardsQuery.OrderBy(b => b.Price);  //...ORDER BY PRICE (ASCENDING)
                    break;
                case ("highToLow"): //IF THE STRING "highToLow" IS PASSED...
                    cardsQuery = cardsQuery.OrderByDescending(b => b.Price); //...ORDER BY PRICE (DESCENDING)
                    break;
            }

            return cardsQuery.ToList();
        }
    }
}