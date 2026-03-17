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
    public class ReservationService : IReservationService
    {


        private readonly IUnitOfWork _myUnit;
        public ReservationService(IUnitOfWork myUnit)
        {
            _myUnit = myUnit;
        }

        public async Task<TransactionResult> CancelReservationAsync(int reservationId)
        {
            var reservation = await _myUnit.Reservations.GetAsync(r => r.Id ==  reservationId);
            if (reservation is null)
            {
                return Utility.GetTransactionResult(false, $"reservation with id: {reservationId} not found");
            }
            reservation.IsActive = false;
            _myUnit.Reservations.Update(reservation);
            await _myUnit.CommitChanges();
            return Utility.GetTransactionResult(true, $"reservation cancelled successfully");
        }

        public async Task<TransactionResult> CreateNewReservation(int copyId, int userId)
        {
            var user = await _myUnit.Users.GetAsync(u => u.Id == userId, "Reservations", "BorrowingRecords");
            var copy = await _myUnit.BookCopies.GetAsync(c =>  c.Id == copyId);
            if (user is null)
            {
                return Utility.GetTransactionResult(false, "User doesn't Exist");

            }
            if (copy is null)
            {
                return Utility.GetTransactionResult(false, "Copy doesn't Exist");
            }

            if (copy.IsAvailable)
            {
                return Utility.GetTransactionResult(false, "Copy is available to borrow go boroow it");
            }
            if (user.BorrowingRecords.Where(br => br.CopyId == copyId && br.ActualReturnDate == null).Any())
            {
                return Utility.GetTransactionResult(false, "Copy already borrowed by this user");

            }
            if (user.Reservations.Where(r => r.CopyId == copyId && r.IsActive == true).Any())
            {
                // I am thinking of adding a property to the reservation called IsActive to soft delete the reservations that are not active any more
                return Utility.GetTransactionResult(false, "Copy already reserved by this user");
            }
            var reservation = new Reservation
            {
                CopyId = copyId,
                UserId = userId,
                ReservationDate = DateTime.Now,
                IsActive = true,
            };
            await _myUnit.Reservations.AddAsync(reservation);
            await _myUnit.CommitChanges();
            return Utility.GetTransactionResult(true, "Sucessfully reserved!");

        }

        public async Task FulfillReservationAsync(int copyId, int userId)
        {
            var reservation = await _myUnit.Reservations.GetAsync(r => r.CopyId == copyId &&
            r.UserId == userId && r.IsActive == true);
            if (reservation == null)
            {
                return;
            }

            reservation.IsActive = false;
            _myUnit.Reservations.Update(reservation);
            await _myUnit.CommitChanges();
        }

        public async Task<User?> GetFirstUserInQueue(int copyId)
        {
            if (!await _myUnit.BookCopies.ExistsAsync(c => c.Id == copyId))
            {
                return null;
            }

            var reservations = await _myUnit.Reservations.GetAllAsync(r => r.CopyId == copyId && r.IsActive == true, "User");

            if (reservations.Count == 0)
            {
                return null;
            }
            var firstInQueue = reservations.OrderBy(r => r.ReservationDate).FirstOrDefault();
            if (firstInQueue is null)
            {
                return null;
            }
            return firstInQueue.User;
            
        }

        public async Task<int> GetUsersTurn(int copyId, int userId)
        {
            var reservations = await _myUnit.Reservations.GetAllAsync(r => r.CopyId == copyId && r.IsActive == true,
                "User", "Copy");

            var reservationsOrdered = reservations.OrderBy(r => r.ReservationDate).ToList();
            int order = 0;
            foreach (var reservation in reservationsOrdered)
            {
                if (reservation.UserId == userId)
                {
                    order++;
                    return order;
                } else
                {
                    order++;
                }
            }

            return -1;
        }
    }
}
