using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models.Models
{
    public class BookCopy
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public Book? Book { get; set; }


        public bool IsAvailable { get; set; }

        public ICollection<BorrowingRecord>? BorrowingRecords { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }

    }
}
