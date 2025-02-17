using AutoMapper;
using Moq;
using ProductApi.Application.DTOs;
using ProductApi.Application.Services;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Interface.Repositories;
using ProductApi.Infrastructure.Services;
using Xunit;

namespace ProductApi.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<RabbitMqService> _rabbitMqServiceMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _rabbitMqServiceMock = new Mock<RabbitMqService>(); // Mock RabbitMqService

           
            _productService = new ProductService(
                _productRepositoryMock.Object, 
                _categoryRepositoryMock.Object, 
                _mapperMock.Object, 
                _rabbitMqServiceMock.Object
            );
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnProductList()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = Guid.NewGuid(), Name = "Product1" } };
            _productRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);
            _mapperMock.Setup(m => m.Map<List<ProductDto>>(It.IsAny<List<Product>>())).Returns(new List<ProductDto> { new ProductDto { Name = "Product1" } });

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Product1", result[0].Name);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Product1" };
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<ProductDto>(It.IsAny<Product>())).Returns(new ProductDto { Name = "Product1" });

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Product1", result.Name);
        }

        [Fact]
        public async Task AddProductAsync_ShouldAddProduct()
        {
            // Arrange
            var productDto = new ProductDto { Name = "Product1", Category = "Category1" };
            var category = new Category { Id = Guid.NewGuid(), Name = "Category1" };
            _categoryRepositoryMock.Setup(repo => repo.GetByNameAsync("Category1")).ReturnsAsync(category);
            _mapperMock.Setup(m => m.Map<Product>(It.IsAny<ProductDto>())).Returns(new Product { Name = "Product1" });

            // Act
            await _productService.AddProductAsync(productDto);

            // Assert
            _productRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productDto = new ProductDto { Name = "UpdatedProduct" };
            _mapperMock.Setup(m => m.Map<Product>(It.IsAny<ProductDto>())).Returns(new Product { Name = "UpdatedProduct" });
            _productRepositoryMock.Setup(repo => repo.UpdateAsync(productId, It.IsAny<Product>())).ReturnsAsync(true);

            // Act
            var result = await _productService.UpdateProductAsync(productId, productDto);

            // Assert
            Assert.True(result);
            _productRepositoryMock.Verify(repo => repo.UpdateAsync(productId, It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldDeleteProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _productRepositoryMock.Setup(repo => repo.DeleteAsync(productId)).ReturnsAsync(true);

            // Act
            var result = await _productService.DeleteProductAsync(productId);

            // Assert
            Assert.True(result);
            _productRepositoryMock.Verify(repo => repo.DeleteAsync(productId), Times.Once);
        }
    }
}
