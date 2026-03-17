using Library.Models.Models;
using Library.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services.Services.Interfaces
{
    public interface IBorrowingService
    {
        Task<TransactionResult> BorrowBookCopy(int copyId, int userId);
        Task<TransactionResult> ReturnBookCopy(int copyId);

        Task<TransactionResult<ICollection<BorrowingRecord>?>> GetActiveBorrowingRecordsForUser(int userId);
    }
}
