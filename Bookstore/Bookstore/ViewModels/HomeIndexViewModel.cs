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

        public HomeIndexViewModel() //On instantiation of a HomeIndexViewModel object:
        {
            this.Featured = new List<FeaturedViewModel>(); //Instantiate its Featured property as a new List of FeaturedViewModels, and
            this.Sections = new List<SectionViewModel>(); //instantiate its Sections property as a new List of SectionViewModels, and
            this.OtherGenres = new List<String>(); //instantiate its OtherGenres property as a new List of Strings;
        }

        public void SetFeatured() //Set four FeaturedViewModel objects as Featured property:
        {
            var db = new BookstoreContext(); //Instantiate BookstoreContext for data access;
            var booksList = db.Books.Take(4).ToList(); //Make a list of four random books named booksList;

            var featuredList = new List<FeaturedViewModel>(); //Instantiate a new List of FeaturedViewModels named featuredList;
            foreach (var book in booksList) //For each item in bookList...
            {
                var featured = new FeaturedViewModel //...instantiate a new FeaturedViewModel with ISBN and ImagePath properties of the item, and
                {
                    ISBN = book.ISBN,
                    ImagePath = book.ImagePath
                };
                featuredList.Add(featured); //add it to the featuredList property;
            }
            this.Featured = featuredList; //Set the featuredList property of the object as the featuredList;
        }

        public void AddSection(string genreName) //Add a SectionViewModel object to Sections property;
        {
            var db = new BookstoreContext(); //Instantiate BookstoreContext for data access;
            var bookCards = BooksIndexViewModel.CardsList(db.Genres.Find(genreName).Books.ToList()); //Find books by the genreName parameter and, save them
                                                                                                     //as a List of BooksIndexViewModel named bookCards;
            var section = new SectionViewModel(); //Instantiate a new SectionViewModel object, and
            section.Title = genreName; //set its Title property as the genreName parameter, and
            section.BookCards = bookCards; //set its bookCards property as bookCards object;

            this.Sections.Add(section); //Add section to the Sections property of the object;
        }

        public void SetOtherGenres() //Add the name of each genre that is not in the Sections property to the OtherGenres property;
        {
            var db = new BookstoreContext(); //Instantiate BookstoreContext for data access;
            var allGenres = db.Genres.ToList(); //Get all genres as allGenres;

            foreach (var sect in this.Sections) //For each item in the object's Section property...
            {
                var rem = allGenres.Find(x => x.Name == sect.Title); //Find the item's genre, and
                allGenres.Remove(rem); //remove it from allGenres;
            }

            foreach (var gen in allGenres) //For each remaining item in the allGenres object...
            {
                this.OtherGenres.Add(gen.Name); //...add its name to the OtherGenres proplerty.
            }
        }
    }
}