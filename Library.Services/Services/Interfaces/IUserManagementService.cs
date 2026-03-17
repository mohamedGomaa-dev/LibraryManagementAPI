using Library.Models.Models;
using Library.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services.Services.Interfaces
{
    public interface IUserManagementService
    {
        Task<TransactionResult<User?>> GetUserByIdAsync(int  id);
        Task<ICollection<User>> SearchUsersByNameAsync(string name);

        Task<ICollection<User>> GetAllUsersAsync();
        Task<TransactionResult<User>> AddUserAsync(User user, string plainPassword);
        Task<TransactionResult> UpdateUserAsync(User user);
        Task<TransactionResult> DeleteUserAsync(int id);
        Task<bool> UserExistsAsync(int userId);
    }
}
