using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class Genre
    {
        [Required]
        public string Name { get; set; }

        public virtual List<Book> Books { get; set; }
    }
}