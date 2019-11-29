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

        public void AddSection(string genreName) //Add a SectionViewModel object to Sections property;
        {
            var db = new BookstoreContext(); //Instantiate BookstoreContext for data access;
            var bookCards = Book.ToVMList(db.Genres.Find(genreName).Books); //Find books by the genreName parameter and, save them
                                                                                                     //as a List of BooksIndexViewModel named bookCards;
            var section = new SectionViewModel(); //Instantiate a new SectionViewModel object, and
            section.Title = genreName; //set its Title property as the genreName parameter, and
            section.BookCards = bookCards; //set its bookCards property as bookCards object;
            section.ImageClass = section.Title.Substring(0, 5).ToLower();

            this.Sections.Add(section); //Add section to the Sections property of the object;
        }

        public void SetOtherGenres() //Add the name of each genre that is not in the Sections property to the OtherGenres property;
        {
            var db = new BookstoreContext(); //Instantiate BookstoreContext for data access;
            var allGenres = db.Genres.ToList(); //Get all genres as allGenres;

            if (this.Sections != null)
            {
                foreach (var sect in this.Sections) //For each item in the object's Section property...
                {
                    var rem = allGenres.Find(x => x.Name == sect.Title); //Find the item's genre, and
                    allGenres.Remove(rem); //remove it from allGenres;
                }
            }

            foreach (var gen in allGenres) //For each remaining item in the allGenres object...
            {
                this.OtherGenres.Add(new GenreViewModel
                {
                    Name = gen.Name,
                    ImageClass = gen.Name.ToLower().Substring(0,5)
                }); //...add its name to the OtherGenres proplerty.
            }
        }
    }
}