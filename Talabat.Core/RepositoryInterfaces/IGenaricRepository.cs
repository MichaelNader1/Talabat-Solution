using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.APIs.Specifications;
using Talabat.Core.Entities;

namespace Talabat.Core.RepositoryInterfaces
{
    public interface IGenaricRepository<T> where T : BaseEntity
    {
        Task<T?> GetAsync (int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetWithSpecAsync(ISpecifications<T> Spec);
        Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T> Spec);
        Task<int> GetCountAsync(ISpecifications<T> Spec);

        Task AddAsync (T item);
        Task Delete (T item);
        Task Update (T item);
    }
}
