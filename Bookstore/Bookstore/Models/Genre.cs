using Bookstore.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace Bookstore.Models
{
    public class Genre
    {
        [Required]
        public string Name { get; set; }

        public virtual List<Book> Books { get; set; }

        // Add a SectionViewModel based on this genre to the HomeIndexViewModel passed
        public static void SetSectionsFor(HomeIndexViewModel homeIndexVM, BookstoreContext db) 
        {
            foreach (var genre in db.Genres.Where(x => x.Books.Count >= 6).Take(6))
            { 
                var bookCards = Book.ToVMList(genre.Books);

                SectionViewModel sectionVM = new SectionViewModel
                {
                    Title = genre.Name,
                    BookCards = bookCards,
                    ImageClass = genre.Name.Substring(0, 5).ToLower()
                };

                homeIndexVM.Sections.Add(sectionVM);
            }
        }

        public static void SetOtherGenresFor(HomeIndexViewModel homeIndexVM, BookstoreContext db)
        {
            var filteredGenres = db.Genres.ToList();

            //Filtering
            if (homeIndexVM.Sections != null)
            {
                foreach (var sect in homeIndexVM.Sections)
                    filteredGenres.Remove(filteredGenres.Find(x => x.Name == sect.Title));
            }

            //For each remaining genre after filtering
            foreach (var genre in filteredGenres)
            {
                homeIndexVM.OtherGenres.Add(new GenreViewModel 
                { 
                    Name = genre.Name, 
                    ImageClass = genre.Name.ToLower().Substring(0, 5) 
                });
            }
        }
    }
}