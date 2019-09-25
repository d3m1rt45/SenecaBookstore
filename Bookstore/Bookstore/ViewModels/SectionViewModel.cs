using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookstore.ViewModels
{
    public class SectionViewModel
    {
        public string Title { get; set; }
        private List<AllTitlesViewModel> _BookCards;
        public List<AllTitlesViewModel> BookCards
        {
            get
            {
                return _BookCards;
            }
            set
            {
                if (value.Count > 6)
                {
                    _BookCards = value.Take(6).ToList();
                }
                else
                {
                    _BookCards = value;
                }
            }
        }
    }
}