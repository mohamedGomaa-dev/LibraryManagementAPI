using Library.Models.Models;
using Library.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services.Services.Interfaces
{
    public interface IFineService
    {
        Task<TransactionResult> CalculateAndApplyFineAsync(int borrowingRecordId, int lateDays);

        Task<bool> HasUnpaidFinesAsync(int userId);

        Task<decimal> GetTotalFineAmountForUserAsync(int userId);

        Task<ICollection<Fine>> GetUnpaidFinesForUserAsync(int userId);

        Task<TransactionResult> UpdatePaymentStatusAsync(int fineId, bool isPaid);
    }
}
