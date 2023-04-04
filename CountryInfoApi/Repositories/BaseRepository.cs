using CountryInfoApi.Abstractions.Repositories;
using CountryInfoApi.DAL;
using CountryInfoApi.Models.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace CountryInfoApi.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseItem
    {
        AppDbContext _context;
        DbSet<T> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task Create(T item)
        {
            _dbSet.Add(item);
            await Save();
        }

        public async Task<T> Get(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public T Get(Guid id,params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query =  _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.FirstOrDefault(e => e.Id == id);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        public async Task Remove(T item)
        {
            _dbSet.Remove(item);
            await Save();
        }

        public async Task Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
