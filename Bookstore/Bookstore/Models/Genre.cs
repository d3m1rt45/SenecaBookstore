using System.Collections.Generic;

namespace Bookstore.Models
{
    public class Genre
    {
        public string Name { get; set; }
        public List<Book> Books { get; set; }

        public Genre()
        {
            Books = new List<Book>();
        }
    }
}