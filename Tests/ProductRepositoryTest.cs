/*using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using ProductApi.Domain;
using ProductApi.Infrastructure.Repositories;

public class ProductRepositoryTests
{
    private readonly Mock<IMongoCollection<Product>> _mockCollection;
    private readonly Mock<IMongoDatabase> _mockDatabase;
    private readonly ProductRepository _repository;

    public ProductRepositoryTests()
    {
        _mockCollection = new Mock<IMongoCollection<Product>>();
        _mockDatabase = new Mock<IMongoDatabase>();

        _mockDatabase.Setup(db => db.GetCollection<Product>("Products", null))
            .Returns(_mockCollection.Object);

        _repository = new ProductRepository(_mockDatabase.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfProducts()
    {
        // Arrange
        var products = new List<Product> { new Product { Id = "1", Name = "Café", Stock = 10 } };
        var mockCursor = new Mock<IAsyncCursor<Product>>();
        mockCursor.Setup(_ => _.Current).Returns(products);
        mockCursor.SetupSequence(_ => _.MoveNextAsync(default)).ReturnsAsync(true).ReturnsAsync(false);

        _mockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<Product>>(),
            It.IsAny<FindOptions<Product, Product>>(), default))
            .ReturnsAsync(mockCursor.Object);

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }
}*/
