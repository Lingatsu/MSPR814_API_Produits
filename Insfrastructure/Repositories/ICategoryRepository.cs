using MongoExample.Domain.Entities;

namespace MongoExample.Infrastructure.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid id);
    Task<Category?> GetByNameAsync(string name);
    Task AddAsync(Category category);
    Task<bool> UpdateAsync(Guid id, Category category);
    Task<bool> DeleteAsync(Guid id);
}
