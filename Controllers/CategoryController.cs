using Microsoft.AspNetCore.Mvc;
using MongoExample.Services;
using MongoExample.Models;

namespace MongoExample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly MongoDBService _mongoDBService;

    public CategoryController(MongoDBService mongoDBService)
    {
        _mongoDBService = mongoDBService;
    }

    [HttpGet]
    public async Task<List<Category>> Get()
    {
        return await _mongoDBService.GetCategoriesAsync();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var category = await _mongoDBService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        await _mongoDBService.CreateCategoryAsync(category);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Category category)
    {
        var updated = await _mongoDBService.UpdateCategoryAsync(id, category);
        if (!updated)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _mongoDBService.DeleteCategoryAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
