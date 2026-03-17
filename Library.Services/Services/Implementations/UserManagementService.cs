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
    public class UserManagementService : IUserManagementService
    {
        private readonly IUnitOfWork _myUnit;

        public UserManagementService(IUnitOfWork myUnit)
        {
            _myUnit = myUnit;
        }

        public async Task<TransactionResult<User>> AddUserAsync(User user, string plainPassword)
        {
           
            if (await _myUnit.Users.ExistsAsync(u => u.LibraryCardNumber  == user.LibraryCardNumber))
            {
                return Utility.GetTransactionResult(false, "can't add user with existing card number", user);
            }

            if (await _myUnit.Users.ExistsAsync(u => u.Email == user.Email))
            {
                return Utility.GetTransactionResult(false, "can't add user with existing email", user);
            }
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);
            await _myUnit.Users.AddAsync(user);
            await _myUnit.CommitChanges();
            return Utility.GetTransactionResult(true, "user added successfully", user);
        }

        public async Task<TransactionResult> DeleteUserAsync(int id)
        {
            var user = await _myUnit.Users.GetAsync(u => u.Id == id);
            if (user is null)
            {
                return Utility.GetTransactionResult(false, "user not found");
            }
            _myUnit.Users.Delete(user);
            await _myUnit.CommitChanges();
            return Utility.GetTransactionResult(true, "user deleted successfully");
        }

        public async Task<ICollection<User>> GetAllUsersAsync()
        {
            return await _myUnit.Users.GetAllAsync();
        }

        public async Task<TransactionResult<User?>> GetUserByIdAsync(int id)
        {

            var user =  await _myUnit.Users.GetAsync(u => u.Id == id);
            if (user is null)
            {
                return Utility.GetTransactionResult(false, $"user with id: {id} not found", user);
            }

            return Utility.GetTransactionResult(true, "user found successfully", user);
        }

        public async Task<ICollection<User>> SearchUsersByNameAsync(string name)
        {
            return await _myUnit.Users.GetAllAsync(u => u.Name.Contains(name.Trim()));
        }

        public async Task<TransactionResult> UpdateUserAsync(User user)
        {
            // I will have to add model state validation later
            if (await _myUnit.Users.ExistsAsync(u => u.LibraryCardNumber == user.LibraryCardNumber
                                                 && u.Id != user.Id))
            {
                return Utility.GetTransactionResult(false, "can't edit a library card number to an existing one");
            }
            if (await _myUnit.Users.ExistsAsync(u => u.Email == user.Email
                                                 && u.Id != user.Id))
            {
                return Utility.GetTransactionResult(false, "can't edit an email to an existing one");
            }
            if (!await _myUnit.Users.ExistsAsync(u => u.Id == user.Id))
            {
                return Utility.GetTransactionResult(false, $"user with id: {user.Id} not found"); 
            }
            
            _myUnit.Users.Update(user);
            await _myUnit.CommitChanges();
            return Utility.GetTransactionResult(true, "updated successfully");
        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            return await _myUnit.Users.ExistsAsync(u => u.Id == userId);
        }
    }
}
