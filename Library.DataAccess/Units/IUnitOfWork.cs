using Library.DataAccess.Repositories.Interfaces;
using Library.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataAccess.Units
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Book> Books { get; }
        IGenericRepository<BookCopy> BookCopies { get; }
        IGenericRepository<BorrowingRecord> BorrowingRecords { get; }
        IGenericRepository<Fine> Fines { get; }
        IGenericRepository<Genre> Genres { get; }
        IGenericRepository<Reservation> Reservations { get; }
        IGenericRepository<Settings> Settings { get; }
        IGenericRepository<User> Users { get; }
        Task<int> CommitChanges();

    }
}
