using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bookstore.Models
{
    public class Author
    {
        [Required]
        public string Name { get; set; }

        public virtual List<Book> Books { get; set; }

    }
}