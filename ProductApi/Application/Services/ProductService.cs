using AutoMapper;
using ProductApi.Application.DTOs;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Interface.Repositories;
using ProductApi.Application.Interface.Services;
using ProductApi.Infrastructure.Services;


namespace ProductApi.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly RabbitMqService _rabbitMqService;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper, RabbitMqService rabbitMqService)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _rabbitMqService = rabbitMqService;
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
    
    public async Task<bool> CheckAndUpdateStockAsync(List<ProductDto> productsToUpdate)
    {
        foreach (var productDto in productsToUpdate)
        {
            var product = await _productRepository.GetByIdAsync(productDto.Id);
            if (product == null)
            {
                return false; // Produit non trouvé
            }

            if (product.Stock < productDto.Stock) // Vérification du stock
            {
                return false; // Stock insuffisant
            }

            // Mise à jour du stock
            product.Stock -= productDto.Stock;
            await _productRepository.UpdateAsync(product.Id, product);
        }

        return true; // Stock mis à jour avec succès
    }

    public async Task<bool> ProcessOrderAsync(List<ProductDto> productsToOrder)
    {
        // Vérification et mise à jour du stock
        bool stockUpdated = await CheckAndUpdateStockAsync(productsToOrder);

        if (!stockUpdated)
        {
            return false; // Retourner faux si le stock est insuffisant
        }

        // Envoi du message à RabbitMQ avec les produits commandés
        _rabbitMqService.SendMessage(productsToOrder, "product_info_queue");

        return true; // Processus réussi
    }

    
}
