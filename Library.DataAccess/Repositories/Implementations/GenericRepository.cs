using Library.DataAccess.Data;
using Library.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataAccess.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T item)
        {
            await _context.Set<T>().AddAsync(item);
        }

        public  void Delete(T item)
        {
             _context.Set<T>().Remove(item);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().AnyAsync(match);
        }

        public async Task<ICollection<T>> GetAllAsync(params string[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes.Count() > 0)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> match)
        {
            return  await _context.Set<T>().Where(match).ToListAsync();
        }

        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> match, params string[] includes)
        {
            IQueryable<T> items = _context.Set<T>().Where(match);
            if (includes.Count() > 0)
            {
                foreach (var eager in includes)
                {
                    items = items.Include(eager);
                }
            }
            return await items.ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(match);
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> match, params string[] includes)
        {
            IQueryable<T> item = _context.Set<T>().Where(match);
            if (includes.Count() > 0)
            {
                foreach (var eager in includes)
                {
                    item = item.Include(eager);
                }
            }

            return await item.FirstOrDefaultAsync();
        }

        public  void Update(T item)
        {
            _context.Set<T>().Update(item);
        }
    }
}
