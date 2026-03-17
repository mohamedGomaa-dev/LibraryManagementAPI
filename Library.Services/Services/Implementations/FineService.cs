using Library.DataAccess.Units;
using Library.Models.Models;
using Library.Services.Helpers;
using Library.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services.Services.Implementations
{
    public class FineService : IFineService
    {
        private readonly IUnitOfWork _myUnit;
        public FineService(IUnitOfWork myUnit)
        {
            _myUnit = myUnit;
        }
        private async Task<Settings> GetSettings()
        {
            var settings =  await _myUnit.Settings.GetAsync(s => s.Id == 1);
            if (settings is null)
            {
                settings = new Settings()
                {
                    DefaultBorrowDays = 10
                    ,DefaultFinePerDay = 10
                };
            }

            return settings;
        }

        private TransactionResult ReturnTransactionResult(bool isSucces, string message)
        {
            return new TransactionResult()
            {
                IsSuccess =isSucces,
                Message = message
            };
        }
        public async Task<TransactionResult> CalculateAndApplyFineAsync(int borrowingRecordId, int lateDays)
        {
            var settings = await GetSettings();
            var borrowingRecord = await _myUnit.BorrowingRecords.GetAsync(b => b.Id == borrowingRecordId);
            if (borrowingRecord is null)
            {
                return new TransactionResult()
                {
                    IsSuccess = false,
                    Message = "Borrowing Record Doesn't exist"
                };
            }

            Fine fine = new Fine()
            {
                BorrowingRecordId = borrowingRecordId,
                UserId = borrowingRecord.UserId,
                IsPaid = false,
                NumberOfLateDays = lateDays,
                FineAmount = lateDays * settings.DefaultFinePerDay
            };
            await _myUnit.Fines.AddAsync(fine);
            await _myUnit.CommitChanges();

            return new TransactionResult()
            {
                IsSuccess = true,
                Message = "Success"
            };
        }

        public async Task<decimal> GetTotalFineAmountForUserAsync(int userId)
        {
            var unpaidFines = await GetUnpaidFinesForUserAsync(userId);

            return unpaidFines.Sum(f => f.FineAmount);
        }

        public async Task<ICollection<Fine>> GetUnpaidFinesForUserAsync(int userId)
        {
             
            return await _myUnit.Fines.GetAllAsync(f => f.UserId == userId && f.IsPaid == false); 
        }

        public async Task<bool> HasUnpaidFinesAsync(int userId)
        {

            return await _myUnit.Fines.ExistsAsync(f => f.UserId == userId && f.IsPaid == false);
        }

        public async Task<TransactionResult> UpdatePaymentStatusAsync(int fineId, bool isPaid)
        {
            var fine = await _myUnit.Fines.GetAsync(f => f.Id == fineId);   
            if (fine is null)
            {
                return ReturnTransactionResult(false, "fine not found");
            }

            fine.IsPaid = isPaid;
            _myUnit.Fines.Update(fine);
            await _myUnit.CommitChanges();
            return ReturnTransactionResult(true, "Success");
        }
    }
}
