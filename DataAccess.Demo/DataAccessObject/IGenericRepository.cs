using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Demo.DataAccessObject
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(int id);
        public Task<List<T>> GetAll();
        Task<int> Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        void Update(T entity);
    }
}
