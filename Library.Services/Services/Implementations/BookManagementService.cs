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
    public class BookManagementService : IBookManagementService
    {

        private readonly IUnitOfWork _myUnit;
        public BookManagementService(IUnitOfWork myUnit)
        {
            _myUnit = myUnit;
        }
        public async Task<TransactionResult<Book>> AddBookAsync(Book book)
        {
            // we will need to add validation but this is a class library so I will need to add something like the model state
            if (await _myUnit.Books.ExistsAsync(b => b.ISBN == book.ISBN))
            {
                return Utility.GetTransactionResult(false, "can't add book with existing isbn", book);
            }
            if (!await _myUnit.Genres.ExistsAsync(g => g.Id == book.GenreId))
            {
                return Utility.GetTransactionResult(false, "add a valid genre to the book", book);
            }
            await _myUnit.Books.AddAsync(book);
            await _myUnit.CommitChanges();
            return Utility.GetTransactionResult(true, "successfully added", book);
        }

        public async Task<TransactionResult<BookCopy?>> CreateBookCopyAsync(int bookId)
        {
            
            
            if (!await _myUnit.Books.ExistsAsync(b => b.Id == bookId))
            {
                return Utility.GetTransactionResult<BookCopy>(false, "book not found to create a copy for", null!);
            }
            var bookCopy = new BookCopy()
            {
                BookId = bookId,
                IsAvailable = true,
            };
            await _myUnit.BookCopies.AddAsync(bookCopy);
            await _myUnit.CommitChanges();

            return Utility.GetTransactionResult(true, "book copy created with id", bookCopy);
        }

        public async Task<TransactionResult> DeleteBookAsync(int id)
        {

            var book = await _myUnit.Books.GetAsync(b => b.Id == id);
            if (book is null)
            {
                return Utility.GetTransactionResult(false, "book not found to delete");
            }
            _myUnit.Books.Delete(book);
            await _myUnit.CommitChanges();

            return Utility.GetTransactionResult(true, "deleted successfully!");
        }

        public async Task<TransactionResult> DeleteBookCopyAsync(int copyId)
        {
            var copy = await _myUnit.BookCopies.GetAsync(b => b.Id == copyId);
            if (copy is null)
            {
                return Utility.GetTransactionResult(false, $"copy with id: {copyId} not found to delete");
            }
            _myUnit.BookCopies.Delete(copy);
            await _myUnit.CommitChanges();

            return Utility.GetTransactionResult(true, $"copy with id: {copyId} deleted successfully");
        }

        public async Task<ICollection<Book>> GetAllBooksAsync()
        {

            return await _myUnit.Books.GetAllAsync("Genre");
        }

        public async Task<TransactionResult<ICollection<BookCopy>?>> GetAllCopiesAsync(int bookId)
        {
            
            ICollection<BookCopy> copies = new List<BookCopy>();

            if (!await _myUnit.Books.ExistsAsync(b => b.Id == bookId)) {
                return Utility.GetTransactionResult(false, $"book with id: {bookId} not found", copies);
            }
            var mycopies =
              await _myUnit.BookCopies.GetAllAsync(b => b.BookId == bookId);

            return Utility.GetTransactionResult(true, $"sucess", mycopies);
        }

        public async Task<TransactionResult<Book?>> GetBookByIdAsync(int id)
        {
            var book = await _myUnit.Books.GetAsync(b => b.Id == id, "Genre");
            if (book is null)
            {
                return Utility.GetTransactionResult(false, $"book with id: {id}, not found", book);
            }

            return Utility.GetTransactionResult(true, $"Found successfully", book);
        }

        public async Task<bool> IsBookCopyAvailableAsync(int copyId)
        {
           
            var copy = await _myUnit.BookCopies.GetAsync(bc => bc.Id == copyId);
            if (copy is null)
            {
                return false;
            }
            return copy.IsAvailable;
        }

        public async Task<ICollection<Book>> SearchBooksByTitleAsync(string title)
        {
            var books = await _myUnit.Books.GetAllAsync(b => b.Title.Contains(title.Trim())); // I find it better to show approximate results by contains rather than exact one maybe someone doesn't remember the exact title!
            return books;
        }

        public async Task<TransactionResult> UpdateBookAsync(Book book)
        {
            if (!await _myUnit.Books.ExistsAsync(b => b.ISBN == book.ISBN && b.Id != book.Id))
            {
                return Utility.GetTransactionResult(false, "can't update isbn to existing one!");

            }
            if (! await _myUnit.Books.ExistsAsync(b => b.Id == book.Id)) {
                return Utility.GetTransactionResult(false, "user to update not found!");
            }
           // we will need to add validation but this is a class library so I will need to add something like the model state
            _myUnit.Books.Update(book);
            await _myUnit.CommitChanges();

            return Utility.GetTransactionResult(true, "User updated successfully");
        }

        public async Task<TransactionResult> UpdateCopyStatusAsync(int copyId, bool copyStatus)
        {
            
            var copy = await _myUnit.BookCopies.GetAsync(b => b.Id == copyId);
            if (copy is null)
            {
                return Utility.GetTransactionResult(false, $"copy with id: {copyId} not found");
            }
            copy.IsAvailable = copyStatus;
            _myUnit.BookCopies.Update(copy);
            await _myUnit.CommitChanges();
            return Utility.GetTransactionResult(true, $"copy with id: {copyId} updated successfully");
        }
    }
}
