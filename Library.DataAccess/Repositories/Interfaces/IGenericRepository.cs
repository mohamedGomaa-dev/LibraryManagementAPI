using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataAccess.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> match);
        Task<T?> GetAsync(Expression<Func<T, bool>> match, params string[] includes);
        Task<ICollection<T>> GetAllAsync(params string[] includes);
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> match);
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> match, params string[] includes);

        Task AddAsync(T item);
        void Update(T item);
        void Delete(T item);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> match);

    }
}
