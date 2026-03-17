using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models.Models
{
    public class Fine
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey(nameof(BorrowingRecord))]
        public int BorrowingRecordId { get; set; }
        public BorrowingRecord? BorrowingRecord { get; set; }


        public int NumberOfLateDays { get; set; }

        public decimal FineAmount { get; set; }

        public bool IsPaid { get; set; }
    }
}
