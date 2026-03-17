using Library.Models.Models;
using Library.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TransactionResult> RegisterAsync(User user, string plainPassword);

        Task<TransactionResult<string>> LoginAsync(string email, string password);
    }
}
