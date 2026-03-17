using Library.DataAccess.Units;
using Library.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services.Helpers
{
    public class Utility
    {
       

        public static TransactionResult GetTransactionResult(bool isSuccess, string message)
        {
            return new TransactionResult()
            {
                IsSuccess = isSuccess,
                Message = message
            };
        }

        public static TransactionResult<T?> GetTransactionResult<T>(bool isSuccess, string message, T data)
        {
            return new TransactionResult<T?>()
            {
                IsSuccess = isSuccess,
                Message = message,
                Data = data
            };
        }
    }
}
