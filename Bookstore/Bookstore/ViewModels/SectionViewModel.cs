using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookstore.ViewModels
{
    public class SectionViewModel
    {
        public string Title { get; set; }
        private List<BooksIndexViewModel> _BookCards;
        public List<BooksIndexViewModel> BookCards
        {
            get
            {
                return _BookCards;
            }
            set
            {
                if (value.Count > 6) //If the List being passed contains more than six items...
                {
                    _BookCards = value.Take(6).ToList(); //...set the _BookCards property as the first six items of the List;
                }
                else //If not...
                {
                    _BookCards = value; //...set the _BookCards property as the whole List;
                }
            }
        }
    }
}