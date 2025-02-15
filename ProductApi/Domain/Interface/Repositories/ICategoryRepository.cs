using ProductApi.Domain.Entities;

namespace ProductApi.Domain.Interface.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid id);
    Task<Category?> GetByNameAsync(string name);
    Task AddAsync(Category category);
    Task<bool> UpdateAsync(Guid id, Category category);
    Task<bool> DeleteAsync(Guid id);
}
