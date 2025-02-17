using Xunit;
using Moq;
using AutoMapper;
using ProductApi.Application.DTOs;
using ProductApi.Application.Services;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Interface.Repositories;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _mapperMock = new Mock<IMapper>();
        _productService = new ProductService(_productRepositoryMock.Object, _categoryRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllProductsAsync_ShouldReturnProductList()
    {
        // Arrange
        var products = new List<Product> { new Product { Id = Guid.NewGuid(), Name = "TestProduct" } };
        var productDtos = new List<ProductDto> { new ProductDto { Name = "TestProduct" } };

        _productRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);
        _mapperMock.Setup(mapper => mapper.Map<List<ProductDto>>(products)).Returns(productDtos);

        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("TestProduct", result[0].Name);
    }

    [Fact]
    public async Task AddProductAsync_ShouldCallRepository()
    {
        // Arrange
        var productDto = new ProductDto { Name = "NewProduct", Category = "NewCategory" };
        var category = new Category { Id = Guid.NewGuid(), Name = "NewCategory" };
        var product = new Product { Id = Guid.NewGuid(), Name = "NewProduct", CategoryId = category.Id };

        _categoryRepositoryMock.Setup(repo => repo.GetByNameAsync(productDto.Category)).ReturnsAsync(category);
        _mapperMock.Setup(mapper => mapper.Map<Product>(productDto)).Returns(product);
        _productRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        // Act
        await _productService.AddProductAsync(productDto);

        // Assert
        _productRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldReturnTrue_WhenSuccessful()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productRepositoryMock.Setup(repo => repo.DeleteAsync(productId)).ReturnsAsync(true);

        // Act
        var result = await _productService.DeleteProductAsync(productId);

        // Assert
        Assert.True(result);
    }
}
