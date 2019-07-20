using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bookstore.Models
{
    public class Book
    {
        [Required]
        [StringLength(13, MinimumLength = 13)]
        public string ISBN { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string AuthorName { get; set; }

        [Required]
        public string GenreName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Publisher { get; set; }

        [Required]
        [Range(1, 1200)]
        public int NumberOfPages { get; set; }

        [Required]
        public string Format { get; set; }

        [Range(1, 1000)]
        public int WeightInGrams { get; set; }
        public int? WidthInMm { get; set; }
        public int? HeightInMm { get; set; }
        public int? LengthInMm { get; set; }

        [DisplayName("Upload Image")]
        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        [ForeignKey("GenreName")]
        public virtual Genre Genre { get; set; }

        [ForeignKey("AuthorName")]
        public virtual Author Author { get; set; }
    }
}