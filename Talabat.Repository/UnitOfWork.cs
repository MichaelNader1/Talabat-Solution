using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.RepositoryInterfaces;
using Talabat.Repository.Data;
using Talabat.Repository.Repository;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbcontext;
        private readonly Hashtable _repository;

        public UnitOfWork(StoreDbContext dbcontext)
        {
            _dbcontext = dbcontext;
            _repository = new Hashtable();
        }

        public IGenaricRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var Type= typeof(TEntity).Name;
            if (!_repository.ContainsKey(Type))
            {
                var repository= new GenaricRepository<TEntity>(_dbcontext);
                _repository.Add(Type, repository);
            }
            return _repository[Type] as IGenaricRepository<TEntity>;
        }

        public async Task<int> CompleteAsync()=> await _dbcontext.SaveChangesAsync();
        public ValueTask DisposeAsync()=> _dbcontext.DisposeAsync();


    }
}
