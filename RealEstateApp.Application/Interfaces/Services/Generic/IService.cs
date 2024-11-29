using System.Linq.Expressions;

namespace RealEstateApp.Application.Interfaces.Services.Generic;

public interface IService<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<int> AddAsync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(int id);
    Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
}