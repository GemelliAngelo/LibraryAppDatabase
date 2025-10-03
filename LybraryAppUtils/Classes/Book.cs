using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Classes
{
    public record Book
    {
        public required long ISBN { get; set; }
        public required string? Title { get; set; }
        public string? Description { get; set; }
        public required DateTime PublishDate { get; set; }
        public required bool Available { get; set; }
    }
}
