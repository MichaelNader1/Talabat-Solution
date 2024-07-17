using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.APIs.Specifications;
using Talabat.Core.Entities;
using Talabat.Core.RepositoryInterfaces;
using Talabat.Repository.Data;
using Talabat.Repository.Specifications;

namespace Talabat.Repository.Repository
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext _dbContext;
        public GenaricRepository (StoreDbContext dbContext)
        {
            _dbContext = dbContext; 

        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToArrayAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T>Spec)
        {
            return await SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(),Spec).ToListAsync();
        }

        public async Task<T?> GetWithSpecAsync(ISpecifications<T> Spec)
        {
            return await SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), Spec).FirstOrDefaultAsync();
        }
        public async Task<int> GetCountAsync(ISpecifications<T> Spec)
        {
            return await applySpecifications(Spec).CountAsync();
        }
        private IQueryable<T>  applySpecifications(ISpecifications<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(),spec);
        }

        public async Task AddAsync(T item)=> await _dbContext.Set<T>().AddAsync(item);

        public async Task Delete(T item)=>  _dbContext.Set<T>().Remove(item);

        public async Task Update(T item) => _dbContext.Set<T>().Update(item);
    }
}
