using ProductApi.Domain.Entities;

namespace ProductApi.Domain.Interface.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task AddAsync(Product product);
    Task<bool> UpdateAsync(Guid id, Product product);
    Task<bool> DeleteAsync(Guid id);
}
