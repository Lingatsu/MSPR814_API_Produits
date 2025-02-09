using AutoMapper;
using ProductApi.Application.DTOs;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Interface.Repositories;
using ProductApi.Application.Interface.Services;

namespace ProductApi.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product == null ? null : _mapper.Map<ProductDto>(product);
    }

    public async Task AddProductAsync(ProductDto productDto)
    {
        var category = await _categoryRepository.GetByNameAsync(productDto.Category);
        if (category == null)
        {
            category = new Category { Name = productDto.Category };
            await _categoryRepository.AddAsync(category);
        }

        var product = _mapper.Map<Product>(productDto);

        product.Id = Guid.NewGuid();
        product.CategoryId = category.Id;

        await _productRepository.AddAsync(product);
    }

    public async Task<bool> UpdateProductAsync(Guid id, ProductDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        return await _productRepository.UpdateAsync(id, product);
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        return await _productRepository.DeleteAsync(id);
    }
}
