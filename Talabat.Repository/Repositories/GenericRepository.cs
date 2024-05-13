using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Interfaces;
using Talabat.Core.Specifications;
using Talabat.Repository.Data.Context;
using Talabat.Repository.Specification;

namespace Talabat.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context)
        {
            _context = context;
        }


        #region Static Query
        public async Task<IEnumerable<T>> GetAllAsync()
            => await _context.Set<T>().ToListAsync();


        public async Task<T> GetByIdAsync(int id)
            => await _context.Set<T>().FindAsync(id);

        #endregion

        #region Dynamic Query
        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec)
            => await ApplaySpecification(spec).ToListAsync();


        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
            => await ApplaySpecification(spec).FirstOrDefaultAsync();

        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
            => await ApplaySpecification(spec).CountAsync();

        #endregion

        private IQueryable<T> ApplaySpecification(ISpecification<T> spec)
            => SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);





        //public async Task<T> Add(T entity)
        //   => await _context.Set<T>().AddAsync(entity);

        public async Task Add(T entity)
            => await _context.Set<T>().AddAsync(entity);


        public void Update(T entity)
            => _context.Set<T>().Update(entity);

        public void Delete(T entity)
            => _context.Set<T>().Remove(entity);


    }
}
