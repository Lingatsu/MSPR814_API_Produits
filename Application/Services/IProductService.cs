using MongoExample.Application.DTOs;

namespace MongoExample.Application.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(Guid id);
    Task AddProductAsync(ProductDto productDto);
    Task<bool> UpdateProductAsync(Guid id, ProductDto productDto);
    Task<bool> DeleteProductAsync(Guid id);
}
