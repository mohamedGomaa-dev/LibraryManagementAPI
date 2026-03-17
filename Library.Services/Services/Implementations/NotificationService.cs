using Library.DataAccess.Units;
using Library.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _myUnit;
        public NotificationService(IUnitOfWork myUnit)
        {
            _myUnit = myUnit;
        }
        public async Task SendNotificationAsync(int userId, string message)
        {
            var user = await _myUnit.Users.GetAsync(u => u.Id == userId);
            if (user is null)
            {
                return;
            }

            Console.WriteLine($"Email to {user.Name}: {message}");
        }
    }
}
