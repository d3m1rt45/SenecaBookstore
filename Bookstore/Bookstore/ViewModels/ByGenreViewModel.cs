using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookstore.ViewModels
{
    public class ByGenreViewModel
    {
        public string Genre { get; set; }
        public List<AllTitlesViewModel> BookCards { get; set; }
    }
}