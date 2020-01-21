using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshelf35.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [Display(Name = "Author")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        [Display(Name = "Year Published")]
        public int YearPublished { get; set; }
        public int Rating { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Genre { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
