using ProductApi.Application.DTOs;

namespace ProductApi.Application.Interface.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(Guid id);
    Task AddProductAsync(ProductDto productDto);
    Task<bool> UpdateProductAsync(Guid id, ProductDto productDto);
    Task<bool> DeleteProductAsync(Guid id);
    Task<bool> ProcessOrderAsync(List<OrderProductDto> productIdsToOrder);
}
