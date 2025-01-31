using System;
using Microsoft.AspNetCore.Mvc;
using MongoExample.Services;
using MongoExample.Models;

namespace MongoExample.Controllers; 

[Controller]
[Route("api/[controller]")]
public class ProductController: Controller {
    
    private readonly MongoDBService _mongoDBService;

    public ProductController(MongoDBService mongoDBService) {
        _mongoDBService = mongoDBService;
    }

    [HttpGet]
    public async Task<List<Product>> Get() {
        return await _mongoDBService.GetAsync();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _mongoDBService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Product product) {
        await _mongoDBService.CreateAsync(product);
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] Product updatedProduct) {
        await _mongoDBService.UpdateProductAsync(id, updatedProduct);
        return NoContent();         
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id) {
        await _mongoDBService.DeleteAsync(id);
        return NoContent();
    }

}