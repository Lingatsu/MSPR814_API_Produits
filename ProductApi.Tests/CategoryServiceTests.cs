using Xunit;
using Moq;
using AutoMapper;
using ProductApi.Application.DTOs;
using ProductApi.Application.Services;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Interface.Repositories;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _mapperMock = new Mock<IMapper>();
        _categoryService = new CategoryService(_categoryRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_ShouldReturnCategoryList()
    {
        // Arrange
        var categories = new List<Category> { new Category { Id = Guid.NewGuid(), Name = "TestCategory" } };
        var categoryDtos = new List<CategoryDto> { new CategoryDto { Name = "TestCategory" } };

        _categoryRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);
        _mapperMock.Setup(mapper => mapper.Map<List<CategoryDto>>(categories)).Returns(categoryDtos);

        // Act
        var result = await _categoryService.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("TestCategory", result[0].Name);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_ShouldReturnCategory_WhenExists()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var category = new Category { Id = categoryId, Name = "TestCategory" };
        var categoryDto = new CategoryDto { Name = "TestCategory" };

        _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);
        _mapperMock.Setup(mapper => mapper.Map<CategoryDto>(category)).Returns(categoryDto);

        // Act
        var result = await _categoryService.GetCategoryByIdAsync(categoryId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestCategory", result?.Name);
    }

    [Fact]
    public async Task AddCategoryAsync_ShouldCallRepository()
    {
        // Arrange
        var categoryDto = new CategoryDto { Name = "NewCategory" };
        var category = new Category { Id = Guid.NewGuid(), Name = "NewCategory" };

        _mapperMock.Setup(mapper => mapper.Map<Category>(categoryDto)).Returns(category);
        _categoryRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

        // Act
        await _categoryService.AddCategoryAsync(categoryDto);

        // Assert
        _categoryRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Category>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCategoryAsync_ShouldReturnTrue_WhenSuccessful()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var categoryDto = new CategoryDto { Name = "UpdatedCategory" };
        var category = new Category { Id = categoryId, Name = "UpdatedCategory" };

        _mapperMock.Setup(mapper => mapper.Map<Category>(categoryDto)).Returns(category);
        _categoryRepositoryMock.Setup(repo => repo.UpdateAsync(categoryId, category)).ReturnsAsync(true);

        // Act
        var result = await _categoryService.UpdateCategoryAsync(categoryId, categoryDto);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteCategoryAsync_ShouldReturnTrue_WhenSuccessful()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _categoryRepositoryMock.Setup(repo => repo.DeleteAsync(categoryId)).ReturnsAsync(true);

        // Act
        var result = await _categoryService.DeleteCategoryAsync(categoryId);

        // Assert
        Assert.True(result);
    }
}
