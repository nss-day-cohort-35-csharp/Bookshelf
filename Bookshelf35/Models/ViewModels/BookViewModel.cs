using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshelf35.Models.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int YearPublished { get; set; }
        public int Rating { get; set; }

        [Display(Name = "Genres")]
        public List<int> GenreIds { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
