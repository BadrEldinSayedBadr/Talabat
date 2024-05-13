using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Interfaces;
using Talabat.Repository.Data.Context;

namespace Talabat.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _context;
        private Hashtable _repositories;

        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }


        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;  //Product or Delivery Method or ...

            if(!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(_context);

                _repositories.Add(type, repository);  //Key, Value
            }

            return _repositories[type] as IGenericRepository<TEntity>;
        }

        public async Task<int> Complete()
            => await _context.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await _context.DisposeAsync();

    }
}
