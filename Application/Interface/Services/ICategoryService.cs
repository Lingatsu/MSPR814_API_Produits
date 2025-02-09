using ProductApi.Application.DTOs;

namespace ProductApi.Application.Interface.Services;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllCategoriesAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
    Task AddCategoryAsync(CategoryDto categoryDto);
    Task<bool> UpdateCategoryAsync(Guid id, CategoryDto categoryDto);
    Task<bool> DeleteCategoryAsync(Guid id);
}
