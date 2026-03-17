using Library.DataAccess.Data;
using Library.DataAccess.Repositories.Implementations;
using Library.DataAccess.Repositories.Interfaces;
using Library.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataAccess.Units
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Books = new GenericRepository<Book>(_context);
            BookCopies = new GenericRepository<BookCopy>(_context);
            BorrowingRecords = new GenericRepository<BorrowingRecord>(_context);
            Fines = new GenericRepository<Fine>(_context);
            Genres = new GenericRepository<Genre>(_context);
            Reservations = new GenericRepository<Reservation>(_context);
            Settings = new GenericRepository<Settings>(_context);
            Users = new GenericRepository<User>(_context);

        }
        public IGenericRepository<Book> Books { get; private set; }

        public IGenericRepository<BookCopy> BookCopies { get; private set; }

        public IGenericRepository<BorrowingRecord> BorrowingRecords { get; private set; }

        public IGenericRepository<Fine> Fines { get; private set; }

        public IGenericRepository<Genre> Genres { get; private set; }

        public IGenericRepository<Reservation> Reservations { get; private set; }

        public IGenericRepository<Settings> Settings { get; private set; }

        public IGenericRepository<User> Users { get; private set; }

        public async Task<int> CommitChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
