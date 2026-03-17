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
    public class BorrowingService : IBorrowingService
    {

        private readonly IUnitOfWork _myUnit;
        private readonly IFineService _fineService;
        private readonly INotificationService _notificationService;
        private readonly IReservationService _reservationService;
        public BorrowingService(IUnitOfWork myUnit, IFineService fineService, INotificationService notificationService, IReservationService reservationService)
        {
            _myUnit = myUnit;
            _fineService = fineService;
            _notificationService = notificationService;
            _reservationService = reservationService;
        }
        public async Task<TransactionResult> BorrowBookCopy(int copyId, int userId)
        {
            var copy = await _myUnit.BookCopies.GetAsync(c => c.Id == copyId);
            var user = await _myUnit.Users.GetAsync(u => u.Id == userId);

            if (copy is null)
            {
                return new TransactionResult() { IsSuccess = false, Message = "Copy doesn't exist" };
            }
            if (user is null)
            {
                return new TransactionResult() { IsSuccess = false, Message = "User doesn't exist" };

            }
            if (await _fineService.HasUnpaidFinesAsync(userId))
            {
                return Utility.GetTransactionResult(false, "User has unpaid fines");
            }
            var isPhysicallyBorrowed = await _myUnit.BorrowingRecords.ExistsAsync(br => br.CopyId == copyId && br.ActualReturnDate == null);
            if (isPhysicallyBorrowed)
            {
                return Utility.GetTransactionResult(false, "Copy is currently physically borrowed by someone else.");
            }
            var firstUserInReservationQueue = await _reservationService.GetFirstUserInQueue(copyId);
            
            if (firstUserInReservationQueue is not null)
            {
                if (user.Id == firstUserInReservationQueue.Id)
                {
                    await _reservationService.FulfillReservationAsync(copyId, userId);
                } else
                {
                    return Utility.GetTransactionResult(false, "This copy is reserved for another user.");
                }
            } else if (!copy.IsAvailable)
            {

                return new TransactionResult() { IsSuccess = false, Message = "Copy Isn't Available" };

            }

            var settings = await _GetSettings();
            var borrowingRecord = new BorrowingRecord
            {
                UserId = userId,
                CopyId = copyId,
                BorrowingDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(settings.DefaultBorrowDays),
            };

            copy.IsAvailable = false;
            _myUnit.BookCopies.Update(copy);
            await _myUnit.BorrowingRecords.AddAsync(borrowingRecord);
            await _myUnit.CommitChanges();

                return new TransactionResult() { IsSuccess = true, Message = "Done Successfully!" };

        }

        public async Task<TransactionResult<ICollection<BorrowingRecord>?>> GetActiveBorrowingRecordsForUser(int userId)
        {
            if (!await _myUnit.Users.ExistsAsync(u => u.Id == userId))
            {
                return Utility.GetTransactionResult(false, "user not found", (ICollection<BorrowingRecord>)null!);
            }
            var activeRecords =  await _myUnit.BorrowingRecords.GetAllAsync(br => br.UserId == userId && br.ActualReturnDate == null);

            return Utility.GetTransactionResult(true, "success!", activeRecords);
        }

        public async Task<TransactionResult> ReturnBookCopy(int copyId)
        {
            var borrowingRecord = await _myUnit.BorrowingRecords.GetAsync(br => br.CopyId == copyId
            && br.ActualReturnDate == null);
            if (borrowingRecord is null)
            {
                return Utility.GetTransactionResult(false, "record doesn't exist");
            }

            borrowingRecord.ActualReturnDate = DateTime.Now;
            var copy = await _myUnit.BookCopies.GetAsync(b => b.Id == copyId, "Book");
            if (copy is null)
            {
                return Utility.GetTransactionResult(false, "copy doesn't exist");

            }

            var firstReservationInQueue = await _reservationService.GetFirstUserInQueue(copyId);
            if (firstReservationInQueue is null)
            {
                copy.IsAvailable = true;
            } else
            {
                copy.IsAvailable = false;
                await _notificationService.SendNotificationAsync(firstReservationInQueue.Id,
                    $"{copy.Book.Title} is available for borrowing");

            }

            TimeSpan difference = borrowingRecord.ActualReturnDate.Value - borrowingRecord.DueDate;
            int lateDays = difference.Days;
            if (lateDays > 0)
            {
                var fine = await _fineService.CalculateAndApplyFineAsync(borrowingRecord.Id, lateDays);

            }
            _myUnit.BookCopies.Update(copy);
            _myUnit.BorrowingRecords.Update(borrowingRecord);
            await _myUnit.CommitChanges();
            return Utility.GetTransactionResult(true, "Success!");
        }

        private async Task<Settings> _GetSettings()
        {
            var settings = await _myUnit.Settings.GetAsync(s => s.Id == 1);
            if (settings == null)
            {
                settings = new Settings()
                {
                    DefaultBorrowDays = 10,
                    DefaultFinePerDay = 10,
                };
            }
            return settings;
        }
    }
}
