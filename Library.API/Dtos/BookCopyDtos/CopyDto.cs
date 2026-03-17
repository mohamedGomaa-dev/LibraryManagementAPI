using Library.Models.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.API.Dtos.BookCopyDtos
{
    public class CopyDto
    {
        public int Id { get; set; }

        public int BookId { get; set; }


        public bool IsAvailable { get; set; }
    }
}
