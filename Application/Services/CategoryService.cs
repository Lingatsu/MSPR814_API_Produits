using MongoExample.Application.DTOs;
using MongoExample.Application.Interfaces;
using MongoExample.Domain.Entities;
using MongoExample.Infrastructure.Repositories;
using AutoMapper;

namespace MongoExample.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return _mapper.Map<List<CategoryDto>>(categories);
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return category == null ? null : _mapper.Map<CategoryDto>(category);
    }

    public async Task AddCategoryAsync(CategoryDto categoryDto)
    {
        var category = _mapper.Map<Category>(categoryDto);

        category.Id = Guid.NewGuid();
        await _categoryRepository.AddAsync(category);
    }

    public async Task<bool> UpdateCategoryAsync(Guid id, CategoryDto categoryDto)
    {
        var category = _mapper.Map<Category>(categoryDto);
        return await _categoryRepository.UpdateAsync(id, category);
    }

    public async Task<bool> DeleteCategoryAsync(Guid id)
    {
        return await _categoryRepository.DeleteAsync(id);
    }
}
