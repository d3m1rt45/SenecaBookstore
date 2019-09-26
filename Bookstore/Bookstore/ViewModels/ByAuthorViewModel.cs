using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookstore.ViewModels
{
    public class ByAuthorViewModel
    {
        public string Author { get; set; }
        public List<BooksIndexViewModel> BookCards { get; set; }
    }
}