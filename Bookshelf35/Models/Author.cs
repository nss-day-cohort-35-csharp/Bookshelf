using System.Collections.Generic;

namespace Bookshelf35.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<Book> Books { get; set; }
    }
}