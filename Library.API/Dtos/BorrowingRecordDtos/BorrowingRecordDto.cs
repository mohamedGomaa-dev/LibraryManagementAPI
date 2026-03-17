using Library.Models.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.API.Dtos.BorrowingRecordDtos
{
    public class BorrowingRecordDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CopyId { get; set; }

        public DateTime BorrowingDate { get; set; } 

        public DateTime DueDate { get; set; }

        public DateTime? ActualReturnDate { get; set; }
    }
}
