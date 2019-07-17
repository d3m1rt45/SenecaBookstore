using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    public class Author
    {
        public Guid AuthorID { get; set; }
        public string Name { get; set; }

        public List<Book> Books { get; set; }
    }
}