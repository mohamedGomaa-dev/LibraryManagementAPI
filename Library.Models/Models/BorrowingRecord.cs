using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models.Models
{
    public class BorrowingRecord
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey(nameof(BookCopy))]
        public int CopyId { get; set; }
        public BookCopy? BookCopy { get; set; } 

        public DateTime BorrowingDate { get; set; } = DateTime.Now;

        public DateTime DueDate { get; set; }

        public DateTime? ActualReturnDate { get; set; }

        public Fine? Fine { get; set; } // navigation for the fine
    }
}
