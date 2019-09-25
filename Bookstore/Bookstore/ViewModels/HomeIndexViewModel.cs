using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookstore.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<FeaturedViewModel> Featured { get; set; }
        public List<SectionViewModel> Sections { get; set; }
        public List<String> OtherGenres{ get; set; }

        public HomeIndexViewModel()
        {
            this.Featured = new List<FeaturedViewModel>();
            this.Sections = new List<SectionViewModel>();
            this.OtherGenres = new List<String>();
        }

        public void SetFeatured(IQueryable<Book> bookList)
        {
            var featuredList = new List<FeaturedViewModel>();
            foreach (var book in bookList)
            {
                var featured = new FeaturedViewModel
                {
                    ISBN = book.ISBN,
                    ImagePath = book.ImagePath
                };
                featuredList.Add(featured);
            }
            this.Featured = featuredList;
        }

        public void AddSection(string genreName)
        {
            var newSection = new SectionViewModel();
            var db = new BookstoreContext();
            var bookCards = AllTitlesViewModel.CardsList(db.Genres.Find(genreName).Books.ToList());
            newSection.Title = genreName;
            newSection.BookCards = bookCards;
            this.Sections.Add(newSection);
        }

        public void SetOtherGenres()
        {
            var db = new BookstoreContext();
            var allGenres = db.Genres.ToList();
            foreach (var sect in this.Sections)
            {
                var rem = allGenres.Find(x => x.Name == sect.Title);
                allGenres.Remove(rem);
            }

            foreach (var gen in allGenres)
            {
                this.OtherGenres.Add(gen.Name);
            }
        }
    }
}