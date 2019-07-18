using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookstore.Models
{
    public class Author
    {
        public string Name { get; set; }

        public virtual IEnumerable<Book> Books { get; set; }

        public Author()
        {
            if(this.Books == null)
                Books = new List<Book>();
        }
    }
}