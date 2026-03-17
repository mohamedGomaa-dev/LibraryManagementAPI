using Library.Models.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.API.Dtos.FineDtos
{
    public class FineDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int BorrowingRecordId { get; set; }


        public int NumberOfLateDays { get; set; }

        public decimal FineAmount { get; set; }

        public bool IsPaid { get; set; }
    }
}
