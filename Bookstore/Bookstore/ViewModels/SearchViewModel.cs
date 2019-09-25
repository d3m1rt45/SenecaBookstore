using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookstore.Models;

namespace Bookstore.ViewModels
{
    public class SearchViewModel
    {
        public string Keyword { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public List<Book> Books { get; set; }
    }
}