using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models.Models
{
    public class Book
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;


        [MaxLength(255)]
        public string ISBN { get; set; } = string.Empty;


        public DateTime PublicationDate { get; set; }

        [ForeignKey(nameof(Genre))]
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }
        public string? AdditionalDetails { get; set; }

        public ICollection<BookCopy>? BookCopies { get; set; }

    }
}
