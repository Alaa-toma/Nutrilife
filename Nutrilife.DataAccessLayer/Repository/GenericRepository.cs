using Microsoft.EntityFrameworkCore;
using Nutrilife.DataAccessLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> createAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // ارجاع كل البيانات وعمل جوين لو يحتاج
        public async Task<List<T>> GetAllAsync(string[]? includes = null)
        {

            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetOne(Expression <Func<T, bool>> filter, string[]? includes =null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(filter);

        }

       public async Task<bool> deleteAsync(T entity)
        {
            _context.Remove(entity);
           var affected = await _context.SaveChangesAsync();
            return affected > 0;
            
        }
    }
}
