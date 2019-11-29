using Bookstore.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bookstore.Models
{
    public class Book
    {
        [Required]
        [StringLength(13, MinimumLength = 13)]
        public string ISBN { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(50)]
        public string AuthorName { get; set; }

        internal static void SetFeaturedForVM(BookstoreContext db)
        {
            throw new NotImplementedException();
        }

        [Required]
        public string GenreName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Publisher { get; set; }

        [Required]
        [Range(1, 1200)]
        public int NumberOfPages { get; set; }

        [Required]
        public string Format { get; set; }

        [Range(1, 1000)]
        public int WeightInGrams { get; set; }
        public int? WidthInMm { get; set; }
        public int? HeightInMm { get; set; }


        [DisplayName("Upload Image")]
        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        [ForeignKey("GenreName")]
        public virtual Genre Genre { get; set; }

        [ForeignKey("AuthorName")]
        public virtual Author Author { get; set; }


        //Take a 'DBSet<Book>' Object and Turn It Into 'List<BookCardViewModel>'s
        public static List<BooksIndexViewModel> ToVMList(IEnumerable<Book> bookSet) 
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

        //Sort a List of BooksIndexViewModels
        public static List<BooksIndexViewModel> SortVMList(List<BooksIndexViewModel> cardsList, string sortBy) 
        {
            var cardsQuery = cardsList.AsQueryable();

            switch (sortBy)
            {
                case ("AtoZ"):
                    cardsQuery = cardsQuery.OrderBy(b => b.Title);
                    break;
                case ("ZtoA"): //If it is "ZtoA"...
                    cardsQuery = cardsQuery.OrderByDescending(b => b.Title);
                    break;
                case ("lowToHigh"): //If it is "lowToHigh"...
                    cardsQuery = cardsQuery.OrderBy(b => b.Price);
                    break;
                case ("highToLow"): //If it is "highToLow"...
                    cardsQuery = cardsQuery.OrderByDescending(b => b.Price);
                    break;
            }

            return cardsQuery.ToList();
        }


        public static void SetFeaturedForVM(BookstoreContext db, HomeIndexViewModel homeIndexVM)
        {
            foreach (var book in db.Books.Take(4).ToList())
                homeIndexVM.Featured.Add(new FeaturedViewModel { ISBN = book.ISBN, ImagePath = book.ImagePath });
        }
    }
}