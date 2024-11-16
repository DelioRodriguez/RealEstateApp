using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Application.Interfaces.Services.Generic;

namespace RealEstateApp.Application.Services.Generic;

public class Service<T> : IService<T> where T : class
{
    private readonly IRepository<T> _repository;

    public Service(IRepository<T> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _repository.GetAllAsync(); 
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id); 
    }

    public async Task<int> AddAsync(T entity)
    {
        return await _repository.AddAsync(entity); 
    }

    public async Task<int> UpdateAsync(T entity)
    {
        return await _repository.UpdateAsync(entity); 
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}