using Library.Models.Models;
using Library.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services.Services.Interfaces
{
    public interface IBookManagementService
    {
        Task<TransactionResult<Book?>> GetBookByIdAsync(int id);
        Task<ICollection<Book>> SearchBooksByTitleAsync(string title);

        Task<ICollection<Book>> GetAllBooksAsync();
        Task<TransactionResult> DeleteBookAsync(int id);
        Task<TransactionResult> UpdateBookAsync(Book book);
        Task<TransactionResult<Book>> AddBookAsync(Book book);

        Task<bool> IsBookCopyAvailableAsync(int copyId);

        Task<TransactionResult> UpdateCopyStatusAsync(int copyId, bool copyStatus);

        Task<TransactionResult<BookCopy?>> CreateBookCopyAsync(int bookId);
        Task<TransactionResult> DeleteBookCopyAsync(int copyId);
        Task<TransactionResult<ICollection<BookCopy>?>> GetAllCopiesAsync(int bookId);
    }
}
