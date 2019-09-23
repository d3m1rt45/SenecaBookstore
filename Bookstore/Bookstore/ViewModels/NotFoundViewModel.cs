using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookstore.Models;

namespace Bookstore.ViewModels
{
    public class NotFoundViewModel
    {
        public string Parameter { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
    }
}