using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshelf35.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<BookGenre> BookGenres { get; set; }
    }
}
