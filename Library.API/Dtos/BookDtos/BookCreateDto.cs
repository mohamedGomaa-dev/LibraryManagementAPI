using Library.Models.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.API.Dtos.BookDtos
{
    public class BookCreateDto
    {

        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;


        [MaxLength(255)]
        public string ISBN { get; set; } = string.Empty;


        public DateTime PublicationDate { get; set; }

        public int GenreId { get; set; }
        public string? AdditionalDetails { get; set; }
    }
}
