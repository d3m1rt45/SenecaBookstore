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
        // Constructor(s)
        public SearchViewModel() { Books = new List<BookViewModel>(); }

        // Properties
        public string Keyword { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public List<BookViewModel> Books { get; set; }
        public IPagedList<BookViewModel> BooksPaged { get; set; }

    }
}