using System.Collections.Generic;

namespace Bookstore.Models
{
    public class Genre
    {
        public string Name { get; set; }

        public virtual IEnumerable<Book> Books { get; set; }

        public Genre()
        {
            if (this.Books == null)
                Books = new List<Book>();
        }
    }
}