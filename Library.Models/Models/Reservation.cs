using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey(nameof(BookCopy))]
        public int CopyId { get; set; }
        public BookCopy? BookCopy { get; set; }
        public DateTime ReservationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
