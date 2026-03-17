using System.ComponentModel.DataAnnotations;

namespace Library.API.Dtos.BookDtos
{
    public class BookUpdateDto
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;


        [MaxLength(255)]
        public string ISBN { get; set; } = string.Empty;


        public DateTime PublicationDate { get; set; }

        public int GenreId { get; set; }
        public string? AdditionalDetails { get; set; }
    }
}
