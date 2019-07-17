using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bookstore.Models
{
    public class Book
    {
        public int IBAN { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }

        public Author Author { get; set; }
    }
}