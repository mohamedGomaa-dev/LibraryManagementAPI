using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services.Helpers
{
    public class TransactionResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class TransactionResult<T> : TransactionResult
    {
        public T? Data { get; set; }
    }
}
