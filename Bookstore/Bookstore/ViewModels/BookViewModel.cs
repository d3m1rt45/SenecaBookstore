using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bookstore.ViewModels
{
    public class BookViewModel
    {
        public string ISBN { get; set; }

        private string _Title;
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (value.Length > 10)
                {
                    _Title = $"{value.Substring(0, 10)}...";
                }
                else //If not...
                {
                    _Title = value;
                }
            }
        }

        private string _AuthorName;
        public string AuthorName
        {
            get
            {
                return _AuthorName;
            }
            set
            {
                if (value.Length > 9)
                {
                    _AuthorName = $"{value.Substring(0, 9)}...";
                }
                else //If not...
                {
                    _AuthorName = value;
                }
            }
        }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
    }
}