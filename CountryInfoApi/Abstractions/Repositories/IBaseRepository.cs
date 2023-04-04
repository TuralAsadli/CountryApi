

using CountryInfoApi.Models.Base;
using System.Linq.Expressions;

namespace CountryInfoApi.Abstractions.Repositories
{
    public interface IBaseRepository<T> where T : BaseItem
    {
        Task Create(T item);
        Task<T> Get(Guid id);
        IQueryable<T> GetAll();
        T Get(Guid Id, params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        Task Remove(T item);
        Task Update(T item);

        Task Save();
    }
}
