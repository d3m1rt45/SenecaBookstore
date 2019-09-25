using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Bookstore.Models
{
    public class Genre
    {
        [Required]
        public string Name { get; set; }

        public virtual List<Book> Books { get; set; }
    }
}