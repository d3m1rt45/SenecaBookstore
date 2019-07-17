using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bookstore.Models
{
    public class Book
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string GenreName { get; set; }
        public string AuthorName { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public int NumberOfPages { get; set; }
        public int WeightInGrams { get; set; }
        public string Dimensions { get; set; }
        public string Format { get; set; }

        public Genre Genre { get; set; }
        public Author Author { get; set; }
    }
}