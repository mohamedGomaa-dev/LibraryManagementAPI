using Library.Models.Models;
using Library.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services.Services.Interfaces
{
    public interface IReservationService
    {
        Task<TransactionResult> CreateNewReservation(int copyId, int userId);
        Task<TransactionResult> CancelReservationAsync(int reservationId);
        Task FulfillReservationAsync(int copyId, int userId);
        Task<int> GetUsersTurn(int copyId, int userId);
        Task<User?> GetFirstUserInQueue(int copyId);

    }
}
