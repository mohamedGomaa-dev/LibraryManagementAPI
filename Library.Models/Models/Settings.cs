using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public int DefaultBorrowDays { get; set; }
        public int DefaultFinePerDay { get; set; }
    }
}
