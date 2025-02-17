using AutoMapper;
using Moq;
using ProductApi.Application.DTOs;
using ProductApi.Application.Services;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Interface.Repositories;
using Xunit;

namespace ProductApi.Tests.Services
{
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
            var categories = new List<Category> { new Category { Id = Guid.NewGuid(), Name = "Category1" } };
            _categoryRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);
            _mapperMock.Setup(m => m.Map<List<CategoryDto>>(It.IsAny<List<Category>>())).Returns(new List<CategoryDto> { new CategoryDto { Name = "Category1" } });

            // Act
            var result = await _categoryService.GetAllCategoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Category1", result[0].Name);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_ShouldReturnCategory()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category { Id = categoryId, Name = "Category1" };
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mapperMock.Setup(m => m.Map<CategoryDto>(It.IsAny<Category>())).Returns(new CategoryDto { Name = "Category1" });

            // Act
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Category1", result.Name);
        }

        [Fact]
        public async Task AddCategoryAsync_ShouldAddCategory()
        {
            // Arrange
            var categoryDto = new CategoryDto { Name = "Category1" };
            _mapperMock.Setup(m => m.Map<Category>(It.IsAny<CategoryDto>())).Returns(new Category { Name = "Category1" });

            // Act
            await _categoryService.AddCategoryAsync(categoryDto);

            // Assert
            _categoryRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldUpdateCategory()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var categoryDto = new CategoryDto { Name = "UpdatedCategory" };
            _mapperMock.Setup(m => m.Map<Category>(It.IsAny<CategoryDto>())).Returns(new Category { Name = "UpdatedCategory" });
            _categoryRepositoryMock.Setup(repo => repo.UpdateAsync(categoryId, It.IsAny<Category>())).ReturnsAsync(true);

            // Act
            var result = await _categoryService.UpdateCategoryAsync(categoryId, categoryDto);

            // Assert
            Assert.True(result);
            _categoryRepositoryMock.Verify(repo => repo.UpdateAsync(categoryId, It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCategoryAsync_ShouldDeleteCategory()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            _categoryRepositoryMock.Setup(repo => repo.DeleteAsync(categoryId)).ReturnsAsync(true);

            // Act
            var result = await _categoryService.DeleteCategoryAsync(categoryId);

            // Assert
            Assert.True(result);
            _categoryRepositoryMock.Verify(repo => repo.DeleteAsync(categoryId), Times.Once);
        }
    }
}