using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookstore.ViewModels
{
    public class HomeIndexViewModel
    {
        public HomeIndexViewModel()
        {
            this.Featured = new List<FeaturedViewModel>();
            this.Sections = new List<SectionViewModel>();
            this.OtherGenres = new List<GenreViewModel>();
        }

        public List<FeaturedViewModel> Featured { get; set; }
        public List<SectionViewModel> Sections { get; set; }
        public List<GenreViewModel> OtherGenres{ get; set; }
    }
}