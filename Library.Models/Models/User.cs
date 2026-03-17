using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [MinLength(5)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(250)]
        [MinLength(5)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [MaxLength(30)]
        [MinLength(5)]
        public string? Phone { get; set; }

        [MinLength(5)]
        [MaxLength(50)]
        public string LibraryCardNumber { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Role { get; set; } = "Member";

        
        public ICollection<BorrowingRecord>? BorrowingRecords { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
        public ICollection<Fine>? Fines { get; set; }

    }
}
